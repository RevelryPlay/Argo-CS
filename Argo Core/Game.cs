using OpenTK;
using OpenTK.Windowing.Desktop;

namespace Argo_Core;

public class Game : GameWindow
{
    // A simple constructor to let us set properties like window size, title, FPS, etc. on the window.
    public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
    {
    }
}
