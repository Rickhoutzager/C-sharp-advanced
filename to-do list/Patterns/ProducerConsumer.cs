using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Channels;

namespace to_do_list.Patterns
{
    // Producer-Consumer Pattern Implementation
    public class TodoTaskQueue
    {
        private readonly Channel<TodoTask> _channel;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly Task _backgroundProcessor;

        public TodoTaskQueue(int maxDegreeOfParallelism = 3)
        {
            var options = new BoundedChannelOptions(maxDegreeOfParallelism * 2)
            {
                FullMode = BoundedChannelFullMode.Wait
            };
            _channel = Channel.CreateBounded<TodoTask>(options);
            _cancellationTokenSource = new CancellationTokenSource();
            
            // Start background processor
            _backgroundProcessor = Task.Run(() => ProcessTasksAsync(_cancellationTokenSource.Token));
        }

        public async Task EnqueueAsync(TodoTask task)
        {
            await _channel.Writer.WriteAsync(task, _cancellationTokenSource.Token);
        }

        private async Task ProcessTasksAsync(CancellationToken cancellationToken)
        {
            await foreach (var task in _channel.Reader.ReadAllAsync(cancellationToken))
            {
                try
                {
                    await task.ExecuteAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    // Log error or handle appropriately
                    System.Diagnostics.Debug.WriteLine($"Task failed: {ex.Message}");
                }
            }
        }

        public async Task ShutdownAsync()
        {
            _channel.Writer.Complete();
            _cancellationTokenSource.Cancel();
            await _backgroundProcessor;
        }
    }

    // Base class for async tasks
    public abstract class TodoTask
    {
        public abstract Task ExecuteAsync(CancellationToken cancellationToken);
    }

    // Specific task types
    public class SaveTodoTask : TodoTask
    {
        private readonly List<TodoItem> _todoList;
        private readonly List<TodoComposite> _projects;

        public SaveTodoTask(List<TodoItem> todoList, List<TodoComposite> projects)
        {
            _todoList = todoList;
            _projects = projects;
        }

        public override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(100, cancellationToken); // Simulate async work
            TodoStorage.Instance.Save(_todoList, _projects);
        }
    }

    public class LoadTodoTask : TodoTask
    {
        private readonly Action<List<TodoItem>, List<TodoComposite>> _callback;

        public LoadTodoTask(Action<List<TodoItem>, List<TodoComposite>> callback)
        {
            _callback = callback;
        }

        public override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(100, cancellationToken); // Simulate async work
            var todoList = TodoStorage.Instance.Load();
            var projects = TodoStorage.Instance.LoadProjects();
            _callback(todoList, projects);
        }
    }
}