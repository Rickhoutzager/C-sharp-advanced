using Xunit;

namespace ToDoList.Tests.UITests
{
    [Collection("Sequential")]
    public class UndoRedoUITests : UITestBase
    {
        [Fact]
        public void Undo_AfterAddingItem_RemovesItem()
        {
            TypeIntoTextBox(0, "Undo Test Item");
            ClickButton("Add Decorated");
            Thread.Sleep(500);
            Assert.Equal(1, GetListItemCount(0));

            ClickButton("Undo");
            Thread.Sleep(500);
            Assert.Equal(0, GetListItemCount(0));
        }

        [Fact]
        public void Redo_AfterUndo_RestoresItem()
        {
            TypeIntoTextBox(0, "Redo Test Item");
            ClickButton("Add Decorated");
            Thread.Sleep(500);

            ClickButton("Undo");
            Thread.Sleep(500);
            Assert.Equal(0, GetListItemCount(0));

            ClickButton("Redo");
            Thread.Sleep(500);
            Assert.Equal(1, GetListItemCount(0));
        }

        [Fact]
        public void Undo_ToggleComplete_RestoresItem()
        {
            TypeIntoTextBox(0, "Toggle Undo Item");
            ClickButton("Add Decorated");
            Thread.Sleep(500);

            SelectListItem(0, 0);
            ClickButton("Mark as Complete");
            Thread.Sleep(500);

            Assert.Equal(1, GetListItemCount(1));

            ClickButton("Undo");
            Thread.Sleep(500);

            Assert.Equal(1, GetListItemCount(0));
            Assert.Equal(0, GetListItemCount(1));
        }
    }
}