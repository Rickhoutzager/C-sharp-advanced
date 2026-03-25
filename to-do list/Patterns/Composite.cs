using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace to_do_list.Patterns
{
    // Abstract base component for the Composite pattern
    public abstract class TodoComponent
    {
        public string Title { get; set; }
        public bool Completed { get; set; }
        public int Priority { get; set; }
        public string Category { get; set; }
        public DateTime? DueDate { get; set; }
        public TodoComponent? Parent { get; set; }

        protected TodoComponent(string title, int priority = 2, string category = "General", DateTime? dueDate = null)
        {
            Title = title;
            Priority = priority;
            Category = category;
            DueDate = dueDate;
            Completed = false;
        }

        // Composite operations - implemented differently by leaves and composites
        public abstract void AddChild(TodoComponent component);
        public abstract void RemoveChild(TodoComponent component);
        public abstract List<TodoComponent> GetChildren();
        public abstract bool IsLeaf();

        // Common operations
        public abstract string Display(int depth = 0);
        
        // Completion operations with propagation for composites
        public virtual void SetCompleted(bool completed)
        {
            Completed = completed;
            OnCompletionChanged();
        }

        protected virtual void OnCompletionChanged()
        {
            // Default implementation - can be overridden by composites
        }

        // Priority operations with inheritance for leaves
        public virtual void SetPriority(int priority)
        {
            Priority = priority;
            OnPriorityChanged();
        }

        protected virtual void OnPriorityChanged()
        {
            // Default implementation - can be overridden by composites
        }

        // Category operations with inheritance for leaves
        public virtual void SetCategory(string category)
        {
            Category = category;
            OnCategoryChanged();
        }

        protected virtual void OnCategoryChanged()
        {
            // Default implementation - can be overridden by composites
        }

        // Due date operations
        public virtual void SetDueDate(DateTime? dueDate)
        {
            DueDate = dueDate;
            OnDueDateChanged();
        }

        protected virtual void OnDueDateChanged()
        {
            // Default implementation - can be overridden by composites
        }

        // Statistics
        public abstract int GetTotalTasks();
        public abstract int GetCompletedTasks();
        public abstract double GetCompletionPercentage();
    }

    // Leaf component - individual todo items
    public class TodoLeaf : TodoComponent
    {
        private TodoItem _todoItem;

        public TodoLeaf(TodoItem todoItem) : base(todoItem.Title, todoItem.Priority, todoItem.Category, todoItem.DueDate)
        {
            _todoItem = todoItem;
            Completed = todoItem.Completed;
        }

        public TodoItem GetTodoItem()
        {
            return _todoItem;
        }

        public override void AddChild(TodoComponent component)
        {
            throw new InvalidOperationException("Cannot add children to a leaf component");
        }

        public override void RemoveChild(TodoComponent component)
        {
            throw new InvalidOperationException("Cannot remove children from a leaf component");
        }

        public override List<TodoComponent> GetChildren()
        {
            return new List<TodoComponent>();
        }

        public override bool IsLeaf()
        {
            return true;
        }

        public override string Display(int depth = 0)
        {
            var indent = new string(' ', depth * 2);
            var status = Completed ? "✓" : "○";
            var priorityText = GetPriorityText(Priority);
            
            var builder = new StringBuilder();
            builder.Append($"{indent}{status} {Title}");
            
            if (!string.IsNullOrEmpty(Category))
                builder.Append($" [{Category}]");
            
            if (Priority > 0)
                builder.Append($" (Priority: {priorityText})");
            
            if (DueDate.HasValue)
                builder.Append($" - Due: {DueDate.Value:MM/dd}");
            
            return builder.ToString();
        }

        public override int GetTotalTasks()
        {
            return 1;
        }

        public override int GetCompletedTasks()
        {
            return Completed ? 1 : 0;
        }

        public override double GetCompletionPercentage()
        {
            return Completed ? 100.0 : 0.0;
        }

        private string GetPriorityText(int priority)
        {
            return priority switch
            {
                1 => "Low",
                2 => "Medium",
                3 => "High",
                4 => "Very High",
                5 => "Critical",
                _ => "Unknown"
            };
        }
    }

    // Composite component - projects/groups containing subtasks
    public class TodoComposite : TodoComponent
    {
        private List<TodoComponent> _children = new List<TodoComponent>();

        public bool IsComplete => _children.All(child => child.Completed || (child is TodoComposite composite && composite.IsComplete));

        public TodoComposite(string title, int priority = 2, string category = "General", DateTime? dueDate = null) 
            : base(title, priority, category, dueDate)
        {
        }

        public override void AddChild(TodoComponent component)
        {
            if (component == null) return;
            
            // Set parent relationship
            component.Parent = this;
            
            // Inherit properties from parent if not set
            if (component.Priority == 0) component.SetPriority(Priority);
            if (string.IsNullOrEmpty(component.Category)) component.SetCategory(Category);
            if (!component.DueDate.HasValue) component.SetDueDate(DueDate);
            
            _children.Add(component);
        }

        public override void RemoveChild(TodoComponent component)
        {
            if (component == null) return;
            
            // Remove parent relationship
            component.Parent = null;
            _children.Remove(component);
        }

        public override List<TodoComponent> GetChildren()
        {
            return new List<TodoComponent>(_children);
        }

        public override bool IsLeaf()
        {
            return false;
        }

        public override string Display(int depth = 0)
        {
            var indent = new string(' ', depth * 2);
            var status = Completed ? "✓" : "○";
            var priorityText = GetPriorityText(Priority);
            var completion = GetCompletionPercentage();
            
            var builder = new StringBuilder();
            builder.Append($"{indent}{status} {Title}");
            
            if (!string.IsNullOrEmpty(Category))
                builder.Append($" [{Category}]");
            
            if (Priority > 0)
                builder.Append($" (Priority: {priorityText})");
            
            if (DueDate.HasValue)
                builder.Append($" - Due: {DueDate.Value:MM/dd}");
            
            builder.Append($" - {completion:F1}% complete");
            
            // Display children
            foreach (var child in _children)
            {
                builder.AppendLine();
                builder.Append(child.Display(depth + 1));
            }
            
            return builder.ToString();
        }

        public override void SetCompleted(bool completed)
        {
            base.SetCompleted(completed);
            
            // Propagate completion to all children
            foreach (var child in _children)
            {
                child.SetCompleted(completed);
            }
        }

        public override void SetPriority(int priority)
        {
            base.SetPriority(priority);
            
            // Propagate priority to all children
            foreach (var child in _children)
            {
                child.SetPriority(priority);
            }
        }

        public override void SetCategory(string category)
        {
            base.SetCategory(category);
            
            // Propagate category to all children
            foreach (var child in _children)
            {
                child.SetCategory(category);
            }
        }

        public override void SetDueDate(DateTime? dueDate)
        {
            base.SetDueDate(dueDate);
            
            // Propagate due date to all children
            foreach (var child in _children)
            {
                child.SetDueDate(dueDate);
            }
        }

        public override int GetTotalTasks()
        {
            return _children.Sum(child => child.GetTotalTasks());
        }

        public override int GetCompletedTasks()
        {
            return _children.Sum(child => child.GetCompletedTasks());
        }

        public override double GetCompletionPercentage()
        {
            var totalTasks = GetTotalTasks();
            if (totalTasks == 0) return 0.0;
            
            var completedTasks = GetCompletedTasks();
            return (completedTasks / (double)totalTasks) * 100.0;
        }

        private string GetPriorityText(int priority)
        {
            return priority switch
            {
                1 => "Low",
                2 => "Medium",
                3 => "High",
                4 => "Very High",
                5 => "Critical",
                _ => "Unknown"
            };
        }

        // Project-specific operations
        public void CompleteAll()
        {
            SetCompleted(true);
        }

        public void IncompleteAll()
        {
            SetCompleted(false);
        }

        public void DeleteAll()
        {
            _children.Clear();
        }

        public List<TodoLeaf> GetAllLeaves()
        {
            var leaves = new List<TodoLeaf>();
            CollectLeaves(this, leaves);
            return leaves;
        }

        private void CollectLeaves(TodoComponent component, List<TodoLeaf> leaves)
        {
            if (component.IsLeaf())
            {
                leaves.Add((TodoLeaf)component);
            }
            else
            {
                foreach (var child in component.GetChildren())
                {
                    CollectLeaves(child, leaves);
                }
            }
        }
    }

    // Factory for creating composite todo structures
    public static class TodoCompositeFactory
    {
        public static TodoComposite CreateProject(string title, int priority = 3, string category = "Work", DateTime? dueDate = null)
        {
            return new TodoComposite(title, priority, category, dueDate);
        }

        public static TodoLeaf CreateTask(TodoItem todoItem)
        {
            return new TodoLeaf(todoItem);
        }

        public static TodoComposite CreateWorkProject(string title, DateTime? dueDate = null)
        {
            return new TodoComposite(title, 3, "Work", dueDate);
        }

        public static TodoComposite CreatePersonalProject(string title, DateTime? dueDate = null)
        {
            return new TodoComposite(title, 2, "Personal", dueDate);
        }

        public static TodoComposite CreateHomeProject(string title, DateTime? dueDate = null)
        {
            return new TodoComposite(title, 2, "Home", dueDate);
        }
    }
}