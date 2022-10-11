using Argo_Core.Shared.System.Graphics;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Argo_Core;

public class Argo
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

    public void CreateWindow(Vector2i size, string title)
    {
        NativeWindowSettings nativeWindowSettings = new()
        {
            Size = size,
            Title = title,
            // This is needed to run on macos
            Flags = ContextFlags.ForwardCompatible
        };

        // This line creates a new instance, and wraps the instance in a using statement so it's automatically disposed once we've exited the block.
        using (BaseWindow baseWindow = new(GameWindowSettings.Default, nativeWindowSettings))
        {
            baseWindow.Run();
        }
    }
}
