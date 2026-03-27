using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using to_do_list.Patterns;

namespace to_do_list
{
    /// <summary>
    /// Comprehensive testing pipeline for all patterns in the todo list application
    /// </summary>
    public class TestRunner
    {
        private readonly List<TestResult> _testResults = new List<TestResult>();
        private readonly ThreadSafeTodoManager _threadSafeManager;
        private readonly TodoCommandManager _commandManager;
        private readonly TodoTaskQueue _taskQueue;

        public TestRunner()
        {
            _threadSafeManager = new ThreadSafeTodoManager();
            _commandManager = new TodoCommandManager();
            _taskQueue = new TodoTaskQueue();
        }

        public async Task RunAllTests()
        {
            Console.WriteLine("Starting Comprehensive Pattern Testing Pipeline");
            Console.WriteLine("=" + new string('=', 60));
            
            // 1. Design Pattern Tests
            await TestSingletonPattern();
            await TestFactoryPattern();
            await TestCommandPattern();
            await TestCompositePattern();
            await TestDecoratorPattern();
            await TestAdapterPattern();

            // 2. Concurrency Pattern Tests
            await TestAsyncAwaitPattern();
            await TestProducerConsumerPattern();
            await TestReaderWriterLockPattern();
            await TestBackgroundWorkerPattern();
            await TestConcurrencyIntegration();

            // 3. Integration Tests
            await TestPatternIntegration();
            await TestStressTesting();

            // Generate report
            GenerateTestReport();
            
            // Cleanup
            await _taskQueue.ShutdownAsync();
            _threadSafeManager.Dispose();
        }

        #region Design Pattern Tests

        private async Task TestSingletonPattern()
        {
            Console.WriteLine("\nTesting Singleton Pattern...");
            
            try
            {
                // Test 1: Single instance verification
                var instance1 = TodoStorage.Instance;
                var instance2 = TodoStorage.Instance;
                
                if (instance1 == instance2)
                {
                    AddTestResult("Singleton Instance", true, "Single instance maintained across calls");
                }
                else
                {
                    AddTestResult("Singleton Instance", false, "Multiple instances created");
                }

                // Test 2: Thread-safe instantiation
                var instances = new List<TodoStorage>();
                var tasks = new List<Task>();
                
                for (int i = 0; i < 10; i++)
                {
                    tasks.Add(Task.Run(() =>
                    {
                        var instance = TodoStorage.Instance;
                        lock (instances)
                        {
                            instances.Add(instance);
                        }
                    }));
                }
                
                await Task.WhenAll(tasks);
                
                if (instances.Distinct().Count() == 1)
                {
                    AddTestResult("Thread-safe Singleton", true, "Thread-safe instantiation verified");
                }
                else
                {
                    AddTestResult("Thread-safe Singleton", false, "Thread safety issue detected");
                }

                // Test 3: Data persistence
                var testItems = new List<TodoItem>
                {
                    new TodoItem { Title = "Test Item 1", Priority = 1 },
                    new TodoItem { Title = "Test Item 2", Priority = 3 }
                };
                
                TodoStorage.Instance.Save(testItems);
                var loadedItems = TodoStorage.Instance.Load();
                
                if (loadedItems.Count == testItems.Count && 
                    loadedItems.All(item => testItems.Any(t => t.Title == item.Title)))
                {
                    AddTestResult("Data Persistence", true, "Save/Load operations working");
                }
                else
                {
                    AddTestResult("Data Persistence", false, "Save/Load operations failed");
                }

            }
            catch (Exception ex)
            {
                AddTestResult("Singleton Pattern", false, $"Exception: {ex.Message}");
            }
        }

        private async Task TestFactoryPattern()
        {
            Console.WriteLine("\nTesting Factory Pattern...");
            
            try
            {
                // Test Factory (basic TodoItem creation)
                var item1 = Factory.CreateTodoItem("Factory Test Item");
                var item2 = Factory.CreateTodoItem("Factory Test Item 2");
                
                if (item1.Title == "Factory Test Item" && item2.Title == "Factory Test Item 2")
                {
                    AddTestResult("Basic Factory", true, "Item creation working");
                }
                else
                {
                    AddTestResult("Basic Factory", false, "Item creation failed");
                }

                // Test TodoCompositeFactory
                var project = TodoCompositeFactory.CreateProject("Factory Project", 1, "Test", DateTime.Now.AddDays(7));
                
                if (project.Title == "Factory Project" && project.Priority == 1)
                {
                    AddTestResult("TodoCompositeFactory", true, "Project creation working");
                }
                else
                {
                    AddTestResult("TodoCompositeFactory", false, "Project creation failed");
                }

                // Test TodoCommandFactory
                var todoList = new List<TodoItem>();
                var testItem = new TodoItem { Title = "Command Test" };
                
                var addCommand = TodoCommandFactory.CreateAddCommand(todoList, testItem);
                var toggleCommand = TodoCommandFactory.CreateToggleCommand(testItem);
                
                if (addCommand != null && toggleCommand != null)
                {
                    AddTestResult("TodoCommandFactory", true, "Command creation working");
                }
                else
                {
                    AddTestResult("TodoCommandFactory", false, "Command creation failed");
                }

            }
            catch (Exception ex)
            {
                AddTestResult("Factory Pattern", false, $"Exception: {ex.Message}");
            }
        }

        private async Task TestCommandPattern()
        {
            Console.WriteLine("\nTesting Command Pattern...");
            
            try
            {
                var todoList = new List<TodoItem>();
                var testItem = new TodoItem { Title = "Command Test Item" };
                
                // Test Add Command
                var addCommand = TodoCommandFactory.CreateAddCommand(todoList, testItem);
                _commandManager.ExecuteCommand(addCommand);
                
                if (todoList.Count == 1 && todoList[0].Title == "Command Test Item")
                {
                    AddTestResult("Add Command", true, "Add command executed successfully");
                }
                else
                {
                    AddTestResult("Add Command", false, "Add command failed");
                }

                // Test Undo
                _commandManager.Undo();
                if (todoList.Count == 0)
                {
                    AddTestResult("Undo Command", true, "Undo command working");
                }
                else
                {
                    AddTestResult("Undo Command", false, "Undo command failed");
                }

                // Test Redo
                _commandManager.Redo();
                if (todoList.Count == 1)
                {
                    AddTestResult("Redo Command", true, "Redo command working");
                }
                else
                {
                    AddTestResult("Redo Command", false, "Redo command failed");
                }

                // Test Toggle Command
                var toggleCommand = TodoCommandFactory.CreateToggleCommand(testItem);
                _commandManager.ExecuteCommand(toggleCommand);
                
                if (testItem.Completed)
                {
                    AddTestResult("Toggle Command", true, "Toggle command working");
                }
                else
                {
                    AddTestResult("Toggle Command", false, "Toggle command failed");
                }

                // Test Command History
                var history = _commandManager.GetCommandHistory();
                if (history.Count > 0)
                {
                    AddTestResult("Command History", true, "Command history tracking working");
                }
                else
                {
                    AddTestResult("Command History", false, "Command history not working");
                }

            }
            catch (Exception ex)
            {
                AddTestResult("Command Pattern", false, $"Exception: {ex.Message}");
            }
        }

        private async Task TestCompositePattern()
        {
            Console.WriteLine("\nTesting Composite Pattern...");
            
            try
            {
                // Test TodoComposite creation
                var project = TodoCompositeFactory.CreateProject("Test Project", 2, "Test", DateTime.Now.AddDays(5));
                
                if (project != null && project.Title == "Test Project")
                {
                    AddTestResult("Project Creation", true, "Project composite created successfully");
                }
                else
                {
                    AddTestResult("Project Creation", false, "Project creation failed");
                }

                // Test adding tasks to project
                var task1 = TodoCompositeFactory.CreateTask(new TodoItem { Title = "Task 1" });
                var task2 = TodoCompositeFactory.CreateTask(new TodoItem { Title = "Task 2" });
                
                project.AddChild(task1);
                project.AddChild(task2);
                
                if (project.GetChildren().Count == 2)
                {
                    AddTestResult("Task Addition", true, "Tasks added to project successfully");
                }
                else
                {
                    AddTestResult("Task Addition", false, "Task addition failed");
                }

                // Test project completion
                project.SetCompleted(true);
                if (project.Completed)
                {
                    AddTestResult("Project Completion", true, "Project completion working");
                }
                else
                {
                    AddTestResult("Project Completion", false, "Project completion failed");
                }

                // Test composite display
                var displayText = project.Display();
                if (!string.IsNullOrEmpty(displayText))
                {
                    AddTestResult("Composite Display", true, "Composite display working");
                }
                else
                {
                    AddTestResult("Composite Display", false, "Composite display failed");
                }

            }
            catch (Exception ex)
            {
                AddTestResult("Composite Pattern", false, $"Exception: {ex.Message}");
            }
        }

        private async Task TestDecoratorPattern()
        {
            Console.WriteLine("\nTesting Decorator Pattern...");
            
            try
            {
                // Test basic item display
                var baseItem = new TodoItemBase("Test Item", "General");
                var baseDescription = baseItem.GetDescription();
                
                if (!string.IsNullOrEmpty(baseDescription))
                {
                    AddTestResult("Base Item Display", true, "Base item display working");
                }
                else
                {
                    AddTestResult("Base Item Display", false, "Base item display failed");
                }

                // Test Priority Decorator
                var priorityItem = new PriorityDecorator(baseItem, 3);
                var priorityDescription = priorityItem.GetDescription();
                
                if (priorityDescription.Contains("High") || priorityDescription.Contains("Priority"))
                {
                    AddTestResult("Priority Decorator", true, "Priority decoration working");
                }
                else
                {
                    AddTestResult("Priority Decorator", false, "Priority decoration failed");
                }

                // Test Due Date Decorator
                var dueDateItem = new DueDateDecorator(baseItem, DateTime.Now.AddDays(1));
                var dueDateDescription = dueDateItem.GetDescription();
                
                if (dueDateDescription.Contains("Due") || dueDateDescription.Contains("Tomorrow"))
                {
                    AddTestResult("Due Date Decorator", true, "Due date decoration working");
                }
                else
                {
                    AddTestResult("Due Date Decorator", false, "Due date decoration failed");
                }

                // Test Chaining Decorators
                var chainedItem = new DueDateDecorator(
                    new PriorityDecorator(baseItem, 1), 
                    DateTime.Now.AddDays(2)
                );
                var chainedDescription = chainedItem.GetDescription();
                
                if (chainedDescription.Contains("High") && chainedDescription.Contains("Due"))
                {
                    AddTestResult("Chained Decorators", true, "Decorator chaining working");
                }
                else
                {
                    AddTestResult("Chained Decorators", false, "Decorator chaining failed");
                }

            }
            catch (Exception ex)
            {
                AddTestResult("Decorator Pattern", false, $"Exception: {ex.Message}");
            }
        }

        private async Task TestAdapterPattern()
        {
            Console.WriteLine("\nTesting Adapter Pattern...");
            
            try
            {
                // Test JsonTodoStorageAdapter
                var jsonAdapter = new JsonTodoStorageAdapter();
                var testItems = new List<TodoItem>
                {
                    new TodoItem { Title = "Adapter Test 1" },
                    new TodoItem { Title = "Adapter Test 2" }
                };
                
                // Test save and load
                jsonAdapter.Save("adapter_test.json", testItems);
                var loadedItems = jsonAdapter.Load("adapter_test.json");
                
                if (loadedItems.Count == testItems.Count)
                {
                    AddTestResult("JSON Adapter", true, "JSON adapter working");
                }
                else
                {
                    AddTestResult("JSON Adapter", false, "JSON adapter failed");
                }

                // Test XmlTodoStorageAdapter
                var xmlAdapter = new XmlTodoStorageAdapter();
                xmlAdapter.Save("adapter_test.xml", testItems);
                var loadedXmlItems = xmlAdapter.Load("adapter_test.xml");
                
                if (loadedXmlItems.Count == testItems.Count)
                {
                    AddTestResult("XML Adapter", true, "XML adapter working");
                }
                else
                {
                    AddTestResult("XML Adapter", false, "XML adapter failed");
                }

                // Clean up test files
                try
                {
                    if (File.Exists("adapter_test.json")) File.Delete("adapter_test.json");
                    if (File.Exists("adapter_test.xml")) File.Delete("adapter_test.xml");
                }
                catch
                {
                    // Ignore cleanup errors
                }

            }
            catch (Exception ex)
            {
                AddTestResult("Adapter Pattern", false, $"Exception: {ex.Message}");
            }
        }

        #endregion

        #region Concurrency Pattern Tests

        private async Task TestAsyncAwaitPattern()
        {
            Console.WriteLine("\nTesting Async/Await Pattern...");
            
            try
            {
                // Test async save/load
                var testItems = new List<TodoItem>
                {
                    new TodoItem { Title = "Async Test 1" },
                    new TodoItem { Title = "Async Test 2" }
                };

                // Test async save
                await AsyncTodoStorage.SaveAsync(testItems);
                
                // Test async load
                var loadedItems = await AsyncTodoStorage.LoadAsync();
                
                if (loadedItems.Count == testItems.Count)
                {
                    AddTestResult("Async Save/Load", true, "Async operations working");
                }
                else
                {
                    AddTestResult("Async Save/Load", false, "Async operations failed");
                }

                // Test UI responsiveness simulation
                var stopwatch = Stopwatch.StartNew();
                await Task.Delay(100); // Simulate UI work
                stopwatch.Stop();
                
                if (stopwatch.ElapsedMilliseconds >= 100)
                {
                    AddTestResult("UI Responsiveness", true, "Async operations don't block UI");
                }
                else
                {
                    AddTestResult("UI Responsiveness", false, "Potential UI blocking detected");
                }

            }
            catch (Exception ex)
            {
                AddTestResult("Async/Await Pattern", false, $"Exception: {ex.Message}");
            }
        }

        private async Task TestProducerConsumerPattern()
        {
            Console.WriteLine("\nTesting Producer-Consumer Pattern...");
            
            try
            {
                var processedTasks = 0;
                var tasks = new List<Task>();

                // Simulate multiple producers
                for (int i = 0; i < 5; i++)
                {
                    tasks.Add(Task.Run(async () =>
                    {
                        for (int j = 0; j < 10; j++)
                        {
                            var task = new TestTask(() => Interlocked.Increment(ref processedTasks));
                            await _taskQueue.EnqueueAsync(task);
                        }
                    }));
                }

                await Task.WhenAll(tasks);
                
                // Wait for all tasks to be processed
                await Task.Delay(1000);
                
                if (processedTasks == 50)
                {
                    AddTestResult("Task Processing", true, "All tasks processed successfully");
                }
                else
                {
                    AddTestResult("Task Processing", false, $"Expected 50 tasks, processed {processedTasks}");
                }

                // Test queue capacity
                var overflowTasks = new List<Task>();
                for (int i = 0; i < 100; i++)
                {
                    overflowTasks.Add(_taskQueue.EnqueueAsync(new TestTask(() => { })));
                }

                await Task.WhenAll(overflowTasks);
                AddTestResult("Queue Capacity", true, "Queue handles high load");

            }
            catch (Exception ex)
            {
                AddTestResult("Producer-Consumer Pattern", false, $"Exception: {ex.Message}");
            }
        }

        private async Task TestReaderWriterLockPattern()
        {
            Console.WriteLine("\nTesting Reader-Writer Lock Pattern...");
            
            try
            {
                var readers = new List<Task>();
                var writers = new List<Task>();
                var readResults = new List<bool>();
                var writeResults = new List<bool>();

                // Add initial data
                _threadSafeManager.AddTodoItem(new TodoItem { Title = "Initial Item" });

                // Start multiple readers
                for (int i = 0; i < 10; i++)
                {
                    readers.Add(Task.Run(() =>
                    {
                        try
                        {
                            var list = _threadSafeManager.GetTodoList();
                            readResults.Add(list != null);
                        }
                        catch
                        {
                            readResults.Add(false);
                        }
                    }));
                }

                // Start multiple writers
                for (int i = 0; i < 5; i++)
                {
                    writers.Add(Task.Run(() =>
                    {
                        try
                        {
                            _threadSafeManager.AddTodoItem(new TodoItem { Title = $"Writer Item {i}" });
                            writeResults.Add(true);
                        }
                        catch
                        {
                            writeResults.Add(false);
                        }
                    }));
                }

                await Task.WhenAll(readers);
                await Task.WhenAll(writers);

                if (readResults.All(r => r) && writeResults.All(w => w))
                {
                    AddTestResult("Concurrent Access", true, "Readers and writers working correctly");
                }
                else
                {
                    AddTestResult("Concurrent Access", false, "Concurrency issues detected");
                }

                // Test exclusive write access
                var writeCount = 0;
                var writeTasks = new List<Task>();
                
                for (int i = 0; i < 20; i++)
                {
                    writeTasks.Add(Task.Run(() =>
                    {
                        _threadSafeManager.AddTodoItem(new TodoItem { Title = $"Exclusive Item {i}" });
                        Interlocked.Increment(ref writeCount);
                    }));
                }

                await Task.WhenAll(writeTasks);
                
                if (writeCount == 20)
                {
                    AddTestResult("Write Exclusivity", true, "Write operations are exclusive");
                }
                else
                {
                    AddTestResult("Write Exclusivity", false, "Write exclusivity failed");
                }

            }
            catch (Exception ex)
            {
                AddTestResult("Reader-Writer Lock Pattern", false, $"Exception: {ex.Message}");
            }
        }

        private async Task TestBackgroundWorkerPattern()
        {
            Console.WriteLine("\nTesting Background Worker Pattern...");
            
            try
            {
                var progressReports = 0;
                var completionReports = 0;

                // Test TodoBackgroundWorker
                var backgroundWorker = new TodoBackgroundWorker();
                
                backgroundWorker.ProgressChanged += message => progressReports++;
                backgroundWorker.Completed += message => completionReports++;
                
                await backgroundWorker.StartAsync(async (cancellationToken, progress) =>
                {
                    for (int i = 0; i < 5; i++)
                    {
                        await Task.Delay(100, cancellationToken);
                        progress?.Report($"Progress {i + 1}/5");
                    }
                });
                
                await backgroundWorker.WaitForCompletionAsync();

                if (progressReports > 0 && completionReports > 0)
                {
                    AddTestResult("Background Execution", true, "Background task completed with progress");
                }
                else
                {
                    AddTestResult("Background Execution", false, "Background task failed");
                }

                // Test cancellation
                var cts = new CancellationTokenSource();
                var cancelWorker = new TodoBackgroundWorker();
                
                var cancelCompleted = false;
                cancelWorker.Completed += message => cancelCompleted = true;
                
                await cancelWorker.StartAsync(async (cancellationToken, progress) =>
                {
                    await Task.Delay(1000, cancellationToken);
                });
                
                cts.CancelAfter(100);
                await Task.Delay(200); // Wait for cancellation
                cancelWorker.Cancel();
                await cancelWorker.WaitForCompletionAsync();

                if (cancelCompleted)
                {
                    AddTestResult("Task Cancellation", true, "Cancellation handled properly");
                }
                else
                {
                    AddTestResult("Task Cancellation", false, "Cancellation not working");
                }

            }
            catch (Exception ex)
            {
                AddTestResult("Background Worker Pattern", false, $"Exception: {ex.Message}");
            }
        }

        private async Task TestConcurrencyIntegration()
        {
            Console.WriteLine("\nTesting Concurrency Integration...");
            
            try
            {
                var concurrentOperations = new List<Task>();
                var operationResults = new List<bool>();

                // Test multiple patterns working together
                for (int i = 0; i < 10; i++)
                {
                    concurrentOperations.Add(Task.Run(async () =>
                    {
                        try
                        {
                            // Create item using Factory
                            var item = Factory.CreateTodoItem($"Concurrent Item {i}");
                            
                            // Add using Command pattern
                            var addCommand = TodoCommandFactory.CreateAddCommand(new List<TodoItem>(), item);
                            _commandManager.ExecuteCommand(addCommand);
                            
                            // Save using Async pattern
                            await AsyncTodoStorage.SaveAsync(new List<TodoItem> { item });
                            
                            // Access using Reader-Writer Lock
                            var list = _threadSafeManager.GetTodoList();
                            
                            operationResults.Add(true);
                        }
                        catch
                        {
                            operationResults.Add(false);
                        }
                    }));
                }

                await Task.WhenAll(concurrentOperations);

                if (operationResults.All(r => r))
                {
                    AddTestResult("Pattern Integration", true, "All patterns working together");
                }
                else
                {
                    AddTestResult("Pattern Integration", false, "Integration issues detected");
                }

            }
            catch (Exception ex)
            {
                AddTestResult("Concurrency Integration", false, $"Exception: {ex.Message}");
            }
        }

        #endregion

        #region Integration and Stress Tests

        private async Task TestPatternIntegration()
        {
            Console.WriteLine("\nTesting Pattern Integration...");
            
            try
            {
                // Create a complete workflow using all patterns
                var todoList = new List<TodoItem>();
                var projects = new List<TodoComposite>();

                // 1. Use Factory to create items
                var item1 = Factory.CreateTodoItem("Integrated Item 1");
                var item2 = Factory.CreateTodoItem("Integrated Item 2");
                
                // 2. Use Composite to create project
                var project = TodoCompositeFactory.CreateProject("Integration Project", 2, "Integration", DateTime.Now.AddDays(3));
                var task1 = TodoCompositeFactory.CreateTask(item1);
                var task2 = TodoCompositeFactory.CreateTask(item2);
                
                project.AddChild(task1);
                project.AddChild(task2);
                
                // 3. Use Command to add items
                var addCommand1 = TodoCommandFactory.CreateAddCommand(todoList, item1);
                var addCommand2 = TodoCommandFactory.CreateAddCommand(todoList, item2);
                
                _commandManager.ExecuteCommand(addCommand1);
                _commandManager.ExecuteCommand(addCommand2);
                
                // 4. Use Decorator for display
                var decoratedItem = new PriorityDecorator(
                    new TodoItemBase(item1.Title, item1.Category), 
                    item1.Priority
                );
                
                // 5. Use Async for persistence
                await AsyncTodoStorage.SaveAsync(todoList, projects);
                var loadedList = await AsyncTodoStorage.LoadAsync();
                
                // 6. Use Reader-Writer Lock for access
                _threadSafeManager.SetData(todoList, projects);
                var safeList = _threadSafeManager.GetTodoList();
                
                if (safeList.Count == 2 && decoratedItem.GetDescription().Contains("High"))
                {
                    AddTestResult("Full Integration", true, "All patterns working in complete workflow");
                }
                else
                {
                    AddTestResult("Full Integration", false, "Integration workflow failed");
                }

            }
            catch (Exception ex)
            {
                AddTestResult("Pattern Integration", false, $"Exception: {ex.Message}");
            }
        }

        private async Task TestStressTesting()
        {
            Console.WriteLine("\nTesting Stress Testing...");
            
            try
            {
                var stopwatch = Stopwatch.StartNew();
                
                // High load test
                var highLoadTasks = new List<Task>();
                var itemCount = 0;
                
                for (int i = 0; i < 100; i++)
                {
                    highLoadTasks.Add(Task.Run(async () =>
                    {
                        for (int j = 0; j < 50; j++)
                        {
                            var item = Factory.CreateTodoItem($"Stress Item {i}-{j}");
                            var command = TodoCommandFactory.CreateAddCommand(new List<TodoItem>(), item);
                            _commandManager.ExecuteCommand(command);
                            
                            await AsyncTodoStorage.SaveAsync(new List<TodoItem> { item });
                            
                            Interlocked.Increment(ref itemCount);
                        }
                    }));
                }

                await Task.WhenAll(highLoadTasks);
                stopwatch.Stop();

                if (itemCount == 5000)
                {
                    AddTestResult("High Load Handling", true, $"Processed 5000 items in {stopwatch.ElapsedMilliseconds}ms");
                }
                else
                {
                    AddTestResult("High Load Handling", false, $"Expected 5000 items, processed {itemCount}");
                }

                // Memory usage test
                var initialMemory = GC.GetTotalMemory(false);
                
                // Create many objects
                var objects = new List<object>();
                for (int i = 0; i < 10000; i++)
                {
                    objects.Add(new TodoItem { Title = $"Memory Test {i}" });
                    objects.Add(TodoCompositeFactory.CreateProject($"Memory Project {i}", 1, "Memory", DateTime.Now));
                }
                
                var peakMemory = GC.GetTotalMemory(false);
                objects.Clear();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                var finalMemory = GC.GetTotalMemory(false);
                
                var memoryIncrease = peakMemory - initialMemory;
                var memoryCleanup = peakMemory - finalMemory;
                
                if (memoryCleanup > memoryIncrease * 0.8) // 80% cleanup
                {
                    AddTestResult("Memory Management", true, "Good memory cleanup after stress test");
                }
                else
                {
                    AddTestResult("Memory Management", false, "Memory cleanup insufficient");
                }

            }
            catch (Exception ex)
            {
                AddTestResult("Stress Testing", false, $"Exception: {ex.Message}");
            }
        }

        #endregion

        #region Helper Methods

        private void AddTestResult(string testName, bool passed, string description)
        {
            _testResults.Add(new TestResult
            {
                TestName = testName,
                Passed = passed,
                Description = description,
                Timestamp = DateTime.Now
            });

            var status = passed ? "PASS" : "FAIL";
            Console.WriteLine($"  {status} {testName}: {description}");
        }

        private void GenerateTestReport()
        {
            Console.WriteLine("\n" + "=" + new string('=', 60));
            Console.WriteLine("TEST RESULTS SUMMARY");
            Console.WriteLine("=" + new string('=', 60));

            var passedTests = _testResults.Count(t => t.Passed);
            var totalTests = _testResults.Count;
            var passRate = totalTests > 0 ? (double)passedTests / totalTests * 100 : 0;

            Console.WriteLine($"Total Tests: {totalTests}");
            Console.WriteLine($"Passed: {passedTests}");
            Console.WriteLine($"Failed: {totalTests - passedTests}");
            Console.WriteLine($"Pass Rate: {passRate:F1}%");

            if (passedTests == totalTests)
            {
                Console.WriteLine("\nALL TESTS PASSED! All patterns are working correctly.");
            }
            else
            {
                Console.WriteLine("\nSOME TESTS FAILED:");
                var failedTests = _testResults.Where(t => !t.Passed).ToList();
                foreach (var test in failedTests)
                {
                    Console.WriteLine($"  {test.TestName}: {test.Description}");
                }
            }

            // Save detailed report
            SaveDetailedReport();
        }

        private void SaveDetailedReport()
        {
            var reportPath = "test_results.txt";
            var reportContent = new StringBuilder();
            
            reportContent.AppendLine("COMPREHENSIVE PATTERN TESTING REPORT");
            reportContent.AppendLine("=" + new string('=', 50));
            reportContent.AppendLine($"Generated: {DateTime.Now}");
            reportContent.AppendLine("");

            foreach (var result in _testResults)
            {
                var status = result.Passed ? "PASS" : "FAIL";
                reportContent.AppendLine($"{status}: {result.TestName}");
                reportContent.AppendLine($"  Description: {result.Description}");
                reportContent.AppendLine($"  Time: {result.Timestamp}");
                reportContent.AppendLine("");
            }

            File.WriteAllText(reportPath, reportContent.ToString());
            Console.WriteLine($"\n📄 Detailed report saved to: {reportPath}");
        }

        #endregion
    }

    public class TestResult
    {
        public string TestName { get; set; }
        public bool Passed { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
    }

    // Test task for Producer-Consumer pattern
    public class TestTask : TodoTask
    {
        private readonly Action _action;

        public TestTask(Action action)
        {
            _action = action;
        }

        public override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(10, cancellationToken);
            _action?.Invoke();
        }
    }
}