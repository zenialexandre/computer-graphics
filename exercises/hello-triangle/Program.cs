using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;

namespace Exercises.HelloTriangle
{
    public static class Program
    {
        private static void Main()
        {
            var nativeWindowSettings = new NativeWindowSettings()
            {
                ClientSize = new Vector2i(800, 600),
                Title = "Creating a Triangle"
            };

            using var window = new Window(GameWindowSettings.Default, nativeWindowSettings);
            window.Run();
        }
    }
}
