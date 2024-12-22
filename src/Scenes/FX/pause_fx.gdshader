shader_type canvas_item;

// Declare SCREEN_TEXTURE with hint_screen_texture
uniform sampler2D SCREEN_TEXTURE : hint_screen_texture;
uniform float additional_zoom = 1;

vec2 uv_curve(vec2 uv) {
    uv = (uv - 0.5) * 2.0;
	
	float factor = 3.0;
    uv.x *= 1.0 + pow(abs(uv.y) / factor, 2.0);
    uv.y *= 1.0 + pow(abs(uv.x) / factor, 2.0);
	uv /= 1.1;
	uv /= additional_zoom;

    uv = (uv / 2.0) + 0.5;
    return uv;
}

void fragment() {
	// curve thing
	vec4 col = texture(SCREEN_TEXTURE, uv_curve(SCREEN_UV), COLOR.a);
	
	// flicker
	if (int(TIME * 10.0) % 2 == 0) {
		// ===
	    if (fract(UV.y * 100.0) < 0.5) {
	        col.rgb *= 0.8;
	    }
	}
	else {
		// ===
	    if (fract(UV.y * 100.0) > 0.5) {
	        col.rgb *= 0.8;
	    }
	}
	
	// corners
    vec2 new_uv = (SCREEN_UV - 0.5) * 2.0;
	col.rgb *= 1.0 - pow(abs(new_uv.x), 3) - pow(abs(new_uv.y), 3);
	COLOR.rgb = col.rgb;
}