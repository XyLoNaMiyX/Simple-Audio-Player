﻿using System;
using System.Windows.Forms;

public static class SuspendUpdate
{
    const int WM_SETREDRAW = 0x000B;

    public static void Suspend(Control control)
    {
        Message msgSuspendUpdate = Message.Create(control.Handle, WM_SETREDRAW, IntPtr.Zero,
            IntPtr.Zero);

        var window = NativeWindow.FromHandle(control.Handle);
        window.DefWndProc(ref msgSuspendUpdate);
    }

    public static void Resume(Control control)
    {
        // Create a C "true" boolean as an IntPtr
        var wparam = new IntPtr(1);
        Message msgResumeUpdate = Message.Create(control.Handle, WM_SETREDRAW, wparam,
            IntPtr.Zero);

        NativeWindow window = NativeWindow.FromHandle(control.Handle);
        window.DefWndProc(ref msgResumeUpdate);

        control.Invalidate();
    }
}