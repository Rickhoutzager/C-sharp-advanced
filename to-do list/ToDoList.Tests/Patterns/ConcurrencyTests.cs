using to_do_list;
using to_do_list.Patterns;

namespace ToDoList.Tests.Patterns
{
    [Collection("Sequential")]
    public class ConcurrencyTests : IDisposable
    {
        private readonly ConcurrencyTodoManager _manager;

        public ConcurrencyTests()
        {
            _manager = new ConcurrencyTodoManager();
        }

        public void Dispose()
        {
            _manager?.Dispose();
            // Clean up any todo.json created during tests
            try { File.Delete("todo.json"); } catch { }
        }

        // --- Reader-Writer Lock Pattern Tests ---

        [Fact]
        public void GetTodoList_Initially_ReturnsEmptyList()
        {
            // Act
            var list = _manager.GetTodoList();

            // Assert
            Assert.NotNull(list);
            Assert.Empty(list);
        }

        [Fact]
        public void GetProjects_Initially_ReturnsEmptyList()
        {
            // Act
            var projects = _manager.GetProjects();

            // Assert
            Assert.NotNull(projects);
            Assert.Empty(projects);
        }

        // --- Async Operations Tests ---

        [Fact]
        public async Task AddTodoItemAsync_AddsItemToList()
        {
            // Arrange
            var item = new TodoItem { Title = "Async Task", Priority = 2 };

            // Act
            await _manager.AddTodoItemAsync(item);

            // Assert
            var list = _manager.GetTodoList();
            Assert.Contains(list, i => i.Title == "Async Task");
        }

        [Fact]
        public async Task AddTodoItemAsync_MultipleItems_AllAdded()
        {
            // Arrange
            var items = Enumerable.Range(1, 10)
                .Select(i => new TodoItem { Title = $"Item {i}" })
                .ToList();

            // Act
            foreach (var item in items)
            {
                await _manager.AddTodoItemAsync(item);
            }

            // Assert
            var list = _manager.GetTodoList();
            Assert.Equal(10, list.Count);
        }

        [Fact]
        public async Task UpdateTodoItemAsync_UpdatesExistingItem()
        {
            // Arrange
            var original = new TodoItem { Title = "Original", Priority = 1 };
            await _manager.AddTodoItemAsync(original);

            var updated = new TodoItem { Title = "Updated", Priority = 5 };

            // Act
            await _manager.UpdateTodoItemAsync(0, updated);

            // Assert
            var list = _manager.GetTodoList();
            Assert.Equal("Updated", list[0].Title);
            Assert.Equal(5, list[0].Priority);
        }

        [Fact]
        public async Task UpdateTodoItemAsync_InvalidIndex_DoesNotThrow()
        {
            // Arrange
            var item = new TodoItem { Title = "Updated" };

            // Act & Assert
            var exception = await Record.ExceptionAsync(() => _manager.UpdateTodoItemAsync(999, item));
            Assert.Null(exception);
        }

        // --- Async File Operations Tests ---

        [Fact]
        public async Task SaveDataAsync_DoesNotThrow()
        {
            // Act & Assert
            var exception = await Record.ExceptionAsync(() => _manager.SaveDataAsync());
            Assert.Null(exception);
        }

        [Fact]
        public async Task LoadDataAsync_DoesNotThrow()
        {
            // Act & Assert
            var exception = await Record.ExceptionAsync(() => _manager.LoadDataAsync());
            Assert.Null(exception);
        }

        // --- Background Worker Tests ---

        [Fact]
        public async Task ImportDataAsync_CompletesSuccessfully()
        {
            // Act
            await _manager.ImportDataAsync("testfile.json");
            await _manager.WaitForImportCompletion();

            // Assert - should complete without throwing
            Assert.True(true);
        }

        // --- Thread Safety Tests ---

        [Fact]
        public async Task ConcurrentAddOperations_AllItemsAdded()
        {
            // Arrange
            var tasks = new List<Task>();
            for (int i = 0; i < 50; i++)
            {
                var item = new TodoItem { Title = $"Concurrent {i}" };
                tasks.Add(_manager.AddTodoItemAsync(item));
            }

            // Act
            await Task.WhenAll(tasks);

            // Assert
            var list = _manager.GetTodoList();
            Assert.Equal(50, list.Count);
        }

        [Fact]
        public async Task ConcurrentReads_DoNotThrow()
        {
            // Arrange
            // First add some items
            for (int i = 0; i < 10; i++)
            {
                await _manager.AddTodoItemAsync(new TodoItem { Title = $"Item {i}" });
            }

            // Act - perform concurrent reads
            var readTasks = Enumerable.Range(0, 20)
                .Select(_ => Task.Run(() => _manager.GetTodoList()))
                .ToList();

            var results = await Task.WhenAll(readTasks);

            // Assert
            foreach (var result in results)
            {
                Assert.Equal(10, result.Count);
            }
        }

        [Fact]
        public async Task MixedReadWrite_DoesNotThrow()
        {
            // Arrange
            var tasks = new List<Task>();

            // Act - mix reads and writes
            for (int i = 0; i < 20; i++)
            {
                var item = new TodoItem { Title = $"Mixed {i}" };
                tasks.Add(_manager.AddTodoItemAsync(item));
                tasks.Add(Task.Run(() => _manager.GetTodoList()));
            }

            // Assert
            var exception = await Record.ExceptionAsync(() => Task.WhenAll(tasks));
            Assert.Null(exception);
        }

        // --- Shutdown Tests ---

        [Fact]
        public async Task ShutdownAsync_DoesNotThrow()
        {
            // Act & Assert
            var exception = await Record.ExceptionAsync(() => _manager.ShutdownAsync());
            Assert.Null(exception);
        }
    }
}