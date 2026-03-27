using System;
using System.Threading;
using System.Threading.Tasks;

namespace to_do_list.Patterns
{
    // Background Worker Pattern Implementation
    public class TodoBackgroundWorker : IDisposable
    {
        private CancellationTokenSource _cancellationTokenSource;
        private Task _backgroundTask;

        public event Action<string> ProgressChanged;
        public event Action<string> Completed;
        public event Action<Exception> Error;

        public TodoBackgroundWorker()
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public async Task StartAsync(Func<CancellationToken, IProgress<string>, Task> workFunction)
        {
            if (_backgroundTask != null && !_backgroundTask.IsCompleted)
            {
                throw new InvalidOperationException("Background worker is already running");
            }

            _cancellationTokenSource.Cancel(); // Cancel any previous work
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();

            var progress = new Progress<string>(message => ProgressChanged?.Invoke(message));

            _backgroundTask = Task.Run(async () =>
            {
                try
                {
                    await workFunction(_cancellationTokenSource.Token, progress);
                    Completed?.Invoke("Background operation completed successfully");
                }
                catch (OperationCanceledException)
                {
                    Completed?.Invoke("Background operation was cancelled");
                }
                catch (Exception ex)
                {
                    Error?.Invoke(ex);
                }
            });
        }

        public void Cancel()
        {
            _cancellationTokenSource.Cancel();
        }

        public async Task WaitForCompletionAsync()
        {
            if (_backgroundTask != null)
            {
                await _backgroundTask;
            }
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }
    }
}