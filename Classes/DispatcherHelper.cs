using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Composition;
using System.Runtime.InteropServices;
using Windows.System;

namespace BingeBox_WPF.Classes
{
    class DispatcherHelper
    {
        private static DispatcherQueueController dispatcherQueueController;

        public static void EnsureispatcherQueue()
        {
            if(DispatcherQueue.GetForCurrentThread() == null)
            {
                DispatcherQueueOptions options = new DispatcherQueueOptions();
                options.dwSize = (uint)Marshal.SizeOf(typeof(DispatcherQueueOptions));
                options.threadType = 2;
                options.apartmentType = 2;

                CreateDispatcherQueueController(options, out dispatcherQueueController);
            } 
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct DispatcherQueueOptions
        {
            public uint dwSize;
            public int threadType;
            public int apartmentType;
        }

        [DllImport("coremessaging.dll")]
        private static extern int CreateDispatcherQueueController(DispatcherQueueOptions options, out DispatcherQueueController dispatcherQueueController);
    }
}
