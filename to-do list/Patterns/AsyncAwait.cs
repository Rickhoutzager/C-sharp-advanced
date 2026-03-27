using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace to_do_list.Patterns
{
    // Async/Await Pattern Implementation for File Operations
    public static class AsyncTodoStorage
    {
        private static readonly SemaphoreSlim _saveSemaphore = new SemaphoreSlim(1, 1);

        public static async Task<List<TodoItem>> LoadAsync()
        {
            return await Task.Run(() =>
            {
                return TodoStorage.Instance.Load();
            });
        }

        public static async Task<List<TodoComposite>> LoadProjectsAsync()
        {
            return await Task.Run(() =>
            {
                return TodoStorage.Instance.LoadProjects();
            });
        }

        public static async Task SaveAsync(List<TodoItem> todoList, List<TodoComposite> projects)
        {
            // Use semaphore to prevent concurrent saves
            await _saveSemaphore.WaitAsync();
            try
            {
                await Task.Run(() =>
                {
                    TodoStorage.Instance.Save(todoList, projects);
                });
            }
            finally
            {
                _saveSemaphore.Release();
            }
        }

        public static async Task SaveAsync(List<TodoItem> todoList)
        {
            await _saveSemaphore.WaitAsync();
            try
            {
                await Task.Run(() =>
                {
                    TodoStorage.Instance.Save(todoList);
                });
            }
            finally
            {
                _saveSemaphore.Release();
            }
        }
    }
}