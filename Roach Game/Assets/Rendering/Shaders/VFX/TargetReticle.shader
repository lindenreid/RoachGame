Shader "Custom/Target Reticle"
{
    Properties
    {
        [MainColor] _BaseColor("BaseColor", Color) = (1,1,1,1)
        [MainTexture] _BaseMap("BaseMap", 2D) = "white" {}
        _Brightness("Brightness", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "LightMode"="UniversalForward"
            "Queue"="Transparent"
            "RenderType"="Transparent" 
            "RenderPipeline"="UniversalPipeline"
        }

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        ENDHLSL

        Pass
        {
            Tags
            {
                "LightMode"="UniversalForward"
                "Queue"="Transparent"
                "RenderType"="Transparent" 
                "RenderPipeline"="UniversalPipeline"
            }

            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            Stencil
            {
                Ref 2
                Comp Greater
                Pass Replace
                Fail Keep
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

            float4 _BaseMap_ST;
            half4 _BaseColor;
            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            half _Brightness;

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
                return tex * _BaseColor;
            }
            ENDHLSL
        }
    }
}