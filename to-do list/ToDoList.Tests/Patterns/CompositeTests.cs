using to_do_list;
using to_do_list.Patterns;

namespace ToDoList.Tests.Patterns
{
    public class CompositeTests
    {
        // --- TodoLeaf Tests ---

        [Fact]
        public void TodoLeaf_IsLeaf_ReturnsTrue()
        {
            // Arrange
            var leaf = new TodoLeaf(new TodoItem { Title = "Task" });

            // Act & Assert
            Assert.True(leaf.IsLeaf());
        }

        [Fact]
        public void TodoLeaf_AddChild_ThrowsInvalidOperationException()
        {
            // Arrange
            var leaf = new TodoLeaf(new TodoItem { Title = "Task" });

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => leaf.AddChild(new TodoLeaf(new TodoItem { Title = "Child" })));
        }

        [Fact]
        public void TodoLeaf_RemoveChild_ThrowsInvalidOperationException()
        {
            // Arrange
            var leaf = new TodoLeaf(new TodoItem { Title = "Task" });

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => leaf.RemoveChild(new TodoLeaf(new TodoItem { Title = "Child" })));
        }

        [Fact]
        public void TodoLeaf_GetChildren_ReturnsEmptyList()
        {
            // Arrange
            var leaf = new TodoLeaf(new TodoItem { Title = "Task" });

            // Act
            var children = leaf.GetChildren();

            // Assert
            Assert.Empty(children);
        }

        [Fact]
        public void TodoLeaf_GetTotalTasks_ReturnsOne()
        {
            // Arrange
            var leaf = new TodoLeaf(new TodoItem { Title = "Task" });

            // Act
            var total = leaf.GetTotalTasks();

            // Assert
            Assert.Equal(1, total);
        }

        [Fact]
        public void TodoLeaf_GetCompletedTasks_WhenCompleted_ReturnsOne()
        {
            // Arrange
            var leaf = new TodoLeaf(new TodoItem { Title = "Task", Completed = true });

            // Act
            var completed = leaf.GetCompletedTasks();

            // Assert
            Assert.Equal(1, completed);
        }

        [Fact]
        public void TodoLeaf_GetCompletedTasks_WhenNotCompleted_ReturnsZero()
        {
            // Arrange
            var leaf = new TodoLeaf(new TodoItem { Title = "Task", Completed = false });

            // Act
            var completed = leaf.GetCompletedTasks();

            // Assert
            Assert.Equal(0, completed);
        }

        [Fact]
        public void TodoLeaf_GetCompletionPercentage_WhenCompleted_Returns100()
        {
            // Arrange
            var leaf = new TodoLeaf(new TodoItem { Title = "Task", Completed = true });

            // Act
            var percentage = leaf.GetCompletionPercentage();

            // Assert
            Assert.Equal(100.0, percentage);
        }

        [Fact]
        public void TodoLeaf_GetCompletionPercentage_WhenNotCompleted_Returns0()
        {
            // Arrange
            var leaf = new TodoLeaf(new TodoItem { Title = "Task", Completed = false });

            // Act
            var percentage = leaf.GetCompletionPercentage();

            // Assert
            Assert.Equal(0.0, percentage);
        }

        [Fact]
        public void TodoLeaf_GetTodoItem_ReturnsOriginalItem()
        {
            // Arrange
            var item = new TodoItem { Title = "Original" };
            var leaf = new TodoLeaf(item);

            // Act
            var result = leaf.GetTodoItem();

            // Assert
            Assert.Same(item, result);
        }

        [Fact]
        public void TodoLeaf_Display_ShowsCorrectFormat()
        {
            // Arrange
            var leaf = new TodoLeaf(new TodoItem { Title = "Simple Task", Category = "Work" });

            // Act
            var display = leaf.Display();

            // Assert
            Assert.Contains("○ Simple Task", display);
            Assert.Contains("[Work]", display);
        }

        // --- TodoComposite Tests ---

        [Fact]
        public void TodoComposite_IsLeaf_ReturnsFalse()
        {
            // Arrange
            var composite = new TodoComposite("Project");

            // Act & Assert
            Assert.False(composite.IsLeaf());
        }

        [Fact]
        public void TodoComposite_AddChild_AddsToChildren()
        {
            // Arrange
            var composite = new TodoComposite("Project");
            var leaf = new TodoLeaf(new TodoItem { Title = "Task" });

            // Act
            composite.AddChild(leaf);

            // Assert
            Assert.Single(composite.GetChildren());
        }

        [Fact]
        public void TodoComposite_AddChild_NullChild_DoesNothing()
        {
            // Arrange
            var composite = new TodoComposite("Project");

            // Act
            composite.AddChild(null);

            // Assert
            Assert.Empty(composite.GetChildren());
        }

        [Fact]
        public void TodoComposite_AddChild_SetsParent()
        {
            // Arrange
            var composite = new TodoComposite("Project");
            var leaf = new TodoLeaf(new TodoItem { Title = "Task" });

            // Act
            composite.AddChild(leaf);

            // Assert
            Assert.Same(composite, leaf.Parent);
        }

        [Fact]
        public void TodoComposite_RemoveChild_RemovesFromChildren()
        {
            // Arrange
            var composite = new TodoComposite("Project");
            var leaf = new TodoLeaf(new TodoItem { Title = "Task" });
            composite.AddChild(leaf);

            // Act
            composite.RemoveChild(leaf);

            // Assert
            Assert.Empty(composite.GetChildren());
        }

        [Fact]
        public void TodoComposite_RemoveChild_ClearsParent()
        {
            // Arrange
            var composite = new TodoComposite("Project");
            var leaf = new TodoLeaf(new TodoItem { Title = "Task" });
            composite.AddChild(leaf);

            // Act
            composite.RemoveChild(leaf);

            // Assert
            Assert.Null(leaf.Parent);
        }

        [Fact]
        public void TodoComposite_RemoveChild_NullChild_DoesNothing()
        {
            // Arrange
            var composite = new TodoComposite("Project");
            var leaf = new TodoLeaf(new TodoItem { Title = "Task" });
            composite.AddChild(leaf);

            // Act
            composite.RemoveChild(null);

            // Assert
            Assert.Single(composite.GetChildren());
        }

        [Fact]
        public void TodoComposite_GetChildren_ReturnsCopyOfList()
        {
            // Arrange
            var composite = new TodoComposite("Project");
            var leaf = new TodoLeaf(new TodoItem { Title = "Task" });
            composite.AddChild(leaf);

            // Act
            var children = composite.GetChildren();
            children.Clear();

            // Assert
            Assert.Single(composite.GetChildren()); // Original unchanged
        }

        [Fact]
        public void TodoComposite_WithNoChildren_GetTotalTasks_ReturnsZero()
        {
            // Arrange
            var composite = new TodoComposite("Empty Project");

            // Act
            var total = composite.GetTotalTasks();

            // Assert
            Assert.Equal(0, total);
        }

        [Fact]
        public void TodoComposite_WithChildren_GetTotalTasks_ReturnsSum()
        {
            // Arrange
            var composite = new TodoComposite("Project");
            composite.AddChild(new TodoLeaf(new TodoItem { Title = "Task 1" }));
            composite.AddChild(new TodoLeaf(new TodoItem { Title = "Task 2" }));
            composite.AddChild(new TodoLeaf(new TodoItem { Title = "Task 3" }));

            // Act
            var total = composite.GetTotalTasks();

            // Assert
            Assert.Equal(3, total);
        }

        [Fact]
        public void TodoComposite_WithNestedComposites_GetTotalTasks_ReturnsAllLeaves()
        {
            // Arrange
            var root = new TodoComposite("Root");
            var subProject = new TodoComposite("Sub");
            subProject.AddChild(new TodoLeaf(new TodoItem { Title = "Sub Task 1" }));
            subProject.AddChild(new TodoLeaf(new TodoItem { Title = "Sub Task 2" }));
            root.AddChild(subProject);
            root.AddChild(new TodoLeaf(new TodoItem { Title = "Root Task" }));

            // Act
            var total = root.GetTotalTasks();

            // Assert
            Assert.Equal(3, total);
        }

        [Fact]
        public void TodoComposite_GetCompletedTasks_WhenNoneCompleted_ReturnsZero()
        {
            // Arrange
            var composite = new TodoComposite("Project");
            composite.AddChild(new TodoLeaf(new TodoItem { Title = "Task 1", Completed = false }));
            composite.AddChild(new TodoLeaf(new TodoItem { Title = "Task 2", Completed = false }));

            // Act
            var completed = composite.GetCompletedTasks();

            // Assert
            Assert.Equal(0, completed);
        }

        [Fact]
        public void TodoComposite_GetCompletedTasks_ReturnsCount()
        {
            // Arrange
            var composite = new TodoComposite("Project");
            composite.AddChild(new TodoLeaf(new TodoItem { Title = "Task 1", Completed = true }));
            composite.AddChild(new TodoLeaf(new TodoItem { Title = "Task 2", Completed = false }));
            composite.AddChild(new TodoLeaf(new TodoItem { Title = "Task 3", Completed = true }));

            // Act
            var completed = composite.GetCompletedTasks();

            // Assert
            Assert.Equal(2, completed);
        }

        [Fact]
        public void TodoComposite_GetCompletionPercentage_WhenEmpty_ReturnsZero()
        {
            // Arrange
            var composite = new TodoComposite("Empty");

            // Act
            var percentage = composite.GetCompletionPercentage();

            // Assert
            Assert.Equal(0.0, percentage);
        }

        [Fact]
        public void TodoComposite_GetCompletionPercentage_CalculatesCorrectly()
        {
            // Arrange
            var composite = new TodoComposite("Project");
            composite.AddChild(new TodoLeaf(new TodoItem { Title = "Task 1", Completed = true }));
            composite.AddChild(new TodoLeaf(new TodoItem { Title = "Task 2", Completed = true }));
            composite.AddChild(new TodoLeaf(new TodoItem { Title = "Task 3", Completed = false }));
            composite.AddChild(new TodoLeaf(new TodoItem { Title = "Task 4", Completed = false }));

            // Act
            var percentage = composite.GetCompletionPercentage();

            // Assert
            Assert.Equal(50.0, percentage);
        }

        [Fact]
        public void TodoComposite_IsComplete_WhenAllCompleted_ReturnsTrue()
        {
            // Arrange
            var composite = new TodoComposite("Project");
            composite.AddChild(new TodoLeaf(new TodoItem { Title = "Task 1", Completed = true }));
            composite.AddChild(new TodoLeaf(new TodoItem { Title = "Task 2", Completed = true }));

            // Act & Assert
            Assert.True(composite.IsComplete);
        }

        [Fact]
        public void TodoComposite_IsComplete_WhenNotAllCompleted_ReturnsFalse()
        {
            // Arrange
            var composite = new TodoComposite("Project");
            composite.AddChild(new TodoLeaf(new TodoItem { Title = "Task 1", Completed = true }));
            composite.AddChild(new TodoLeaf(new TodoItem { Title = "Task 2", Completed = false }));

            // Act & Assert
            Assert.False(composite.IsComplete);
        }

        // --- SetCompleted Propagation Tests ---

        [Fact]
        public void SetCompleted_True_PropagatesToAllChildren()
        {
            // Arrange
            var composite = new TodoComposite("Project");
            var leaf1 = new TodoLeaf(new TodoItem { Title = "Task 1", Completed = false });
            var leaf2 = new TodoLeaf(new TodoItem { Title = "Task 2", Completed = false });
            composite.AddChild(leaf1);
            composite.AddChild(leaf2);

            // Act
            composite.SetCompleted(true);

            // Assert
            Assert.True(composite.Completed);
            Assert.True(leaf1.Completed);
            Assert.True(leaf2.Completed);
        }

        [Fact]
        public void SetCompleted_False_PropagatesToAllChildren()
        {
            // Arrange
            var composite = new TodoComposite("Project");
            var leaf1 = new TodoLeaf(new TodoItem { Title = "Task 1", Completed = true });
            var leaf2 = new TodoLeaf(new TodoItem { Title = "Task 2", Completed = true });
            composite.AddChild(leaf1);
            composite.AddChild(leaf2);

            // Act
            composite.SetCompleted(false);

            // Assert
            Assert.False(composite.Completed);
            Assert.False(leaf1.Completed);
            Assert.False(leaf2.Completed);
        }

        [Fact]
        public void SetPriority_PropagatesToChildren()
        {
            // Arrange
            var composite = new TodoComposite("Project", priority: 2);
            var leaf = new TodoLeaf(new TodoItem { Title = "Task", Priority = 1 });
            composite.AddChild(leaf);

            // Act
            composite.SetPriority(5);

            // Assert
            Assert.Equal(5, composite.Priority);
            Assert.Equal(5, leaf.Priority);
        }

        [Fact]
        public void SetCategory_PropagatesToChildren()
        {
            // Arrange
            var composite = new TodoComposite("Project", category: "Work");
            var leaf = new TodoLeaf(new TodoItem { Title = "Task", Category = "Personal" });
            composite.AddChild(leaf);

            // Act
            composite.SetCategory("Home");

            // Assert
            Assert.Equal("Home", composite.Category);
            Assert.Equal("Home", leaf.Category);
        }

        [Fact]
        public void SetDueDate_PropagatesToChildren()
        {
            // Arrange
            var date = new DateTime(2025, 12, 31);
            var composite = new TodoComposite("Project");
            var leaf = new TodoLeaf(new TodoItem { Title = "Task" });
            composite.AddChild(leaf);

            // Act
            composite.SetDueDate(date);

            // Assert
            Assert.Equal(date, composite.DueDate);
            Assert.Equal(date, leaf.DueDate);
        }

        // --- Project Operations Tests ---

        [Fact]
        public void CompleteAll_SetsAllChildrenCompleted()
        {
            // Arrange
            var composite = new TodoComposite("Project");
            composite.AddChild(new TodoLeaf(new TodoItem { Title = "Task 1", Completed = false }));
            composite.AddChild(new TodoLeaf(new TodoItem { Title = "Task 2", Completed = false }));

            // Act
            composite.CompleteAll();

            // Assert
            Assert.True(composite.Completed);
            Assert.True(composite.GetChildren()[0].Completed);
            Assert.True(composite.GetChildren()[1].Completed);
        }

        [Fact]
        public void IncompleteAll_SetsAllChildrenNotCompleted()
        {
            // Arrange
            var composite = new TodoComposite("Project");
            composite.AddChild(new TodoLeaf(new TodoItem { Title = "Task 1", Completed = true }));
            composite.AddChild(new TodoLeaf(new TodoItem { Title = "Task 2", Completed = true }));

            // Act
            composite.IncompleteAll();

            // Assert
            Assert.False(composite.Completed);
            Assert.False(composite.GetChildren()[0].Completed);
            Assert.False(composite.GetChildren()[1].Completed);
        }

        [Fact]
        public void DeleteAll_RemovesAllChildren()
        {
            // Arrange
            var composite = new TodoComposite("Project");
            composite.AddChild(new TodoLeaf(new TodoItem { Title = "Task 1" }));
            composite.AddChild(new TodoLeaf(new TodoItem { Title = "Task 2" }));

            // Act
            composite.DeleteAll();

            // Assert
            Assert.Empty(composite.GetChildren());
        }

        [Fact]
        public void GetAllLeaves_FlatList_ReturnsAllLeaves()
        {
            // Arrange
            var composite = new TodoComposite("Project");
            var leaf1 = new TodoLeaf(new TodoItem { Title = "Task 1" });
            var leaf2 = new TodoLeaf(new TodoItem { Title = "Task 2" });
            composite.AddChild(leaf1);
            composite.AddChild(leaf2);

            // Act
            var leaves = composite.GetAllLeaves();

            // Assert
            Assert.Equal(2, leaves.Count);
            Assert.Contains(leaf1, leaves);
            Assert.Contains(leaf2, leaves);
        }

        [Fact]
        public void GetAllLeaves_NestedHierarchy_ReturnsAllNestedLeaves()
        {
            // Arrange
            var root = new TodoComposite("Root");
            var sub = new TodoComposite("Sub");
            var leaf1 = new TodoLeaf(new TodoItem { Title = "Sub Task" });
            var leaf2 = new TodoLeaf(new TodoItem { Title = "Root Task" });
            sub.AddChild(leaf1);
            root.AddChild(sub);
            root.AddChild(leaf2);

            // Act
            var leaves = root.GetAllLeaves();

            // Assert
            Assert.Equal(2, leaves.Count);
            Assert.Contains(leaf1, leaves);
            Assert.Contains(leaf2, leaves);
        }

        [Fact]
        public void AddChild_InheritsDefaultProperties_WhenChildHasDefaults()
        {
            // Arrange
            var composite = new TodoComposite("Project", priority: 3, category: "Work", dueDate: new DateTime(2025, 6, 1));
            var leaf = new TodoLeaf(new TodoItem { Title = "Task", Priority = 0, Category = "" });

            // Act
            composite.AddChild(leaf);

            // Assert
            Assert.Equal(3, leaf.Priority);
            Assert.Equal("Work", leaf.Category);
            Assert.Equal(new DateTime(2025, 6, 1), leaf.DueDate);
        }

        // --- TodoCompositeFactory Tests ---

        [Fact]
        public void Factory_CreateProject_ReturnsComposite()
        {
            // Arrange & Act
            var project = TodoCompositeFactory.CreateProject("My Project");

            // Assert
            Assert.IsType<TodoComposite>(project);
            Assert.Equal("My Project", project.Title);
            Assert.Equal(3, project.Priority);
            Assert.Equal("Work", project.Category);
        }

        [Fact]
        public void Factory_CreateTask_ReturnsLeaf()
        {
            // Arrange
            var item = new TodoItem { Title = "Task Item" };

            // Act
            var task = TodoCompositeFactory.CreateTask(item);

            // Assert
            Assert.IsType<TodoLeaf>(task);
            Assert.Equal("Task Item", task.Title);
            Assert.Same(item, task.GetTodoItem());
        }

        [Fact]
        public void Factory_CreateWorkProject_SetsWorkCategory()
        {
            // Arrange & Act
            var project = TodoCompositeFactory.CreateWorkProject("Work Project");

            // Assert
            Assert.Equal("Work Project", project.Title);
            Assert.Equal(3, project.Priority);
            Assert.Equal("Work", project.Category);
        }

        [Fact]
        public void Factory_CreatePersonalProject_SetsPersonalCategory()
        {
            // Arrange & Act
            var project = TodoCompositeFactory.CreatePersonalProject("Personal Project");

            // Assert
            Assert.Equal("Personal Project", project.Title);
            Assert.Equal(2, project.Priority);
            Assert.Equal("Personal", project.Category);
        }

        [Fact]
        public void Factory_CreateHomeProject_SetsHomeCategory()
        {
            // Arrange & Act
            var project = TodoCompositeFactory.CreateHomeProject("Home Project");

            // Assert
            Assert.Equal("Home Project", project.Title);
            Assert.Equal(2, project.Priority);
            Assert.Equal("Home", project.Category);
        }
    }
}