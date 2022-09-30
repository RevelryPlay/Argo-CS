#version 460

in vec2 position;
in vec2 textureCoords;

out vec2 pass_textureCoords;

uniform vec2 translation;

void main(void) {
    gl_position = vec4(postion + translation * vec2(2.0, -2.0), 0.0, 1.0);
    pass_textureCoords = textureCoords;
}