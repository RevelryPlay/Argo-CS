using Argo_Utilities.Shared;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using static Argo_Utilities.Shared.ColorConverter;

namespace Argo_Core.Shared.Core.System.Graphics;

public class Window : GameWindow
{
    readonly float[] _vertices =
    {
        -0.5f, -0.5f, 0.0f, //Bottom-left vertex
        0.5f, -0.5f, 0.0f, //Bottom-right vertex
        0.0f, 0.5f, 0.0f //Top vertex
    };

    Shader? _shader;
    int _vertexArrayObject;

    int _vertexBufferObject;

    // A simple constructor to let us set properties like window size, title, FPS, etc. on the window.
    public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
    {
    }

    protected override void OnLoad()
    {
        // This will be the color of the background after we clear it, in normalized colors.
        NormalizedColor color = HexToNormalizedColor("#3c2c4a");
        GL.ClearColor(color.Red, color.Green, color.Blue, color.Alpha);

        _vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        _shader = new("Shared/Core/System/Graphics/Shaders/Simple.vert", "Shared/Core/System/Graphics/Shaders/Simple.frag");
        _shader.Use();
    }

    protected override void OnUnload()
    {
        // Unbind all the resources by binding the targets to 0/null.
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);
        GL.UseProgram(0);

        // Delete all the resources.
        GL.DeleteBuffer(_vertexBufferObject);
        GL.DeleteVertexArray(_vertexArrayObject);

        if (_shader != null)
            GL.DeleteProgram(_shader.Handle);

        base.OnUnload();
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        GL.Clear(ClearBufferMask.ColorBufferBit);
        _shader?.Use();

        GL.BindVertexArray(_vertexArrayObject);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

        SwapBuffers();
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        GL.Viewport(0, 0, Size.X, Size.Y);
        base.OnResize(e);
    }

    protected override void OnKeyDown(KeyboardKeyEventArgs e)
    {
        if (KeyboardState.IsKeyDown(Keys.Escape))
        {
            Close();
        }

        base.OnKeyDown(e);
    }
}