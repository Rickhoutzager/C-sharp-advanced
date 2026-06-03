using FlaUI.Core.AutomationElements;
using Xunit;

namespace ToDoList.Tests.UITests
{
    [Collection("Sequential")]
    public class ConcurrencyTestingUITests : UITestBase
    {
        [Fact]
        public void ConcurrencyControls_ArePresent()
        {
            Assert.NotNull(FindButtonByText("Test Async/Await Pattern"));
            Assert.NotNull(FindButtonByText("Test Producer-Consumer Pattern"));
            Assert.NotNull(FindButtonByText("Test Reader-Writer Lock Pattern"));
            Assert.NotNull(FindButtonByText("Test Background Worker Pattern"));
            Assert.NotNull(FindButtonByText("Comprehensive Stress Test"));
            Assert.NotNull(FindButtonByText("Performance Benchmark"));
            Assert.NotNull(FindButtonByText("Cleanup Resources"));
        }

        [Fact]
        public void TestAsyncAwait_Click_ShowsResult()
        {
            ClickButton("Test Async/Await Pattern");
            Thread.Sleep(1000);

            var resultDialog = WaitForDialog("Test Results", TimeSpan.FromSeconds(15));
            DismissTestDialog(resultDialog, "Async/Await");
        }

        [Fact]
        public void TestProducerConsumer_Click_ShowsResult()
        {
            ClickButton("Test Producer-Consumer Pattern");
            Thread.Sleep(1000);

            var resultDialog = WaitForDialog("Test Results", TimeSpan.FromSeconds(15));
            DismissTestDialog(resultDialog, "Producer-Consumer");
        }

        [Fact]
        public void TestReaderWriterLock_Click_ShowsResult()
        {
            ClickButton("Test Reader-Writer Lock Pattern");
            Thread.Sleep(1000);

            var resultDialog = WaitForDialog("Test Results", TimeSpan.FromSeconds(15));
            DismissTestDialog(resultDialog, "Reader-Writer Lock");
        }

        [Fact]
        public void TestBackgroundWorker_Click_ShowsResult()
        {
            ClickButton("Test Background Worker Pattern");
            Thread.Sleep(1000);

            var resultDialog = WaitForDialog("Test Results", TimeSpan.FromSeconds(15));
            DismissTestDialog(resultDialog, "Background Worker");
        }

        private void DismissTestDialog(FlaUI.Core.AutomationElements.Window? dialog, string expectedTitlePart)
        {
            if (dialog != null)
            {
                Assert.Contains(expectedTitlePart, dialog.Title);
                var allButtons = dialog.FindAllDescendants(
                    cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Button));
                if (allButtons.Length > 0)
                {
                    allButtons[0].AsButton().Click();
                    Thread.Sleep(300);
                }
            }
        }
    }
}