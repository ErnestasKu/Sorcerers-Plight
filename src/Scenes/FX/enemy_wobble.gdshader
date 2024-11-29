shader_type canvas_item;

uniform vec2 deformation = vec2(0, 0);
uniform float knockbackFactor = 1.0;

void vertex() {
	vec2 dStrength = abs(deformation);
	float sideDeformation = min(dStrength.x, dStrength.y);
	float spriteWidth = abs(VERTEX.x);
	
	if (sign(VERTEX.y) != sign(deformation.y)){
		//VERTEX.x += sideDeformation * spriteWidth * sign(deformation.x);
	}
	
	vec2 scale = 1.0 - dStrength;
	vec2 vertexOld = VERTEX;
	
	// deformation
	VERTEX.x = vertexOld.x * scale.x / scale.y;
	VERTEX.y = vertexOld.y * scale.y / scale.x;
	
	// knockback
	VERTEX.xy += deformation * spriteWidth * knockbackFactor;
}
