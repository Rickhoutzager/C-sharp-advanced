using System;
using System.IO;
using System.Threading;
using Xunit;

namespace ToDoList.Tests.UITests
{
    [Collection("Sequential")]
    public class FileIOUITests : UITestBase
    {
        [Fact]
        public void SaveButton_IsPresentAndClickable()
        {
            var btn = FindButtonByText("Save");
            Assert.NotNull(btn);
            Assert.True(btn!.IsEnabled);
        }

        [Fact]
        public void LoadButton_IsPresentAndClickable()
        {
            var btn = FindButtonByText("Load");
            Assert.NotNull(btn);
            Assert.True(btn!.IsEnabled);
        }

        [Fact]
        public void SaveButton_Click_TriggersDialog()
        {
            ClickButton("Save");
            Thread.Sleep(500);
            DismissFileDialog();
        }

        [Fact]
        public void LoadButton_Click_TriggersDialog()
        {
            ClickButton("Load");
            Thread.Sleep(500);
            DismissFileDialog();
        }

        // Poll for a list to reach an expected item count (with timeout),
        // to avoid race conditions between file dialog acceptance and UI refresh.
        private void AssertListCountEventually(int listIndex, int expectedCount, TimeSpan? timeout = null)
        {
            timeout ??= TimeSpan.FromSeconds(5);
            var start = DateTime.Now;
            int lastCount = -1;

            while (DateTime.Now - start < timeout.Value)
            {
                lastCount = GetListItemCount(listIndex);
                if (lastCount == expectedCount)
                    return;
                Thread.Sleep(200);
            }

            Assert.Equal(expectedCount, lastCount);
        }

        [Fact]
        public void LoadFile_LoadsItemsIntoList()
        {
            // Resolve the path to the test data JSON file (relative to test assembly output)
            string testDataPath = Path.GetFullPath(
                Path.Combine(
                    AppContext.BaseDirectory,
                    @"..\..\..\TestData\test_load_data.json"
                )
            );

            Assert.True(File.Exists(testDataPath),
                $"Test data file not found at: {testDataPath}");

            // Copy the test data to the app's working directory so the file dialog
            // has a simple, accessible path the user can navigate to via SendKeys.
            string appExePath = Path.GetFullPath(
                Path.Combine(
                    AppContext.BaseDirectory,
                    @"..\..\..\..\bin\Debug\net8.0-windows\ToDoList.exe"
                )
            );
            string appDir = Path.GetDirectoryName(appExePath)!;
            Assert.NotNull(appDir);

            string sourcePath = testDataPath;
            string destFileName = "test_load_data.json";
            string destPath = Path.Combine(appDir, destFileName);
            File.Copy(sourcePath, destPath, overwrite: true);

            // Verify lists are empty before loading
            AssertListCountEventually(0, 0);
            AssertListCountEventually(1, 0);

            // Click Load to open the file dialog
            ClickButton("Load");
            Thread.Sleep(1000);

            // Type the file path into the dialog's filename field and press Enter
            System.Windows.Forms.SendKeys.SendWait(destPath);
            Thread.Sleep(500);
            System.Windows.Forms.SendKeys.SendWait("{ENTER}");
            Thread.Sleep(500);

            // After loading, the incomplete list should have 2 items
            // (Test Task 1 is incomplete, Test Task 3 is incomplete)
            // and the complete list should have 1 item (Test Task 2 is complete)
            AssertListCountEventually(0, 2);
            AssertListCountEventually(1, 1);

            // Verify item text contains expected titles
            var item0Text = GetListItemText(0, 0);
            var item1Text = GetListItemText(0, 1);
            Assert.Contains("Test Task 1", item0Text);
            Assert.Contains("Test Task 3", item1Text);

            // Clean up the copied file
            if (File.Exists(destPath))
                File.Delete(destPath);
        }
    }
}
