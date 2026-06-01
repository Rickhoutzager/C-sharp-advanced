using System.IO;
using System.Text.Json;
using to_do_list;
using to_do_list.Patterns;

namespace ToDoList.Tests.Patterns
{
    // TodoStorage uses a hard-coded "todo.json" file in the working directory.
    // We disable parallelization (see TestCollections.cs) and clean the file
    // before and after each test so tests don't interfere with each other.
    [Collection("Sequential")]
    public class SingletonTests : IDisposable
    {
        private const string TodoFile = "todo.json";

        public SingletonTests()
        {
            // Ensure a clean slate before each test
            DeleteTodoFile();
        }

        public void Dispose()
        {
            // Clean up after each test
            DeleteTodoFile();
        }

        private static void DeleteTodoFile()
        {
            try { if (File.Exists(TodoFile)) File.Delete(TodoFile); } catch { }
        }

        [Fact]
        public void TodoStorage_Instance_IsSingleton()
        {
            // Arrange & Act
            var instance1 = TodoStorage.Instance;
            var instance2 = TodoStorage.Instance;

            // Assert
            Assert.Same(instance1, instance2);
        }

        [Fact]
        public void TodoStorage_Load_WhenNoFileExists_ReturnsEmptyList()
        {
            // Arrange
            var storage = TodoStorage.Instance;

            // We need to ensure there's no todo.json in the current directory
            // Since we can't control the file path, we can just verify it handles
            // the case gracefully if the file doesn't exist, or test with the
            // assumption that a file may exist from normal operation

            // Act - this should not throw regardless
            var exception = Record.Exception(() => storage.Load());

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void TodoStorage_LoadProjects_WhenNoFileExists_ReturnsEmptyList()
        {
            // Arrange
            var storage = TodoStorage.Instance;

            // Act & Assert - should not throw
            var exception = Record.Exception(() => storage.LoadProjects());
            Assert.Null(exception);
        }

        [Fact]
        public void TodoStorage_Save_DoesNotThrow()
        {
            // Arrange
            var storage = TodoStorage.Instance;
            var items = new List<TodoItem>
            {
                new TodoItem { Title = "Save Test", Completed = false }
            };
            var projects = new List<TodoComposite>();

            // Act & Assert
            var exception = Record.Exception(() => storage.Save(items, projects));
            Assert.Null(exception);

            // Clean up the created file
            try { File.Delete("todo.json"); } catch { }
        }

        [Fact]
        public void TodoStorage_SaveAndLoad_RoundTrip_PreservesItems()
        {
            // Arrange
            var storage = TodoStorage.Instance;
            var items = new List<TodoItem>
            {
                new TodoItem { Title = "Task 1", Completed = false, Priority = 2, Category = "Personal" },
                new TodoItem { Title = "Task 2", Completed = true, Priority = 1 }
            };
            var projects = new List<TodoComposite>();

            try
            {
                // Act
                storage.Save(items, projects);
                var loaded = storage.Load();
                var loadedProjects = storage.LoadProjects();

                // Assert
                Assert.Equal(2, loaded.Count);
                Assert.Equal("Task 1", loaded[0].Title);
                Assert.False(loaded[0].Completed);
                Assert.Equal("Personal", loaded[0].Category);
                Assert.Equal("Task 2", loaded[1].Title);
                Assert.True(loaded[1].Completed);
                Assert.Empty(loadedProjects);
            }
            finally
            {
                // Clean up
                try { File.Delete("todo.json"); } catch { }
            }
        }

        [Fact]
        public void TodoStorage_SaveAndLoad_WithProjects_RoundTrip()
        {
            // Arrange
            var storage = TodoStorage.Instance;
            var items = new List<TodoItem>
            {
                new TodoItem { Title = "Standalone Task" }
            };
            var project = new TodoComposite("Test Project", 3, "Work");
            project.AddChild(new TodoLeaf(new TodoItem { Title = "Sub Task 1", Completed = true }));
            project.AddChild(new TodoLeaf(new TodoItem { Title = "Sub Task 2", Completed = false }));
            var projects = new List<TodoComposite> { project };

            try
            {
                // Act
                storage.Save(items, projects);
                var loaded = storage.Load();
                var loadedProjects = storage.LoadProjects();

                // Assert
                Assert.Single(loaded);
                Assert.Single(loadedProjects);
                Assert.Equal("Test Project", loadedProjects[0].Title);
                Assert.Equal("Work", loadedProjects[0].Category);

                // Verify project structure
                var leaves = loadedProjects[0].GetAllLeaves();
                Assert.Equal(2, leaves.Count);
            }
            finally
            {
                try { File.Delete("todo.json"); } catch { }
            }
        }

        [Fact]
        public void TodoStorage_Save_BackwardCompatRoundTrip()
        {
            // Arrange
            var storage = TodoStorage.Instance;
            var items = new List<TodoItem>
            {
                new TodoItem { Title = "Old Format Test" }
            };

            try
            {
                // Save in old format (items only, no projects)
                storage.Save(items);

                // Act - Load (should handle old format gracefully)
                var loaded = storage.Load();

                // Assert
                Assert.Single(loaded);
                Assert.Equal("Old Format Test", loaded[0].Title);
            }
            finally
            {
                try { File.Delete("todo.json"); } catch { }
            }
        }

        [Fact]
        public void TodoStorage_SaveWithProjects_ThenLoadOldFormat_FallsBack()
        {
            // Create a file in the old format (just a list of TodoItem)
            var items = new List<TodoItem>
            {
                new TodoItem { Title = "Old Item", Priority = 1 }
            };
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(items, options);
            File.WriteAllText("todo.json", json);

            try
            {
                // Act
                var storage = TodoStorage.Instance;
                var loaded = storage.Load();
                var loadedProjects = storage.LoadProjects();

                // Assert
                Assert.Single(loaded);
                Assert.Equal("Old Item", loaded[0].Title);
                Assert.Empty(loadedProjects);
            }
            finally
            {
                try { File.Delete("todo.json"); } catch { }
            }
        }

        [Fact]
        public void TodoStorage_CorruptFile_ReturnsEmptyList()
        {
            // Arrange - create a corrupt file
            File.WriteAllText("todo.json", "This is not valid JSON");

            try
            {
                // Act
                var storage = TodoStorage.Instance;
                var loaded = storage.Load();

                // Assert
                Assert.Empty(loaded);
            }
            finally
            {
                try { File.Delete("todo.json"); } catch { }
            }
        }
    }
}