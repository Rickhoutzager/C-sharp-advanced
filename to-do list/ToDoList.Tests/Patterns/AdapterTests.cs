using System.IO;
using System.Text.Json;
using to_do_list;
using to_do_list.Patterns;

namespace ToDoList.Tests.Patterns
{
    public class AdapterTests : IDisposable
    {
        private readonly string _tempDir;

        public AdapterTests()
        {
            _tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(_tempDir);
        }

        public void Dispose()
        {
            if (Directory.Exists(_tempDir))
                Directory.Delete(_tempDir, true);
        }

        private string GetTempPath(string extension) =>
            Path.Combine(_tempDir, $"test.{extension}");

        // --- JsonTodoStorageAdapter Tests ---

        [Fact]
        public void JsonAdapter_SaveAndLoad_PreservesItems()
        {
            // Arrange
            var adapter = new JsonTodoStorageAdapter();
            var path = GetTempPath("json");
            var items = new List<TodoItem>
            {
                new TodoItem { Title = "Task 1", Completed = false, Priority = 2, Category = "Work" },
                new TodoItem { Title = "Task 2", Completed = true, Priority = 1, Category = "Personal" }
            };

            // Act
            adapter.Save(path, items);
            var loaded = adapter.Load(path);

            // Assert
            Assert.Equal(2, loaded.Count);
            Assert.Equal("Task 1", loaded[0].Title);
            Assert.False(loaded[0].Completed);
            Assert.Equal(2, loaded[0].Priority);
            Assert.Equal("Work", loaded[0].Category);
            Assert.Equal("Task 2", loaded[1].Title);
            Assert.True(loaded[1].Completed);
        }

        [Fact]
        public void JsonAdapter_Load_NonExistentFile_ReturnsEmptyList()
        {
            // Arrange
            var adapter = new JsonTodoStorageAdapter();

            // Act
            var loaded = adapter.Load(GetTempPath("json"));

            // Assert
            Assert.Empty(loaded);
        }

        [Fact]
        public void JsonAdapter_SaveAndLoad_ItemWithDueDate_PreservesDueDate()
        {
            // Arrange
            var adapter = new JsonTodoStorageAdapter();
            var path = GetTempPath("json");
            var dueDate = new DateTime(2025, 12, 31, 10, 30, 0);
            var items = new List<TodoItem>
            {
                new TodoItem { Title = "With Due Date", DueDate = dueDate }
            };

            // Act
            adapter.Save(path, items);
            var loaded = adapter.Load(path);

            // Assert
            Assert.NotNull(loaded[0].DueDate);
            Assert.Equal(dueDate, loaded[0].DueDate.Value);
        }

        [Fact]
        public void JsonAdapter_SaveAndLoad_ItemWithNullDueDate_RemainsNull()
        {
            // Arrange
            var adapter = new JsonTodoStorageAdapter();
            var path = GetTempPath("json");
            var items = new List<TodoItem>
            {
                new TodoItem { Title = "No Due Date", DueDate = null }
            };

            // Act
            adapter.Save(path, items);
            var loaded = adapter.Load(path);

            // Assert
            Assert.Null(loaded[0].DueDate);
        }

        [Fact]
        public void JsonAdapter_SaveAndLoad_RoundTrip_MaintainsCount()
        {
            // Arrange
            var adapter = new JsonTodoStorageAdapter();
            var path = GetTempPath("json");
            var items = Enumerable.Range(1, 100)
                .Select(i => new TodoItem { Title = $"Task {i}", Priority = i % 5 + 1 })
                .ToList();

            // Act
            adapter.Save(path, items);
            var loaded = adapter.Load(path);

            // Assert
            Assert.Equal(100, loaded.Count);
        }

        // --- XmlTodoStorageAdapter Tests ---

        [Fact]
        public void XmlAdapter_SaveAndLoad_PreservesItems()
        {
            // Arrange
            var adapter = new XmlTodoStorageAdapter();
            var path = GetTempPath("xml");
            var items = new List<TodoItem>
            {
                new TodoItem { Title = "XML Task 1", Completed = true, Priority = 3, Category = "Test" },
                new TodoItem { Title = "XML Task 2", Completed = false, Priority = 1, Category = "Demo" }
            };

            // Act
            adapter.Save(path, items);
            var loaded = adapter.Load(path);

            // Assert
            Assert.Equal(2, loaded.Count);
            Assert.Equal("XML Task 1", loaded[0].Title);
            Assert.True(loaded[0].Completed);
            Assert.Equal("XML Task 2", loaded[1].Title);
            Assert.False(loaded[1].Completed);
        }

        [Fact]
        public void XmlAdapter_Load_NonExistentFile_ReturnsEmptyList()
        {
            // Arrange
            var adapter = new XmlTodoStorageAdapter();

            // Act
            var loaded = adapter.Load(GetTempPath("xml"));

            // Assert
            Assert.Empty(loaded);
        }

        [Fact]
        public void XmlAdapter_SaveAndLoad_MultipleItems_RoundTrip()
        {
            // Arrange
            var adapter = new XmlTodoStorageAdapter();
            var path = GetTempPath("xml");
            var items = new List<TodoItem>
            {
                new TodoItem { Title = "A", Priority = 1 },
                new TodoItem { Title = "B", Priority = 2 },
                new TodoItem { Title = "C", Priority = 3 }
            };

            // Act
            adapter.Save(path, items);
            var loaded = adapter.Load(path);

            // Assert
            Assert.Equal(3, loaded.Count);
            Assert.Equal("A", loaded[0].Title);
            Assert.Equal("C", loaded[2].Title);
            Assert.Equal(3, loaded[2].Priority);
        }

        [Fact]
        public void XmlAdapter_SaveAndLoad_EmptyList_RoundTrip()
        {
            // Arrange
            var adapter = new XmlTodoStorageAdapter();
            var path = GetTempPath("xml");
            var items = new List<TodoItem>();

            // Act
            adapter.Save(path, items);
            var loaded = adapter.Load(path);

            // Assert
            Assert.Empty(loaded);
        }

        // --- Cross-format Tests ---

        [Fact]
        public void JsonAndXmlAdapters_ContainSameData()
        {
            // Arrange
            var jsonAdapter = new JsonTodoStorageAdapter();
            var xmlAdapter = new XmlTodoStorageAdapter();
            var jsonPath = GetTempPath("json");
            var xmlPath = GetTempPath("xml");
            var items = new List<TodoItem>
            {
                new TodoItem { Title = "Cross Format", Completed = true, Priority = 2, Category = "Test" }
            };

            // Act
            jsonAdapter.Save(jsonPath, items);
            xmlAdapter.Save(xmlPath, items);
            var jsonLoaded = jsonAdapter.Load(jsonPath);
            var xmlLoaded = xmlAdapter.Load(xmlPath);

            // Assert
            Assert.Equal(jsonLoaded.Count, xmlLoaded.Count);
            Assert.Equal(jsonLoaded[0].Title, xmlLoaded[0].Title);
            Assert.Equal(jsonLoaded[0].Completed, xmlLoaded[0].Completed);
        }
    }
}