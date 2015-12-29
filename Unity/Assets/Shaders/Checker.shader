Shader "Custom/Checker" {
	Properties {
		_Color ("Tint", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_CheckerTex ("Checker (RGB)", 2D) = "white" {}
		_Color1 ("Triangle Tint Start", Color) = (1,1,1,1)
		_Color2 ("Triangle Tint End", Color) = (1,1,1,1)
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_SizeMultiplier ("Size", Range(1,100)) = 8.0
		_Offset ("Offset", Vector) = (0,0,0,0)
		_Frequency ("Frequency", Float) = 1.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _CheckerTex;
		half _SizeMultiplier;
		float3 _Offset;

		struct Input {
			float2 uv_MainTex;
			float2 uv_CheckerTex;
			float3 worldPos;
		};

		half _Glossiness;
		half _Metallic;
		float _Frequency;
		fixed4 _Color;
		fixed4 _Color1;
		fixed4 _Color2;

		float rand(float2 co){
    		return frac(sin(dot(co.xy ,float2(12.9898,78.233))) * 43758.5453);
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c;

			// Use world position as our input coordinates. Perhaps later, we'd use
			// UV coordinates instead.
			float3 inputCoord = IN.worldPos;

			// Compute isChecker or isTriangle
			// First, compute the coordinates in tile-space
			float xo = (inputCoord.x+_Offset.x)*_SizeMultiplier;
			float yo = (inputCoord.y+_Offset.y)*_SizeMultiplier;
			float zo = (inputCoord.z+_Offset.z)*_SizeMultiplier;

			// Then, compute whether we're in a checker or a triangle, or both
			bool isChecker = ((int)xo) & 2 != ((int)yo) & 2 != ((int)zo) & 2;
			bool isTriangle = frac(xo) > frac(zo);

			// Compute index used to sample random function
			float2 index = (int2(xo, zo) << 1) + (isTriangle ? int2(1,1) : int2(0,0));

			// Only branch once.
			if ((isChecker && isTriangle) || (!isChecker && isTriangle)) {
				c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			} else {
				c = tex2D (_CheckerTex, IN.uv_CheckerTex) * _Color;
			}

			float r = rand(index);
			c *= lerp(_Color1, _Color2, (sin((_Time.y+r)*_Frequency)+1)/2);

			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
