using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace BingeBox_WPF.Classes
{
    public class NativeMethods
    {
        [DllImport("user32.dll")]
        public static extern IntPtr MonitorFromWindow(IntPtr hwnd, int dwFlags);

        [DllImport("user32.dll")]
        public static extern bool GetMonitorInfo(IntPtr hwnd, ref MONITORINFO lpmi);

        [DllImport ("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hwnd, ref RECT lpRect);

        [DllImport("user32.dll")]
        public static extern int SetProcessDpiAwarenessContext(int dpiAwarenessContext);

        [DllImport("user32.dll")]
        public static extern int GetDpiForMonitor(IntPtr hmonitor, int dpiType, out uint dpiX, out uint dpiY);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        public const int SWP_NOMOVE = 0x0002;
        public const int SWP_NOSIZE = 0x0001;
        public const int SWP_NOACTIVE = 0x0010;

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(
            IntPtr hWnd,
            IntPtr hWndInsertAfter,
            int X,
            int Y,
            int cx,
            int cy,
            uint uFlags);

        public const int MONITOR_DPI_TYPE = 0;

        public const int MONITOR_DEFAULTTONEAREST = 0x00000002;

        [StructLayout(LayoutKind.Sequential)]
        public struct MONITORINFO
        {
            public int cbSize;
            public RECT rcMonitor;
            public RECT rcWork;
            public int dwFlags;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public int Width => Right - Left;
            public int Height => Bottom - Top;
        }

        public static bool IsOnMainMonitor(IntPtr hWnd, MONITORINFO monitorInfo)
        {
            RECT windowRect = new RECT();
            GetWindowRect(hWnd, ref windowRect);

            return (windowRect.Left >= monitorInfo.rcMonitor.Left &&
                windowRect.Right <= monitorInfo.rcMonitor.Right &&
                windowRect.Top >= monitorInfo.rcMonitor.Top &&
                windowRect.Bottom <= monitorInfo.rcMonitor.Bottom);
        }

        public static float GetDpiScalingFactor(IntPtr hmonitor)
        {
            uint dpiX, dpiY;

            int result = GetDpiForMonitor(hmonitor, MONITOR_DPI_TYPE, out dpiX, out dpiY);

            if(result == 0)
            {
                return dpiX / 96.0f;
            }
            else
            {
                return 1.0f;
            }
        }
    }
}
