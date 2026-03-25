using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;
using to_do_list.Patterns;
using Microsoft.VisualBasic;

namespace to_do_list
{
    public partial class Form1 : Form
    {
        List<TodoItem> todoList = new List<TodoItem>();
        private TodoCommandManager commandManager = new TodoCommandManager();

        public Form1()
        {
            InitializeComponent();
            LoadTodoList();
            UpdateUI();
            
            // Set up command manager callbacks for UI updates
            commandManager.AddCommandExecutedCallback(UpdateCommandUI);
        }

        void LoadTodoList()
        {
            todoList = TodoStorage.Instance.Load(); // Load todo items
            projects = TodoStorage.Instance.LoadProjects(); // Load projects
            UpdateProjectsComboBox(); // Update the projects dropdown
        }

        void UpdateUI()
        {
            var incompleteItems = todoList.Where(item => !item.Completed).ToList();
            var completedItems = todoList.Where(item => item.Completed).ToList();

            // Add projects to the appropriate lists
            foreach (var project in projects)
            {
                if (project.IsComplete)
                {
                    completedItems.Add(new TodoItem
                    {
                        Title = project.Title,
                        Completed = true,
                        Priority = project.Priority,
                        Category = project.Category,
                        DueDate = project.DueDate
                    });
                }
                else
                {
                    incompleteItems.Add(new TodoItem
                    {
                        Title = project.Title,
                        Completed = false,
                        Priority = project.Priority,
                        Category = project.Category,
                        DueDate = project.DueDate
                    });
                }
            }

            listBoxIncomplete.DataSource = null;
            listBoxIncomplete.DataSource = incompleteItems;
            listBoxIncomplete.DisplayMember = "ToString";

            listBoxComplete.DataSource = null;
            listBoxComplete.DataSource = completedItems;
            listBoxComplete.DisplayMember = "ToString";
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            string newTitle = textBoxNewItem.Text.Trim();
            if (!string.IsNullOrEmpty(newTitle))
            {
                todoList.Add(new TodoItem { Title = newTitle, Completed = false });
                TodoStorage.Instance.Save(todoList, projects); // Save both todo items and projects
                UpdateUI();
                textBoxNewItem.Clear();
            }
        }

        private void btnToggleComplete_Click_1(object sender, EventArgs e)
        {
            TodoItem? selectedItem = null;

            if (listBoxIncomplete.SelectedItem != null)
            {
                selectedItem = (TodoItem)listBoxIncomplete.SelectedItem;
            }
            else if (listBoxComplete.SelectedItem != null)
            {
                selectedItem = (TodoItem)listBoxComplete.SelectedItem;
            }

            if (selectedItem != null)
            {
                // Execute using Command pattern
                var command = TodoCommandFactory.CreateToggleCommand(selectedItem);
                commandManager.ExecuteCommand(command);
                TodoStorage.Instance.Save(todoList, projects);
                UpdateUI(); // Refresh the UI to show the updated lists
            }
            listBoxIncomplete.ClearSelected();
            listBoxComplete.ClearSelected();
            UpdateToggleButtonLabel();
        }

        private void listBoxIncomplete_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxIncomplete.SelectedIndex != -1)
            {
                listBoxComplete.ClearSelected();
            }
            UpdateToggleButtonLabel();
        }

        private void listBoxComplete_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxComplete.SelectedIndex != -1)
            {
                listBoxIncomplete.ClearSelected();
            }
            UpdateToggleButtonLabel();
        }
        void UpdateToggleButtonLabel()
        {
            if (listBoxIncomplete.SelectedItem != null)
            {
                btnToggleComplete.Text = "Mark as Complete";
                btnToggleComplete.Enabled = true;
            }
            else if (listBoxComplete.SelectedItem != null)
            {
                btnToggleComplete.Text = "Mark as Incomplete";
                btnToggleComplete.Enabled = true;
            }
            else
            {
                btnToggleComplete.Text = "Toggle Status";
                btnToggleComplete.Enabled = false;
            }
        }
        private void btnSaveFile_Click_1(object sender, EventArgs e)
        {
            using SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JSON files (*.json)|*.json|XML files (*.xml)|*.xml";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string path = saveFileDialog.FileName;
                ITodoStorageAdapter adapter = GetAdapterByExtension(path);
                adapter.Save(path, todoList);
            }
        }

        private void btnLoadFile_Click_1(object sender, EventArgs e)
        {
            using OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JSON files (*.json)|*.json|XML files (*.xml)|*.xml";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog.FileName;
                ITodoStorageAdapter adapter = GetAdapterByExtension(path);
                todoList = adapter.Load(path);
                UpdateUI();
            }
        }

        private ITodoStorageAdapter GetAdapterByExtension(string filePath)
        {
            string ext = Path.GetExtension(filePath).ToLower();
            return ext switch
            {
                ".json" => new JsonTodoStorageAdapter(),
                ".xml" => new XmlTodoStorageAdapter(),
                _ => throw new NotSupportedException("Unsupported file format.")
            };
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btnAddDecorated_Click(object sender, EventArgs e)
        {
            string newTitle = textBoxNewItem.Text.Trim();
            if (!string.IsNullOrEmpty(newTitle))
            {
                // Get selected values from UI controls
                string selectedPriority = comboBoxPriority.SelectedItem?.ToString() ?? "1 - Low";
                int priority = int.Parse(selectedPriority.Split(' ')[0]);
                
                string selectedCategory = comboBoxCategory.SelectedItem?.ToString() ?? "General";
                DateTime dueDate = dateTimePickerDueDate.Value;

                // Create a todo item with decorators
                var todoItem = new TodoItem
                {
                    Title = newTitle,
                    Completed = false,
                    Category = selectedCategory,
                    Priority = priority,
                    DueDate = dueDate
                };

                // Execute using Command pattern
                var command = TodoCommandFactory.CreateAddCommand(todoList, todoItem);
                commandManager.ExecuteCommand(command);
                TodoStorage.Instance.Save(todoList, projects);
                textBoxNewItem.Clear();
            }
        }

        private void btnEditSelected_Click(object sender, EventArgs e)
        {
            TodoItem? selectedItem = null;

            if (listBoxIncomplete.SelectedItem != null)
            {
                selectedItem = (TodoItem)listBoxIncomplete.SelectedItem;
            }
            else if (listBoxComplete.SelectedItem != null)
            {
                selectedItem = (TodoItem)listBoxComplete.SelectedItem;
            }

            if (selectedItem != null)
            {
                // Populate edit controls with selected item's data
                textBoxEditTitle.Text = selectedItem.Title;
                comboBoxEditPriority.SelectedItem = $"{selectedItem.Priority} - {(selectedItem.Priority == 1 ? "Low" : selectedItem.Priority == 2 ? "Medium" : selectedItem.Priority == 3 ? "High" : selectedItem.Priority == 4 ? "Very High" : "Urgent")}";
                comboBoxEditCategory.SelectedItem = selectedItem.Category;
                dateTimePickerEditDueDate.Value = selectedItem.DueDate ?? DateTime.Now;

                // Update header to show which item is being edited
                labelEditHeader.Text = $"Editing: {selectedItem.Title}";
                labelEditHeader.ForeColor = System.Drawing.Color.Blue;
            }
            else
            {
                MessageBox.Show("Please select an item to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            TodoItem selectedItem = null;

            if (listBoxIncomplete.SelectedItem != null)
            {
                selectedItem = (TodoItem)listBoxIncomplete.SelectedItem;
            }
            else if (listBoxComplete.SelectedItem != null)
            {
                selectedItem = (TodoItem)listBoxComplete.SelectedItem;
            }

            if (selectedItem != null)
            {
                string newTitle = textBoxEditTitle.Text.Trim();
                if (string.IsNullOrEmpty(newTitle))
                {
                    MessageBox.Show("Title cannot be empty.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Get updated values from edit controls
                string selectedPriority = comboBoxEditPriority.SelectedItem?.ToString() ?? "1 - Low";
                int priority = int.Parse(selectedPriority.Split(' ')[0]);
                
                string selectedCategory = comboBoxEditCategory.SelectedItem?.ToString() ?? "General";
                DateTime dueDate = dateTimePickerEditDueDate.Value;

                // Create updated item
                var updatedItem = new TodoItem
                {
                    Title = newTitle,
                    Category = selectedCategory,
                    Priority = priority,
                    DueDate = dueDate,
                    Completed = selectedItem.Completed
                };

                // Execute using Command pattern
                var command = TodoCommandFactory.CreateUpdateCommand(todoList, selectedItem, updatedItem);
                commandManager.ExecuteCommand(command);
                TodoStorage.Instance.Save(todoList, projects);

                // Clear edit controls and reset header
                ClearEditControls();
                
                MessageBox.Show("Item updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No item selected to save changes.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ClearEditControls()
        {
            textBoxEditTitle.Clear();
            comboBoxEditPriority.SelectedIndex = 0;
            comboBoxEditCategory.SelectedIndex = 0;
            dateTimePickerEditDueDate.Value = DateTime.Now;
            labelEditHeader.Text = "Select item to edit it";
            labelEditHeader.ForeColor = System.Drawing.Color.Black;
        }

        private void UpdateCommandUI()
        {
            labelUndoCount.Text = commandManager.UndoCount.ToString();
            labelRedoCount.Text = commandManager.RedoCount.ToString();
            
            btnUndo.Enabled = commandManager.CanUndo;
            btnRedo.Enabled = commandManager.CanRedo;
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
            if (commandManager.CanUndo)
            {
                commandManager.Undo();
                UpdateUI();
                UpdateCommandUI();
            }
        }

        private void btnRedo_Click(object sender, EventArgs e)
        {
            if (commandManager.CanRedo)
            {
                commandManager.Redo();
                UpdateUI();
                UpdateCommandUI();
            }
        }

        // Composite Pattern - Project Management Methods
        private List<TodoComposite> projects = new List<TodoComposite>();

        private void btnCreateProject_Click(object sender, EventArgs e)
        {
            var projectName = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter project name:", "Create Project", "New Project");
            
            if (!string.IsNullOrEmpty(projectName))
            {
                var project = TodoCompositeFactory.CreateProject(projectName);
                projects.Add(project);
                UpdateProjectsComboBox();
                MessageBox.Show($"Project '{projectName}' created successfully!", "Project Created", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnAddToProject_Click(object sender, EventArgs e)
        {
            if (comboBoxProjects.SelectedItem == null)
            {
                MessageBox.Show("Please select a project first.", "No Project Selected", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (listBoxIncomplete.SelectedItem == null && listBoxComplete.SelectedItem == null)
            {
                MessageBox.Show("Please select a todo item to add to the project.", "No Item Selected", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedProject = (TodoComposite)comboBoxProjects.SelectedItem;
            TodoItem selectedItem = null;

            if (listBoxIncomplete.SelectedItem != null)
            {
                selectedItem = (TodoItem)listBoxIncomplete.SelectedItem;
            }
            else if (listBoxComplete.SelectedItem != null)
            {
                selectedItem = (TodoItem)listBoxComplete.SelectedItem;
            }

            if (selectedItem != null)
            {
                // Create a TodoLeaf from the selected item
                var todoLeaf = selectedItem.ToTodoLeaf();
                selectedProject.AddChild(todoLeaf);
                
                // Save both todo items and projects (keep todo item in main list)
                TodoStorage.Instance.Save(todoList, projects);
                UpdateUI();
                
                MessageBox.Show($"Added '{selectedItem.Title}' to project '{selectedProject.Title}'", 
                    "Item Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnCompleteProject_Click(object sender, EventArgs e)
        {
            if (comboBoxProjects.SelectedItem == null)
            {
                MessageBox.Show("Please select a project first.", "No Project Selected", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedProject = (TodoComposite)comboBoxProjects.SelectedItem;
            
            // Complete all tasks in the project
            selectedProject.CompleteAll();
            
            // Save both todo items and projects
            TodoStorage.Instance.Save(todoList, projects);
            
            // Update UI to reflect changes
            UpdateUI();
            
            MessageBox.Show($"Project '{selectedProject.Title}' and all its tasks marked as complete!", 
                "Project Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void UpdateProjectsComboBox()
        {
            comboBoxProjects.DataSource = null;
            comboBoxProjects.DataSource = projects;
            comboBoxProjects.DisplayMember = "Title";
        }

        private void btnShowProjectContents_Click(object sender, EventArgs e)
        {
            if (comboBoxProjects.SelectedItem != null)
            {
                var selectedProject = (TodoComposite)comboBoxProjects.SelectedItem;
                DisplayProjectContents(selectedProject);
            }
            else
            {
                MessageBox.Show("Please select a project first.", "No Project Selected", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void DisplayProjectContents(TodoComposite project)
        {
            // Create a message box showing the hierarchical structure
            var projectDisplay = project.Display(0);
            MessageBox.Show(projectDisplay, $"Project: {project.Title}", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}