using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace to_do_list.Patterns
{
    public interface ITodoStorageAdapter
    {
        List<TodoItem> Load(string filePath);
        void Save(string filePath, List<TodoItem> items);
    }

    public class JsonTodoStorageAdapter : ITodoStorageAdapter
    {
        public List<TodoItem> Load(string filePath)
        {
            if (!File.Exists(filePath)) return new List<TodoItem>();
            string json = File.ReadAllText(filePath);
            
            // Try new format (TodoData with TodoItems + Projects)
            try
            {
                var todoData = JsonSerializer.Deserialize<TodoData>(json);
                if (todoData?.TodoItems != null)
                    return todoData.TodoItems;
            }
            catch { }
            
            // Fall back to old format (just a list of TodoItem)
            try
            {
                return JsonSerializer.Deserialize<List<TodoItem>>(json) ?? new List<TodoItem>();
            }
            catch
            {
                return new List<TodoItem>();
            }
        }

        public void Save(string filePath, List<TodoItem> items)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(items, options);
            File.WriteAllText(filePath, json);
        }
    }
    public class XmlTodoStorageAdapter : ITodoStorageAdapter
    {
        public List<TodoItem> Load(string filePath)
        {
            if (!File.Exists(filePath)) return new List<TodoItem>();
            var serializer = new XmlSerializer(typeof(List<TodoItem>));
            using var reader = new StreamReader(filePath);
            var result = serializer.Deserialize(reader);
            return result as List<TodoItem> ?? new List<TodoItem>();
        }
        public void Save(string filePath, List<TodoItem> items)
        {
            var serializer = new XmlSerializer(typeof(List<TodoItem>));
            using var writer = new StreamWriter(filePath);
            serializer.Serialize(writer, items);
        }
    }
}
