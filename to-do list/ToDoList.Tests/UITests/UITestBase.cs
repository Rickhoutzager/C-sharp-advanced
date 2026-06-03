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

            // FlaUI's .Select() internally calls Expand() which can throw
            // NullReferenceException on DropDownList ComboBoxes whose UIA
            // expand-collapse pattern isn't ready yet. Retry with delays.
            for (int attempt = 0; attempt < 5; attempt++)
            {
                try
                {
                    cb!.Select(itemText);
                    Thread.Sleep(100);
                    return;
                }
                catch (NullReferenceException)
                {
                    if (attempt == 4) throw;
                    Thread.Sleep(300);
                    // Re-find the ComboBox to get a fresh UIA element
                    cb = FindComboBoxByIndex(comboIndex);
                    if (cb == null) throw;
                }
            }
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
                // Try UIA button click first
                var okButton = dialog.FindFirstDescendant(
                    c => c.ByControlType(FlaUI.Core.Definitions.ControlType.Button));
                okButton?.AsButton().Click();

                // Verify the dialog actually closed; if not, use SendKeys fallback.
                // MessageBox buttons in WinForms can be inside a pane that
                // FindFirstDescendant misses, leaving the dialog open and
                // breaking subsequent tests.
                Thread.Sleep(200);
                var stillAlive = WaitForDialog(expectedTitlePart, TimeSpan.FromSeconds(1));
                if (stillAlive != null)
                {
                    // SendKeys fallback: ENTER activates the default button (OK)
                    System.Windows.Forms.SendKeys.SendWait("{ENTER}");
                    Thread.Sleep(300);
                }
            }
            else
            {
                // Dialog not found via UIA — try SendKeys ENTER as last resort
                // (may dismiss a lingering MessageBox that UIA couldn't see)
                System.Windows.Forms.SendKeys.SendWait("{ENTER}");
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
        private static extern void SwitchToThisWindow(nint hWnd, bool fAltTab);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ShowWindow(nint hWnd, uint nCmdShow);

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern nint FindWindow(string? lpClassName, string? lpWindowName);

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern nint FindWindowEx(nint hwndParent, nint hwndChildAfter, string? lpszClass, string? lpszWindow);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern nint GetDlgItem(nint hDlg, int nIDDlgItem);

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern nint SendMessage(nint hWnd, uint Msg, nint wParam, string lParam);

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern nint SendMessage(nint hWnd, uint Msg, nint wParam, nint lParam);

        private const uint WM_SETTEXT = 0x000C;
        private const uint WM_COMMAND = 0x0111;
        private const uint BM_CLICK = 0x00F5;
        private const uint CB_GETCOUNT = 0x0146;
        private const uint CB_SELECTSTRING = 0x014D;
        private const int IDOK = 1;

        protected void FillInputBoxAndClickOk(string inputText)
        {
            var inputDialog = WaitForDialog();
            if (inputDialog == null)
                return;

            // Find the InputBox top-level window. Use a polling retry loop because
            // the Win32 HWND may not be registered yet even though the UIA tree
            // already has the element (timing race between UIA and Win32).
            nint hwnd = nint.Zero;
            var hwndDeadline = DateTime.Now.AddSeconds(3);
            while (hwnd == nint.Zero && DateTime.Now < hwndDeadline)
            {
                try
                {
                    // The VB InputBox title is the second parameter of Interaction.InputBox
                    // e.g. InputBox("Enter project name:", "Create Project", "New Project")
                    hwnd = FindWindow(null, "Create Project");
                    if (hwnd == nint.Zero)
                        hwnd = FindWindow(null, "InputBox");
                }
                catch { }

                if (hwnd == nint.Zero)
                    Thread.Sleep(100);
            }

            if (hwnd == nint.Zero)
            {
                // Last resort: try to bring any dialog to foreground and use SendKeys
                BringWindowToForeground(inputDialog.Properties.NativeWindowHandle);
                Thread.Sleep(200);
                System.Windows.Forms.SendKeys.SendWait("^a");
                Thread.Sleep(100);
                System.Windows.Forms.SendKeys.SendWait(inputText);
                Thread.Sleep(100);
                System.Windows.Forms.SendKeys.SendWait("{ENTER}");
                Thread.Sleep(300);
                return;
            }

            // Get child HWNDs via FindWindowEx with standard Win32 class names.
            // The VB InputBox is a native Win32 dialog with class names "Edit" and "Button".
            nint hEdit = FindWindowEx(hwnd, nint.Zero, "Edit", null);
            if (hEdit == nint.Zero)
                hEdit = FindWindowEx(hwnd, nint.Zero, null, null); // wildcard: any class

            // Set the text directly via WM_SETTEXT (no foreground focus needed).
            if (hEdit != nint.Zero)
            {
                SendMessage(hEdit, WM_SETTEXT, nint.Zero, inputText);
                Thread.Sleep(100);
            }
            else
            {
                // Fallback: use SendKeys if we couldn't find the Edit HWND
                SwitchToThisWindow(hwnd, fAltTab: false);
                Thread.Sleep(200);
                System.Windows.Forms.SendKeys.SendWait("^a");
                Thread.Sleep(100);
                System.Windows.Forms.SendKeys.SendWait(inputText);
                Thread.Sleep(100);
            }

            // Dismiss the InputBox: switch it to foreground and send Enter.
            // WM_COMMAND/GetDlgItem don't work on .NET WinForms dialogs, and
            // UIA's Invoke()/Click() are unreliable on the VB InputBox.
            // Using SendKeys for just the Enter key is safe because we
            // explicitly foreground the InputBox right before sending it.
            SwitchToThisWindow(hwnd, fAltTab: false);
            Thread.Sleep(100);
            System.Windows.Forms.SendKeys.SendWait("{ENTER}");
            Thread.Sleep(300);
        }

        protected int GetListItemCount(int listIndex)
        {
            var listBox = FindListBoxByIndex(listIndex);
            if (listBox == null) return 0;
            return listBox.Items.Length;
        }

        /// <summary>
        /// Safely get the number of items in a ComboBox.
        /// Uses a retry loop with the native Win32 CB_GETCOUNT message because
        /// FlaUI's .Items property internally calls Expand() which throws
        /// NullReferenceException for DataSource-bound DropDownList-style
        /// ComboBoxes (the UIA ExpandCollapse pattern provider is not available
        /// for these controls). Falls back to FlaUI .Items if the native handle
        /// is unavailable.
        /// </summary>
        protected int GetComboBoxItemCountSafe(int comboIndex)
        {
            // Retry loop: the UIA tree and native window handle may need
            // time after programmatic DataSource changes.
            for (int retry = 0; retry < 5; retry++)
            {
                var combo = FindComboBoxByIndex(comboIndex);
                if (combo == null) return 0;

                // Approach 1: Win32 CB_GETCOUNT – bypasses UIA ExpandCollapse
                try
                {
                    var hwnd = combo.Properties.NativeWindowHandle;
                    if (hwnd != nint.Zero)
                    {
                        int count = (int)SendMessage(hwnd, CB_GETCOUNT, nint.Zero, nint.Zero);
                        if (count > 0) return count;
                    }
                }
                catch { /* CB_GETCOUNT may fail if handle is stale; fall through */ }

                // Approach 2: FlaUI .Items (may throw NRE for DataSource-bound combos)
                try
                {
                    return combo.Items.Length;
                }
                catch (NullReferenceException)
                {
                    // UIA ExpandCollapse not ready; retry after delay
                    if (retry == 4) return 0;
                }
                catch
                {
                    return 0;
                }

                Thread.Sleep(300);
            }
            return 0;
        }

        /// <summary>
        /// Select a ComboBox item using the native Win32 CB_SELECTSTRING message.
        /// This bypasses FlaUI's .Select() which internally calls Expand() and
        /// throws NullReferenceException on DataSource-bound DropDownList combos.
        /// Falls back to FlaUI .Select() if the native handle is unavailable.
        /// </summary>
        protected void SelectComboItemNative(int comboIndex, string itemText)
        {
            var cb = FindComboBoxByIndex(comboIndex);
            Assert.NotNull(cb);

            // Try Win32 CB_SELECTSTRING first – avoids UIA ExpandCollapse entirely
            for (int attempt = 0; attempt < 5; attempt++)
            {
                try
                {
                    var hwnd = cb!.Properties.NativeWindowHandle;
                    if (hwnd != nint.Zero)
                    {
                        nint result = SendMessage(hwnd, CB_SELECTSTRING, new nint(-1), itemText);
                        if (result != -1) // CB_ERR
                        {
                            Thread.Sleep(100);
                            return;
                        }
                    }
                }
                catch { /* fall through to FlaUI */ }

                // Fallback: FlaUI's .Select() with retry for NRE
                try
                {
                    cb!.Select(itemText);
                    Thread.Sleep(100);
                    return;
                }
                catch (NullReferenceException)
                {
                    if (attempt == 4) throw;
                    Thread.Sleep(300);
                    cb = FindComboBoxByIndex(comboIndex);
                    if (cb == null) throw;
                }
            }
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