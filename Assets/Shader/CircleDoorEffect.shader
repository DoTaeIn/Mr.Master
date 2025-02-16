Shader "Custom/CircleDoorEffect"
{
    Properties
    {
        _MaskRadius ("Mask Radius", Range(0, 1)) = 0.5
        _AspectRatio ("Aspect Ratio", Float) = 1.77778
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
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
            float _MaskRadius;
            float _AspectRatio;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Adjust UV coordinates for aspect ratio
                float2 adjustedUV = float2(i.uv.x, i.uv.y / _AspectRatio);

                // Calculate distance from center (0.5, 0.5 normalized screen space)
                float dist = distance(adjustedUV, float2(0.5, 0.5));

                // Apply the mask based on distance and radius
                if (dist < _MaskRadius)
                    return float4(0, 0, 0, 1); // Black for masked area
                else
                    return tex2D(_MainTex, i.uv); // Render the rest of the texture
            }
            ENDCG
        }
    }
}