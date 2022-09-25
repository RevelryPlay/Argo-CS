using OpenTK.Mathematics;

namespace Argo_Core.Shared.Core.System.Graphics;

public class Camera
{

    // The field of view of the camera (radians)
    float _fov = MathHelper.PiOver2;
    // Those vectors are directions pointing outwards from the camera to define how it rotated.
    Vector3 _front = -Vector3.UnitZ;

    // Rotation around the X axis (radians)
    float _pitch;

    // Rotation around the Y axis (radians)
    float _yaw = -MathHelper.PiOver2; // Without this, you would be started rotated 90 degrees right.

    public Camera(Vector3 position, float aspectRatio)
    {
        Position = position;
        AspectRatio = aspectRatio;
    }

    // The position of the camera
    public Vector3 Position { get; set; }

    // This is simply the aspect ratio of the viewport, used for the projection matrix.
    public float AspectRatio { private get; set; }

    public Vector3 Front
    {
        get
        {
            return _front;
        }
    }

    public Vector3 Up { get; private set; } = Vector3.UnitY;

    public Vector3 Right { get; private set; } = Vector3.UnitX;

    // We convert from degrees to radians as soon as the property is set to improve performance.
    public float Pitch
    {
        get
        {
            return MathHelper.RadiansToDegrees(_pitch);
        }
        set
        {
            // We clamp the pitch value between -89 and 89 to prevent the camera from going upside down, and a bunch
            // of weird "bugs" when you are using euler angles for rotation.
            // If you want to read more about this you can try researching a topic called gimbal lock
            float angle = MathHelper.Clamp(value, -89f, 89f);
            _pitch = MathHelper.DegreesToRadians(angle);
            UpdateVectors();
        }
    }

    // We convert from degrees to radians as soon as the property is set to improve performance.
    public float Yaw
    {
        get
        {
            return MathHelper.RadiansToDegrees(_yaw);
        }
        set
        {
            _yaw = MathHelper.DegreesToRadians(value);
            UpdateVectors();
        }
    }

    // The field of view (FOV) is the vertical angle of the camera view.
    // This has been discussed more in depth in a previous tutorial,
    // but in this tutorial, you have also learned how we can use this to simulate a zoom feature.
    // We convert from degrees to radians as soon as the property is set to improve performance.
    public float Fov
    {
        get
        {
            return MathHelper.RadiansToDegrees(_fov);
        }
        set
        {
            float angle = MathHelper.Clamp(value, 1f, 90f);
            _fov = MathHelper.DegreesToRadians(angle);
        }
    }

    // Get the view matrix using the amazing LookAt function described more in depth on the web tutorials
    public Matrix4 GetViewMatrix()
    {
        return Matrix4.LookAt(Position, Position + _front, Up);
    }

    // Get the projection matrix using the same method we have used up until this point
    public Matrix4 GetProjectionMatrix()
    {
        return Matrix4.CreatePerspectiveFieldOfView(_fov, AspectRatio, 0.01f, 100f);
    }

    // This function is going to update the direction vertices using some of the math learned in the web tutorials.
    void UpdateVectors()
    {
        // First, the front matrix is calculated using some basic trigonometry.
        _front.X = MathF.Cos(_pitch) * MathF.Cos(_yaw);
        _front.Y = MathF.Sin(_pitch);
        _front.Z = MathF.Cos(_pitch) * MathF.Sin(_yaw);

        // We need to make sure the vectors are all normalized, as otherwise we would get some funky results.
        _front = Vector3.Normalize(_front);

        // Calculate both the right and the up vector using cross product.
        // Note that we are calculating the right from the global up; this behaviour might
        // not be what you need for all cameras so keep this in mind if you do not want a FPS camera.
        Right = Vector3.Normalize(Vector3.Cross(_front, Vector3.UnitY));
        Up = Vector3.Normalize(Vector3.Cross(Right, _front));
    }
}