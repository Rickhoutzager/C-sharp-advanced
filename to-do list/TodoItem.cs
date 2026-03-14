using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using to_do_list.Patterns;

namespace to_do_list
{
    public class TodoItem
    {
        public string Title { get; set; }
        public bool Completed { get; set; }
        public string Category { get; set; }
        public DateTime? DueDate { get; set; }
        public int Priority { get; set; }

        public TodoItem()
        {
            Category = "General";
            Priority = 1;
        }

        public TodoItem(string title, string category = "General", int priority = 1, DateTime? dueDate = null)
        {
            Title = title;
            Completed = false;
            Category = category;
            Priority = priority;
            DueDate = dueDate;
        }

        // Method to create a decorated version of this todo item
        public ITodoItemComponent CreateDecoratedItem()
        {
            ITodoItemComponent baseItem = new TodoItemBase(Title, Category);
            
            if (Priority > 1)
            {
                baseItem = new PriorityDecorator(baseItem, Priority);
            }
            
            if (DueDate.HasValue)
            {
                baseItem = new DueDateDecorator(baseItem, DueDate.Value);
            }
            
            return baseItem;
        }

        public override string ToString()
        {
            var decoratedItem = CreateDecoratedItem();
            return decoratedItem.GetDescription();
        }
    }
}
