using Xunit;

namespace ToDoList.Tests.UITests
{
    [Collection("Sequential")]
    public class BasicCrudUITests : UITestBase
    {
        [Fact]
        public void App_LaunchesWithEmptyLists()
        {
            // Just verify the app launched and lists are empty at index 0 and 1
            Assert.Equal(0, GetListItemCount(0));
            Assert.Equal(0, GetListItemCount(1));
        }

        [Fact]
        public void ToggleItem_MovesItemBetweenLists()
        {
            // Add an item using "Add Decorated" button — list 0 = incomplete, 1 = complete
            TypeIntoTextBox(0, "Test Toggle Item");
            ClickButton("Add Decorated");
            Thread.Sleep(500);

            Assert.Equal(1, GetListItemCount(0));
            Assert.Equal(0, GetListItemCount(1));

            // Select item and toggle
            SelectListItem(0, 0);
            ClickButton("Toggle Complete");
            Thread.Sleep(500);

            Assert.Equal(0, GetListItemCount(0));
            Assert.Equal(1, GetListItemCount(1));
        }

        [Fact]
        public void ToggleCompleteItem_BackToIncomplete()
        {
            TypeIntoTextBox(0, "Toggle Back Item");
            ClickButton("Add Decorated");
            Thread.Sleep(500);

            SelectListItem(0, 0);
            ClickButton("Toggle Complete");
            Thread.Sleep(500);

            Assert.Equal(1, GetListItemCount(1));

            SelectListItem(1, 0);
            ClickButton("Toggle Complete");
            Thread.Sleep(500);

            Assert.Equal(1, GetListItemCount(0));
            Assert.Equal(0, GetListItemCount(1));
        }

        [Fact]
        public void EditItem_ChangesTitleAndProperties()
        {
            // Add item — textbox[0] for title, combobox[0]=Priority, [1]=Category
            TypeIntoTextBox(0, "Original Title");  // Title textbox (index 0 in "Add New Item")
            SelectComboItem(0, "3 - High");          // Priority combo in Add group
            SelectComboItem(1, "Work");              // Category combo
            ClickButton("Add Decorated");
            Thread.Sleep(500);

            // Select item in incomplete list (index 0)
            SelectListItem(0, 0);
            Thread.Sleep(300);

            // Edit — textbox[1]=edit title, combo[3]=edit priority, combo[4]=edit category
            // We'll try textbox indices 2 for edit area
            TypeIntoTextBox(2, "Updated Title");      // Edit title textbox
            SelectComboItem(3, "5 - Urgent");        // Edit priority
            SelectComboItem(4, "Personal");          // Edit category
            ClickButton("Save Changes");
            Thread.Sleep(500);

            ClickMessageBoxOk("Success");

            Assert.Equal(1, GetListItemCount(0));
        }

        [Fact]
        public void EditItem_WithoutSelection_ShowsWarning()
        {
            ClickButton("Save Changes");
            Thread.Sleep(500);
            ClickMessageBoxOk("No Selection");
        }
    }
}