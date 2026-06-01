using to_do_list.Patterns;

namespace ToDoList.Tests.Patterns
{
    public class DecoratorTests
    {
        // --- TodoItemBase Tests ---

        [Fact]
        public void TodoItemBase_GetDescription_ReturnsTitleAndCategory()
        {
            // Arrange
            var item = new TodoItemBase("Test Task", "Work");

            // Act
            var desc = item.GetDescription();

            // Assert
            Assert.Equal("Test Task (Work)", desc);
        }

        [Fact]
        public void TodoItemBase_GetDescription_WithDefaultCategory_UsesGeneral()
        {
            // Arrange
            var item = new TodoItemBase("Task");

            // Act
            var desc = item.GetDescription();

            // Assert
            Assert.Equal("Task (General)", desc);
        }

        [Fact]
        public void TodoItemBase_GetPriority_ReturnsDefault()
        {
            // Arrange
            var item = new TodoItemBase("Task");

            // Act
            var priority = item.GetPriority();

            // Assert
            Assert.Equal(1, priority);
        }

        // --- PriorityDecorator Tests ---

        [Fact]
        public void PriorityDecorator_LowPriority_NoPrefix()
        {
            // Arrange
            var baseItem = new TodoItemBase("Task");
            var decorator = new PriorityDecorator(baseItem, 1);

            // Act
            var desc = decorator.GetDescription();

            // Assert
            Assert.Equal("Task (General)", desc);
        }

        [Fact]
        public void PriorityDecorator_MediumPriority_AddsMediumPrefix()
        {
            // Arrange
            var baseItem = new TodoItemBase("Task");
            var decorator = new PriorityDecorator(baseItem, 2);

            // Act
            var desc = decorator.GetDescription();

            // Assert
            Assert.Equal("Medium: Task (General)", desc);
        }

        [Fact]
        public void PriorityDecorator_HighPriority_AddsHighPrefix()
        {
            // Arrange
            var baseItem = new TodoItemBase("Task");
            var decorator = new PriorityDecorator(baseItem, 3);

            // Act
            var desc = decorator.GetDescription();

            // Assert
            Assert.Equal("High: Task (General)", desc);
        }

        [Fact]
        public void PriorityDecorator_VeryHighPriority_AddsHighPrefix()
        {
            // Arrange
            var baseItem = new TodoItemBase("Task");
            var decorator = new PriorityDecorator(baseItem, 4);

            // Act
            var desc = decorator.GetDescription();

            // Assert
            Assert.Equal("High: Task (General)", desc);
        }

        [Fact]
        public void PriorityDecorator_CriticalPriority_AddsUrgentPrefix()
        {
            // Arrange
            var baseItem = new TodoItemBase("Task");
            var decorator = new PriorityDecorator(baseItem, 5);

            // Act
            var desc = decorator.GetDescription();

            // Assert
            Assert.Equal("URGENT: Task (General)", desc);
        }

        [Fact]
        public void PriorityDecorator_GetPriority_ReturnsDecoratorPriority()
        {
            // Arrange
            var baseItem = new TodoItemBase("Task");
            var decorator = new PriorityDecorator(baseItem, 4);

            // Act
            var priority = decorator.GetPriority();

            // Assert
            Assert.Equal(4, priority);
        }

        // --- DueDateDecorator Tests ---

        [Fact]
        public void DueDateDecorator_FutureDueDate_ShowsDueDate()
        {
            // Arrange
            var futureDate = DateTime.Now.AddDays(10);
            var baseItem = new TodoItemBase("Task");
            var decorator = new DueDateDecorator(baseItem, futureDate);

            // Act
            var desc = decorator.GetDescription();

            // Assert
            Assert.Contains("Due", desc);
            Assert.Contains(futureDate.ToString("MM/dd"), desc);
            Assert.Contains("Task (General)", desc);
        }

        [Fact]
        public void DueDateDecorator_DueToday_ShowsDueToday()
        {
            // Arrange
            // Use a time later today (but still today's date) so it is not treated as overdue
            var today = DateTime.Now.Date.AddHours(23).AddMinutes(59);
            var baseItem = new TodoItemBase("Task");
            var decorator = new DueDateDecorator(baseItem, today);

            // Act
            var desc = decorator.GetDescription();

            // Assert
            Assert.Contains("DUE TODAY:", desc);
        }

        [Fact]
        public void DueDateDecorator_Overdue_ShowsOverdue()
        {
            // Arrange
            var pastDate = DateTime.Now.AddDays(-5);
            var baseItem = new TodoItemBase("Task");
            var decorator = new DueDateDecorator(baseItem, pastDate);

            // Act
            var desc = decorator.GetDescription();

            // Assert
            Assert.Contains("OVERDUE:", desc);
        }

        [Fact]
        public void DueDateDecorator_FutureDate_DoesNotIncreasePriority()
        {
            // Arrange
            var futureDate = DateTime.Now.AddDays(10);
            var baseItem = new TodoItemBase("Task");
            var decorator = new DueDateDecorator(baseItem, futureDate);

            // Act
            var priority = decorator.GetPriority();

            // Assert
            Assert.Equal(1, priority); // Base priority is 1
        }

        [Fact]
        public void DueDateDecorator_DueToday_IncreasesPriorityBy2()
        {
            // Arrange
            // Use a time later today (but still today's date) so it is not treated as overdue
            var today = DateTime.Now.Date.AddHours(23).AddMinutes(59);
            var baseItem = new TodoItemBase("Task");
            var decorator = new DueDateDecorator(baseItem, today);

            // Act
            var priority = decorator.GetPriority();

            // Assert
            Assert.Equal(3, priority); // Base 1 + 2 = 3
        }

        [Fact]
        public void DueDateDecorator_Overdue_IncreasesPriorityBy3()
        {
            // Arrange
            var pastDate = DateTime.Now.AddDays(-1);
            var baseItem = new TodoItemBase("Task");
            var decorator = new DueDateDecorator(baseItem, pastDate);

            // Act
            var priority = decorator.GetPriority();

            // Assert
            Assert.Equal(4, priority); // Base 1 + 3 = 4
        }

        [Fact]
        public void DueDateDecorator_DueDate_ExposesProperty()
        {
            // Arrange
            var date = new DateTime(2025, 7, 4);
            var baseItem = new TodoItemBase("Task");
            var decorator = new DueDateDecorator(baseItem, date);

            // Act
            var dueDate = decorator.DueDate;

            // Assert
            Assert.Equal(date, dueDate);
        }

        // --- Composed Decorators Tests ---

        [Fact]
        public void PriorityAndDueDate_BothApplied_DescriptionContainsBoth()
        {
            // Arrange
            var baseItem = new TodoItemBase("Important Task", "Work");
            var priorityDecorator = new PriorityDecorator(baseItem, 3);
            var dueDateDecorator = new DueDateDecorator(priorityDecorator, DateTime.Now.AddDays(5));

            // Act
            var desc = dueDateDecorator.GetDescription();

            // Assert
            Assert.Contains("High:", desc);
            Assert.Contains("Due", desc);
            Assert.Contains("Important Task (Work)", desc);
        }

        [Fact]
        public void PriorityAndDueDate_OverdueAndHigh_ReflectsHighestPriorityBoost()
        {
            // Arrange
            var baseItem = new TodoItemBase("Task");
            var priorityDecorator = new PriorityDecorator(baseItem, 3);
            var dueDateDecorator = new DueDateDecorator(priorityDecorator, DateTime.Now.AddDays(-2));

            // Act
            var priority = dueDateDecorator.GetPriority();

            // Assert
            Assert.Equal(6, priority); // Priority 3 + 3 (overdue boost) = 6
        }

        // --- TodoItemDecoratorFactory Tests ---

        [Fact]
        public void Factory_CreatePriorityItem_ReturnsPriorityDecorator()
        {
            // Arrange & Act
            var item = TodoItemDecoratorFactory.CreatePriorityItem("Task", 4, "Work");

            // Assert
            Assert.Equal(4, item.GetPriority());
            Assert.Contains("High:", item.GetDescription());
        }

        [Fact]
        public void Factory_CreateDueDateItem_ReturnsDueDateDecorator()
        {
            // Arrange
            var futureDate = DateTime.Now.AddDays(3);

            // Act
            var item = TodoItemDecoratorFactory.CreateDueDateItem("Task", futureDate);

            // Assert
            Assert.Contains("Due", item.GetDescription());
        }

        [Fact]
        public void Factory_CreateComplexItem_ReturnsBothDecorators()
        {
            // Arrange
            var futureDate = DateTime.Now.AddDays(7);

            // Act
            var item = TodoItemDecoratorFactory.CreateComplexItem("Complex", 4, futureDate, "Personal");

            // Assert
            var desc = item.GetDescription();
            Assert.Contains("High:", desc);
            Assert.Contains("Due", desc);
            Assert.Contains("Complex (Personal)", desc);
        }

        // --- Decorator Base Tests ---

        [Fact]
        public void Decorator_TitleProperty_DelegatesToComponent()
        {
            // Arrange
            var baseItem = new TodoItemBase("Original");
            var decorator = new PriorityDecorator(baseItem, 2);

            // Act
            decorator.Title = "Changed";

            // Assert
            Assert.Equal("Changed", baseItem.Title);
            Assert.Equal("Changed", decorator.Title);
        }

        [Fact]
        public void Decorator_CompletedProperty_DelegatesToComponent()
        {
            // Arrange
            var baseItem = new TodoItemBase("Task");
            var decorator = new PriorityDecorator(baseItem, 2);

            // Act
            decorator.Completed = true;

            // Assert
            Assert.True(baseItem.Completed);
            Assert.True(decorator.Completed);
        }
    }
}