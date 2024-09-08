// Made with Amplify Shader Editor v1.9.1.5
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Mesh_Shader01"
{
	Properties
	{
		_Texture("Texture", 2D) = "white" {}
		[HDR]_Color0("Color 0", Color) = (0.6775098,0.7433916,0.8207547,0)
		[HDR]_Light_Color("Light_Color", Color) = (1,1,1,0)
		_Fresnel("Fresnel", Range( 0 , 5)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Opaque" }
	LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend SrcAlpha OneMinusSrcAlpha
		AlphaToMask On
		Cull Back
		ColorMask RGBA
		ZWrite On
		ZTest LEqual
		Offset 0 , 0
		
		
		
		Pass
		{
			Name "Unlit"

			CGPROGRAM

			

			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			//only defining to not throw compilation error over Unity 5.5
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			#define ASE_NEEDS_FRAG_WORLD_POSITION


			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 worldPos : TEXCOORD0;
				#endif
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_color : COLOR;
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform float4 _Light_Color;
			uniform float4 _Color0;
			uniform float _Fresnel;
			uniform sampler2D _Texture;
			uniform float4 _Texture_ST;

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				float3 ase_worldNormal = UnityObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord1.xyz = ase_worldNormal;
				
				o.ase_color = v.color;
				o.ase_texcoord2.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.w = 0;
				o.ase_texcoord2.zw = 0;
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
				vertexValue = v.vertex.xyz;
				#endif
				vertexValue = vertexValue;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);

				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				#endif
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 WorldPosition = i.worldPos;
				#endif
				#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
				float3 ase_worldlightDir = 0;
				#else //aseld
				float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( WorldPosition ) );
				#endif //aseld
				float3 ase_worldNormal = i.ase_texcoord1.xyz;
				float fresnelNdotV28 = dot( ase_worldNormal, ase_worldlightDir );
				float f028 = 0.08;
				float fresnelNode28 = ( f028 + ( 1.0 - f028 ) * pow( 1.0 - fresnelNdotV28, 5 ) );
				float3 ase_worldViewDir = UnityWorldSpaceViewDir(WorldPosition);
				ase_worldViewDir = normalize(ase_worldViewDir);
				float fresnelNdotV39 = dot( ase_worldNormal, ase_worldViewDir );
				float fresnelNode39 = ( 0.08 + _Fresnel * pow( 1.0 - fresnelNdotV39, 2.0 ) );
				float2 uv_Texture = i.ase_texcoord2.xy * _Texture_ST.xy + _Texture_ST.zw;
				float4 appendResult9 = (float4(( ( ( _Light_Color + ( saturate( ( _Color0 * ( 1.0 - fresnelNode28 ) ) ) * ( fresnelNode39 * 5.0 ) ) ) * i.ase_color ) * tex2D( _Texture, uv_Texture ) ).rgb , 1.0));
				
				
				finalColor = appendResult9;
				return finalColor;
			}
			ENDCG
		}
	}

	Fallback Off
}