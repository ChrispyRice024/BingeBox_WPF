using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace BingeBox_WPF.Classes
{
    class MonitorHelper
    {
        public static NativeMethods.RECT GetMonitorDetails(IntPtr hwnd)
        {
            IntPtr monitorHandle = NativeMethods.MonitorFromWindow(hwnd, NativeMethods.MONITOR_DEFAULTTONEAREST);

            if(monitorHandle != IntPtr.Zero)
            {
                var monitorInfo = new NativeMethods.MONITORINFO();
                monitorInfo.cbSize = Marshal.SizeOf(typeof(NativeMethods.MONITORINFO));

                if(NativeMethods.GetMonitorInfo(monitorHandle, ref monitorInfo))
                {
                    var bounds = monitorInfo.rcMonitor;
                    Debug.WriteLine($"Monitor Width: {bounds.Width}\n Monitor Height: {bounds.Height}");
                    return bounds;
                }
                else
                {
                    //Debug.WriteLine($"monitorInfo");
                    Debug.WriteLine("Failed to get the monitor info");
                    return new NativeMethods.RECT();
                }
            }
            else
            {
                Debug.WriteLine("Could not find the current monitor");
                return new NativeMethods.RECT();
            }
        }
    }
}
