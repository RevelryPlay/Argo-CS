using Argo_Utilities.Shared.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Argo_Core.Shared.Core.System.Graphics;

public class Window : GameWindow
{

    readonly uint[] _indices =
    {
        0, 1, 3, 
        1, 2, 3
    };
    readonly float[] _vertices =
    {
        // Position       Texture coordinates
        0.5f, 0.5f, 0.0f, 1.0f, 1.0f, // top right
        0.5f, -0.5f, 0.0f, 1.0f, 0.0f, // bottom right
        -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, // bottom left
        -0.5f, 0.5f, 0.0f, 0.0f, 1.0f // top left
    };

    int _elementBufferObject;
    int _vertexArrayObject;
    int _vertexBufferObject;
    
    Shader? _shader;
    Texture? _texture;

    // A simple constructor to let us set properties like window size, title, FPS, etc. on the window.
    public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
    {
    }

    protected override void OnLoad()
    {
        base.OnLoad();
        
        // This will be the color of the background after we clear it, in normalized colors.
        NormalizedColor color = ColorConverter.HexToNormalizedColor("#141f1e");
        GL.ClearColor(color.Red, color.Green, color.Blue, color.Alpha);

        _vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);
        
        _elementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);
        
        _shader = new("Shared/Core/System/Graphics/Shaders/Simple.vert", "Shared/Core/System/Graphics/Shaders/Simple.frag");
        _shader.Use();

        if (_shader != null)
        {
            int vertexLocation = _shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            
            int texCoordLocation = _shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
        }

        _texture = Texture.LoadFromFile("Shared/Textures/container.png");
        _texture.Use(TextureUnit.Texture0);
        
        _shader?.SetInt("texture0", 0);

        _renderFrame();
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
        GL.DeleteBuffer(_elementBufferObject);

        if (_shader != null)
            GL.DeleteProgram(_shader.Handle);

        base.OnUnload();
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        GL.Viewport(0, 0, Size.X, Size.Y);
        _renderFrame();

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

    void _renderFrame()
    {
        GL.Clear(ClearBufferMask.ColorBufferBit);

        GL.BindVertexArray(_vertexArrayObject);
        
        Matrix4 transform = Matrix4.Identity;
        transform = transform * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(20f));
        transform = transform * Matrix4.CreateScale(1.1f);
        transform = transform * Matrix4.CreateTranslation(0.1f, 0.1f, 0.0f);
        
        _texture?.Use(TextureUnit.Texture0);
        _shader?.Use();
        
        _shader?.SetMatrix4("transform", transform);

        GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

        SwapBuffers();
    }
}