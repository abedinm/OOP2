using System.Runtime.InteropServices;

namespace SenseAssist.UI;

static class NativeMethods
{
    [StructLayout(LayoutKind.Sequential)]
    struct FlashInfo
    {
        public uint Size;
        public IntPtr Window;
        public uint Flags;
        public uint Count;
        public uint Timeout;
    }

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool FlashWindowEx(ref FlashInfo info);

    /// <summary>Flash the window and its taskbar button — a visual alert for users who cannot hear sounds.</summary>
    public static void Flash(IntPtr window)
    {
        const uint FlashAll = 3, FlashUntilForeground = 12;
        var info = new FlashInfo
        {
            Size = (uint)Marshal.SizeOf<FlashInfo>(),
            Window = window,
            Flags = FlashAll | FlashUntilForeground,
            Count = 6,
        };
        FlashWindowEx(ref info);
    }
}
