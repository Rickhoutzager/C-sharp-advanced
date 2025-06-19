using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace to_do_list.Patterns
{
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
            return JsonSerializer.Deserialize<List<TodoItem>>(json) ?? new List<TodoItem>();
        }

        public void Save(List<TodoItem> list)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(list, options);
            File.WriteAllText(filePath, json);
        }
    }
}
