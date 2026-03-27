using System;
using System.Collections.Generic;
using System.Threading;

namespace to_do_list.Patterns
{
    // Reader-Writer Lock Pattern Implementation
    public class ThreadSafeTodoManager : IDisposable
    {
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        private List<TodoItem> _todoList = new List<TodoItem>();
        private List<TodoComposite> _projects = new List<TodoComposite>();

        // Reader operations (multiple concurrent readers)
        public List<TodoItem> GetTodoList()
        {
            _lock.EnterReadLock();
            try
            {
                return new List<TodoItem>(_todoList);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public List<TodoComposite> GetProjects()
        {
            _lock.EnterReadLock();
            try
            {
                return new List<TodoComposite>(_projects);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public TodoItem GetTodoItem(int index)
        {
            _lock.EnterReadLock();
            try
            {
                return index >= 0 && index < _todoList.Count ? _todoList[index] : null;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        // Writer operations (exclusive access)
        public void AddTodoItem(TodoItem item)
        {
            _lock.EnterWriteLock();
            try
            {
                _todoList.Add(item);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public void UpdateTodoItem(int index, TodoItem updatedItem)
        {
            _lock.EnterWriteLock();
            try
            {
                if (index >= 0 && index < _todoList.Count)
                {
                    _todoList[index] = updatedItem;
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public void RemoveTodoItem(int index)
        {
            _lock.EnterWriteLock();
            try
            {
                if (index >= 0 && index < _todoList.Count)
                {
                    _todoList.RemoveAt(index);
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public void AddProject(TodoComposite project)
        {
            _lock.EnterWriteLock();
            try
            {
                _projects.Add(project);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public void UpdateProject(int index, TodoComposite updatedProject)
        {
            _lock.EnterWriteLock();
            try
            {
                if (index >= 0 && index < _projects.Count)
                {
                    _projects[index] = updatedProject;
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public void RemoveProject(int index)
        {
            _lock.EnterWriteLock();
            try
            {
                if (index >= 0 && index < _projects.Count)
                {
                    _projects.RemoveAt(index);
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public void SetData(List<TodoItem> todoList, List<TodoComposite> projects)
        {
            _lock.EnterWriteLock();
            try
            {
                _todoList = new List<TodoItem>(todoList);
                _projects = new List<TodoComposite>(projects);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public void Dispose()
        {
            _lock?.Dispose();
        }
    }
}