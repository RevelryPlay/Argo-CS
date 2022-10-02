using System.Drawing;
using Argo_Core.Shared.Core.System.Graphics.UIText;
using Argo_Utilities.Shared.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using ColorConverter = Argo_Utilities.Shared.Graphics.ColorConverter;

namespace Argo_Core.Shared.Core.System.Graphics;

public class Window : GameWindow
{

    public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
    {
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.Enable(EnableCap.DepthTest);

        #region Clear the Window

        // This will be the color of the background after we clear it, in normalized colors.
        NormalizedColor color = ColorConverter.HexToNormalizedColor("#141f1e");
        GL.ClearColor(color.Red, color.Green, color.Blue, color.Alpha);

        #endregion

        #region Build Vertex Buffers

        _vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);

        _elementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

        #endregion

        #region Load Textures

        _texture = Texture.LoadFromFile("Shared/Textures/container.png");
        _texture.Use(TextureUnit.Texture0);

        #endregion

        #region Shader Setup

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

        _shader?.SetInt("texture0", 0);

        #endregion

        _camera = new(Vector3.UnitZ * 3, Size.X / (float)Size.Y);

        CursorState = CursorState.Grabbed;

        string ret = _textBuilder.AddText("This is a test line of text.", "Gabriola", 24.0f, Point.Empty);
        Dictionary<string, TextProperties> addedStrings = _textBuilder.GetAddedText();

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
        _renderFrame();
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        GL.Viewport(0, 0, Size.X, Size.Y);
        _renderFrame();

        base.OnResize(e);
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        if (!IsFocused) // Check to see if the window is focused
            return;

        const float cameraSpeed = 1.5f;

        if (_camera == null)
            return;

        #region Set Key Bindings

        if (KeyboardState.IsKeyDown(Keys.Escape))
            Close();

        if (KeyboardState.IsKeyDown(Keys.W))
        {
            _camera.Position += _camera.Front * cameraSpeed * (float)args.Time; // Forward
        }

        if (KeyboardState.IsKeyDown(Keys.S))
        {
            _camera.Position -= _camera.Front * cameraSpeed * (float)args.Time; // Backwards
        }
        if (KeyboardState.IsKeyDown(Keys.A))
        {
            _camera.Position -= _camera.Right * cameraSpeed * (float)args.Time; // Left
        }
        if (KeyboardState.IsKeyDown(Keys.D))
        {
            _camera.Position += _camera.Right * cameraSpeed * (float)args.Time; // Right
        }
        if (KeyboardState.IsKeyDown(Keys.Space))
        {
            _camera.Position += _camera.Up * cameraSpeed * (float)args.Time; // Up
        }
        if (KeyboardState.IsKeyDown(Keys.LeftShift))
        {
            _camera.Position -= _camera.Up * cameraSpeed * (float)args.Time; // Down
        }

        #endregion

    }

    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
        base.OnMouseWheel(e);

        if (_camera == null)
            return;

        _camera.Fov -= e.OffsetY;
    }

    void _renderFrame()
    {
        if (_shader == null || _camera == null)
            return;

        #region Update Previous Positions

        if (_previousProjection == _camera.GetProjectionMatrix()
            && _previousView == _camera.GetViewMatrix())
            return;

        _previousProjection = _camera.GetProjectionMatrix();
        _previousView = _camera.GetViewMatrix();

        #endregion

        #region Clear the Window and Set Vertex Buffer

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        GL.BindVertexArray(_vertexArrayObject);

        #endregion

        #region Pass Data to Shaders

        Matrix4 model = Matrix4.Identity;

        _shader?.SetMatrix4("model", model);
        _shader?.SetMatrix4("view", _camera.GetViewMatrix());
        _shader?.SetMatrix4("projection", _camera.GetProjectionMatrix());

        #endregion

        #region Draw the Current Buffers

        GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
        SwapBuffers();

        #endregion

    }

    #region Properties

    readonly uint[] _indices =
    {
        0, 1, 3, 1, 2, 3
    };

    readonly float[] _vertices =
    {
        // Position       Texture coordinates
        0.5f, 0.5f, 0.0f, 1.0f, 1.0f, // top right
        0.5f, -0.5f, 0.0f, 1.0f, 0.0f, // bottom right
        -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, // bottom left
        -0.5f, 0.5f, 0.0f, 0.0f, 1.0f // top left
    };

    Camera? _camera;

    int _elementBufferObject;

    Shader? _shader;
    Texture? _texture;

    int _vertexArrayObject;
    int _vertexBufferObject;

    Matrix4 _previousProjection;
    Matrix4 _previousView;

    readonly TextBuilder _textBuilder = new();

    #endregion

}