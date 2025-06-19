using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace to_do_list.Patterns
{
    public class Factory
    {
        public static TodoItem CreateTodoItem(string title)
        {
            return new TodoItem
            {
                Title = title,
                Completed = false
            };
        }
    }
}
