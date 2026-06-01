using to_do_list;
using to_do_list.Patterns;

namespace ToDoList.Tests.Models
{
    public class TodoItemTests
    {
        [Fact]
        public void Constructor_SetsDefaultValues()
        {
            // Arrange & Act
            var item = new TodoItem();

            // Assert
            Assert.Equal(2, item.Priority);
            Assert.Equal("General", item.Category);
            Assert.False(item.Completed);
            Assert.Null(item.DueDate);
            Assert.Null(item.Title);
        }

        [Fact]
        public void ToString_WithNoDecorators_ReturnsSimpleString()
        {
            // Arrange
            var item = new TodoItem { Title = "Test Task", Completed = false, Priority = 1 };

            // Act
            var result = item.ToString();

            // Assert
            Assert.Contains("○ Test Task", result);
            Assert.Contains("(General)", result);
        }

        [Fact]
        public void ToString_WhenCompleted_ShowsCheckMark()
        {
            // Arrange
            var item = new TodoItem { Title = "Done Task", Completed = true, Priority = 1 };

            // Act
            var result = item.ToString();

            // Assert
            Assert.Contains("✓ Done Task", result);
        }

        [Fact]
        public void ToString_WithHighPriority_IncludesPriorityDecorator()
        {
            // Arrange
            var item = new TodoItem { Title = "Important", Priority = 3 };

            // Act
            var result = item.ToString();

            // Assert
            Assert.Contains("High:", result);
        }

        [Fact]
        public void ToString_WithDueDate_IncludesDueDateDecorator()
        {
            // Arrange
            var item = new TodoItem { Title = "Timely", Priority = 1, DueDate = DateTime.Now.AddDays(5) };

            // Act
            var result = item.ToString();

            // Assert
            Assert.Contains("Due", result);
        }

        [Fact]
        public void ToTodoLeaf_ReturnsLeafWithSameData()
        {
            // Arrange
            var item = new TodoItem { Title = "Leaf Task", Priority = 3, Category = "Work", DueDate = new DateTime(2025, 6, 1) };

            // Act
            var leaf = item.ToTodoLeaf();

            // Assert
            Assert.Equal("Leaf Task", leaf.Title);
            Assert.Equal(3, leaf.Priority);
            Assert.Equal("Work", leaf.Category);
            Assert.Equal(new DateTime(2025, 6, 1), leaf.DueDate);
            Assert.True(leaf.IsLeaf());
        }

        [Fact]
        public void ToTodoComposite_ReturnsCompositeWithSameData()
        {
            // Arrange
            var item = new TodoItem { Title = "Project", Priority = 3, Category = "Work", DueDate = new DateTime(2025, 12, 31) };

            // Act
            var composite = item.ToTodoComposite();

            // Assert
            Assert.Equal("Project", composite.Title);
            Assert.Equal(3, composite.Priority);
            Assert.Equal("Work", composite.Category);
            Assert.Equal(new DateTime(2025, 12, 31), composite.DueDate);
            Assert.False(composite.IsLeaf());
        }

        [Fact]
        public void AddChild_SetsParentRelationship()
        {
            // Arrange
            var parent = new TodoItem { Title = "Parent" };
            var child = new TodoItem { Title = "Child" };
            var childLeaf = child.ToTodoLeaf();

            // Act
            parent.AddChild(childLeaf);

            // Assert
            Assert.Single(parent.ChildComponents);
            Assert.NotNull(childLeaf.Parent);
        }

        [Fact]
        public void RemoveChild_ClearsParentRelationship()
        {
            // Arrange
            var parent = new TodoItem { Title = "Parent" };
            var child = new TodoItem { Title = "Child" };
            var childLeaf = child.ToTodoLeaf();
            parent.AddChild(childLeaf);

            // Act
            parent.RemoveChild(childLeaf);

            // Assert
            Assert.Empty(parent.ChildComponents);
            Assert.Null(childLeaf.Parent);
        }

        [Fact]
        public void IsPartOfProject_WhenHasParentComponent_ReturnsTrue()
        {
            // Arrange
            var parent = new TodoItem { Title = "Parent" };
            var child = new TodoItem { Title = "Child" };

            // Act
            // Simulate having a parent component via AddChild on the parent TodoItem
            var parentLeaf = parent.ToTodoLeaf();
            child.ParentComponent = parentLeaf;

            // Assert
            Assert.True(child.IsPartOfProject());
        }

        [Fact]
        public void IsPartOfProject_WhenNoParentComponent_ReturnsFalse()
        {
            // Arrange
            var item = new TodoItem { Title = "Alone" };

            // Act & Assert
            Assert.False(item.IsPartOfProject());
        }

        [Fact]
        public void GetProject_WhenPartOfProject_ReturnsProject()
        {
            // Arrange
            // GetProject() walks up the ParentComponent chain, so we must set ParentComponent.
            var project = new TodoComposite("My Project");
            var taskItem = new TodoItem { Title = "Task 1" };
            taskItem.ParentComponent = project;

            // Act
            var foundProject = taskItem.GetProject();

            // Assert
            Assert.NotNull(foundProject);
            Assert.Equal("My Project", foundProject.Title);
        }

        [Fact]
        public void GetProject_WhenStandalone_ReturnsNull()
        {
            // Arrange
            var item = new TodoItem { Title = "Standalone" };

            // Act
            var result = item.GetProject();

            // Assert
            Assert.Null(result);
        }
    }
}