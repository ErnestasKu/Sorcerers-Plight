shader_type canvas_item;

uniform float dissolve : hint_range(-1.0, 1.0) = 1;
uniform float fragment_number = 20;

void fragment() {
	
	vec2 pixel_fract = fract(UV * fragment_number);
	float dist = distance(pixel_fract, vec2(0.5, 0.5));
	
	// fract demo
//	COLOR.rgb = vec3(pixel_fract, 0);

//	COLOR = texture(TEXTURE, UV);
//	COLOR.a *= step(dist, dissolve + UV.y);

	// if calculated distance is less than (y + dissolve)
	if (dist <= dissolve + UV.y)
		COLOR.a *= 1.0;
	else
		COLOR.a *= 0.0;
}