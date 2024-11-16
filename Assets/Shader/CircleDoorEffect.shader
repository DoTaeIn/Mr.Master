Shader "Custom/CircleDoorEffect"
{
   Properties
    {
        _MaskCenter ("Mask Center", Vector) = (0.5, 0.5, 0, 0) // Center of the mask
        _MaskRadius ("Mask Radius", Float) = 0.5               // Radius of the transparent circle
        _MaskSoftness ("Mask Softness", Float) = 0.02          // Softness at the edge of the circle
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
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
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float2 _MaskCenter;
            float _MaskRadius;
            float _MaskSoftness;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.vertex.xy / float2(1920, 1080); // Normalize for 1920x1080
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Calculate distance from the center
                float dist = distance(i.uv, _MaskCenter);

                // Create a hole effect
                float alpha = smoothstep(_MaskRadius, _MaskRadius - _MaskSoftness, dist);

                // Black screen with a transparent hole in the center
                return fixed4(0, 0, 0, 1 - alpha);
            }
            ENDCG
        }
    }
}
