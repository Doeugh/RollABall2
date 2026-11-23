Shader "Custom/SimpleToonURP"
{
    Properties
    {
        _Color ("Base Color", Color) = (1,1,1,1)
        _ShadowColor ("Shadow Color", Color) = (0.4,0.4,0.5,1)

        _ShadowThreshold ("Shadow Threshold", Range(-1,1)) = 0
        _BandSmoothness ("Band Smoothness", Range(0.001,0.2)) = 0.01

        _SpecularColor ("Specular Color", Color) = (1,1,1,1)
        _Glossiness ("Specular Size", Range(1,128)) = 32

        _RimColor ("Rim Color", Color) = (1,1,1,1)
        _RimAmount ("Rim Amount", Range(0,1)) = 0.7
        _RimPower ("Rim Power", Range(0.5,8)) = 3
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "Geometry"
        }

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_SCREEN
            #pragma multi_compile _ _SHADOWS_SOFT

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                float3 viewDirWS : TEXCOORD2;
                float4 shadowCoord : TEXCOORD3;
            };

            CBUFFER_START(UnityPerMaterial)

                float4 _Color;
                float4 _ShadowColor;

                float _ShadowThreshold;
                float _BandSmoothness;

                float4 _SpecularColor;
                float _Glossiness;

                float4 _RimColor;
                float _RimAmount;
                float _RimPower;

            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                VertexPositionInputs positionInputs =
                    GetVertexPositionInputs(IN.positionOS.xyz);

                OUT.positionCS = positionInputs.positionCS;
                OUT.positionWS = positionInputs.positionWS;

                OUT.normalWS =
                    TransformObjectToWorldNormal(IN.normalOS);

                OUT.viewDirWS =
                    GetWorldSpaceViewDir(positionInputs.positionWS);

                OUT.shadowCoord =
                    GetShadowCoord(positionInputs);

                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float3 normal = normalize(IN.normalWS);
                float3 viewDir = normalize(IN.viewDirWS);

                // Main directional light
                Light mainLight = GetMainLight(IN.shadowCoord);

                float3 lightDir = normalize(mainLight.direction);

                // Toon lighting
                float NdotL = dot(normal, lightDir);

                float lighting = smoothstep(
                    _ShadowThreshold - _BandSmoothness,
                    _ShadowThreshold + _BandSmoothness,
                    NdotL * mainLight.shadowAttenuation
                );

                float3 toonColor = lerp(
                    _ShadowColor.rgb,
                    mainLight.color,
                    lighting
                );

                // Specular highlights
                float3 halfVector =
                    normalize(lightDir + viewDir);

                float NdotH = saturate(dot(normal, halfVector));

                float specular = pow(NdotH, _Glossiness);

                specular = smoothstep(0.5, 0.55, specular);
                specular *= lighting;

                float3 specularColor =
                    specular * _SpecularColor.rgb;

                // Rim lighting
                float rim = 1.0 - saturate(dot(viewDir, normal));

                rim = pow(rim, _RimPower);

                rim = smoothstep(
                    _RimAmount,
                    _RimAmount + 0.05,
                    rim
                );

                float3 rimColor = rim * _RimColor.rgb;

                // Final result
                float3 finalColor =
                    _Color.rgb * toonColor
                    + specularColor
                    + rimColor;

                return half4(finalColor, _Color.a);
            }

            ENDHLSL
        }

        // Shadow casting
        UsePass "Universal Render Pipeline/Lit/ShadowCaster"
    }

    FallBack Off
}
