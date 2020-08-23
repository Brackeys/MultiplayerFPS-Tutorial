// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/DisplayNightEarthTexture" {
	Properties {
_MainTex ("Base (RGB)", 2D) = "white" { }
_SpotAngle ("Spot Angle", Float) = 30.0
_Range ("Range", Float) = 5.0
_Contrast ("Contrast", Range (20.0, 80.0)) = 50.0
}
 
Subshader {
Tags {"RenderType"="Transparent" "Queue"="Transparent"}
Pass {
Blend SrcAlpha OneMinusSrcAlpha
ZTest LEqual
 
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"
 
uniform sampler2D _MainTex;
uniform float4 _LightPos; // light world position - set via script
uniform float4 _LightDir; // light world direction - set via script
uniform float _SpotAngle;
uniform float _Range;
uniform float _Contrast;
 
struct v2f_interpolated {
float4 pos : SV_POSITION;
float2 texCoord : TEXCOORD0;
float3 lightDir : TEXCOORD1;
};
 
v2f_interpolated vert(appdata_full v){
v2f_interpolated o;
o.texCoord.xy = v.texcoord.xy;
o.pos = UnityObjectToClipPos(v.vertex);
half3 worldSpaceVertex = mul(unity_ObjectToWorld, v.vertex).xyz;
// calculate light direction to vertex
o.lightDir = worldSpaceVertex-_LightPos.xyz;
return o;
}
 
half4 frag(v2f_interpolated i) : COLOR {
half dist = saturate(1-(length(i.lightDir)/_Range)); // get distance factor
half cosLightDir = dot(normalize(i.lightDir), normalize(_LightDir)); // get light angle
half ang = cosLightDir-cos(radians(_SpotAngle/2)); // calculate angle factor
half alpha = saturate(dist * ang * _Contrast); // combine distance, angle and contrast
half4 c = tex2D(_MainTex, i.texCoord); // get texel
c.a *= alpha; // combine texel and calculated alpha
return c;
}
ENDCG
}
}
	FallBack "Diffuse"
}
