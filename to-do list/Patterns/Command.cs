using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace to_do_list.Patterns
{
    // Command interface
    public interface ITodoCommand
    {
        void Execute();
        void Undo();
        void Redo();
        string GetDescription();
        bool CanUndo { get; }
        bool CanRedo { get; }
    }

    // Base command class with common functionality
    public abstract class TodoCommandBase : ITodoCommand
    {
        public abstract void Execute();
        public abstract void Undo();
        public abstract void Redo();
        public abstract string GetDescription();

        public bool CanUndo { get; protected set; } = true;
        public bool CanRedo { get; protected set; } = false;

        protected void MarkAsExecuted()
        {
            CanUndo = true;
            CanRedo = false;
        }

        protected void MarkAsUndone()
        {
            CanUndo = false;
            CanRedo = true;
        }

        protected void MarkAsRedone()
        {
            CanUndo = true;
            CanRedo = false;
        }
    }

    // Concrete Command - Add Todo Item
    public class AddTodoCommand : TodoCommandBase
    {
        private readonly List<TodoItem> _todoList;
        private readonly TodoItem _itemToAdd;
        private int _insertIndex;

        public AddTodoCommand(List<TodoItem> todoList, TodoItem itemToAdd)
        {
            _todoList = todoList;
            _itemToAdd = itemToAdd;
        }

        public override void Execute()
        {
            _insertIndex = _todoList.Count;
            _todoList.Add(_itemToAdd);
            MarkAsExecuted();
        }

        public override void Undo()
        {
            if (_insertIndex >= 0 && _insertIndex < _todoList.Count)
            {
                _todoList.RemoveAt(_insertIndex);
                MarkAsUndone();
            }
        }

        public override void Redo()
        {
            if (_insertIndex >= 0)
            {
                _todoList.Insert(_insertIndex, _itemToAdd);
                MarkAsRedone();
            }
        }

        public override string GetDescription()
        {
            return $"Add: {_itemToAdd.Title}";
        }
    }

    // Concrete Command - Remove Todo Item
    public class RemoveTodoCommand : TodoCommandBase
    {
        private readonly List<TodoItem> _todoList;
        private readonly TodoItem _itemToRemove;
        private int _removeIndex;

        public RemoveTodoCommand(List<TodoItem> todoList, TodoItem itemToRemove)
        {
            _todoList = todoList;
            _itemToRemove = itemToRemove;
        }

        public override void Execute()
        {
            _removeIndex = _todoList.IndexOf(_itemToRemove);
            if (_removeIndex >= 0)
            {
                _todoList.RemoveAt(_removeIndex);
                MarkAsExecuted();
            }
        }

        public override void Undo()
        {
            if (_removeIndex >= 0)
            {
                _todoList.Insert(_removeIndex, _itemToRemove);
                MarkAsUndone();
            }
        }

        public override void Redo()
        {
            if (_removeIndex >= 0 && _removeIndex < _todoList.Count)
            {
                _todoList.RemoveAt(_removeIndex);
                MarkAsRedone();
            }
        }

        public override string GetDescription()
        {
            return $"Remove: {_itemToRemove.Title}";
        }
    }

    // Concrete Command - Toggle Complete
    public class ToggleCompleteCommand : TodoCommandBase
    {
        private readonly TodoItem _item;
        private bool _originalState;

        public ToggleCompleteCommand(TodoItem item)
        {
            _item = item;
        }

        public override void Execute()
        {
            _originalState = _item.Completed;
            _item.Completed = !_item.Completed;
            MarkAsExecuted();
        }

        public override void Undo()
        {
            _item.Completed = _originalState;
            MarkAsUndone();
        }

        public override void Redo()
        {
            _item.Completed = !_originalState;
            MarkAsRedone();
        }

        public override string GetDescription()
        {
            return $"{(_item.Completed ? "Complete" : "Incomplete")}: {_item.Title}";
        }
    }

    // Concrete Command - Update Todo Item
    public class UpdateTodoCommand : TodoCommandBase
    {
        private readonly List<TodoItem> _todoList;
        private readonly TodoItem _originalItem;
        private readonly TodoItem _updatedItem;
        private int _updateIndex;

        public UpdateTodoCommand(List<TodoItem> todoList, TodoItem originalItem, TodoItem updatedItem)
        {
            _todoList = todoList;
            _originalItem = originalItem;
            _updatedItem = updatedItem;
        }

        public override void Execute()
        {
            _updateIndex = _todoList.IndexOf(_originalItem);
            if (_updateIndex >= 0)
            {
                _todoList[_updateIndex] = _updatedItem;
                MarkAsExecuted();
            }
        }

        public override void Undo()
        {
            if (_updateIndex >= 0 && _updateIndex < _todoList.Count)
            {
                _todoList[_updateIndex] = _originalItem;
                MarkAsUndone();
            }
        }

        public override void Redo()
        {
            if (_updateIndex >= 0 && _updateIndex < _todoList.Count)
            {
                _todoList[_updateIndex] = _updatedItem;
                MarkAsRedone();
            }
        }

        public override string GetDescription()
        {
            return $"Update: {_originalItem.Title}";
        }
    }

    // Command Manager - Invoker
    public class TodoCommandManager
    {
        private readonly Stack<ITodoCommand> _undoStack = new Stack<ITodoCommand>();
        private readonly Stack<ITodoCommand> _redoStack = new Stack<ITodoCommand>();
        private readonly List<Action> _commandExecutedCallbacks = new List<Action>();

        public int UndoCount => _undoStack.Count;
        public int RedoCount => _redoStack.Count;

        public void ExecuteCommand(ITodoCommand command)
        {
            command.Execute();
            _undoStack.Push(command);
            _redoStack.Clear(); // Clear redo stack when new command is executed
            
            // Notify listeners
            foreach (var callback in _commandExecutedCallbacks)
            {
                callback?.Invoke();
            }
        }

        public void Undo()
        {
            if (_undoStack.Count > 0)
            {
                var command = _undoStack.Pop();
                command.Undo();
                _redoStack.Push(command);
            }
        }

        public void Redo()
        {
            if (_redoStack.Count > 0)
            {
                var command = _redoStack.Pop();
                command.Redo();
                _undoStack.Push(command);
            }
        }

        public bool CanUndo => _undoStack.Count > 0;
        public bool CanRedo => _redoStack.Count > 0;

        public string GetUndoDescription()
        {
            return _undoStack.Count > 0 ? _undoStack.Peek().GetDescription() : "No actions to undo";
        }

        public string GetRedoDescription()
        {
            return _redoStack.Count > 0 ? _redoStack.Peek().GetDescription() : "No actions to redo";
        }

        public void ClearHistory()
        {
            _undoStack.Clear();
            _redoStack.Clear();
        }

        public void AddCommandExecutedCallback(Action callback)
        {
            _commandExecutedCallbacks.Add(callback);
        }

        public void RemoveCommandExecutedCallback(Action callback)
        {
            _commandExecutedCallbacks.Remove(callback);
        }

        public List<string> GetCommandHistory()
        {
            var history = new List<string>();
            
            // Add redo commands (in reverse order)
            var redoList = _redoStack.ToList();
            redoList.Reverse();
            foreach (var command in redoList)
            {
                history.Add($"[Redo] {command.GetDescription()}");
            }
            
            // Add current state marker
            history.Add("--- CURRENT STATE ---");
            
            // Add undo commands
            var undoList = _undoStack.ToList();
            undoList.Reverse();
            foreach (var command in undoList)
            {
                history.Add($"[Undo] {command.GetDescription()}");
            }
            
            return history;
        }
    }

    // Command Factory for easy creation
    public static class TodoCommandFactory
    {
        public static ITodoCommand CreateAddCommand(List<TodoItem> todoList, TodoItem item)
        {
            return new AddTodoCommand(todoList, item);
        }

        public static ITodoCommand CreateRemoveCommand(List<TodoItem> todoList, TodoItem item)
        {
            return new RemoveTodoCommand(todoList, item);
        }

        public static ITodoCommand CreateToggleCommand(TodoItem item)
        {
            return new ToggleCompleteCommand(item);
        }

        public static ITodoCommand CreateUpdateCommand(List<TodoItem> todoList, TodoItem original, TodoItem updated)
        {
            return new UpdateTodoCommand(todoList, original, updated);
        }
    }
}