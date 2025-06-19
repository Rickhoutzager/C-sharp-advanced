using System;
using System.Collections.Generic;
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
            listBoxIncomplete.DisplayMember = "Title";

            listBoxComplete.DataSource = null;
            listBoxComplete.DataSource = completedItems;
            listBoxComplete.DisplayMember = "Title";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string newTitle = textBoxNewItem.Text.Trim();
            if (!string.IsNullOrEmpty(newTitle))
            {
                TodoItem newItem = Factory.CreateTodoItem(newTitle); // Using Factory pattern
                todoList.Add(newItem);
                TodoStorage.Instance.Save(todoList); // Save the updated list using Singleton pattern
                UpdateUI();
                textBoxNewItem.Clear();
            }
        }

        private void btnToggleComplete_Click(object sender, EventArgs e)
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
    }
}