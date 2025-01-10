shader_type canvas_item;

uniform float scale : hint_range(0.0, 2.0) = 1.0;
uniform float swirl : hint_range(0.0, 1.0) = 0.0;
uniform float dissolve : hint_range(-1.0, 1.0) = 1;
uniform float fragment_number = 24;

const vec2 center = vec2(0.5, 0.5);


mat2 rotate2D(float r) {
    return mat2(vec2(cos(r), sin(r)), vec2(-sin(r), cos(r)));
}

void fragment() {
	// swirl
    vec2 offset = UV - vec2(0.5, 0.5);
    float angle = swirl * length(offset) * 40.0;
    mat2 rotation = rotate2D(angle);
    vec2 swirl_uv = rotation * offset + center;
    COLOR = texture(TEXTURE, swirl_uv);
	
	// dissolve
	vec2 pixel_fract = fract(UV * fragment_number);
	float dist = distance(pixel_fract, vec2(0.5, 0.5));
	float dissolve_threshold = dissolve + (abs(UV.x - 0.5) + abs(UV.y - 0.5));
	if (dist > dissolve_threshold) {
        COLOR.a *= 0.0;
    }
}

void vertex(){
	// scale
	VERTEX.xy *= scale;
	
//	float amount = (dissolve + 1.0) / 2.0;
//	VERTEX.xy *= (1.0 - amount * 1.0) * 2.0;
}


//// top left
//if (UV.x < 0.5 && UV.y < 0.5)
//	if (dist > dissolve + UV.x + UV.y)
//		COLOR.a *= 0.0;
//
//// top right
//if (UV.x > 0.5 && UV.y < 0.5)
//	if (dist > dissolve + (1.0 - UV.x) + UV.y)
//		COLOR.a *= 0.0;
//
//// bottom left
//if (UV.x < 0.5 && UV.y > 0.5)
//	if (dist > dissolve + UV.x + (1.0 - UV.y))
//		COLOR.a *= 0.0;
//
//// bottom right
//if (UV.x >= 0.5 && UV.y >= 0.5)
//	if (dist > dissolve + (1.0 - UV.x) + (1.0 - UV.y))
//		COLOR.a *= 0.0;

