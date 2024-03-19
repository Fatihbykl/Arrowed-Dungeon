Shader "Indicator/Unlit/CircleShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
        _FillColor ("Fill Color", Color) = (1, 1, 1, 1)
        _FillProgress ("Fill Progress", Range(0, 1)) = 1
    }
    SubShader
    {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		LOD 100    
		 
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha 

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog
            
            #pragma multi_compile_instancing
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            fixed4 _FillColor;
            float _FillProgress;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 tex = tex2D(_MainTex, i.uv);
                fixed4 col = tex * _Color;

                float dist = length(i.uv - float2(0.5, 0.5));
                float cutoff = dist > _FillProgress / 2 ? 1 : 0;
                
                UNITY_APPLY_FOG(i.fogCoord, col);
                return cutoff == 0 ? col : tex * _FillColor;
            }
            ENDCG
        }
    }
}
