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

            // Check which list has the selected item
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
                // Get selected values from UI controls
                string selectedPriority = comboBoxPriority.SelectedItem?.ToString() ?? "1 - Low";
                int priority = int.Parse(selectedPriority.Split(' ')[0]);
                
                string selectedCategory = comboBoxCategory.SelectedItem?.ToString() ?? "General";
                DateTime dueDate = dateTimePickerDueDate.Value;

                // Update the existing todo item with new decorator values
                selectedItem.Title = textBoxNewItem.Text.Trim();
                selectedItem.Category = selectedCategory;
                selectedItem.Priority = priority;
                selectedItem.DueDate = dueDate;

                TodoStorage.Instance.Save(todoList);
                UpdateUI();
                textBoxNewItem.Clear();
            }
            else
            {
                MessageBox.Show("Please select a todo item to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}