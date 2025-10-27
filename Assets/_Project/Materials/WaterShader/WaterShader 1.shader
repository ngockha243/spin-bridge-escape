Shader "Custom/WaterShader_URP"
{
    Properties
    {
        _MainTex("Main Tex", 2D) = "white" {}
        _NormalMap("Normal Map", 2D) = "bump" {}
        _NormalMap2("Normal Map 2", 2D) = "bump" {}
        _Cube("Reflection Cubemap", Cube) = "" {}
        _UVSpeed1("UV Speed 1", Vector) = (0.1,0.1,0,0)
        _UVSpeed2("UV Speed 2", Vector) = (-0.1,0.1,0,0)
        _Scale("Tiling", Float) = 4
        _WaveStrength("Wave Strength", Float) = 0.2
        _NormalStrength("Normal Strength", Range(0,1)) = 1
        _ColorA("Color A", Color) = (0.43,0.71,1,1)
        [HDR]_ColorB("Color B (HDR)", Color) = (1.96,2.28,2.30,1)
        _Opacity("Opacity", Range(0,1)) = 0.8
        _SpecColor("Specular Color", Color) = (1,1,1,1)
        _Gloss("Gloss", Range(0,1)) = 0.5
        _FresnelPower("Fresnel Power", Range(0.1,8)) = 2
        _EdgeFade("Edge Fade (0-1)", Range(0,1)) = 0.0
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "DisableBatching"="False" "RenderPipeline"="UniversalRenderPipeline" }
        LOD 200

        Pass
        {
            // Use URP includes and helper functions
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "UnityCG.cginc"

            // Properties -> shader variables
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            TEXTURE2D(_NormalMap);
            SAMPLER(sampler_NormalMap);
            TEXTURE2D(_NormalMap2);
            SAMPLER(sampler_NormalMap2);
            UNITY_DECLARE_TEXCUBE(_Cube);

            float4 _UVSpeed1;
            float4 _UVSpeed2;
            float _Scale;
            float _WaveStrength;
            float _NormalStrength;
            float4 _ColorA;
            float4 _ColorB;
            float _Opacity;
            float4 _SpecColor;
            float _Gloss;
            float _FresnelPower;
            float _EdgeFade;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float4 tangentOS  : TANGENT;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 positionWS  : TEXCOORD0;
                float3 normalWS    : TEXCOORD1;
                float3 tangentWS   : TEXCOORD2;
                float2 uv0         : TEXCOORD3;
                float2 uv1         : TEXCOORD4;
                float3 viewDirWS   : TEXCOORD5;
                UNITY_FOG_COORDS(6)
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                // Transforms
                float4 worldPos = TransformObjectToWorld(IN.positionOS);
                OUT.positionHCS = TransformWorldToHClip(worldPos.xyz);
                OUT.positionWS = worldPos.xyz;

                float3 normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.normalWS = normalize(normalWS);

                float3 tangentWS = TransformObjectToWorldDir(IN.tangentOS.xyz);
                OUT.tangentWS = normalize(tangentWS);

                // UVs with scale
                OUT.uv0 = IN.uv * _Scale;
                OUT.uv1 = IN.uv * _Scale;

                // view dir
                float3 camPosWS = GetMainLightPosition(); // dummy but this returns something in URP; safer to use _WorldSpaceCameraPos below
                float3 camWS = _WorldSpaceCameraPos;
                OUT.viewDirWS = normalize(camWS - worldPos.xyz);

                UNITY_TRANSFER_FOG(OUT, OUT.positionHCS);
                return OUT;
            }

            // Helpers
            float3 UnpackNormalMap(TEXTURE2D tex, SAMPLER samp, float2 uv)
            {
                float4 n = SAMPLE_TEXTURE2D(tex, samp, uv);
                // Unpack from 0..1 to -1..1
                return normalize(n.xyz * 2.0 - 1.0);
            }

            float3 CombineNormals(float3 n1, float3 n2, float strength)
            {
                // Simple blending / perturbation
                float3 up = float3(0,0,1);
                n1 = normalize(n1);
                n2 = normalize(n2);
                float3 combined = normalize(lerp(n1, n2, 0.5));
                combined = normalize(lerp(up, combined, strength));
                return combined;
            }

            float3 ApplyNormalToWorld(float3 normalTS, float3 tangentWS, float3 normalWS)
            {
                float3 binormal = cross(normalWS, tangentWS) * (INTERNAL_DATA_TANGENT_SIGN(tangentWS)); // not reliable, fallback
                float3x3 TBN = float3x3(tangentWS, normalize(binormal), normalWS);
                return normalize(mul(TBN, normalTS));
            }

            // Fragment
            half4 frag(Varyings IN) : SV_Target
            {
                // Time-based UV scrolling
                float2 uvMain = IN.uv0 + _Time.y * _UVSpeed1.xy;
                float2 uvMain2 = IN.uv1 + _Time.y * _UVSpeed2.xy;

                // Sample maps
                float4 baseCol = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uvMain);
                float3 n1 = UnpackNormalMap(_NormalMap, sampler_NormalMap, uvMain);
                float3 n2 = UnpackNormalMap(_NormalMap2, sampler_NormalMap2, uvMain2);

                // Combine normals (strength controlled)
                float3 worldNormal = normalize(IN.normalWS);
                // Build approximate TBN using IN.tangentWS and IN.normalWS
                float3 tangentWS = IN.tangentWS;
                float3 binormalWS = normalize(cross(IN.normalWS, tangentWS)) * (tangentWS.x * 0.0 + 1.0); // simple sign fallback
                float3x3 TBN = float3x3(tangentWS, binormalWS, IN.normalWS);
                float3 combinedNormalTS = normalize(lerp(n1, n2, 0.5));
                float3 finalNormalWS = normalize(mul(TBN, combinedNormalTS));
                finalNormalWS = normalize(lerp(worldNormal, finalNormalWS, _NormalStrength));

                // Base color mixing
                float4 colorMix = lerp(_ColorA, _ColorB, baseCol.r); // simple blend using base texture's red channel
                float alpha = _Opacity * baseCol.a;

                // Fresnel
                float3 viewDir = normalize(IN.viewDirWS);
                float fresnel = pow(1.0 - saturate(dot(viewDir, finalNormalWS)), _FresnelPower);

                // Specular (Blinn-Phong)
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                float3 halfDir = normalize(lightDir + viewDir);
                float ndotl = saturate(dot(finalNormalWS, lightDir));
                float spec = pow(saturate(dot(finalNormalWS, halfDir)), lerp(8.0, 64.0, _Gloss));
                float3 specCol = _SpecColor.rgb * spec * ndotl;

                // Reflection via cubemap (if provided)
                float3 refl = float3(0,0,0);
                #if defined(UNITY_DECLARE_TEXCUBE)
                float3 worldRefl = reflect(-viewDir, finalNormalWS);
                refl = UNITY_SAMPLE_TEXCUBE(_Cube, worldRefl).rgb;
                #endif

                // Wave tint & distortion
                float wave = sin((uvMain.x + uvMain.y) * 10.0 + _Time.y * 2.0) * _WaveStrength;
                colorMix.rgb += wave * 0.1;

                // Edge fade (optional): fade alpha based on view angle or custom _EdgeFade
                float edgeFactor = 1.0 - _EdgeFade * saturate(length(IN.positionWS.xz)); // crude world-space fade
                alpha *= edgeFactor;

                // Composite final color: mix base, color tint, fresnel, specular, reflection
                float3 finalColor = colorMix.rgb * baseCol.rgb;
                finalColor = lerp(finalColor, refl, fresnel * 0.5);
                finalColor += specCol;

                // Apply gamma correction if required (URP normally in linear)
                half4 outCol = half4(finalColor, alpha);

                UNITY_APPLY_FOG(IN.fogCoord, outCol);
                return outCol;
            }

            ENDHLSL
        } // End Pass
    } // End SubShader

    FallBack "Unlit/Texture"
}
