using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Gojyuon_KeyHook
{
    abstract class AbstractInterceptKeyboard
    {
        #region Win32 Constants
        protected const int WH_KEYBOARD_LL = 0x000D;
        protected const int WM_KEYDOWN = 0x0100;
        protected const int WM_KEYUP = 0x0101;
        protected const int WM_SYSKEYDOWN = 0x0104;
        protected const int WM_SYSKEYUP = 0x0105;
        #endregion

        #region Win32API Structures
        [StructLayout(LayoutKind.Sequential)]
        public class KBDLLHOOKSTRUCT
        {
            public uint vkCode;
            public uint scanCode;
            public KBDLLHOOKSTRUCTFlags flags;
            public uint time;
            public UIntPtr dwExtraInfo;
        }

        [Flags]
        public enum KBDLLHOOKSTRUCTFlags : uint
        {
            KEYEVENTF_EXTENDEDKEY = 0x0001,
            KEYEVENTF_KEYUP = 0x0002,
            KEYEVENTF_SCANCODE = 0x0008,
            KEYEVENTF_UNICODE = 0x0004,
        }
        #endregion

        #region Win32 Methods
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, KeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
        #endregion

        #region Delegate
        private delegate IntPtr KeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
        #endregion

        #region Fields
        private KeyboardProc proc;
        private IntPtr hookId = IntPtr.Zero;
        #endregion

        public void Hook()
        {
            if (hookId == IntPtr.Zero)
            {
                proc = HookProcedure;
                using (var curProcess = Process.GetCurrentProcess())
                {
                    using (ProcessModule curModule = curProcess.MainModule)
                    {
                        hookId = SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
                    }
                }
            }
        }

        public void UnHook()
        {
            UnhookWindowsHookEx(hookId);
            hookId = IntPtr.Zero;
        }

        public virtual IntPtr HookProcedure(int nCode, IntPtr wParam, IntPtr lParam)
        {
            return CallNextHookEx(hookId, nCode, wParam, lParam);
        }
    }

    class InterceptKeyboard : AbstractInterceptKeyboard
    {
        #region InputEvent
        public class OriginalKeyEventArg : EventArgs
        {
            public Keys KeyCode { get; }

            public bool IsCancel { get; set; } = false;

            public bool IsVirtualInput { get; }

            public OriginalKeyEventArg(Keys keyCode, bool isVirtualInput)
            {
                KeyCode = keyCode;
                IsVirtualInput = isVirtualInput;
            }
        }
        public delegate void KeyEventHandler(object sender, OriginalKeyEventArg e);
        public event KeyEventHandler KeyDownEvent;
        public event KeyEventHandler KeyUpEvent;

        protected OriginalKeyEventArg OnKeyDownEvent(Keys keyCode, bool isVirtualInput)
        {
            var eventArg = new OriginalKeyEventArg(keyCode, isVirtualInput);
            KeyDownEvent?.Invoke(this, eventArg);
            return eventArg;
        }
        protected OriginalKeyEventArg OnKeyUpEvent(Keys keyCode, bool isVirtualInput)
        {
            var eventArg = new OriginalKeyEventArg(keyCode, isVirtualInput);
            KeyUpEvent?.Invoke(this, eventArg);
            return eventArg;
        }
        #endregion

        public override IntPtr HookProcedure(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN))
            {
                var kb = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));
                Keys vkCode = (Keys)(short)Marshal.ReadInt32(lParam);
                var eventArg = OnKeyDownEvent(vkCode, kb.dwExtraInfo.ToUInt32() != 0x0);
                if (eventArg.IsCancel)
                    return new IntPtr(1);
            }
            else if (nCode >= 0 && (wParam == (IntPtr)WM_KEYUP || wParam == (IntPtr)WM_SYSKEYUP))
            {
                var kb = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));
                Keys vkCode = (Keys)(short)Marshal.ReadInt32(lParam);
                var eventArg = OnKeyUpEvent(vkCode, kb.dwExtraInfo.ToUInt32() != 0x0);
                if (eventArg.IsCancel)
                    return new IntPtr(1);
            }

            return base.HookProcedure(nCode, wParam, lParam);
        }
    }
}
