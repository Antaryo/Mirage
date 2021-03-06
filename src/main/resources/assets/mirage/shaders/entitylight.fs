#version 120

varying float shift;
varying vec3 position;
varying float intens;
varying vec4 lcolor;
varying vec4 uv;
varying vec3 lightsum;

uniform sampler2D sampler;
uniform sampler2D lightmap;
uniform sampler2D brightlayer;
uniform vec3 playerPos;
uniform vec4 colorMult;
uniform float fogIntensity;

void main() {
	vec3 lightdark = texture2D(lightmap,gl_TexCoord[1].st).xyz;
	lightdark = clamp(lightdark,0.0f,1.0f);
	
	vec4 lcolor = vec4(max(lightdark,lcolor.xyz),lcolor.w);
	
	vec4 baseColor = gl_Color * texture2D(sampler,gl_TexCoord[0].st);
	if (baseColor.w == 1 && !(baseColor.x == 0 && baseColor.y == 0 && baseColor.z == 0)) {
		baseColor = vec4(mix(baseColor.xyz,colorMult.xyz,colorMult.w),baseColor.w);
	}
	
	//if (fogIntensity > 0) {
		float dist = max((gl_FragCoord.z / gl_FragCoord.w) - gl_Fog.start,0.0f);
		
		float fog = gl_Fog.density * dist * gl_Fog.density * dist * fogIntensity * fogIntensity;
					  
		fog = 1.0f-clamp( fog, 0.0f, 1.0f );
		  
		baseColor = vec4(mix( vec3( gl_Fog.color ), baseColor.xyz, fog ).xyz,baseColor.w);
	//}
	
	vec4 color = vec4((mix(baseColor.xyz*lightdark,baseColor.xyz*lcolor.xyz,intens)),baseColor.w);
	gl_FragColor = vec4(color.xyz * lightsum,color.w);
}