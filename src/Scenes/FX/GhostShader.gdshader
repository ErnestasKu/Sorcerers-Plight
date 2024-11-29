shader_type canvas_item;

// -- Speed of hue shift
uniform float speed : hint_range(0.1, 10.0) = 1.0;
uniform float alpha : hint_range(0.0, 1.0) = 0.3;


void fragment() {
	
	// -- greyscale convert ----------------------------------------------------
    vec4 input = texture(TEXTURE, UV);

	// -- ver 1
    //float luminance = dot(input.rgb, vec3(0.299, 0.587, 0.114));
	//COLOR = vec4(vec3(luminance), input.a);
	
	// -- ver 2
	//float average = (input.r + input.g + input.b) / 3.0;
	//COLOR = vec4(average, average, average, input.a);
	
	
	// -- rainbow --------------------------------------------------------------
    input = texture(TEXTURE, UV);
    
    // -- saturation calc part 1
    float max_val = max(input.r, max(input.g, input.b));
    float min_val = min(input.r, min(input.g, input.b));
    float delta = max_val - min_val;

    float hue = 0.0;
    float saturation = delta / max_val;
    float lightness = (max_val + min_val) * 0.5;

    if (delta != 0.0) {
        if (max_val == input.r) {
            hue = mod((input.g - input.b) / delta, 6.0);
        } else if (max_val == input.g) {
            hue = ((input.b - input.r) / delta) + 2.0;
        } else {
            hue = ((input.r - input.g) / delta) + 4.0;
        }
    }
    hue /= 6.0;

    hue += TIME * speed;
	
	// -- prevents: HUE > 1
    hue = fract(hue);

    // -- Converts HSL to RGB
    float c = (1.0 - abs(2.0 * lightness - 1.0)) * saturation;
    float x = c * (1.0 - abs(mod(hue * 6.0, 2.0) - 1.0));
    float m = lightness - c * 0.5;

    vec3 rgb;
    if (hue < 1.0 / 6.0) {
        rgb = vec3(c, x, 0.0);
    } else if (hue < 2.0 / 6.0) {
        rgb = vec3(x, c, 0.0);
    } else if (hue < 3.0 / 6.0) {
        rgb = vec3(0.0, c, x);
    } else if (hue < 4.0 / 6.0) {
        rgb = vec3(0.0, x, c);
    } else if (hue < 5.0 / 6.0) {
        rgb = vec3(x, 0.0, c);
    } else {
        rgb = vec3(c, 0.0, x);
    }

    // -- Output the final color 
    COLOR = vec4(rgb + m, input.a * alpha);
}
