using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace to_do_list.Patterns
{
    // Base component interface
    public interface ITodoItemComponent
    {
        string Title { get; set; }
        bool Completed { get; set; }
        string GetDescription();
        int GetPriority();
    }

    // Concrete component
    public class TodoItemBase : ITodoItemComponent
    {
        public string Title { get; set; }
        public bool Completed { get; set; }
        public string Category { get; set; }

        public TodoItemBase(string title, string category = "General")
        {
            Title = title;
            Completed = false;
            Category = category;
        }

        public virtual string GetDescription()
        {
            return $"{Title} ({Category})";
        }

        public virtual int GetPriority()
        {
            return 1; // Default priority
        }
    }

    // Abstract decorator
    public abstract class TodoItemDecorator : ITodoItemComponent
    {
        protected ITodoItemComponent _component;

        public TodoItemDecorator(ITodoItemComponent component)
        {
            _component = component;
        }

        public virtual string Title
        {
            get => _component.Title;
            set => _component.Title = value;
        }

        public virtual bool Completed
        {
            get => _component.Completed;
            set => _component.Completed = value;
        }

        public abstract string GetDescription();
        public abstract int GetPriority();
    }

    // Concrete decorator for Priority
    public class PriorityDecorator : TodoItemDecorator
    {
        private int _priority;

        public PriorityDecorator(ITodoItemComponent component, int priority) : base(component)
        {
            _priority = priority;
        }

        public override string GetDescription()
        {
            string priorityText = _priority switch
            {
                >= 5 => "URGENT: ",
                >= 3 => "High: ",
                >= 2 => "Medium: ",
                _ => ""
            };
            return $"{priorityText}{_component.GetDescription()}";
        }

        public override int GetPriority()
        {
            return _priority;
        }
    }

    // Concrete decorator for Due Date
    public class DueDateDecorator : TodoItemDecorator
    {
        private DateTime _dueDate;

        public DueDateDecorator(ITodoItemComponent component, DateTime dueDate) : base(component)
        {
            _dueDate = dueDate;
        }

        public override string GetDescription()
        {
            string dueDateText = _dueDate < DateTime.Now ? "OVERDUE: " : 
                                _dueDate.Date == DateTime.Now.Date ? "DUE TODAY: " :
                                $"Due {_dueDate:MM/dd}: ";
            return $"{dueDateText}{_component.GetDescription()}";
        }

        public override int GetPriority()
        {
            // Higher priority for overdue items
            if (_dueDate < DateTime.Now)
                return _component.GetPriority() + 3;
            if (_dueDate.Date == DateTime.Now.Date)
                return _component.GetPriority() + 2;
            return _component.GetPriority();
        }

        public DateTime DueDate => _dueDate;
    }

    // Decorator factory for easy creation
    public static class TodoItemDecoratorFactory
    {
        public static ITodoItemComponent CreatePriorityItem(string title, int priority, string category = "General")
        {
            var baseItem = new TodoItemBase(title, category);
            return new PriorityDecorator(baseItem, priority);
        }

        public static ITodoItemComponent CreateDueDateItem(string title, DateTime dueDate, string category = "General")
        {
            var baseItem = new TodoItemBase(title, category);
            return new DueDateDecorator(baseItem, dueDate);
        }

        public static ITodoItemComponent CreateComplexItem(string title, int priority, DateTime dueDate, string category = "General")
        {
            var baseItem = new TodoItemBase(title, category);
            var priorityItem = new PriorityDecorator(baseItem, priority);
            return new DueDateDecorator(priorityItem, dueDate);
        }
    }
}