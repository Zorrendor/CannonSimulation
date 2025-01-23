Shader "Unlit/BulletTrajectory"
{
    Properties
    {
        _Color("Color", Color) = (1, 1, 1, 1)
        _InitialPosition("Initial Position", Vector) = (0, 0, 0, 0)
        _InitialVelocity("Initial Velocity", Vector) = (0, 0, 1, 0)
        _MaxTime("Max time", Float) = 10
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct Attributes
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 vertex : SV_POSITION;
            };

            CBUFFER_START(UnityPerMaterial)
            half4 _Color;
            float4 _InitialPosition;
            float4 _InitialVelocity;
            float _MaxTime;
            CBUFFER_END

            Varyings vert (Attributes v)
            {
                Varyings o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                // TO DO Calculate offset for vertex width
                // float t = v.uv.x * _MaxTime;
                // float3 displacement = _InitialVelocity * t;
                // displacement.y -= 0.5 * 9.81 * t * t;
                // float3 worldPos = _InitialPosition.xyz + displacement; 
                // o.vertex = mul(UNITY_MATRIX_VP, float4(worldPos, 1));
                
                return o;
            }

            half4 frag (Varyings i) : SV_Target
            {
                return _Color;
            }
            ENDCG
        }
    }
}
