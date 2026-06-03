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
        /// Searches both top-level desktop children AND descendants of the main window,
        /// because WinForms MessageBox dialogs are owned by the form and may appear
        /// in either place in the UIA tree.
        /// </summary>
        protected FlaUI.Core.AutomationElements.Window? WaitForDialog(string? titleContains = null, TimeSpan? timeout = null)
        {
            timeout ??= TimeSpan.FromSeconds(5);
            var startTime = DateTime.Now;

            while (DateTime.Now - startTime < timeout.Value)
            {
                // Search 1: top-level desktop children (catches most dialogs)
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

                // Search 2: descendants of the main window (catches owned MessageBox dialogs
                // that WinForms shows as child windows in the UIA tree)
                if (mainWindow != null)
                {
                    try
                    {
                        var childWindows = mainWindow.FindAllDescendants(
                            cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Window));
                        foreach (var window in childWindows)
                        {
                            if (titleContains == null ||
                                window.Name.Contains(titleContains, StringComparison.OrdinalIgnoreCase))
                            {
                                return window.AsWindow();
                            }
                        }
                    }
                    catch { /* UIA tree may be in flux; retry */ }
                }

                Thread.Sleep(200);
            }
            return null;
        }

        /// <summary>
        /// Click OK on a MessageBox and wait for it to close.
        /// Uses PostMessage(BM_CLICK) as the primary approach — PostMessage is
        /// asynchronous and never blocks even when the UI thread is inside a
        /// modal MessageBox pump. Falls back to UIA Invoke if the button HWND
        /// cannot be found via Win32.
        /// No SendKeys, no SetForegroundWindow.
        /// </summary>
        protected void ClickMessageBoxOk(string? expectedTitlePart = null)
        {
            var dialog = WaitForDialog(expectedTitlePart, TimeSpan.FromSeconds(5));
            if (dialog == null)
                return; // dialog never appeared or already closed

            nint dialogHwnd = nint.Zero;
            try { dialogHwnd = dialog.Properties.NativeWindowHandle; } catch { }

            // --- Approach 1: PostMessage(BM_CLICK) to the first Button child ---
            // PostMessage is fire-and-forget — it never blocks even when the UI
            // thread is inside a modal MessageBox message pump.
            if (dialogHwnd != nint.Zero)
            {
                // Walk the child chain to find all Button HWNDs
                nint hBtn = FindWindowEx(dialogHwnd, nint.Zero, "Button", null);
                if (hBtn != nint.Zero)
                {
                    PostMessage(hBtn, BM_CLICK, nint.Zero, nint.Zero);
                    Thread.Sleep(400);
                    if (WaitForDialog(expectedTitlePart, TimeSpan.FromSeconds(1)) == null)
                        return; // closed successfully
                }
            }

            // --- Approach 2: UIA Invoke on the OK button ---
            // Only reached if PostMessage didn't find a Button child (rare).
            try
            {
                var okButton = dialog.FindFirstDescendant(
                    c => c.ByControlType(FlaUI.Core.Definitions.ControlType.Button));
                if (okButton != null)
                {
                    var invokePattern = okButton.Patterns.Invoke.PatternOrDefault;
                    if (invokePattern != null)
                        invokePattern.Invoke();
                    else
                        okButton.AsButton().Click();
                    Thread.Sleep(300);
                }
            }
            catch { }
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

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern nint GetParent(nint hWnd);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int GetDlgCtrlID(nint hWnd);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(nint hWnd, out uint lpdwProcessId);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        private static extern bool PostMessage(nint hWnd, uint Msg, nint wParam, nint lParam);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool IsWindowVisible(nint hWnd);

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern nint SendMessage(nint hWnd, uint Msg, nint wParam, string lParam);

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern nint SendMessage(nint hWnd, uint Msg, nint wParam, nint lParam);

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern nint SendMessage(nint hWnd, uint Msg, nint wParam, System.Text.StringBuilder lParam);

        private const uint WM_SETTEXT = 0x000C;
        private const uint WM_COMMAND = 0x0111;
        private const uint BM_CLICK = 0x00F5;
        private const uint CB_GETCOUNT = 0x0146;
        private const uint CB_FINDSTRING = 0x014C;
        private const uint CB_SELECTSTRING = 0x014D;
        private const uint CB_GETCURSEL = 0x0147;
        private const uint CB_GETLBTEXT = 0x0148;
        private const uint CB_GETLBTEXTLEN = 0x0149;
        private const uint CB_SETCURSEL = 0x014E;
        private const int IDOK = 1;

        protected void FillInputBoxAndClickOk(string inputText)
        {
            // ==================================================================
            // Approach 1 — UIA: find the dialog via the UIA tree.
            // ==================================================================
            var inputDialog = WaitForDialog(null, TimeSpan.FromSeconds(3));

            // ==================================================================
            // Approach 2 — Win32: find the VB InputBox by its native HWND.
            // Use a polling retry loop because the HWND may not be registered
            // yet even though the dialog is visible (timing race between the
            // Win32 window manager and UIA).
            // ==================================================================
            nint hwnd = nint.Zero;
            var hwndDeadline = DateTime.Now.AddSeconds(4);
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

            // ==================================================================
            // Approach 3 — Last resort: use SendKeys blindly.
            // ==================================================================
            if (hwnd == nint.Zero && inputDialog == null)
            {
                // We have no handle to the dialog at all.
                // Try to bring whatever is in front into focus and use SendKeys.
                Thread.Sleep(300);
                System.Windows.Forms.SendKeys.SendWait("^a");
                Thread.Sleep(100);
                System.Windows.Forms.SendKeys.SendWait(inputText);
                Thread.Sleep(100);
                System.Windows.Forms.SendKeys.SendWait("{ENTER}");
                Thread.Sleep(500);
                return;
            }

            // ==================================================================
            // If we have the Win32 HWND, interact via native messages.
            // ==================================================================
            if (hwnd != nint.Zero)
            {
                // Get child HWNDs via FindWindowEx with standard Win32 class names.
                // The VB InputBox is a native Win32 dialog with class names "Edit" and "Button".
                nint hEdit = FindWindowEx(hwnd, nint.Zero, "Edit", null);

                // Set the text directly via WM_SETTEXT (no foreground focus needed).
                if (hEdit != nint.Zero)
                {
                    SendMessage(hEdit, WM_SETTEXT, nint.Zero, inputText);
                    Thread.Sleep(100);
                }

                // Dismiss the InputBox via BM_CLICK on the OK button.
                // This avoids SendKeys/SetForegroundWindow which can steal focus
                // and cause buffered keystrokes to reach the main form.
                // The VB InputBox OK button is the first Button child.
                nint hOkBtn = FindWindowEx(hwnd, nint.Zero, "Button", null);
                if (hOkBtn != nint.Zero)
                {
                    SendMessage(hOkBtn, BM_CLICK, nint.Zero, nint.Zero);
                    Thread.Sleep(300);
                    return;
                }

                // Fallback: if BM_CLICK didn't work, use SendKeys as last resort
                // (only foreground the InputBox itself, not the main form)
                SwitchToThisWindow(hwnd, fAltTab: false);
                Thread.Sleep(100);
                System.Windows.Forms.SendKeys.SendWait("{ENTER}");
                Thread.Sleep(300);
                return;
            }

            // ==================================================================
            // We have the UIA element but not the Win32 HWND.
            // Try foreground + SendKeys.
            // ==================================================================
            if (inputDialog != null)
            {
                BringWindowToForeground(inputDialog.Properties.NativeWindowHandle);
                Thread.Sleep(200);
                System.Windows.Forms.SendKeys.SendWait("^a");
                Thread.Sleep(100);
                System.Windows.Forms.SendKeys.SendWait(inputText);
                Thread.Sleep(100);
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
        /// Select a ComboBox item using native Win32 messages.
        /// For AddString-style combos (attempt 0): CB_SELECTSTRING.
        /// For DataSource-bound DropDownList combos (attempt 1+):
        /// CB_FINDSTRING to find the item index + CB_SETCURSEL to select it.
        /// Verification runs inside the retry loop so transient failures
        /// (DataSource rebinding delays, handle recreation) are retried
        /// rather than aborting the test immediately.
        /// </summary>
        protected void SelectComboItemNative(int comboIndex, string itemText)
        {
            // If a dialog is still open (e.g. a MessageBox blocking the UI thread),
            // SendMessage to the combo box will deadlock. Dismiss any lingering dialog first.
            var blocker = WaitForDialog(null, TimeSpan.FromSeconds(1));
            if (blocker != null)
                ClickMessageBoxOk(null);

            var cb = FindComboBoxByIndex(comboIndex);
            Assert.NotNull(cb);

            for (int attempt = 0; attempt < 10; attempt++)
            {
                try
                {
                    var hwnd = cb!.Properties.NativeWindowHandle;
                    if (hwnd == nint.Zero)
                        throw new InvalidOperationException("ComboBox has no window handle");

                    if (attempt == 0)
                    {
                        // First attempt: Win32 CB_SELECTSTRING (works for AddString-
                        // style combos but returns CB_ERR for DataSource-bound ones).
                        nint result = SendMessage(hwnd, CB_SELECTSTRING, new nint(-1), itemText);
                        if (result != -1)
                            Thread.Sleep(150);
                    }
                    else
                    {
                        // DataSource-bound DropDownList: CB_SELECTSTRING returns
                        // CB_ERR, so use CB_FINDSTRING to locate the item index
                        // (starts searching from -1 = beginning) and CB_SETCURSEL
                        // to set the Win32 selection. This works regardless of
                        // whether the UIA ExpandCollapse pattern is available.
                        int idx = (int)SendMessage(hwnd, CB_FINDSTRING, new nint(-1), itemText);
                        if (idx >= 0)
                        {
                            SendMessage(hwnd, CB_SETCURSEL, new nint(idx), nint.Zero);

                            // CB_SETCURSEL moves the Win32 visual selection but does NOT
                            // fire CBN_SELCHANGE, so WinForms' DataSource binding never
                            // updates SelectedIndex — it stays -1 and button handlers
                            // show "No Project Selected". We must send WM_COMMAND with
                            // CBN_SELCHANGE to the form's HWND so WinForms picks it up.
                            // comboBoxProjects is inside a GroupBox, so GetParent() returns
                            // the GroupBox — we walk up to the top-level form window.
                            int ctrlId = GetDlgCtrlID(hwnd);
                            nint parentHwnd = GetParent(hwnd);
                            // Walk up to the top-level window (the Form itself)
                            nint topHwnd = parentHwnd;
                            while (topHwnd != nint.Zero)
                            {
                                nint grandParent = GetParent(topHwnd);
                                if (grandParent == nint.Zero) break;
                                topHwnd = grandParent;
                            }
                            nint targetHwnd = topHwnd != nint.Zero ? topHwnd : parentHwnd;
                            if (targetHwnd != nint.Zero && ctrlId != 0)
                            {
                                const uint CBN_SELCHANGE = 1;
                                nint wParam = new nint((int)((CBN_SELCHANGE << 16) | (ctrlId & 0xFFFF)));
                                SendMessage(targetHwnd, WM_COMMAND, wParam, hwnd);
                            }

                            Thread.Sleep(150);
                        }
                    }

                    // Inline verification (retried on failure)
                    string? selectedText = GetComboBoxSelectedText(comboIndex);
                    if (selectedText != null &&
                        itemText != null &&
                        selectedText.Equals(itemText, StringComparison.OrdinalIgnoreCase))
                    {
                        return; // success
                    }
                }
                catch
                {
                    // Re-acquire the combo box (handle may be recreated after
                    // DataSource rebinding, etc.)
                }

                Thread.Sleep(400);
                cb = FindComboBoxByIndex(comboIndex);
                if (cb == null)
                    throw new InvalidOperationException("ComboBox not found after refresh");
            }

            Assert.Fail(
                $"Failed to select '{itemText}' in ComboBox index {comboIndex}: all attempts exhausted");
        }

        /// <summary>
        /// Read the currently selected text from a ComboBox using Win32
        /// CB_GETCURSEL + CB_GETLBTEXT. Returns null if the handle cannot be
        /// obtained or no item is selected (CB_ERR).
        /// </summary>
        protected string? GetComboBoxSelectedText(int comboIndex)
        {
            var cb = FindComboBoxByIndex(comboIndex);
            if (cb == null) return null;

            try
            {
                var hwnd = cb.Properties.NativeWindowHandle;
                if (hwnd == nint.Zero) return null;

                // Get current selection index
                int selIndex = (int)SendMessage(hwnd, CB_GETCURSEL, nint.Zero, nint.Zero);
                if (selIndex == -1) // CB_ERR — no selection
                    return null;

                // Get the text of the selected item
                int textLen = (int)SendMessage(hwnd, CB_GETLBTEXTLEN, new nint(selIndex), nint.Zero);
                if (textLen <= 0) return null;

                var sb = new System.Text.StringBuilder(textLen + 1);
                SendMessage(hwnd, CB_GETLBTEXT, new nint(selIndex), sb);
                return sb.ToString();
            }
            catch
            {
                return null;
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