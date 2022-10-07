#version 330

in vec4 position;
in vec4 textureCoords;

out vec4 pass_textureCoords;

uniform vec4 translation;

void main(void) {
    gl_position = vec4(postion + translation * vec4(2.0, -2.0, 0.0, 0.0), 0.0, 1.0, 0.0, 0.0);
    pass_textureCoords = textureCoords;
}
