shader_type canvas_item;

uniform float strength = 0;
uniform sampler2D SCREEN_TEXTURE : hint_screen_texture;

// pseudo random nr
float random (float uv) {
    return fract(sin(dot(vec2(uv, uv), vec2(12.9898,78.233))) * 43758.5453123);
}

void fragment() {
	// shake
    vec2 uv = SCREEN_UV;
    uv.x += (random((trunc(SCREEN_UV.y * 32.0) / 3.0) * TIME)) * strength / 100.0;
    
	// discoloration
    vec4 col = texture(SCREEN_TEXTURE, uv, 0.0);
	col.r = texture(SCREEN_TEXTURE, uv + vec2(SCREEN_PIXEL_SIZE.x * strength * 3.0), 0.0).r;
	col.b = texture(SCREEN_TEXTURE, uv + vec2(SCREEN_PIXEL_SIZE.x * strength * 3.0), 0.0).b;
	
    COLOR = col;
}
