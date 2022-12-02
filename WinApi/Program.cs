using System.Runtime.InteropServices;

namespace WinApi;

public static class Program
{
    [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern int MessageBox(IntPtr hWnd, string lpText, string lpCaption, uint uType);

    public static void Main()
    {
        MessageBox(IntPtr.Zero, "description", "title", 0);
    }
}