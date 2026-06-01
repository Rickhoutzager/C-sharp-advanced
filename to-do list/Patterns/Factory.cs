using System;

namespace to_do_list.Patterns
{
    // =====================================================================
    // Factory Method Pattern (GoF)
    // ---------------------------------------------------------------------
    // Required elements:
    //   - Product (interface)        : ITodoItem
    //   - Concrete Products          : WorkTodoItem, PersonalTodoItem,
    //                                  UrgentTodoItem
    //   - Creator (abstract)         : TodoItemCreator  -> CreateTodoItem()
    //   - Concrete Creators          : WorkTodoItemCreator,
    //                                  PersonalTodoItemCreator,
    //                                  UrgentTodoItemCreator
    //
    // The pattern relies on inheritance and polymorphism: subclasses of the
    // Creator override the factory method to decide which concrete Product
    // gets instantiated.
    // =====================================================================

    // ---------------------------------------------------------------------
    // Product interface
    // ---------------------------------------------------------------------
    public interface ITodoItem
    {
        string Title { get; set; }
        bool Completed { get; set; }
        int Priority { get; set; }
        string Category { get; set; }
        DateTime? DueDate { get; set; }
    }

    // ---------------------------------------------------------------------
    // Concrete Products
    // Each concrete product configures its own defaults, so the *type*
    // produced by a creator carries meaning (polymorphism).
    // ---------------------------------------------------------------------

    // A todo item geared towards work tasks.
    public class WorkTodoItem : TodoItem, ITodoItem
    {
        public WorkTodoItem()
        {
            Category = "Work";
        }
    }

    // A todo item geared towards personal tasks.
    public class PersonalTodoItem : TodoItem, ITodoItem
    {
        public PersonalTodoItem()
        {
            Category = "Personal";
        }
    }

    // A todo item that is always created with the highest priority.
    public class UrgentTodoItem : TodoItem, ITodoItem
    {
        public UrgentTodoItem()
        {
            Priority = 5; // Highest priority
            Category = "Urgent";
        }
    }

    // ---------------------------------------------------------------------
    // Creator (abstract)
    // Declares the factory method that returns a Product. May also define a
    // default implementation that returns a default Concrete Product.
    // ---------------------------------------------------------------------
    public abstract class TodoItemCreator
    {
        // The Factory Method. Subclasses override this to decide which
        // concrete ITodoItem is created.
        public abstract ITodoItem CreateTodoItem(string title);

        // An "operation" that relies on the factory method. This shows that
        // the Creator's logic is decoupled from the concrete product type:
        // the actual product is determined polymorphically by the subclass.
        public ITodoItem CreateConfiguredTodoItem(string title)
        {
            ITodoItem item = CreateTodoItem(title);
            item.Completed = false;
            return item;
        }
    }

    // ---------------------------------------------------------------------
    // Concrete Creators
    // Override the factory method to return a specific Concrete Product.
    // ---------------------------------------------------------------------
    public class WorkTodoItemCreator : TodoItemCreator
    {
        public override ITodoItem CreateTodoItem(string title)
        {
            return new WorkTodoItem { Title = title };
        }
    }

    public class PersonalTodoItemCreator : TodoItemCreator
    {
        public override ITodoItem CreateTodoItem(string title)
        {
            return new PersonalTodoItem { Title = title };
        }
    }

    public class UrgentTodoItemCreator : TodoItemCreator
    {
        public override ITodoItem CreateTodoItem(string title)
        {
            return new UrgentTodoItem { Title = title };
        }
    }
}
