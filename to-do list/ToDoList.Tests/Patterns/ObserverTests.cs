using to_do_list;
using to_do_list.Patterns;
using Moq;

namespace ToDoList.Tests.Patterns
{
    public class ObserverTests
    {
        // --- TodoListSelectionManager Tests ---

        [Fact]
        public void SelectionManager_InitialState_HasNullSelection()
        {
            // Arrange
            var manager = new TodoListSelectionManager();

            // Act
            var state = manager.CurrentState;

            // Assert
            Assert.Null(state.SelectedItem);
            Assert.False(state.IsIncompleteListSelected);
            Assert.False(state.IsCompleteListSelected);
        }

        [Fact]
        public void SelectionManager_AttachObserver_AddsToList()
        {
            // Arrange
            var manager = new TodoListSelectionManager();
            var mockObserver = new Mock<IUIObserver>();

            // Act
            manager.Attach(mockObserver.Object);

            // Assert
            // Verify that notification works (observer was attached)
            manager.UpdateSelection(new TodoItem { Title = "Test" }, true, false);
            mockObserver.Verify(o => o.Update(It.IsAny<TodoListSelectionState>()), Times.Once);
        }

        [Fact]
        public void SelectionManager_DetachObserver_RemovesFromList()
        {
            // Arrange
            var manager = new TodoListSelectionManager();
            var mockObserver = new Mock<IUIObserver>();
            manager.Attach(mockObserver.Object);
            manager.Detach(mockObserver.Object);

            // Act
            manager.UpdateSelection(new TodoItem { Title = "Test" }, true, false);

            // Assert
            mockObserver.Verify(o => o.Update(It.IsAny<TodoListSelectionState>()), Times.Never);
        }

        [Fact]
        public void SelectionManager_UpdateSelection_NotifiesObservers()
        {
            // Arrange
            var manager = new TodoListSelectionManager();
            var mockObserver = new Mock<IUIObserver>();
            manager.Attach(mockObserver.Object);

            // Act
            manager.UpdateSelection(new TodoItem { Title = "Selected" }, true, false);

            // Assert
            mockObserver.Verify(o => o.Update(It.IsAny<TodoListSelectionState>()), Times.Once);
        }

        [Fact]
        public void SelectionManager_ClearSelection_NotifiesObservers()
        {
            // Arrange
            var manager = new TodoListSelectionManager();
            var mockObserver = new Mock<IUIObserver>();
            manager.Attach(mockObserver.Object);

            // Act
            manager.ClearSelection();

            // Assert
            mockObserver.Verify(o => o.Update(It.IsAny<TodoListSelectionState>()), Times.Once);
        }

        [Fact]
        public void SelectionManager_MultipleObservers_AllNotified()
        {
            // Arrange
            var manager = new TodoListSelectionManager();
            var mockObserver1 = new Mock<IUIObserver>();
            var mockObserver2 = new Mock<IUIObserver>();
            var mockObserver3 = new Mock<IUIObserver>();
            manager.Attach(mockObserver1.Object);
            manager.Attach(mockObserver2.Object);
            manager.Attach(mockObserver3.Object);

            // Act
            manager.UpdateSelection(new TodoItem { Title = "Test" }, false, true);

            // Assert
            mockObserver1.Verify(o => o.Update(It.IsAny<TodoListSelectionState>()), Times.Once);
            mockObserver2.Verify(o => o.Update(It.IsAny<TodoListSelectionState>()), Times.Once);
            mockObserver3.Verify(o => o.Update(It.IsAny<TodoListSelectionState>()), Times.Once);
        }

        [Fact]
        public void SelectionManager_UpdateSelection_SetsCorrectState()
        {
            // Arrange
            var manager = new TodoListSelectionManager();
            TodoListSelectionState capturedState = null;
            var mockObserver = new Mock<IUIObserver>();
            mockObserver.Setup(o => o.Update(It.IsAny<TodoListSelectionState>()))
                .Callback<TodoListSelectionState>(s => capturedState = s);
            manager.Attach(mockObserver.Object);

            var item = new TodoItem { Title = "Test Item", Completed = true };

            // Act
            manager.UpdateSelection(item, false, true);

            // Assert
            Assert.NotNull(capturedState);
            Assert.Same(item, capturedState.SelectedItem);
            Assert.False(capturedState.IsIncompleteListSelected);
            Assert.True(capturedState.IsCompleteListSelected);
        }

        [Fact]
        public void SelectionManager_ClearSelection_ResetsState()
        {
            // Arrange
            var manager = new TodoListSelectionManager();
            TodoListSelectionState capturedState = null;
            var mockObserver = new Mock<IUIObserver>();
            mockObserver.Setup(o => o.Update(It.IsAny<TodoListSelectionState>()))
                .Callback<TodoListSelectionState>(s => capturedState = s);
            manager.Attach(mockObserver.Object);

            // Pre-set state
            manager.UpdateSelection(new TodoItem { Title = "Test" }, true, false);

            // Act
            manager.ClearSelection();

            // Assert
            Assert.NotNull(capturedState);
            Assert.Null(capturedState.SelectedItem);
            Assert.False(capturedState.IsIncompleteListSelected);
            Assert.False(capturedState.IsCompleteListSelected);
        }

        [Fact]
        public void SelectionManager_Attach_DuplicateObserver_DoesNotAddTwice()
        {
            // Arrange
            var manager = new TodoListSelectionManager();
            var mockObserver = new Mock<IUIObserver>();

            // Act
            manager.Attach(mockObserver.Object);
            manager.Attach(mockObserver.Object);
            manager.UpdateSelection(new TodoItem { Title = "Test" }, true, false);

            // Assert - should only be called once because duplicate isn't added
            mockObserver.Verify(o => o.Update(It.IsAny<TodoListSelectionState>()), Times.Once);
        }

        // --- TodoListSelectionState Tests ---

        [Fact]
        public void SelectionState_Constructor_SetsDefaults()
        {
            // Arrange & Act
            var state = new TodoListSelectionState();

            // Assert
            Assert.Null(state.SelectedItem);
            Assert.False(state.IsIncompleteListSelected);
            Assert.False(state.IsCompleteListSelected);
        }

        [Fact]
        public void SelectionState_CanSetProperties()
        {
            // Arrange
            var state = new TodoListSelectionState();
            var item = new TodoItem { Title = "Item" };

            // Act
            state.SelectedItem = item;
            state.IsIncompleteListSelected = true;
            state.IsCompleteListSelected = false;

            // Assert
            Assert.Same(item, state.SelectedItem);
            Assert.True(state.IsIncompleteListSelected);
            Assert.False(state.IsCompleteListSelected);
        }

        // --- ButtonTextObserver Tests (using Moq for Button) ---

        [Fact]
        public void ButtonTextObserver_Constructor_NullButton_Throws()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ButtonTextObserver(null));
        }

        [Fact]
        public void ButtonTextObserver_WithIncompleteSelection_SetsCorrectText()
        {
            // Arrange
            var button = new Button();
            var observer = new ButtonTextObserver(button);
            var state = new TodoListSelectionState
            {
                SelectedItem = new TodoItem { Title = "Task" },
                IsIncompleteListSelected = true,
                IsCompleteListSelected = false
            };

            // Act
            observer.Update(state);

            // Assert
            Assert.Equal("Mark as Complete", button.Text);
            Assert.True(button.Enabled);
        }

        [Fact]
        public void ButtonTextObserver_WithCompleteSelection_SetsCorrectText()
        {
            // Arrange
            var button = new Button();
            var observer = new ButtonTextObserver(button);
            var state = new TodoListSelectionState
            {
                SelectedItem = new TodoItem { Title = "Task" },
                IsIncompleteListSelected = false,
                IsCompleteListSelected = true
            };

            // Act
            observer.Update(state);

            // Assert
            Assert.Equal("Mark as Incomplete", button.Text);
            Assert.True(button.Enabled);
        }

        [Fact]
        public void ButtonTextObserver_WithNoSelection_DisablesButton()
        {
            // Arrange
            var button = new Button { Enabled = true, Text = "Previous" };
            var observer = new ButtonTextObserver(button);
            var state = new TodoListSelectionState();

            // Act
            observer.Update(state);

            // Assert
            Assert.Equal("Toggle Status", button.Text);
            Assert.False(button.Enabled);
        }

        // --- CommandUIObserver Tests (using real WinForms controls) ---

        [Fact]
        public void CommandUIObserver_Constructor_NullLabels_Throws()
        {
            // Arrange
            var label = new Label();
            var button = new Button();
            var manager = new TodoCommandManager();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new CommandUIObserver(null, label, button, button, manager));
            Assert.Throws<ArgumentNullException>(() => new CommandUIObserver(label, null, button, button, manager));
            Assert.Throws<ArgumentNullException>(() => new CommandUIObserver(label, label, null, button, manager));
            Assert.Throws<ArgumentNullException>(() => new CommandUIObserver(label, label, button, null, manager));
            Assert.Throws<ArgumentNullException>(() => new CommandUIObserver(label, label, button, button, null));
        }

        [Fact]
        public void CommandUIObserver_Update_ReflectsCommandManagerState()
        {
            // Arrange
            var undoCountLabel = new Label();
            var redoCountLabel = new Label();
            var undoButton = new Button { Enabled = false };
            var redoButton = new Button { Enabled = false };
            var commandManager = new TodoCommandManager();
            var observer = new CommandUIObserver(undoCountLabel, redoCountLabel, undoButton, redoButton, commandManager);

            // Add a command to the manager
            var list = new List<TodoItem>();
            commandManager.ExecuteCommand(new AddTodoCommand(list, new TodoItem { Title = "Test" }));

            var state = new TodoListSelectionState();

            // Act
            observer.Update(state);

            // Assert
            Assert.Equal("1", undoCountLabel.Text);
            Assert.Equal("0", redoCountLabel.Text);
            Assert.True(undoButton.Enabled);
            Assert.False(redoButton.Enabled);
        }

        // --- StatusLabelObserver Tests ---

        [Fact]
        public void StatusLabelObserver_Constructor_NullLabel_Throws()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new StatusLabelObserver(null));
        }

        [Fact]
        public void StatusLabelObserver_WithSelected_Incomplete_ShowsCorrectStatus()
        {
            // Arrange
            var label = new Label();
            var observer = new StatusLabelObserver(label);
            var state = new TodoListSelectionState
            {
                SelectedItem = new TodoItem { Title = "My Task", Completed = false }
            };

            // Act
            observer.Update(state);

            // Assert
            Assert.Equal("Selected: My Task (Incomplete)", label.Text);
        }

        [Fact]
        public void StatusLabelObserver_WithSelected_Completed_ShowsCorrectStatus()
        {
            // Arrange
            var label = new Label();
            var observer = new StatusLabelObserver(label);
            var state = new TodoListSelectionState
            {
                SelectedItem = new TodoItem { Title = "Done", Completed = true }
            };

            // Act
            observer.Update(state);

            // Assert
            Assert.Equal("Selected: Done (Completed)", label.Text);
        }

        [Fact]
        public void StatusLabelObserver_WithNoSelection_ShowsDefaultText()
        {
            // Arrange
            var label = new Label();
            var observer = new StatusLabelObserver(label);
            var state = new TodoListSelectionState();

            // Act
            observer.Update(state);

            // Assert
            Assert.Equal("No item selected", label.Text);
        }

        // --- UIObserverFactory Tests ---

        [Fact]
        public void Factory_CreateButtonTextObserver_ReturnsCorrectType()
        {
            // Arrange
            var button = new Button();

            // Act
            var observer = UIObserverFactory.CreateButtonTextObserver(button);

            // Assert
            Assert.IsType<ButtonTextObserver>(observer);
        }

        [Fact]
        public void Factory_CreateCommandUIObserver_ReturnsCorrectType()
        {
            // Arrange
            var label = new Label();
            var button = new Button();
            var manager = new TodoCommandManager();

            // Act
            var observer = UIObserverFactory.CreateCommandUIObserver(label, label, button, button, manager);

            // Assert
            Assert.IsType<CommandUIObserver>(observer);
        }

        [Fact]
        public void Factory_CreateStatusLabelObserver_ReturnsCorrectType()
        {
            // Arrange
            var label = new Label();

            // Act
            var observer = UIObserverFactory.CreateStatusLabelObserver(label);

            // Assert
            Assert.IsType<StatusLabelObserver>(observer);
        }
    }
}