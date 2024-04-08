using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace window_creation
{
    public static class Program
    {
        private static void Main()
        {
            var nativeWindowSettings = new NativeWindowSettings()
            {
                ClientSize = new Vector2i(800, 600),
                Title = "Window Creation Example"
            };

            using var window = new Window(GameWindowSettings.Default, nativeWindowSettings);
            window.Run();
        }
    }
}
