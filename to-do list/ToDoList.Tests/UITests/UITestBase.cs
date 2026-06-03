using System.Diagnostics;
using System.Runtime.InteropServices;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;
using FlaUI.UIA3;
using Xunit;

namespace ToDoList.Tests.UITests
{
    /// <summary>
    /// Base class for WinForms UI tests using FlaUI.
    /// Launches the application, provides helpers to find controls,
    /// and cleans up after each test.
    /// </summary>
    public abstract class UITestBase : IAsyncLifetime
    {
        protected FlaUI.Core.Application? app;
        protected UIA3Automation? automation;
        protected FlaUI.Core.AutomationElements.Window? mainWindow;

        private string? _originalTodoJson;

        protected virtual string AppExePath =>
            Path.GetFullPath(
                Path.Combine(
                    AppContext.BaseDirectory,
                    @"..\..\..\..\bin\Debug\net8.0-windows\ToDoList.exe"
                )
            );

        protected virtual string TodoJsonPath =>
            Path.GetFullPath(
                Path.Combine(
                    AppContext.BaseDirectory,
                    @"..\..\..\..\bin\Debug\net8.0-windows\todo.json"
                )
            );

        protected virtual string MainWindowTitle => "Todo List with Composite Pattern";

        public async Task InitializeAsync()
        {
            if (File.Exists(TodoJsonPath))
                _originalTodoJson = File.ReadAllText(TodoJsonPath);

            if (File.Exists(TodoJsonPath))
                File.Delete(TodoJsonPath);

            if (!File.Exists(AppExePath))
                throw new FileNotFoundException(
                    $"App exe not found at: {AppExePath}");

            var psi = new ProcessStartInfo(AppExePath)
            {
                WorkingDirectory = Path.GetDirectoryName(AppExePath),
                UseShellExecute = false
            };
            app = FlaUI.Core.Application.Launch(psi);
            automation = new UIA3Automation();
            mainWindow = app.GetMainWindow(automation, TimeSpan.FromSeconds(10));
            await Task.Delay(1000);
        }

        public async Task DisposeAsync()
        {
            // Force-close any lingering file dialogs first
            CloseAllFileDialogs();

            try
            {
                mainWindow?.Close();
                if (app?.HasExited == false)
                {
                    var process = System.Diagnostics.Process.GetProcessById(app.ProcessId);
                    process?.Kill(entireProcessTree: true);
                }
            }
            catch { }

            if (_originalTodoJson != null)
                await File.WriteAllTextAsync(TodoJsonPath, _originalTodoJson);
            else if (File.Exists(TodoJsonPath))
                File.Delete(TodoJsonPath);

            automation?.Dispose();
            app?.Dispose();
        }

        // ========== Find controls by Display Text ==========

        protected FlaUI.Core.AutomationElements.Button? FindButtonByText(string text, TimeSpan? timeout = null)
        {
            timeout ??= TimeSpan.FromSeconds(3);
            var start = DateTime.Now;

            while (DateTime.Now - start < timeout.Value)
            {
                var buttons = mainWindow?.FindAllDescendants(
                    cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Button));
                if (buttons != null)
                {
                    // First try exact match
                    foreach (var btn in buttons)
                    {
                        if (btn.Name.Equals(text, StringComparison.OrdinalIgnoreCase))
                        {
                            return btn.AsButton();
                        }
                    }
                    // Fall back to StartsWith if no exact match found
                    foreach (var btn in buttons)
                    {
                        if (btn.Name.StartsWith(text, StringComparison.OrdinalIgnoreCase))
                        {
                            return btn.AsButton();
                        }
                    }
                }
                Thread.Sleep(200);
            }
            return null;
        }

        protected FlaUI.Core.AutomationElements.TextBox? FindTextBoxByIndex(int index, TimeSpan? timeout = null)
        {
            timeout ??= TimeSpan.FromSeconds(3);
            var start = DateTime.Now;

            while (DateTime.Now - start < timeout.Value)
            {
                var textBoxes = mainWindow?.FindAllDescendants(
                    cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Edit));
                if (textBoxes != null && textBoxes.Length > index)
                    return textBoxes[index].AsTextBox();
                Thread.Sleep(200);
            }
            return null;
        }

        protected FlaUI.Core.AutomationElements.ComboBox? FindComboBoxByIndex(int index, TimeSpan? timeout = null)
        {
            timeout ??= TimeSpan.FromSeconds(3);
            var start = DateTime.Now;

            while (DateTime.Now - start < timeout.Value)
            {
                var combos = mainWindow?.FindAllDescendants(
                    cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.ComboBox));
                if (combos != null && combos.Length > index)
                    return combos[index].AsComboBox();
                Thread.Sleep(200);
            }
            return null;
        }

        // Known AutomationIds for the two list boxes (set by the WinForms designer Name property)
        private static readonly string[] ListBoxAutomationIds = { "listBoxIncomplete", "listBoxComplete" };

        protected FlaUI.Core.AutomationElements.ListBox? FindListBoxByIndex(int index, TimeSpan? timeout = null)
        {
            timeout ??= TimeSpan.FromSeconds(3);
            var start = DateTime.Now;

            // If we have a known AutomationId for this index, use it directly (most reliable)
            if (index < ListBoxAutomationIds.Length)
            {
                string automationId = ListBoxAutomationIds[index];
                while (DateTime.Now - start < timeout.Value)
                {
                    var element = mainWindow?.FindFirstDescendant(
                        cf => cf.ByAutomationId(automationId));
                    if (element != null)
                        return element.AsListBox();
                    Thread.Sleep(200);
                }
                return null;
            }

            // Fallback: find by ControlType.List for any index beyond the known ones
            while (DateTime.Now - start < timeout.Value)
            {
                var lists = mainWindow?.FindAllDescendants(
                    cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.List));
                if (lists != null && lists.Length > index)
                    return lists[index].AsListBox();
                Thread.Sleep(200);
            }
            return null;
        }

        protected void ClickButton(string text)
        {
            var btn = FindButtonByText(text);
            Assert.NotNull(btn);
            btn!.Click();
            Thread.Sleep(300);
        }

        protected void TypeIntoTextBox(int index, string text)
        {
            var tb = FindTextBoxByIndex(index);
            Assert.NotNull(tb);
            tb!.Text = string.Empty;
            tb.Text = text;
            Thread.Sleep(100);
        }

        protected void SelectComboItem(int comboIndex, string itemText)
        {
            var cb = FindComboBoxByIndex(comboIndex);
            Assert.NotNull(cb);
            cb!.Select(itemText);
            Thread.Sleep(100);
        }

        /// <summary>
        /// Wait for a modal dialog window to appear (e.g., MessageBox, InputBox, FileDialog).
        /// </summary>
        protected FlaUI.Core.AutomationElements.Window? WaitForDialog(string? titleContains = null, TimeSpan? timeout = null)
        {
            timeout ??= TimeSpan.FromSeconds(5);
            var startTime = DateTime.Now;

            while (DateTime.Now - startTime < timeout.Value)
            {
                var desktop = automation?.GetDesktop();
                if (desktop != null)
                {
                    var allWindows = desktop.FindAllChildren(
                        cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Window));
                    foreach (var window in allWindows)
                    {
                        if (window.Name == MainWindowTitle)
                            continue;

                        if (titleContains == null ||
                            window.Name.Contains(titleContains, StringComparison.OrdinalIgnoreCase))
                        {
                            return window.AsWindow();
                        }
                    }
                }
                Thread.Sleep(200);
            }
            return null;
        }

        /// <summary>
        /// Click OK on a MessageBox and wait for it to close.
        /// </summary>
        protected void ClickMessageBoxOk(string? expectedTitlePart = null)
        {
            var dialog = WaitForDialog(expectedTitlePart);
            if (dialog != null)
            {
                var okButton = dialog.FindFirstDescendant(
                    c => c.ByControlType(FlaUI.Core.Definitions.ControlType.Button));
                okButton?.AsButton().Click();
                Thread.Sleep(300);
            }
        }

        /// <summary>
        /// Dismiss a file dialog using native SendKeys (guaranteed to work on modal dialogs).
        /// </summary>
        protected void DismissFileDialog()
        {
            // Send ESCAPE at the Windows message level using SendKeys
            // This works on modal file dialogs where UIA interactions sometimes fail
            try
            {
                // Use Windows SendKeys to send ESCAPE
                System.Windows.Forms.SendKeys.SendWait("{ESC}");
                Thread.Sleep(500);
                System.Windows.Forms.SendKeys.SendWait("{ESC}");
                Thread.Sleep(500);
                System.Windows.Forms.SendKeys.SendWait("{ESC}");
                Thread.Sleep(500);
            }
            catch
            {
                // If SendKeys fails, try to find and close via UIA
                var dialog = WaitForDialog(null, TimeSpan.FromSeconds(1));
                if (dialog != null)
                {
                    try { dialog.Close(); } catch { }
                }
            }
        }

        /// <summary>
        /// Close any lingering file dialogs by sending Alt+F4 using SendKeys.
        /// </summary>
        private void CloseAllFileDialogs()
        {
            try
            {
                System.Windows.Forms.SendKeys.SendWait("%{F4}");
                Thread.Sleep(200);
                System.Windows.Forms.SendKeys.SendWait("{ESC}");
                Thread.Sleep(200);
            }
            catch { }
        }

        /// <summary>
        /// Bring a window to the foreground using its native HWND.
        /// Returns true if successful.
        /// </summary>
        private static bool BringWindowToForeground(nint hwnd)
        {
            const uint SW_RESTORE = 9;

            bool result = false;
            try
            {
                // If minimized, restore it first
                result = ShowWindow(hwnd, SW_RESTORE);
                // Bring to foreground
                result = SetForegroundWindow(hwnd);
            }
            catch { }
            return result;
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(nint hWnd);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ShowWindow(nint hWnd, uint nCmdShow);

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern nint FindWindow(string? lpClassName, string? lpWindowName);

        protected void FillInputBoxAndClickOk(string inputText)
        {
            var inputDialog = WaitForDialog();
            if (inputDialog != null)
            {
                // Try to find the InputBox by its window title and bring it to foreground
                nint hwnd = nint.Zero;
                try
                {
                    // The VB InputBox title is the second parameter of Interaction.InputBox
                    // e.g. InputBox("Enter project name:", "Create Project", "New Project")
                    hwnd = FindWindow(null, "Create Project");
                    if (hwnd == nint.Zero)
                    {
                        hwnd = FindWindow(null, "InputBox");
                    }
                }
                catch { }

                if (hwnd != nint.Zero)
                {
                    BringWindowToForeground(hwnd);
                    Thread.Sleep(200);
                }

                // Focus the edit field in the InputBox dialog by clicking it via UIA
                var editField = inputDialog.FindFirstDescendant(
                    c => c.ByControlType(FlaUI.Core.Definitions.ControlType.Edit));
                if (editField != null)
                {
                    editField.Click();
                    Thread.Sleep(100);
                }

                // Use System.Windows.Forms.SendKeys which is already working in this file
                // Ctrl+A to select all existing text in the InputBox
                System.Windows.Forms.SendKeys.SendWait("^a");
                Thread.Sleep(100);

                // Type the input text
                System.Windows.Forms.SendKeys.SendWait(inputText);
                Thread.Sleep(100);

                // Press Enter to click the default OK button
                System.Windows.Forms.SendKeys.SendWait("{ENTER}");
                Thread.Sleep(300);
            }
        }

        protected int GetListItemCount(int listIndex)
        {
            var listBox = FindListBoxByIndex(listIndex);
            if (listBox == null) return 0;
            return listBox.Items.Length;
        }

        protected string GetListItemText(int listIndex, int itemIndex)
        {
            var listBox = FindListBoxByIndex(listIndex);
            if (listBox == null || listBox.Items.Length <= itemIndex)
                return string.Empty;
            return listBox.Items[itemIndex].Text;
        }

        protected void SelectListItem(int listIndex, int itemIndex)
        {
            var listBox = FindListBoxByIndex(listIndex);
            if (listBox != null && listBox.Items.Length > itemIndex)
            {
                listBox.Items[itemIndex].Click();
                Thread.Sleep(200);
            }
        }
    }
}