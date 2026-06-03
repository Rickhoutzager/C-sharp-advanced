using Xunit;

namespace ToDoList.Tests.UITests
{
    [Collection("Sequential")]
    public class ProjectManagementUITests : UITestBase
    {
        // ComboBox index for comboBoxProjects (6th ComboBox in depth-first traversal:
        // comboBoxPriority=0, comboBoxCategory=1, comboBoxItemType=2,
        // comboBoxEditCategory=3, comboBoxEditPriority=4, comboBoxProjects=5)
        private const int ProjectsComboBoxIndex = 5;

        [Fact]
        public void ProjectManagementControls_ArePresent()
        {
            Assert.NotNull(FindButtonByText("Create Project"));
            Assert.NotNull(FindButtonByText("Add to Project"));
            Assert.NotNull(FindButtonByText("Complete Project"));
            Assert.NotNull(FindButtonByText("Show Project Contents"));
        }

        [Fact]
        public void AddToProject_WhenNoProject_ShowsWarning()
        {
            TypeIntoTextBox(0, "Project Item");
            ClickButton("Add Decorated");
            Thread.Sleep(500);

            SelectListItem(0, 0);
            Thread.Sleep(300);

            ClickButton("Add to Project");
            Thread.Sleep(500);

            ClickMessageBoxOk("No Project Selected");
        }

        [Fact]
        public void CompleteProject_WhenNoProjectSelected_ShowsWarning()
        {
            ClickButton("Complete Project");
            Thread.Sleep(500);
            ClickMessageBoxOk("No Project Selected");
        }

        [Fact]
        public void ShowProjectContents_WhenNoProjectSelected_ShowsWarning()
        {
            ClickButton("Show Project Contents");
            Thread.Sleep(500);
            ClickMessageBoxOk("No Project Selected");
        }

        [Fact]
        public void CreateProject_CreatesProjectSuccessfully()
        {
            ClickButton("Create Project");
            Thread.Sleep(500);

            // The VB InputBox appears; fill it with a project name and click OK
            FillInputBoxAndClickOk("Test Project");
            Thread.Sleep(500);

            // Dismiss the success message box
            ClickMessageBoxOk("Project Created");

            // Allow the UI thread time to process the DataSource binding update
            Thread.Sleep(500);

            // Verify the project was added to the combo box by selecting it.
            // SelectComboItemNative uses Win32 CB_SELECTSTRING to avoid the
            // FlaUI ExpandCollapse NullReferenceException on DataSource-bound combos.
            SelectComboItemNative(ProjectsComboBoxIndex, "Test Project");
        }

        [Fact]
        public void CreateProject_WithDefaultName_HandlesWhitespace()
        {
            ClickButton("Create Project");
            Thread.Sleep(500);

            // Leave the default "New Project" name by just clicking OK
            FillInputBoxAndClickOk("New Project");
            Thread.Sleep(500);

            ClickMessageBoxOk("Project Created");

            // Allow the UI thread time to process the DataSource binding update
            Thread.Sleep(500);

            SelectComboItemNative(ProjectsComboBoxIndex, "New Project");
        }

        [Fact]
        public void AddItemToProject_AddsItemSuccessfully()
        {
            // Step 1: Add a todo item
            TypeIntoTextBox(0, "Project Task");
            ClickButton("Add Decorated");
            Thread.Sleep(500);

            // Step 2: Create a project
            ClickButton("Create Project");
            Thread.Sleep(500);
            FillInputBoxAndClickOk("Test Project");
            Thread.Sleep(500);
            ClickMessageBoxOk("Project Created");

            // Step 3: Select the item from the incomplete list
            SelectListItem(0, 0);
            Thread.Sleep(300);

            // Step 4: Select the project from the projects combo box
            SelectComboItemNative(ProjectsComboBoxIndex, "Test Project");
            Thread.Sleep(300);

            // Step 5: Click "Add to Project" and dismiss the success message
            ClickButton("Add to Project");
            Thread.Sleep(500);
            ClickMessageBoxOk("Item Added");
        }

        [Fact]
        public void CompleteProject_WithItems_MarksProjectComplete()
        {
            // Arrange: Create a project with an item
            TypeIntoTextBox(0, "Task for Completion");
            ClickButton("Add Decorated");
            Thread.Sleep(500);

            ClickButton("Create Project");
            Thread.Sleep(500);
            FillInputBoxAndClickOk("Completion Project");
            Thread.Sleep(500);
            ClickMessageBoxOk("Project Created");

            SelectListItem(0, 0);
            Thread.Sleep(300);
            SelectComboItemNative(ProjectsComboBoxIndex, "Completion Project");
            Thread.Sleep(300);
            ClickButton("Add to Project");
            Thread.Sleep(500);
            ClickMessageBoxOk("Item Added");

            // Act: Complete the project
            SelectComboItemNative(ProjectsComboBoxIndex, "Completion Project");
            Thread.Sleep(300);
            ClickButton("Complete Project");
            Thread.Sleep(500);
            ClickMessageBoxOk("Project Completed");

            // Assert: The project should now appear in the completed list
            // The UpdateUI adds completed projects to listBoxComplete (index 1)
            Assert.True(GetListItemCount(1) >= 1,
                "Completed list should contain at least the completed project");
        }

        [Fact]
        public void CompleteProject_WithMultipleItems_CompletesAll()
        {
            // Arrange: Create a project with multiple items
            TypeIntoTextBox(0, "Task One");
            ClickButton("Add Decorated");
            Thread.Sleep(500);

            TypeIntoTextBox(0, "Task Two");
            ClickButton("Add Decorated");
            Thread.Sleep(500);

            ClickButton("Create Project");
            Thread.Sleep(500);
            FillInputBoxAndClickOk("Multi Project");
            Thread.Sleep(500);
            ClickMessageBoxOk("Project Created");

            // Add first item to project
            SelectListItem(0, 0);
            Thread.Sleep(300);
            SelectComboItemNative(ProjectsComboBoxIndex, "Multi Project");
            Thread.Sleep(300);
            ClickButton("Add to Project");
            Thread.Sleep(500);
            ClickMessageBoxOk("Item Added");

            // Add second item to project
            SelectListItem(0, 0);
            Thread.Sleep(300);
            SelectComboItemNative(ProjectsComboBoxIndex, "Multi Project");
            Thread.Sleep(300);
            ClickButton("Add to Project");
            Thread.Sleep(500);
            ClickMessageBoxOk("Item Added");

            // Act: Complete the project
            SelectComboItemNative(ProjectsComboBoxIndex, "Multi Project");
            Thread.Sleep(300);
            ClickButton("Complete Project");
            Thread.Sleep(500);
            ClickMessageBoxOk("Project Completed");

            // Assert: Both tasks and the project moved to completed list
            Assert.True(GetListItemCount(1) >= 3,
                "Completed list should contain the project and both tasks");
        }

        [Fact]
        public void ShowProjectContents_DisplaysProjectHierarchy()
        {
            // Arrange: Create a project with an item
            TypeIntoTextBox(0, "Nested Task");
            ClickButton("Add Decorated");
            Thread.Sleep(500);

            ClickButton("Create Project");
            Thread.Sleep(500);
            FillInputBoxAndClickOk("Hierarchy Project");
            Thread.Sleep(500);
            ClickMessageBoxOk("Project Created");

            SelectListItem(0, 0);
            Thread.Sleep(300);
            SelectComboItemNative(ProjectsComboBoxIndex, "Hierarchy Project");
            Thread.Sleep(300);
            ClickButton("Add to Project");
            Thread.Sleep(500);
            ClickMessageBoxOk("Item Added");

            // Act: Show the project contents
            SelectComboItemNative(ProjectsComboBoxIndex, "Hierarchy Project");
            Thread.Sleep(300);
            ClickButton("Show Project Contents");
            Thread.Sleep(500);

            // Assert: A dialog appears showing the project hierarchy
            var dialog = WaitForDialog("Hierarchy Project");
            Assert.NotNull(dialog);
            Assert.Contains("Hierarchy Project", dialog!.Name);
            ClickMessageBoxOk("Hierarchy Project");
        }

        [Fact]
        public void ProjectOperations_AfterItemDeletedFromMainList_ItemStillInProject()
        {
            // Arrange: Create a project with an item
            TypeIntoTextBox(0, "Shared Task");
            ClickButton("Add Decorated");
            Thread.Sleep(500);

            ClickButton("Create Project");
            Thread.Sleep(500);
            FillInputBoxAndClickOk("Persistent Project");
            Thread.Sleep(500);
            ClickMessageBoxOk("Project Created");

            SelectListItem(0, 0);
            Thread.Sleep(300);
            SelectComboItemNative(ProjectsComboBoxIndex, "Persistent Project");
            Thread.Sleep(300);
            ClickButton("Add to Project");
            Thread.Sleep(500);
            ClickMessageBoxOk("Item Added");

            // Act: Delete the item from the main list via Undo (removes it from todoList)
            // First verify it's in the incomplete list
            Assert.True(GetListItemCount(0) >= 1);

            // Undo the add to remove the item from the main list
            ClickButton("Undo");
            Thread.Sleep(500);

            // Assert: The project still exists and can show its contents
            // The project should retain the item even though it was removed from the main list
            SelectComboItemNative(ProjectsComboBoxIndex, "Persistent Project");
            Thread.Sleep(300);
            ClickButton("Show Project Contents");
            Thread.Sleep(500);

            var dialog = WaitForDialog("Persistent Project");
            Assert.NotNull(dialog);
            Assert.Contains("Shared Task", dialog!.Name);
            ClickMessageBoxOk("Persistent Project");
        }
    }
}