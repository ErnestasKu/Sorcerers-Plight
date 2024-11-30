shader_type canvas_item;

uniform float speed : hint_range(0.0, 100.0) = 1.0;
uniform vec4 nucleaus_color: source_color = vec4(1.0, 1.0, 1.0, 1.0);

// 2x2 matrix rotation
mat2 rotate2D(float r) {
    return mat2(vec2(cos(r), sin(r)), vec2(-sin(r), cos(r)));
}

void fragment() {
	// previous
	//COLOR = vec4(nucleaus_color.r, nucleaus_color.g, nucleaus_color.b, COLOR.a);
	
	//float temp = COLOR.r;
	//COLOR.r = COLOR.b;
	//COLOR.b = temp;
	
	// fade
	if (COLOR.a > 0.1) {
	    COLOR.a = pow(abs(sin(TIME * PI / 2.0)), 2.0);
	}
}

void vertex() {
	// previous	
    //VERTEX.x += 60.0 * sin(TIME * PI / speed + phase);
    //VERTEX.y += 25.0 * sin(TIME * PI / speed * 2.0 + phase);
	
	VERTEX.xy *= rotate2D(8.0 * TIME * radians(30));
	
    VERTEX.x += 350.0 * sin(TIME * PI / speed);
    VERTEX.y += 75.0 * sin(TIME * PI / speed * 2.0);
	
}