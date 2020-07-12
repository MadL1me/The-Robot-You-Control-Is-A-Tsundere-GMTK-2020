Shader "Hidden/CameraFeedPost"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _InterlaceFreq ("InterlaceFrequency", Float) = 4.0
        _GlitchY ("GlitchY", Float) = 0.5
        _GlitchAmount ("GlitchAmount", Float) = 0.01
        _NoiseAmount ("NoiseAmount", Float) = 0.2
        _FadeOutAmount ("FadeOutAmount", Float) = 0.0
        _Interference ("Interference", Float) = 0.0
    }
    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float _InterlaceFreq;
            float _GlitchY;
            float _GlitchAmount;
            float _NoiseAmount;
            float _FadeOutAmount;
            float _Interference;

            float noise(float2 p)
            {
                p.x += _Time * 0.01;
                p.y += _Time * 0.01;
                return (frac(sin(dot(p.xy, float2(12.9898, 78.233))) * 43758.5453));
            }

            fixed4 frag(v2f i) : SV_Target
            {
                if (i.uv.y > _GlitchY)
                    i.uv.x += _GlitchAmount;

                i.uv.x += noise(0) * 0.2 * _Interference;

                fixed4 col = tex2D(_MainTex, i.uv);
                
                if (uint(i.uv.y * 1080 / _InterlaceFreq) % 2 == 0)
                    col *= 0.85;

                col *= 1.0 - distance(i.uv, float2(0.5, 0.5)) * 1.25;

                col *= (1.0 - _NoiseAmount) + noise(floor(i.uv * 1080 / _InterlaceFreq)) * _NoiseAmount;

                col *= max(0.0, min(1.0, i.uv.y + (0.5 - _FadeOutAmount) * 2.0));

                if (_Interference > 0.3)
                {
                    float cols = (col.r + col.g + col.b) / 3.0;
                    col = fixed4(cols, cols, cols, col.a);
                }

                return col;
            }
            ENDCG
        }
    }
}
