using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace to_do_list.Patterns
{
    // Data structure to hold both todo items and projects
    public class TodoData
    {
        public List<TodoItem> TodoItems { get; set; } = new List<TodoItem>();
        public List<SerializableProject> Projects { get; set; } = new List<SerializableProject>();
    }

    // Serializable project data for JSON storage
    public class SerializableProject
    {
        public string Title { get; set; }
        public bool Completed { get; set; }
        public int Priority { get; set; }
        public string Category { get; set; }
        public DateTime? DueDate { get; set; }
        public List<TodoItem> Tasks { get; set; } = new List<TodoItem>();
    }

    public class TodoStorage
    {
        private static readonly Lazy<TodoStorage> instance = new Lazy<TodoStorage>(() => new TodoStorage());
        public static TodoStorage Instance => instance.Value;

        private string filePath = "todo.json";

        private TodoStorage() { }

        public List<TodoItem> Load()
        {
            if (!File.Exists(filePath))
                return new List<TodoItem>();

            string json = File.ReadAllText(filePath);
            
            // Try to deserialize as TodoData first (new format)
            try
            {
                var todoData = JsonSerializer.Deserialize<TodoData>(json);
                return todoData?.TodoItems ?? new List<TodoItem>();
            }
            catch
            {
                // Fall back to old format (just a list of TodoItem)
                try
                {
                    var todoItems = JsonSerializer.Deserialize<List<TodoItem>>(json);
                    return todoItems ?? new List<TodoItem>();
                }
                catch
                {
                    return new List<TodoItem>();
                }
            }
        }

        public List<TodoComposite> LoadProjects()
        {
            if (!File.Exists(filePath))
                return new List<TodoComposite>();

            string json = File.ReadAllText(filePath);
            
            // Try to deserialize as TodoData first (new format)
            try
            {
                var todoData = JsonSerializer.Deserialize<TodoData>(json);
                var projects = new List<TodoComposite>();
                
                if (todoData?.Projects != null)
                {
                    foreach (var serializableProject in todoData.Projects)
                    {
                        var project = TodoCompositeFactory.CreateProject(
                            serializableProject.Title,
                            serializableProject.Priority,
                            serializableProject.Category,
                            serializableProject.DueDate
                        );
                        project.SetCompleted(serializableProject.Completed);
                        
                        // Add tasks to the project
                        foreach (var task in serializableProject.Tasks)
                        {
                            var todoLeaf = TodoCompositeFactory.CreateTask(task);
                            project.AddChild(todoLeaf);
                        }
                        
                        projects.Add(project);
                    }
                }
                
                return projects;
            }
            catch
            {
                // Old format doesn't have projects
                return new List<TodoComposite>();
            }
        }

        public void Save(List<TodoItem> list, List<TodoComposite> projects)
        {
            var todoData = new TodoData
            {
                TodoItems = list,
                Projects = new List<SerializableProject>()
            };
            
            // Convert TodoComposite projects to SerializableProject
            foreach (var project in projects)
            {
                var serializableProject = new SerializableProject
                {
                    Title = project.Title,
                    Completed = project.Completed,
                    Priority = project.Priority,
                    Category = project.Category,
                    DueDate = project.DueDate,
                    Tasks = new List<TodoItem>()
                };
                
                // Extract tasks from the project
                var leaves = project.GetAllLeaves();
                foreach (var leaf in leaves)
                {
                    serializableProject.Tasks.Add(leaf.GetTodoItem());
                }
                
                todoData.Projects.Add(serializableProject);
            }
            
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(todoData, options);
            File.WriteAllText(filePath, json);
        }

        public void Save(List<TodoItem> list)
        {
            // For backward compatibility, save only todo items
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(list, options);
            File.WriteAllText(filePath, json);
        }
    }
}
