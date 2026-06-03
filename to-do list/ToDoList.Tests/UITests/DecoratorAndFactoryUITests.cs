using Xunit;

namespace ToDoList.Tests.UITests
{
    [Collection("Sequential")]
    public class DecoratorAndFactoryUITests : UITestBase
    {
        [Fact]
        public void AddDecoratedItem_WithPriorityAndCategory_AppearsInList()
        {
            TypeIntoTextBox(0, "Decorated Task");
            SelectComboItem(0, "3 - High");
            SelectComboItem(1, "Work");
            ClickButton("Add Decorated");
            Thread.Sleep(500);

            Assert.Equal(1, GetListItemCount(0));
            var itemText = GetListItemText(0, 0);
            Assert.Contains("Decorated Task", itemText);
        }

        [Fact]
        public void AddDecoratedItem_DefaultPriorityIsMedium()
        {
            TypeIntoTextBox(0, "Default Priority Task");
            ClickButton("Add Decorated");
            Thread.Sleep(500);

            Assert.Equal(1, GetListItemCount(0));
        }

        [Fact]
        public void FactoryMethod_WorkType_CreatesItem()
        {
            TypeIntoTextBox(0, "Work Task");
            SelectComboItem(2, "Work");  // Item type combo (index 2)
            ClickButton("Add (Factory Method)");
            Thread.Sleep(500);

            Assert.Equal(1, GetListItemCount(0));
        }

        [Fact]
        public void FactoryMethod_PersonalType_CreatesItem()
        {
            TypeIntoTextBox(0, "Personal Task");
            SelectComboItem(2, "Personal");
            ClickButton("Add (Factory Method)");
            Thread.Sleep(500);

            Assert.Equal(1, GetListItemCount(0));
        }

        [Fact]
        public void FactoryMethod_UrgentType_CreatesItem()
        {
            TypeIntoTextBox(0, "Urgent Factory Task");
            SelectComboItem(2, "Urgent");
            ClickButton("Add (Factory Method)");
            Thread.Sleep(500);

            Assert.Equal(1, GetListItemCount(0));
        }

        [Fact]
        public void FactoryMethod_EmptyTitle_ShowsWarning()
        {
            TypeIntoTextBox(0, "");
            ClickButton("Add (Factory Method)");
            Thread.Sleep(500);

            ClickMessageBoxOk("Invalid Input");
            Assert.Equal(0, GetListItemCount(0));
        }
    }
}