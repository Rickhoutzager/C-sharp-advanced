using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace to_do_list.Patterns
{
    /// <summary>
    /// Observer Pattern Implementation for UI Updates
    /// Provides a clean way to observe changes in todo list selection and update UI elements accordingly
    /// </summary>
    
    // Observer Interface
    public interface IUIObserver
    {
        void Update(TodoListSelectionState state);
    }

    // Subject Interface
    public interface ITodoListSubject
    {
        void Attach(IUIObserver observer);
        void Detach(IUIObserver observer);
        void Notify();
    }

    // Selection State
    public class TodoListSelectionState
    {
        public TodoItem SelectedItem { get; set; }
        public bool IsIncompleteListSelected { get; set; }
        public bool IsCompleteListSelected { get; set; }
        
        public TodoListSelectionState()
        {
            SelectedItem = null;
            IsIncompleteListSelected = false;
            IsCompleteListSelected = false;
        }
    }

    // Concrete Subject - Todo List Selection Manager
    public class TodoListSelectionManager : ITodoListSubject
    {
        private List<IUIObserver> _observers = new List<IUIObserver>();
        private TodoListSelectionState _currentState = new TodoListSelectionState();

        public TodoListSelectionState CurrentState => _currentState;

        public void Attach(IUIObserver observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
        }

        public void Detach(IUIObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (var observer in _observers)
            {
                observer.Update(_currentState);
            }
        }

        // Update selection state and notify observers
        public void UpdateSelection(TodoItem selectedItem, bool isIncompleteList, bool isCompleteList)
        {
            _currentState.SelectedItem = selectedItem;
            _currentState.IsIncompleteListSelected = isIncompleteList;
            _currentState.IsCompleteListSelected = isCompleteList;
            
            Notify();
        }

        // Clear selection
        public void ClearSelection()
        {
            _currentState.SelectedItem = null;
            _currentState.IsIncompleteListSelected = false;
            _currentState.IsCompleteListSelected = false;
            
            Notify();
        }
    }

    // Concrete Observer - Button Text Updater
    public class ButtonTextObserver : IUIObserver
    {
        private Button _toggleButton;

        public ButtonTextObserver(Button toggleButton)
        {
            _toggleButton = toggleButton ?? throw new ArgumentNullException(nameof(toggleButton));
        }

        public void Update(TodoListSelectionState state)
        {
            if (state.SelectedItem != null)
            {
                if (state.IsIncompleteListSelected)
                {
                    _toggleButton.Text = "Mark as Complete";
                    _toggleButton.Enabled = true;
                }
                else if (state.IsCompleteListSelected)
                {
                    _toggleButton.Text = "Mark as Incomplete";
                    _toggleButton.Enabled = true;
                }
            }
            else
            {
                _toggleButton.Text = "Toggle Status";
                _toggleButton.Enabled = false;
            }
        }
    }

    // Concrete Observer - Command UI Updater
    public class CommandUIObserver : IUIObserver
    {
        private Label _undoCountLabel;
        private Label _redoCountLabel;
        private Button _undoButton;
        private Button _redoButton;
        private TodoCommandManager _commandManager;

        public CommandUIObserver(
            Label undoCountLabel, 
            Label redoCountLabel,
            Button undoButton,
            Button redoButton,
            TodoCommandManager commandManager)
        {
            _undoCountLabel = undoCountLabel ?? throw new ArgumentNullException(nameof(undoCountLabel));
            _redoCountLabel = redoCountLabel ?? throw new ArgumentNullException(nameof(redoCountLabel));
            _undoButton = undoButton ?? throw new ArgumentNullException(nameof(undoButton));
            _redoButton = redoButton ?? throw new ArgumentNullException(nameof(redoButton));
            _commandManager = commandManager ?? throw new ArgumentNullException(nameof(commandManager));
        }

        public void Update(TodoListSelectionState state)
        {
            // Update command UI based on command manager state
            _undoCountLabel.Text = _commandManager.UndoCount.ToString();
            _redoCountLabel.Text = _commandManager.RedoCount.ToString();
            
            _undoButton.Enabled = _commandManager.CanUndo;
            _redoButton.Enabled = _commandManager.CanRedo;
        }
    }

    // Concrete Observer - Status Label Updater
    public class StatusLabelObserver : IUIObserver
    {
        private Label _statusLabel;

        public StatusLabelObserver(Label statusLabel)
        {
            _statusLabel = statusLabel ?? throw new ArgumentNullException(nameof(statusLabel));
        }

        public void Update(TodoListSelectionState state)
        {
            if (state.SelectedItem != null)
            {
                var status = state.SelectedItem.Completed ? "Completed" : "Incomplete";
                _statusLabel.Text = $"Selected: {state.SelectedItem.Title} ({status})";
            }
            else
            {
                _statusLabel.Text = "No item selected";
            }
        }
    }

    // Observer Factory for easy creation
    public static class UIObserverFactory
    {
        public static ButtonTextObserver CreateButtonTextObserver(Button toggleButton)
        {
            return new ButtonTextObserver(toggleButton);
        }

        public static CommandUIObserver CreateCommandUIObserver(
            Label undoCountLabel,
            Label redoCountLabel,
            Button undoButton,
            Button redoButton,
            TodoCommandManager commandManager)
        {
            return new CommandUIObserver(undoCountLabel, redoCountLabel, undoButton, redoButton, commandManager);
        }

        public static StatusLabelObserver CreateStatusLabelObserver(Label statusLabel)
        {
            return new StatusLabelObserver(statusLabel);
        }
    }
}