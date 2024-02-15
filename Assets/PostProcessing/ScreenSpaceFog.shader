Shader "Custom Post-Processing/Screen Space Fog"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
    }

    SubShader
    {
        ZWrite Off
        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct vertexData 
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(vertexData v)
            {
                v2f outF;
                outF.vertex = UnityObjectToClipPos(v.vertex);
                outF.uv = v.uv;
                return outF;
            }

            sampler2D _MainTex, _CameraDepthTexture;
            float4 _FogColor;
            float _FogDensity;
            float _FogOffset;
            float _FarFogFactor;

            float4 frag(v2f i) : SV_Target
            {
                int x, y;
                float4 col = tex2D(_MainTex, i.uv);

                // Get depth
                float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv);
                depth = Linear01Depth(depth);

                float viewDistance = depth * _ProjectionParams.z;

                // Increase fog when further away
                float fog = (_FogDensity / sqrt(log(2))) * 
                    max(0.0f, viewDistance - _FogOffset);
                fog = exp2(-fog * fog);

                if (depth >= 0.99999)
                {
                    fog = _FarFogFactor;
                }
                
                float4 output = lerp(_FogColor, col, saturate(fog));

                return output;
            }

            ENDCG
        }
    }
}