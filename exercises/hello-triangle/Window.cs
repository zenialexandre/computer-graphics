using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace HelloTriangle
{
    public class Window(
        GameWindowSettings gameWindowSettings,
        NativeWindowSettings nativeWindowSettings
    ) : GameWindow(gameWindowSettings, nativeWindowSettings)
    {
        private readonly float[] _vertices =
        [
            -0.5f, -0.5f, 0.0f,
            0.5f, -0.5f, 0.0f,
            0.0f, 0.5f, 0.0f
        ];
        private int _vertexBufferObject;
        private int _vertexArrayObject;
        private Shader

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
