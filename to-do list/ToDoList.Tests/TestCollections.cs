namespace ToDoList.Tests
{
    /// <summary>
    /// Tests that share the on-disk "todo.json" file (Singleton and Concurrency
    /// tests) are placed in this collection so xUnit runs them sequentially
    /// rather than in parallel, preventing file contention.
    /// </summary>
    [CollectionDefinition("Sequential", DisableParallelization = true)]
    public class SequentialCollection
    {
    }
}