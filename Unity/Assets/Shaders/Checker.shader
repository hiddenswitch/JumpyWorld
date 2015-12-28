Shader "Custom/Checker" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_CheckerTex ("Checker (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_SizeMultiplier ("Size", Range(1,100)) = 8.0
		_Offset ("Offset", Vector) = (0,0,0,0)
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
		fixed4 _Color;

		bool isChecker(float3 inputCoord) {
			bool x = (int)((inputCoord.x+_Offset.x)*_SizeMultiplier) & 2;
		    bool y = (int)((inputCoord.y+_Offset.y)*_SizeMultiplier) & 2;
		    bool z = (int)((inputCoord.z+_Offset.z)*_SizeMultiplier) & 2;
 
		    // Checkerboard pattern is formed by inverting the boolean flag
		    // at each dimension separately:
   			return (x != y != z);
		}
		bool isTriangle(float3 inputCoord) {
			half x = frac((inputCoord.x+_Offset.x)*_SizeMultiplier);
			half y = frac((inputCoord.y+_Offset.y)*_SizeMultiplier);
			half z = frac((inputCoord.z+_Offset.z)*_SizeMultiplier);
			return (x > z);
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c;
			bool check = isChecker(IN.worldPos);
			bool tri = isTriangle(IN.worldPos);

			if ((check && tri) || (!check && tri)) {
				c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			} else {
				c = tex2D (_CheckerTex, IN.uv_CheckerTex) * _Color;
			}
			
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
