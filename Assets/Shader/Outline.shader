Shader "Unlit/Outline"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (1,1,1,1)
        _OutlineThickness ("Outline Thickness", Float) = 1
    }
    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            ZWrite Off
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _OutlineColor;
            float _OutlineThickness;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 texSize = _ScreenParams.xy;
                float2 pixelSize = _OutlineThickness / texSize;

                // Original texture color
                fixed4 originalColor = tex2D(_MainTex, i.uv);

                // Check surrounding pixels
                float outline = 0.0;
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        if (x == 0 && y == 0) continue; // Skip the center pixel

                        float2 offset = float2(x, y) * pixelSize;
                        fixed4 neighbor = tex2D(_MainTex, i.uv + offset);

                        // If a neighboring pixel is non-transparent, add to outline
                        outline += neighbor.a > 0 ? 1.0 : 0.0;
                    }
                }

                // If current pixel is part of the outline, draw the outline color
                if (outline > 0 && originalColor.a == 0)
                {
                    return _OutlineColor;
                }

                // Otherwise, draw the original color
                return originalColor;
            }
            ENDCG
        }
    }
}
