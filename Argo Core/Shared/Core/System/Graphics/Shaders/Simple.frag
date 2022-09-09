#version 460 core
out vec4 FragColor;

in vec3 fillColor;

void main()
{
    FragColor = vec4(fillColor, 1.0);
}
