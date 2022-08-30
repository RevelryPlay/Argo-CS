using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Argo_Core;

public class Core
{
    // Get System Modules
        // Base Error Handlers
        // File System Utilities
            // Loggers
            // Serializers
            // Asset Loaders
            // Save / Load
            // Settings Handler
        // OS Specific Modules
        // Graphic Utilities
        // Audio Utilities
        // Input Utilities
        // Networking Utilities
        
    // Get Registered Extensions 
    // Handle Preloads
    // Core Event Loop

    public static void Start()
    {
        NativeWindowSettings nativeWindowSettings = new()
        {
            Size = new Vector2i(1920, 1080),
            Title = "Main Window",
            // This is needed to run on macos
            Flags = ContextFlags.ForwardCompatible
        };

        // This line creates a new instance, and wraps the instance in a using statement so it's automatically disposed once we've exited the block.
        using (Game game = new(GameWindowSettings.Default, nativeWindowSettings))
        {
            game.Run();
        }
    }
}
