using to_do_list;
using to_do_list.Patterns;

namespace ToDoList.Tests.Patterns
{
    public class CommandTests
    {
        // --- AddTodoCommand Tests ---

        [Fact]
        public void AddTodoCommand_Execute_AddsItemToList()
        {
            // Arrange
            var list = new List<TodoItem>();
            var item = new TodoItem { Title = "New Task" };
            var command = new AddTodoCommand(list, item);

            // Act
            command.Execute();

            // Assert
            Assert.Single(list);
            Assert.Equal("New Task", list[0].Title);
        }

        [Fact]
        public void AddTodoCommand_Undo_RemovesAddedItem()
        {
            // Arrange
            var list = new List<TodoItem>();
            var item = new TodoItem { Title = "New Task" };
            var command = new AddTodoCommand(list, item);
            command.Execute();

            // Act
            command.Undo();

            // Assert
            Assert.Empty(list);
        }

        [Fact]
        public void AddTodoCommand_Redo_ReaddsItem()
        {
            // Arrange
            var list = new List<TodoItem>();
            var item = new TodoItem { Title = "New Task" };
            var command = new AddTodoCommand(list, item);
            command.Execute();
            command.Undo();

            // Act
            command.Redo();

            // Assert
            Assert.Single(list);
            Assert.Equal("New Task", list[0].Title);
        }

        [Fact]
        public void AddTodoCommand_GetDescription_ReturnsDescription()
        {
            // Arrange
            var list = new List<TodoItem>();
            var item = new TodoItem { Title = "Test Item" };
            var command = new AddTodoCommand(list, item);

            // Act
            var desc = command.GetDescription();

            // Assert
            Assert.Equal("Add: Test Item", desc);
        }

        [Fact]
        public void AddTodoCommand_AfterExecute_CanUndoIsTrue_CanRedoIsFalse()
        {
            // Arrange
            var list = new List<TodoItem>();
            var command = new AddTodoCommand(list, new TodoItem { Title = "Item" });

            // Act
            command.Execute();

            // Assert
            Assert.True(command.CanUndo);
            Assert.False(command.CanRedo);
        }

        // --- RemoveTodoCommand Tests ---

        [Fact]
        public void RemoveTodoCommand_Execute_RemovesItemFromList()
        {
            // Arrange
            var item = new TodoItem { Title = "To Remove" };
            var list = new List<TodoItem> { item };
            var command = new RemoveTodoCommand(list, item);

            // Act
            command.Execute();

            // Assert
            Assert.Empty(list);
        }

        [Fact]
        public void RemoveTodoCommand_Undo_RestoresItem()
        {
            // Arrange
            var item = new TodoItem { Title = "To Remove" };
            var list = new List<TodoItem> { item };
            var command = new RemoveTodoCommand(list, item);
            command.Execute();

            // Act
            command.Undo();

            // Assert
            Assert.Single(list);
            Assert.Equal("To Remove", list[0].Title);
        }

        [Fact]
        public void RemoveTodoCommand_Redo_RemovesItemAgain()
        {
            // Arrange
            var item = new TodoItem { Title = "To Remove" };
            var list = new List<TodoItem> { item };
            var command = new RemoveTodoCommand(list, item);
            command.Execute();
            command.Undo();

            // Act
            command.Redo();

            // Assert
            Assert.Empty(list);
        }

        [Fact]
        public void RemoveTodoCommand_ExecuteOnItemNotInList_DoesNotThrow()
        {
            // Arrange
            var item = new TodoItem { Title = "Not in list" };
            var list = new List<TodoItem>();
            var command = new RemoveTodoCommand(list, item);

            // Act & Assert
            var exception = Record.Exception(() => command.Execute());
            Assert.Null(exception);
        }

        // --- ToggleCompleteCommand Tests ---

        [Fact]
        public void ToggleCompleteCommand_Execute_TogglesCompleted()
        {
            // Arrange
            var item = new TodoItem { Title = "Task", Completed = false };
            var command = new ToggleCompleteCommand(item);

            // Act
            command.Execute();

            // Assert
            Assert.True(item.Completed);
        }

        [Fact]
        public void ToggleCompleteCommand_Undo_RestoresOriginalState()
        {
            // Arrange
            var item = new TodoItem { Title = "Task", Completed = false };
            var command = new ToggleCompleteCommand(item);
            command.Execute();

            // Act
            command.Undo();

            // Assert
            Assert.False(item.Completed);
        }

        [Fact]
        public void ToggleCompleteCommand_Redo_TogglesAgain()
        {
            // Arrange
            var item = new TodoItem { Title = "Task", Completed = false };
            var command = new ToggleCompleteCommand(item);
            command.Execute();
            command.Undo();

            // Act
            command.Redo();

            // Assert
            Assert.True(item.Completed);
        }

        [Fact]
        public void ToggleCompleteCommand_ToggleCompletedItem_MakesItIncomplete()
        {
            // Arrange
            var item = new TodoItem { Title = "Done", Completed = true };
            var command = new ToggleCompleteCommand(item);

            // Act
            command.Execute();

            // Assert
            Assert.False(item.Completed);
        }

        // --- UpdateTodoCommand Tests ---

        [Fact]
        public void UpdateTodoCommand_Execute_UpdatesItemInList()
        {
            // Arrange
            var original = new TodoItem { Title = "Original", Priority = 1 };
            var updated = new TodoItem { Title = "Updated", Priority = 3 };
            var list = new List<TodoItem> { original };
            var command = new UpdateTodoCommand(list, original, updated);

            // Act
            command.Execute();

            // Assert
            Assert.Equal("Updated", list[0].Title);
            Assert.Equal(3, list[0].Priority);
        }

        [Fact]
        public void UpdateTodoCommand_Undo_RestoresOriginalItem()
        {
            // Arrange
            var original = new TodoItem { Title = "Original" };
            var updated = new TodoItem { Title = "Updated" };
            var list = new List<TodoItem> { original };
            var command = new UpdateTodoCommand(list, original, updated);
            command.Execute();

            // Act
            command.Undo();

            // Assert
            Assert.Equal("Original", list[0].Title);
        }

        [Fact]
        public void UpdateTodoCommand_Redo_AppliesUpdateAgain()
        {
            // Arrange
            var original = new TodoItem { Title = "Original" };
            var updated = new TodoItem { Title = "Updated" };
            var list = new List<TodoItem> { original };
            var command = new UpdateTodoCommand(list, original, updated);
            command.Execute();
            command.Undo();

            // Act
            command.Redo();

            // Assert
            Assert.Equal("Updated", list[0].Title);
        }

        // --- TodoCommandManager Tests ---

        [Fact]
        public void TodoCommandManager_ExecuteCommand_AddsToUndoStack()
        {
            // Arrange
            var manager = new TodoCommandManager();
            var list = new List<TodoItem>();
            var command = new AddTodoCommand(list, new TodoItem { Title = "Item" });

            // Act
            manager.ExecuteCommand(command);

            // Assert
            Assert.Equal(1, manager.UndoCount);
            Assert.Equal(0, manager.RedoCount);
            Assert.True(manager.CanUndo);
            Assert.False(manager.CanRedo);
        }

        [Fact]
        public void TodoCommandManager_Undo_PopsFromUndoAndPushesToRedo()
        {
            // Arrange
            var manager = new TodoCommandManager();
            var list = new List<TodoItem>();
            var item = new TodoItem { Title = "Item" };
            var command = new AddTodoCommand(list, item);
            manager.ExecuteCommand(command);

            // Act
            manager.Undo();

            // Assert
            Assert.Equal(0, manager.UndoCount);
            Assert.Equal(1, manager.RedoCount);
            Assert.False(manager.CanUndo);
            Assert.True(manager.CanRedo);
            Assert.Empty(list);
        }

        [Fact]
        public void TodoCommandManager_Redo_PopsFromRedoAndPushesToUndo()
        {
            // Arrange
            var manager = new TodoCommandManager();
            var list = new List<TodoItem>();
            var item = new TodoItem { Title = "Item" };
            var command = new AddTodoCommand(list, item);
            manager.ExecuteCommand(command);
            manager.Undo();

            // Act
            manager.Redo();

            // Assert
            Assert.Equal(1, manager.UndoCount);
            Assert.Equal(0, manager.RedoCount);
            Assert.Single(list);
        }

        [Fact]
        public void TodoCommandManager_ExecuteCommand_ClearsRedoStack()
        {
            // Arrange
            var manager = new TodoCommandManager();
            var list = new List<TodoItem>();
            var command1 = new AddTodoCommand(list, new TodoItem { Title = "First" });
            manager.ExecuteCommand(command1);
            manager.Undo();

            // Act
            var command2 = new AddTodoCommand(list, new TodoItem { Title = "Second" });
            manager.ExecuteCommand(command2);

            // Assert
            Assert.Equal(1, manager.UndoCount);
            Assert.Equal(0, manager.RedoCount);
        }

        [Fact]
        public void TodoCommandManager_Undo_WhenStackIsEmpty_DoesNotThrow()
        {
            // Arrange
            var manager = new TodoCommandManager();

            // Act & Assert
            var exception = Record.Exception(() => manager.Undo());
            Assert.Null(exception);
        }

        [Fact]
        public void TodoCommandManager_Redo_WhenStackIsEmpty_DoesNotThrow()
        {
            // Arrange
            var manager = new TodoCommandManager();

            // Act & Assert
            var exception = Record.Exception(() => manager.Redo());
            Assert.Null(exception);
        }

        [Fact]
        public void TodoCommandManager_CanUndoCanRedo_ReflectCorrectState()
        {
            // Arrange
            var manager = new TodoCommandManager();
            var list = new List<TodoItem>();
            var command = new AddTodoCommand(list, new TodoItem { Title = "Item" });

            // Assert initial state
            Assert.False(manager.CanUndo);
            Assert.False(manager.CanRedo);

            // Act - execute
            manager.ExecuteCommand(command);
            Assert.True(manager.CanUndo);
            Assert.False(manager.CanRedo);

            // Act - undo
            manager.Undo();
            Assert.False(manager.CanUndo);
            Assert.True(manager.CanRedo);

            // Act - redo
            manager.Redo();
            Assert.True(manager.CanUndo);
            Assert.False(manager.CanRedo);
        }

        [Fact]
        public void TodoCommandManager_GetUndoDescription_WhenEmpty_ReturnsDefaultMessage()
        {
            // Arrange
            var manager = new TodoCommandManager();

            // Act
            var desc = manager.GetUndoDescription();

            // Assert
            Assert.Equal("No actions to undo", desc);
        }

        [Fact]
        public void TodoCommandManager_GetRedoDescription_WhenEmpty_ReturnsDefaultMessage()
        {
            // Arrange
            var manager = new TodoCommandManager();

            // Act
            var desc = manager.GetRedoDescription();

            // Assert
            Assert.Equal("No actions to redo", desc);
        }

        [Fact]
        public void TodoCommandManager_ClearHistory_ResetsStacks()
        {
            // Arrange
            var manager = new TodoCommandManager();
            var list = new List<TodoItem>();
            manager.ExecuteCommand(new AddTodoCommand(list, new TodoItem { Title = "Item" }));
            manager.Undo();

            // Act
            manager.ClearHistory();

            // Assert
            Assert.Equal(0, manager.UndoCount);
            Assert.Equal(0, manager.RedoCount);
            Assert.False(manager.CanUndo);
            Assert.False(manager.CanRedo);
        }

        [Fact]
        public void TodoCommandManager_AddCommandExecutedCallback_IsInvokedOnExecute()
        {
            // Arrange
            var manager = new TodoCommandManager();
            var list = new List<TodoItem>();
            var invoked = false;
            manager.AddCommandExecutedCallback(() => invoked = true);

            // Act
            manager.ExecuteCommand(new AddTodoCommand(list, new TodoItem { Title = "Item" }));

            // Assert
            Assert.True(invoked);
        }

        [Fact]
        public void TodoCommandManager_RemoveCommandExecutedCallback_IsNotInvoked()
        {
            // Arrange
            var manager = new TodoCommandManager();
            var list = new List<TodoItem>();
            var invoked = false;
            Action callback = () => invoked = true;
            manager.AddCommandExecutedCallback(callback);
            manager.RemoveCommandExecutedCallback(callback);

            // Act
            manager.ExecuteCommand(new AddTodoCommand(list, new TodoItem { Title = "Item" }));

            // Assert
            Assert.False(invoked);
        }

        [Fact]
        public void TodoCommandManager_GetCommandHistory_ReturnsCorrectOrder()
        {
            // Arrange
            var manager = new TodoCommandManager();
            var list = new List<TodoItem>();
            manager.ExecuteCommand(new AddTodoCommand(list, new TodoItem { Title = "First" }));
            manager.ExecuteCommand(new AddTodoCommand(list, new TodoItem { Title = "Second" }));

            // Act
            var history = manager.GetCommandHistory();

            // Assert
            Assert.Contains("--- CURRENT STATE ---", history);
            Assert.Contains("[Undo] Add: Second", history);
            Assert.Contains("[Undo] Add: First", history);
        }

        // --- TodoCommandFactory Tests ---

        [Fact]
        public void TodoCommandFactory_CreateAddCommand_ReturnsAddTodoCommand()
        {
            // Arrange
            var list = new List<TodoItem>();
            var item = new TodoItem { Title = "New" };

            // Act
            var command = TodoCommandFactory.CreateAddCommand(list, item);

            // Assert
            Assert.IsType<AddTodoCommand>(command);
        }

        [Fact]
        public void TodoCommandFactory_CreateRemoveCommand_ReturnsRemoveTodoCommand()
        {
            // Arrange
            var list = new List<TodoItem>();
            var item = new TodoItem { Title = "Remove" };

            // Act
            var command = TodoCommandFactory.CreateRemoveCommand(list, item);

            // Assert
            Assert.IsType<RemoveTodoCommand>(command);
        }

        [Fact]
        public void TodoCommandFactory_CreateToggleCommand_ReturnsToggleCompleteCommand()
        {
            // Arrange
            var item = new TodoItem { Title = "Toggle" };

            // Act
            var command = TodoCommandFactory.CreateToggleCommand(item);

            // Assert
            Assert.IsType<ToggleCompleteCommand>(command);
        }

        [Fact]
        public void TodoCommandFactory_CreateUpdateCommand_ReturnsUpdateTodoCommand()
        {
            // Arrange
            var list = new List<TodoItem>();
            var original = new TodoItem { Title = "Original" };
            var updated = new TodoItem { Title = "Updated" };

            // Act
            var command = TodoCommandFactory.CreateUpdateCommand(list, original, updated);

            // Assert
            Assert.IsType<UpdateTodoCommand>(command);
        }

        [Fact]
        public void FullUndoRedoCycle_MultipleCommands_WorksCorrectly()
        {
            // Arrange
            var manager = new TodoCommandManager();
            var list = new List<TodoItem>();
            var item1 = new TodoItem { Title = "A" };
            var item2 = new TodoItem { Title = "B" };
            var item3 = new TodoItem { Title = "C" };

            // Act - add three items
            manager.ExecuteCommand(new AddTodoCommand(list, item1));
            manager.ExecuteCommand(new AddTodoCommand(list, item2));
            manager.ExecuteCommand(new AddTodoCommand(list, item3));
            Assert.Equal(3, list.Count);

            // Undo two
            manager.Undo();
            manager.Undo();
            Assert.Single(list);
            Assert.Equal("A", list[0].Title);

            // Redo one
            manager.Redo();
            Assert.Equal(2, list.Count);
            Assert.Equal("B", list[1].Title);

            // Execute new command (should clear redo)
            manager.ExecuteCommand(new AddTodoCommand(list, new TodoItem { Title = "D" }));
            Assert.Equal(3, list.Count);
            Assert.False(manager.CanRedo);
        }
    }
}