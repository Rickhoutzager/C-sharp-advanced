using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using to_do_list.Patterns;

namespace to_do_list
{
    public class TodoItem : ITodoItem
    {

        public string Title { get; set; }
        public bool Completed { get; set; }
        public int Priority { get; set; }
        public string Category { get; set; }
        public DateTime? DueDate { get; set; }
        
        // Hierarchy support for Composite pattern
        public TodoComponent ParentComponent { get; set; }
        public List<TodoComponent> ChildComponents { get; set; } = new List<TodoComponent>();

        public TodoItem()
        {
            Priority = 2; // Default to Medium priority
            Category = "General"; // Default category
        }

        public override string ToString()
        {
            // Create decorated description using the Decorator pattern
            ITodoItemComponent baseItem = new TodoItemBase(Title, Category);
            
            if (Priority > 1)
            {
                baseItem = new PriorityDecorator(baseItem, Priority);
            }
            
            if (DueDate.HasValue)
            {
                baseItem = new DueDateDecorator(baseItem, DueDate.Value);
            }
            
            var status = Completed ? "✓" : "○";
            return $"{status} {baseItem.GetDescription()}";
        }

        // Helper method to create a TodoLeaf from this TodoItem
        public TodoLeaf ToTodoLeaf()
        {
            return new TodoLeaf(this);
        }

        // Helper method to create a TodoComposite from this TodoItem
        public TodoComposite ToTodoComposite()
        {
            return new TodoComposite(Title, Priority, Category, DueDate);
        }

        // Hierarchy management methods
        public void AddChild(TodoComponent child)
        {
            ChildComponents.Add(child);
            child.Parent = this.ToTodoLeaf(); // Set parent relationship
        }

        public void RemoveChild(TodoComponent child)
        {
            ChildComponents.Remove(child);
            child.Parent = null;
        }

        public bool IsPartOfProject()
        {
            return ParentComponent != null;
        }

        public TodoComposite GetProject()
        {
            if (ParentComponent != null)
            {
                var current = ParentComponent;
                while (current.Parent != null && !current.IsLeaf())
                {
                    current = current.Parent;
                }
                return current as TodoComposite;
            }
            return null;
        }
    }
}
