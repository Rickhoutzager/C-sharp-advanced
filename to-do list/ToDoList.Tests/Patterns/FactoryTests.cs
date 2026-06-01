using to_do_list;
using to_do_list.Patterns;

namespace ToDoList.Tests.Patterns
{
    public class FactoryTests
    {
        // --- Concrete Creators return the correct Concrete Product (polymorphism) ---

        [Fact]
        public void WorkCreator_CreateTodoItem_ReturnsWorkTodoItem()
        {
            // Arrange
            TodoItemCreator creator = new WorkTodoItemCreator();

            // Act
            ITodoItem item = creator.CreateTodoItem("Write report");

            // Assert
            Assert.IsType<WorkTodoItem>(item);
            Assert.Equal("Write report", item.Title);
            Assert.Equal("Work", item.Category);
        }

        [Fact]
        public void PersonalCreator_CreateTodoItem_ReturnsPersonalTodoItem()
        {
            // Arrange
            TodoItemCreator creator = new PersonalTodoItemCreator();

            // Act
            ITodoItem item = creator.CreateTodoItem("Buy groceries");

            // Assert
            Assert.IsType<PersonalTodoItem>(item);
            Assert.Equal("Buy groceries", item.Title);
            Assert.Equal("Personal", item.Category);
        }

        [Fact]
        public void UrgentCreator_CreateTodoItem_ReturnsUrgentTodoItem()
        {
            // Arrange
            TodoItemCreator creator = new UrgentTodoItemCreator();

            // Act
            ITodoItem item = creator.CreateTodoItem("Call client");

            // Assert
            Assert.IsType<UrgentTodoItem>(item);
            Assert.Equal("Call client", item.Title);
            Assert.Equal(5, item.Priority);
            Assert.Equal("Urgent", item.Category);
        }

        // --- The factory method is used polymorphically through the abstract Creator ---

        [Fact]
        public void Creator_IsPolymorphic_ProducesDifferentConcreteTypes()
        {
            // Arrange: the same abstract Creator reference produces different products
            var creators = new TodoItemCreator[]
            {
                new WorkTodoItemCreator(),
                new PersonalTodoItemCreator(),
                new UrgentTodoItemCreator()
            };

            // Act
            var products = creators.Select(c => c.CreateTodoItem("Task")).ToList();

            // Assert
            Assert.IsType<WorkTodoItem>(products[0]);
            Assert.IsType<PersonalTodoItem>(products[1]);
            Assert.IsType<UrgentTodoItem>(products[2]);
        }

        // --- The Creator's operation relies on the factory method ---

        [Fact]
        public void CreateConfiguredTodoItem_UsesFactoryMethod_AndSetsDefaults()
        {
            // Arrange
            TodoItemCreator creator = new WorkTodoItemCreator();

            // Act
            ITodoItem item = creator.CreateConfiguredTodoItem("Plan sprint");

            // Assert
            Assert.IsType<WorkTodoItem>(item);
            Assert.False(item.Completed);
            Assert.Equal("Work", item.Category);
        }

        // --- Concrete Products are also usable as TodoItem (works with the rest of the app) ---

        [Fact]
        public void ConcreteProduct_IsAlsoTodoItem()
        {
            // Arrange & Act
            ITodoItem item = new UrgentTodoItemCreator().CreateTodoItem("Deploy");

            // Assert
            Assert.IsAssignableFrom<TodoItem>(item);
        }
    }
}
