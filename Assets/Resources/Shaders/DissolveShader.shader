Shader "Custom/DissolveShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_BumpMap("Bumpmap", 2D) = "bump" {}
		_Metallic("Metallic", Range(0,1)) = 0.0
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_EmissionColor("Emission Color", Color) = (0,0,0,0)
		_EmissionTex("Emission Texture", 2D) = "emission" {}
		_Emission ("Emission", Float) = 1
		_DissolveTex("Dissolve Texture", 2D) = "white" {}
		_AlphaCut("AlphaCut", Range(0,1)) = 1
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
		sampler2D _BumpMap;
		sampler2D _EmissionTex;
		sampler2D _DissolveTex;

		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
			float2 uv_EmissionTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		fixed4 _EmissionColor;
		half _Emission;
		half _AlphaCut;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			clip(tex2D(_DissolveTex, IN.uv_MainTex).rgb - _AlphaCut);
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Emission = _EmissionColor * _Emission * tex2D(_EmissionTex, IN.uv_EmissionTex);
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
