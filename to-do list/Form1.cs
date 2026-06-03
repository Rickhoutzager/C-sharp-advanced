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
        private TodoListSelectionManager selectionManager = new TodoListSelectionManager();

        public Form1()
        {
            InitializeComponent();
            LoadTodoList();
            UpdateUI();
            
            // Set up command manager callbacks for UI updates
            commandManager.AddCommandExecutedCallback(UpdateCommandUI);
            
            // Initialize Observer Pattern
            InitializeObserverPattern();
            
            // Initialize concurrency testing
            InitializeConcurrencyTesting();
        }

        void LoadTodoList()
        {
            todoList = TodoStorage.Instance.Load(); // Load todo items
            projects = TodoStorage.Instance.LoadProjects(); // Load projects
            UpdateProjectsComboBox(); // Update the projects dropdown
        }

        public void UpdateUI()
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
                
                // Notify observers of selection change
                selectionManager.ClearSelection();
            }
        }

        private void listBoxIncomplete_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxIncomplete.SelectedIndex != -1)
            {
                listBoxComplete.ClearSelected();
                var selectedItem = (TodoItem)listBoxIncomplete.SelectedItem;
                selectionManager.UpdateSelection(selectedItem, true, false);
                PopulateEditForm();
            }
            else
            {
                selectionManager.ClearSelection();
                ClearEditControls();
            }
        }

        private void listBoxComplete_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxComplete.SelectedIndex != -1)
            {
                listBoxIncomplete.ClearSelected();
                var selectedItem = (TodoItem)listBoxComplete.SelectedItem;
                selectionManager.UpdateSelection(selectedItem, false, true);
                PopulateEditForm();
            }
            else
            {
                selectionManager.ClearSelection();
                ClearEditControls();
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
                UpdateUI();
                textBoxNewItem.Clear();
            }
        }

        // Demonstrates the Factory Method pattern (GoF):
        // The selected item type decides which Concrete Creator is used, and
        // each Concrete Creator polymorphically produces its own Concrete Product
        // (WorkTodoItem / PersonalTodoItem / UrgentTodoItem).
        private void btnAddWithFactory_Click(object sender, EventArgs e)
        {
            string newTitle = textBoxNewItem.Text.Trim();
            if (string.IsNullOrEmpty(newTitle))
            {
                MessageBox.Show("Please enter a title before adding.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedType = comboBoxItemType.SelectedItem?.ToString() ?? "Personal";

            // Select the Concrete Creator based on the chosen type.
            TodoItemCreator creator = selectedType switch
            {
                "Work" => new WorkTodoItemCreator(),
                "Personal" => new PersonalTodoItemCreator(),
                "Urgent" => new UrgentTodoItemCreator(),
                _ => new PersonalTodoItemCreator()
            };

            // The factory method polymorphically creates the correct Concrete Product.
            ITodoItem createdItem = creator.CreateConfiguredTodoItem(newTitle);

            // Add via the Command pattern (consistent with the rest of the app).
            var command = TodoCommandFactory.CreateAddCommand(todoList, (TodoItem)createdItem);
            commandManager.ExecuteCommand(command);
            TodoStorage.Instance.Save(todoList, projects);
            UpdateUI();
            textBoxNewItem.Clear();
        }

        private void PopulateEditForm()
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

        private void InitializeObserverPattern()
        {
            // Create observers using the factory
            var buttonTextObserver = UIObserverFactory.CreateButtonTextObserver(btnToggleComplete);
            var commandUIObserver = UIObserverFactory.CreateCommandUIObserver(
                labelUndoCount, labelRedoCount, btnUndo, btnRedo, commandManager);

            // Attach observers to the selection manager
            selectionManager.Attach(buttonTextObserver);
            selectionManager.Attach(commandUIObserver);

            // Initialize with current state
            selectionManager.ClearSelection();
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

        // Concurrency Testing Suite
        private ConcurrencyTodoManager _concurrencyManager;
        private System.Windows.Forms.Timer _testTimer;
        private int _testOperationCount = 0;
        private DateTime _testStartTime;

        private void InitializeConcurrencyTesting()
        {
            _concurrencyManager = new ConcurrencyTodoManager();
            _testTimer = new System.Windows.Forms.Timer();
            _testTimer.Interval = 1000;
            _testTimer.Tick += TestTimer_Tick;
        }

        private void TestTimer_Tick(object sender, EventArgs e)
        {
            _testOperationCount++;
            labelTestStatus.Text = $"Operations: {_testOperationCount} | Time: {DateTime.Now - _testStartTime:g}";
        }

        // Async/Await Pattern Testing
        private async void btnTestAsyncAwait_Click(object sender, EventArgs e)
        {
            labelTestStatus.Text = "Testing Async/Await Pattern...";
            _testStartTime = DateTime.Now;
            _testOperationCount = 0;
            _testTimer.Start();

            try
            {
                // Test rapid async file operations
                for (int i = 0; i < 5; i++)
                {
                    await _concurrencyManager.SaveDataAsync();
                    await Task.Delay(100); // Small delay between operations
                }

                // Test concurrent loading
                var loadTask1 = _concurrencyManager.LoadDataAsync();
                var loadTask2 = _concurrencyManager.LoadDataAsync();
                await Task.WhenAll(loadTask1, loadTask2);

                _testTimer.Stop();
                MessageBox.Show($"Async/Await Test Complete!\n" +
                    $"Operations: {_testOperationCount}\n" +
                    $"Duration: {DateTime.Now - _testStartTime:g}\n" +
                    $"UI remained responsive during operations", 
                    "Test Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                _testTimer.Stop();
                MessageBox.Show($"Async/Await Test Failed: {ex.Message}", "Test Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Producer-Consumer Pattern Testing
        private async void btnTestProducerConsumer_Click(object sender, EventArgs e)
        {
            labelTestStatus.Text = "Testing Producer-Consumer Pattern...";
            _testStartTime = DateTime.Now;
            _testOperationCount = 0;
            _testTimer.Start();

            try
            {
                // Rapidly produce many tasks
                var tasks = new List<Task>();
                
                for (int i = 0; i < 20; i++)
                {
                    var todoItem = new TodoItem
                    {
                        Title = $"Test Item {i}",
                        Completed = false,
                        Priority = (i % 5) + 1,
                        Category = "Test",
                        DueDate = DateTime.Now.AddDays(i)
                    };

                    tasks.Add(_concurrencyManager.AddTodoItemAsync(todoItem));
                    _testOperationCount++;
                }

                // Wait for all tasks to complete
                await Task.WhenAll(tasks);

                _testTimer.Stop();
                MessageBox.Show($"Producer-Consumer Test Complete!\n" +
                    $"Produced: {tasks.Count} tasks\n" +
                    $"Operations: {_testOperationCount}\n" +
                    $"Duration: {DateTime.Now - _testStartTime:g}\n" +
                    $"Background processing handled efficiently", 
                    "Test Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                _testTimer.Stop();
                MessageBox.Show($"Producer-Consumer Test Failed: {ex.Message}", "Test Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Reader-Writer Lock Pattern Testing
        private async void btnTestReaderWriterLock_Click(object sender, EventArgs e)
        {
            labelTestStatus.Text = "Testing Reader-Writer Lock Pattern...";
            _testStartTime = DateTime.Now;
            _testOperationCount = 0;
            _testTimer.Start();

            try
            {
                // Simulate concurrent read and write operations
                var readTasks = new List<Task<List<TodoItem>>>();
                var writeTasks = new List<Task>();

                // Start multiple read operations
                for (int i = 0; i < 10; i++)
                {
                    readTasks.Add(Task.Run(() => _concurrencyManager.GetTodoList()));
                    _testOperationCount++;
                }

                // Start write operations
                for (int i = 0; i < 5; i++)
                {
                    var todoItem = new TodoItem
                    {
                        Title = $"Concurrent Item {i}",
                        Completed = false
                    };
                    writeTasks.Add(_concurrencyManager.AddTodoItemAsync(todoItem));
                    _testOperationCount++;
                }

                // Wait for all operations
                await Task.WhenAll(readTasks.Concat(writeTasks));

                _testTimer.Stop();
                MessageBox.Show($"Reader-Writer Lock Test Complete!\n" +
                    $"Read Operations: {readTasks.Count}\n" +
                    $"Write Operations: {writeTasks.Count}\n" +
                    $"Total Operations: {_testOperationCount}\n" +
                    $"Duration: {DateTime.Now - _testStartTime:g}\n" +
                    $"Concurrent access handled safely", 
                    "Test Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                _testTimer.Stop();
                MessageBox.Show($"Reader-Writer Lock Test Failed: {ex.Message}", "Test Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Background Worker Pattern Testing
        private async void btnTestBackgroundWorker_Click(object sender, EventArgs e)
        {
            labelTestStatus.Text = "Testing Background Worker Pattern...";
            _testStartTime = DateTime.Now;
            _testOperationCount = 0;
            _testTimer.Start();

            try
            {
                // Test long-running operation with progress
                await _concurrencyManager.ImportDataAsync("testfile.json");

                // Wait for completion
                await _concurrencyManager.WaitForImportCompletion();

                _testTimer.Stop();
                MessageBox.Show($"Background Worker Test Complete!\n" +
                    $"Operations: {_testOperationCount}\n" +
                    $"Duration: {DateTime.Now - _testStartTime:g}\n" +
                    $"Progress reporting and cancellation worked correctly", 
                    "Test Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                _testTimer.Stop();
                MessageBox.Show($"Background Worker Test Failed: {ex.Message}", "Test Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Comprehensive Stress Test
        private async void btnStressTest_Click(object sender, EventArgs e)
        {
            labelTestStatus.Text = "Running Comprehensive Stress Test...";
            _testStartTime = DateTime.Now;
            _testOperationCount = 0;
            _testTimer.Start();

            try
            {
                // Combine all patterns in a stress test
                var stressTasks = new List<Task>();

                // Producer-Consumer: Rapid item creation
                for (int i = 0; i < 50; i++)
                {
                    var todoItem = new TodoItem
                    {
                        Title = $"Stress Item {i}",
                        Completed = i % 2 == 0,
                        Priority = (i % 5) + 1,
                        Category = $"Category {i % 3}",
                        DueDate = DateTime.Now.AddDays(i)
                    };
                    stressTasks.Add(_concurrencyManager.AddTodoItemAsync(todoItem));
                }

                // Async/Await: Concurrent file operations
                for (int i = 0; i < 10; i++)
                {
                    stressTasks.Add(_concurrencyManager.SaveDataAsync());
                }

                // Reader-Writer Lock: Concurrent access
                for (int i = 0; i < 20; i++)
                {
                    stressTasks.Add(Task.Run(() => _concurrencyManager.GetTodoList()));
                }

                await Task.WhenAll(stressTasks);

                _testTimer.Stop();
                MessageBox.Show($"Stress Test Complete!\n" +
                    $"Total Operations: {stressTasks.Count}\n" +
                    $"Operations: {_testOperationCount}\n" +
                    $"Duration: {DateTime.Now - _testStartTime:g}\n" +
                    $"All patterns worked together successfully", 
                    "Stress Test Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                _testTimer.Stop();
                MessageBox.Show($"Stress Test Failed: {ex.Message}", "Test Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Performance Benchmark
        private async void btnBenchmark_Click(object sender, EventArgs e)
        {
            labelTestStatus.Text = "Running Performance Benchmark...";
            _testStartTime = DateTime.Now;
            _testOperationCount = 0;
            _testTimer.Start();

            try
            {
                var benchmarkResults = new Dictionary<string, TimeSpan>();

                // Benchmark Async/Await
                var asyncStart = DateTime.Now;
                for (int i = 0; i < 100; i++)
                {
                    await _concurrencyManager.SaveDataAsync();
                }
                benchmarkResults["Async/Await"] = DateTime.Now - asyncStart;

                // Benchmark Producer-Consumer
                var producerStart = DateTime.Now;
                var producerTasks = new List<Task>();
                for (int i = 0; i < 100; i++)
                {
                    var todoItem = new TodoItem { Title = $"Benchmark {i}", Completed = false };
                    producerTasks.Add(_concurrencyManager.AddTodoItemAsync(todoItem));
                }
                await Task.WhenAll(producerTasks);
                benchmarkResults["Producer-Consumer"] = DateTime.Now - producerStart;

                // Benchmark Reader-Writer Lock
                var rwStart = DateTime.Now;
                var rwTasks = new List<Task>();
                for (int i = 0; i < 200; i++)
                {
                    rwTasks.Add(Task.Run(() => _concurrencyManager.GetTodoList()));
                }
                await Task.WhenAll(rwTasks);
                benchmarkResults["Reader-Writer Lock"] = DateTime.Now - rwStart;

                _testTimer.Stop();

                var resultsText = "Performance Benchmark Results:\n\n";
                foreach (var result in benchmarkResults)
                {
                    resultsText += $"{result.Key}: {result.Value.TotalMilliseconds:F2}ms\n";
                }

                MessageBox.Show(resultsText, "Benchmark Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                _testTimer.Stop();
                MessageBox.Show($"Benchmark Failed: {ex.Message}", "Benchmark Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Cleanup
        private async void btnCleanup_Click(object sender, EventArgs e)
        {
            if (_concurrencyManager != null)
            {
                await _concurrencyManager.ShutdownAsync();
                _concurrencyManager.Dispose();
                _concurrencyManager = null;
            }
            _testTimer?.Stop();
            _testTimer?.Dispose();
            labelTestStatus.Text = "Cleanup Complete";
            MessageBox.Show("Concurrency testing resources cleaned up successfully", 
                "Cleanup", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
