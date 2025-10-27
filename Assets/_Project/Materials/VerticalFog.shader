Shader "Custom/VerticalFogTransparent"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _FogColor ("Fog Color", Color) = (0.7, 0.7, 0.7, 1)
        _FogStartY ("Fog Start Height", Float) = 0
        _FogEndY ("Fog End Height", Float) = 5
        _FogIntensity ("Fog Intensity", Range(0,1)) = 1
        _Alpha ("Transparency", Range(0,1)) = 0.5
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }
        LOD 200
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Back

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _FogColor;
            float _FogStartY;
            float _FogEndY;
            float _FogIntensity;
            float _Alpha;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float worldY : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
                float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldY = worldPos.y;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 baseCol = tex2D(_MainTex, i.uv);

                // Tính độ dày sương theo độ cao
                float fogFactor = saturate(1 - (i.worldY - _FogStartY) / (_FogEndY - _FogStartY));
                fogFactor *= _FogIntensity;

                // Trộn màu sương
                fixed4 fogCol = _FogColor;
                fogCol.a = fogFactor * _Alpha;

                // Blend thủ công với texture
                fixed4 final = lerp(baseCol, fogCol, fogCol.a);

                // Xuất alpha để blend với nền
                final.a = fogCol.a;

                return final;
            }
            ENDCG
        }
    }

    FallBack Off
}
