using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace window_creation
{
    public class Window(
        GameWindowSettings gameWindowSettings,
        NativeWindowSettings nativeWindowSettings
    ) : GameWindow(gameWindowSettings, nativeWindowSettings)
    {
        protected override void OnUpdateFrame(FrameEventArgs frameEventArgs)
        {
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
            base.OnUpdateFrame(frameEventArgs);
        }
    }
}
