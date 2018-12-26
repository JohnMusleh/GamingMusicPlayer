﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.Diagnostics;

namespace GamingMusicPlayer
{
    //this class creates a low level windows hook to scan keyboard keys, each key is grabbed by subscribing to the event OnKeyPressed  
    //this will be used later to create a signal from keyboard data.
    public static class KeyboardListener
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int WM_SYSKEYDOWN = 0x0104;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
        IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        public static event EventHandler<KeyPressedArgs> OnKeyPressed;

        private static LowLevelKeyboardProc proc=hookCallback;
        private static IntPtr hookid = IntPtr.Zero;


        public static void HookKeyboard()
        {
            hookid = SetHook(proc);
        }
        
        public static void UnHookKeyboard()
        {
            UnhookWindowsHookEx(hookid);
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private static IntPtr hookCallback(int ncode, IntPtr wparam, IntPtr lparam)
        {
            if (ncode >= 0 && wparam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lparam);
                if (OnKeyPressed != null)
                {
                    OnKeyPressed(null, new KeyPressedArgs(KeyInterop.KeyFromVirtualKey(vkCode),true));
                }
            }
            else if (ncode >= 0 && wparam == (IntPtr)WM_KEYUP)
            {
                int vkCode = Marshal.ReadInt32(lparam);
                if (OnKeyPressed != null)
                {
                    OnKeyPressed(null, new KeyPressedArgs(KeyInterop.KeyFromVirtualKey(vkCode), false));
                }
            }
            return CallNextHookEx(hookid, ncode, wparam, lparam);
        }
    }
}