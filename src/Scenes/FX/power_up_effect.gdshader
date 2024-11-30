shader_type canvas_item;

uniform float speed : hint_range(0.0, 10.0) = 2.0;
uniform float amplitude : hint_range(0.0, 10.0) = 5.0;

void vertex() {
    float pulse = sin(TIME * PI * speed) * amplitude;
    VERTEX += normalize(VERTEX) * pulse;
}

void fragment() {
    //float pulse = sin(TIME * PI * speed + PI * 2.0) * 0.5 + 0.5; 
    //COLOR.rgb /= (1.0 - pulse * 0.5);
	
    float pulse = sin(TIME * PI * speed);
    //COLOR.rgb /= (1.0 - pulse * 0.5);
    COLOR.rgb /= (1.0 - pulse * 0.5);
}



void fragment1() {
    //COLOR *= 0.5+  abs(sin(TIME / 2.0 * speed));
	
    float pulse = sin(TIME * speed);// * amplitude;
	//COLOR.rgb *= abs(sin(TIME / 2.0 * speed)) + pulse;
	//COLOR.rgb *= abs(sin(TIME * speed * PI /2.0));
}