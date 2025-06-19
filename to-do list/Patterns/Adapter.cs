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
            return JsonSerializer.Deserialize<List<TodoItem>>(json) ?? new List<TodoItem>();
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
            return (List<TodoItem>)serializer.Deserialize(reader);
        }

        public void Save(string filePath, List<TodoItem> items)
        {
            var serializer = new XmlSerializer(typeof(List<TodoItem>));
            using var writer = new StreamWriter(filePath);
            serializer.Serialize(writer, items);
        }
    }
}
