﻿/* MouseListener class is used to record mouse data in real time,
 * creates a low level windows hook to scan mouse movements,
 * this data will be used later to create a signal/function. [read MouseProcessor]*/
using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows;

namespace GamingMusicPlayer.SignalProcessing.Mouse
{
    public static class MouseListener
    {
        public static bool hooked = false;
        private const int WM_MOUSEMOVE = 0x0200;
        private const int WM_LBUTTONDOWN = 0x0201;
        private const int WH_MOUSE_LL = 14;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
        IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        public delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

        public static event EventHandler<GlobalMouseEventArgs> OnMouseMoved;

        private static LowLevelMouseProc proc=hookCallback;
        private static IntPtr hookid = IntPtr.Zero;

 

        public static void HookMouse()
        {
            if (!hooked)
            {
                hookid = SetHook(proc);
                hooked = true;
            }
        }

        public static void UnhookMouse()
        {
            if (hooked)
            {
                UnhookWindowsHookEx(hookid);
                hooked = false;
            }
        }

        private static IntPtr hookCallback(int ncode, IntPtr wparam, IntPtr lparam)
        {
            if (ncode >= 0 && wparam == (IntPtr)WM_MOUSEMOVE)
            {
                MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lparam, typeof(MSLLHOOKSTRUCT));
                if (OnMouseMoved != null)
                {
                    OnMouseMoved(null, new GlobalMouseEventArgs(new Point(hookStruct.pt.x,hookStruct.pt.y)));
                }
            }
            return CallNextHookEx(hookid, ncode, wparam, lparam);
        }

        private static IntPtr SetHook(LowLevelMouseProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_MOUSE_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;

        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOKSTRUCT
        {
            public POINT pt;        
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }
    }
}
