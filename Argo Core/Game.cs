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
}
