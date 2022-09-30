#version 460

in vec2 pass_textureCoords;

out vec4 out_color;

uniform float width = 0.5;
uniform float edge = 0.1;
uniform vec3 color = vec3(0.0, 0.0, 0.0);
uniform sampler2D fontAtlas;

void main(void) {
    float distance = 1.0 - texture(fontAtlas, pass_textureCoords).a;
    float alpha = 1.0 - smoothstep(width, width + edge, distance);

    out_color = vec4(color, alpha);
}