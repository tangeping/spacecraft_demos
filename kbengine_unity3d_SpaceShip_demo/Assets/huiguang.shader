// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Ill_RimLight" 
{
    Properties {
        _Color ("Main Color", Color) = (.5,.5,.5,1)
        _OutlineColor ("Rim Color", Color) = (0,0,0,1)
        _Outline ("Rim width", Range (.002, 200.03)) = .005
        _MainTex ("Base (RGB)", 2D) = "white" { }
        _Illum ("Illumin (A)", 2D) = "white" { }
        _EmissionLM ("Emission (Lightmapper)", Float) = 0
    }
    
    SubShader 
    {

        Tags { "RenderType"="Opaque" }
        LOD 200
    
        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _MainTex;
        sampler2D _Illum;
        fixed4 _Color;

        struct Input {
            float2 uv_MainTex;
            float2 uv_Illum;
        };

        void surf (Input IN, inout SurfaceOutput o) {
            fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
            fixed4 c = tex * _Color;
            o.Albedo = c.rgb;
            o.Emission = c.rgb * tex2D(_Illum, IN.uv_Illum).a;
            o.Alpha = c.a;
        }
        ENDCG

        Pass 
        {
            CGINCLUDE
            #include "UnityCG.cginc"
    
            struct appdata {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f {
                float4 pos : POSITION;
                float4 color : COLOR;
            };
    
            uniform float _Outline;
            uniform float4 _OutlineColor;
    
            v2f vert(appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);

                float3 norm   = mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal);
                float2 offset = TransformViewToProjection(norm.xy);

                o.pos.xy += offset * o.pos.z * _Outline;
                o.color = _OutlineColor;
                return o;
            }
            ENDCG

            Name "OUTLINE"
            Tags { "LightMode" = "Always" }
            Cull Front
            ZWrite On
            ColorMask RGB
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            half4 frag(v2f i) :COLOR { return i.color; }
            ENDCG
        }
    } 
    
    Fallback "Self-Illumin/VertexLit"
}