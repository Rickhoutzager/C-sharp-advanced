using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;
using to_do_list.Patterns;

namespace to_do_list
{
    public partial class Form1 : Form
    {
        List<TodoItem> todoList = new List<TodoItem>();

        public Form1()
        {
            InitializeComponent();
            LoadTodoList();
            UpdateUI();
        }

        void LoadTodoList()
        {
            todoList = TodoStorage.Instance.Load(); // Using Singleton pattern to load the todo list
        }

        void UpdateUI()
        {
            var incompleteItems = todoList.Where(item => !item.Completed).ToList();
            var completedItems = todoList.Where(item => item.Completed).ToList();

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
                TodoStorage.Instance.Save(todoList); // Save the updated list using Singleton pattern
                UpdateUI();
                textBoxNewItem.Clear();
            }
        }

        private void btnToggleComplete_Click_1(object sender, EventArgs e)
        {
            TodoItem selectedItem = null;

            if (listBoxIncomplete.SelectedItem != null)
            {
                selectedItem = (TodoItem)listBoxIncomplete.SelectedItem;
                selectedItem.Completed = true;
            }
            else if (listBoxComplete.SelectedItem != null)
            {
                selectedItem = (TodoItem)listBoxComplete.SelectedItem;
                selectedItem.Completed = false;
            }

            if (selectedItem != null)
            {
                TodoStorage.Instance.Save(todoList); // Save the updated list using Singleton pattern
                UpdateUI();
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

                todoList.Add(todoItem);
                TodoStorage.Instance.Save(todoList);
                UpdateUI();
                textBoxNewItem.Clear();
            }
        }

        private void btnEditSelected_Click(object sender, EventArgs e)
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

                // Update the selected item
                selectedItem.Title = newTitle;
                selectedItem.Category = selectedCategory;
                selectedItem.Priority = priority;
                selectedItem.DueDate = dueDate;

                // Save changes and update UI
                TodoStorage.Instance.Save(todoList);
                UpdateUI();

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
    }
}