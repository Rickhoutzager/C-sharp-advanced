using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace to_do_list.Patterns
{
    // Integration class showing how to use all concurrency patterns together
    public class ConcurrencyTodoManager : IDisposable
    {
        private readonly ThreadSafeTodoManager _threadSafeManager;
        private readonly TodoTaskQueue _taskQueue;
        private readonly TodoBackgroundWorker _backgroundWorker;
        private readonly List<TodoItem> _todoList;
        private readonly List<TodoComposite> _projects;

        public ConcurrencyTodoManager()
        {
            _threadSafeManager = new ThreadSafeTodoManager();
            _taskQueue = new TodoTaskQueue(maxDegreeOfParallelism: 3);
            _backgroundWorker = new TodoBackgroundWorker();
            _todoList = new List<TodoItem>();
            _projects = new List<TodoComposite>();

            // Set up background worker events
            _backgroundWorker.ProgressChanged += OnProgressChanged;
            _backgroundWorker.Completed += OnCompleted;
            _backgroundWorker.Error += OnError;
        }

        // Async operations using Async/Await pattern
        public async Task LoadDataAsync()
        {
            try
            {
                // Use Async/Await pattern for non-blocking file operations
                var todoList = await AsyncTodoStorage.LoadAsync();
                var projects = await AsyncTodoStorage.LoadProjectsAsync();

                // Use Reader-Writer Lock for thread-safe data updates
                _threadSafeManager.SetData(todoList, projects);
                _todoList.Clear();
                _todoList.AddRange(todoList);
                _projects.Clear();
                _projects.AddRange(projects);
            }
            catch (Exception ex)
            {
                // Handle error appropriately
                System.Diagnostics.Debug.WriteLine($"Load failed: {ex.Message}");
            }
        }

        public async Task SaveDataAsync()
        {
            try
            {
                // Use Async/Await pattern for non-blocking file operations
                await AsyncTodoStorage.SaveAsync(_todoList, _projects);
            }
            catch (Exception ex)
            {
                // Handle error appropriately
                System.Diagnostics.Debug.WriteLine($"Save failed: {ex.Message}");
            }
        }

        // Producer-Consumer pattern for background task processing
        public async Task AddTodoItemAsync(TodoItem item)
        {
            // Add to in-memory list immediately for responsive UI
            _todoList.Add(item);
            _threadSafeManager.AddTodoItem(item);

            // Queue save operation in background using Producer-Consumer pattern
            var saveTask = new SaveTodoTask(_todoList, _projects);
            await _taskQueue.EnqueueAsync(saveTask);
        }

        public async Task UpdateTodoItemAsync(int index, TodoItem updatedItem)
        {
            // Update in-memory list immediately for responsive UI
            if (index >= 0 && index < _todoList.Count)
            {
                _todoList[index] = updatedItem;
                _threadSafeManager.UpdateTodoItem(index, updatedItem);

                // Queue save operation in background
                var saveTask = new SaveTodoTask(_todoList, _projects);
                await _taskQueue.EnqueueAsync(saveTask);
            }
        }

        // Background Worker pattern for long-running operations
        public async Task ImportDataAsync(string filePath)
        {
            await _backgroundWorker.StartAsync(async (cancellationToken, progress) =>
            {
                progress.Report("Starting import...");

                // Simulate long-running import operation
                await Task.Delay(1000, cancellationToken);
                progress.Report("Reading file...");

                // Simulate processing
                await Task.Delay(2000, cancellationToken);
                progress.Report("Processing data...");

                // Simulate finalization
                await Task.Delay(1000, cancellationToken);
                progress.Report("Import completed!");

                // Update UI on completion (this would typically be done via event or callback)
            });
        }

        public async Task WaitForImportCompletion()
        {
            await _backgroundWorker.WaitForCompletionAsync();
        }

        public void CancelImport()
        {
            _backgroundWorker.Cancel();
        }

        // Thread-safe data access using Reader-Writer Lock pattern
        public List<TodoItem> GetTodoList()
        {
            return _threadSafeManager.GetTodoList();
        }

        public List<TodoComposite> GetProjects()
        {
            return _threadSafeManager.GetProjects();
        }

        public TodoItem GetTodoItem(int index)
        {
            return _threadSafeManager.GetTodoItem(index);
        }

        // Cleanup
        public async Task ShutdownAsync()
        {
            // Wait for all background tasks to complete
            await _taskQueue.ShutdownAsync();

            // Cancel and wait for background worker
            _backgroundWorker.Cancel();
            await _backgroundWorker.WaitForCompletionAsync();
        }

        private void OnProgressChanged(string message)
        {
            // Update UI with progress (would typically use Invoke for thread safety)
            System.Diagnostics.Debug.WriteLine($"Progress: {message}");
        }

        private void OnCompleted(string message)
        {
            // Handle completion (would typically update UI)
            System.Diagnostics.Debug.WriteLine($"Completed: {message}");
        }

        private void OnError(Exception ex)
        {
            // Handle errors (would typically show error message to user)
            System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
        }

        public void Dispose()
        {
            _threadSafeManager.Dispose();
            _backgroundWorker.Dispose();
        }
    }

    // Extension methods to show integration with existing Form1
    public static class ConcurrencyExtensions
    {
        // Example of how to integrate with existing Form1
        public static async Task<bool> SafeLoadTodoList(this Form1 form, ConcurrencyTodoManager manager)
        {
            try
            {
                await manager.LoadDataAsync();
                form.UpdateUI(); // This would need to be made thread-safe
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load todo list: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static async Task<bool> SafeSaveTodoList(this Form1 form, ConcurrencyTodoManager manager)
        {
            try
            {
                await manager.SaveDataAsync();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save todo list: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}