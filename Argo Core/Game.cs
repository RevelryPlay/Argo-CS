using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Argo_Core;

public class Game : GameWindow
{
    // A simple constructor to let us set properties like window size, title, FPS, etc. on the window.
    public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
    {
    }
    
    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        if (KeyboardState.IsKeyDown(Keys.Escape))
        { 
            this.Close();
        }

        base.OnUpdateFrame(e);
    }
    
    protected override void OnRenderFrame(FrameEventArgs e)
    {
        // Show that we can use OpenGL: Clear the window to cornflower blue.
        GL.ClearColor(0.39f, 0.58f, 0.93f, 1.0f);
        GL.Clear(ClearBufferMask.ColorBufferBit);

        // Show in the window the results of the rendering calls.
        SwapBuffers();
    }
}
