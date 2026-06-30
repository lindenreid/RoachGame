Shader "Custom/Roach"
{
    Properties
    {
        [MainColor] _BaseColor("BaseColor", Color) = (1,1,1,1)
        [MainTexture] _BaseMap("BaseMap", 2D) = "white" {}
        _Brightness("Brightness", Float) = 0
        _AlphaClip("Alpha Clip", Float) = 0.5
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Transparent" 
            "RenderPipeline"="UniversalRenderPipeline"
        }

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        CBUFFER_START(UnityPerMaterial)
        float4 _BaseMap_ST;
        half4 _BaseColor;
        CBUFFER_END

        ENDHLSL

        Pass
        {
            Tags
            {
                "LightMode"="UniversalForward"
                "Queue"="Transparent"
            }

            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            Stencil
            {
                Ref 3
                Comp Always
                Pass Replace
                Fail Replace
                ZFail Keep
            }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            struct Attributes
            {
                float4 positionOS   : POSITION;
                float2 uv           : TEXCOORD0;
            };

            struct Varyings
            {
                float2 uv           : TEXCOORD0;
                float4 positionHCS  : SV_POSITION;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            half _Brightness;
            half _AlphaClip;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                half4 tex = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv);
                tex += tex * _Brightness;
                tex = saturate(tex);

                half alpha = tex.a * _BaseColor.a;
                clip(alpha - _AlphaClip);

                return tex * _BaseColor;
            }
            ENDHLSL
        }
    }
}