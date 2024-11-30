shader_type canvas_item;

uniform vec4 wave_color: source_color = vec4(1.0, 2.0, 4.0, 1.0);
uniform float threshold: hint_range(0.0, 1.0) = 0.7;

// 2x2 matrix rotation
mat2 rotate2D(float r) {
    return mat2(vec2(cos(r), sin(r)), vec2(-sin(r), cos(r)));
}

void fragment() {
    //vec2 uv = (FRAGCOORD.xy / TEXTURE_PIXEL_SIZE.y) * 0.001;
    //vec2 uv = (FRAGCOORD.xy) * 0.015;
    vec2 uv = (UV.xy) * 1.0;
	
    vec3 col = vec3(0.0);
    vec2 n = vec2(0.0);
    vec2 q = vec2(0.0);
    vec2 N = vec2(0.0);
	
    //vec2 p = uv + sin(TIME*0.1/10.0);
    vec2 p = uv;
    float S = 10.0;
	
    //mat2 m = rotate2D(1.0 - 0.001);
    mat2 m = rotate2D(radians(60));
	
	/*
    for (float j = 0.0; j < 30.0; j++) {
	
        p *= m;
        n *= m;
		
        q = p * S + j + n + TIME;
        n += sin(q);
        N += cos(q) / S;
		
        S *= 1.2;
    }
	*/
	
    for (float j = 0.0; j < 4.0; j++) {
        p *= m;
        n *= m;
		
        q = p * S + j + n + TIME * 10.0;
		
        n += sin(q);
        //N += cos(q) / S;
        N += 2.0 * sin(q) / S;
		
        S *= 1.5;
    }
	
    //float lengthN = max(length(N), 0.001);
    //col = wave_color.rgb * pow((N.x + N.y + 0.4) + 0.005 / lengthN, 2);
	
    col = wave_color.rgb * pow((N.x + N.y + 0.4) + 0.005 / length(N), 2);
	
	if (COLOR.r > threshold || COLOR.g > threshold || COLOR.b > threshold)
		COLOR = vec4(col, COLOR.a);
	else
		COLOR = vec4(1, 1, 1, COLOR.a);
}