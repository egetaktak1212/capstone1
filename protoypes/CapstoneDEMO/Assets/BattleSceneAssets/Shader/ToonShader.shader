Shader "Supyrb/Unlit/ToonShadowTexture"
{
    Properties
    {
        [HDR]_Color("Albedo Tint", Color) = (1,1,1,1)
        _MainTex("Texture", 2D) = "white" {}

        _ShadowThreshold("Shadow Threshold", Range(0,1)) = 0.5
        _ShadowDarken("Shadow Multiply Amount", Range(0,1)) = 0.5
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }

        Pass
        {
            Name "ForwardUnlit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _SHADOWS_SOFT

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            float4 _MainTex_ST;
            float4 _Color;
            float _ShadowThreshold;
            float _ShadowDarken;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 shadowCoord : TEXCOORD1;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                float3 worldPos = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);

                OUT.shadowCoord = TransformWorldToShadowCoord(worldPos);

                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv) * _Color;

                float shadow = MainLightRealtimeShadow(IN.shadowCoord);

                float toonShadow = step(shadow, _ShadowThreshold);
                float shade = lerp(1.0, _ShadowDarken, toonShadow);

                return col * shade;
            }
            ENDHLSL
        }

        Pass
{
    Name "ShadowCaster"
    Tags { "LightMode" = "ShadowCaster" }

    ZWrite On
    ZTest LEqual
    Cull Back
    ColorMask 0

    HLSLPROGRAM
    #pragma vertex ShadowCasterVertex
    #pragma fragment ShadowCasterFragment

    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

    struct Attributes
    {
        float4 positionOS : POSITION;
        float3 normalOS   : NORMAL;
    };

    struct Varyings
    {
        float4 positionHCS : SV_POSITION;
    };

    Varyings ShadowCasterVertex(Attributes IN)
    {
        Varyings OUT;

        float3 worldPos    = TransformObjectToWorld(IN.positionOS.xyz);
        float3 worldNormal = TransformObjectToWorldNormal(IN.normalOS);

        float3 lightDir = _MainLightPosition.xyz;

        float4 shadowPos = float4(worldPos, 1.0);
        ApplyShadowBias(shadowPos, worldNormal, lightDir);

        OUT.positionHCS = TransformWorldToHClip(shadowPos.xyz);

        return OUT;
    }

    float4 ShadowCasterFragment(Varyings IN) : SV_Target
    {
        return 0;
    }
    ENDHLSL
}

    }
}
