using System;
using System.Threading.Tasks;
using to_do_list;

namespace to_do_list
{
    /// <summary>
    /// Standalone testing program for the pattern testing pipeline.
    /// Run this file to execute all pattern tests without affecting the main application.
    /// </summary>
    class TestProgram
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("C# Advanced Todo List - Pattern Testing Pipeline");
            Console.WriteLine("=" + new string('=', 60));
            Console.WriteLine("This testing pipeline validates all design patterns and");
            Console.WriteLine("concurrency patterns implemented in the todo list application.");
            Console.WriteLine();

            var testRunner = new TestRunner();
            await testRunner.RunAllTests();

            Console.WriteLine("\nTesting Pipeline Complete!");
            Console.WriteLine("Press any key to exit or just close the window...");
            try
            {
                Console.ReadKey();
            }
            catch
            {
                // Ignore console read errors in headless environments
            }
        }
    }
}
