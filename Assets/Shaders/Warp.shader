Shader "Common/Warp" {
    // Inspector variables
    Properties {
        _Color ("Color", Color) = (1, 1, 1, 1)
    }

    SubShader {
        Tags { 
            "RenderType"="Transparent"
            "Queue"="Transparent"
        }
        LOD 100

        Pass {

            // Backface culling : Do not render faces that don't *face* the camera
            //Cull Back // applies culling to the back faces (default behaviour)
            //Cull Front // applies culling to the front faces
            Cull Off // turns off culling from both faces

            ZWrite Off // Should this shader write to the depth buffer? On/Off

            //ZTest LEqual // Reads from the depth buffer,
                         // and will only render this object if its depth is less or equal to the depth written in the buffer
                         // "Will draw the object if it is *in front of* something"

            //ZTest GEqual // Reads from the depth buffer,
                         // and will only render this object if its depth is greater or equal to the depth written in the buffer
                         // "Will draw the object if it is *behind* something"

            // Blending modes
            // - Additive blending : Makes things brighter when blending
            // - Multiplicative (or Multiply) blending : Darkens things when blending
            // 
            // To define a blending mode : [src*A +- dst*B], 
            // where you can only change the value of A, B and the operator + or -

            Blend One One // Aditive blending : [src*1 + dst*1] -> 1 = One
            //Blend DstColor Zero // Multiply blending : [src*dst + dst*0] -> DstColor = dst ; 0 = Zero

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            #define TAU 6.28318530718

            float4 _Color;

            // Mesh data
            struct MeshData {
                float4 vertex : POSITION;
                float2 uv0 : TEXCOORD0;
                float3 normals : NORMAL;
            };

            // Interpolator
            struct Interpolator {
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD0;
                float2 uv : TEXCOORD1;
            };

            // Vertex Shader
            Interpolator vert (MeshData v) {
                Interpolator o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normals); // same as mul((float3x3)unity_ObjectToWorld, v.normals);
                o.uv = v.uv0;
                return o;
            }

            float InverseLerp(float a, float b, float v) {
                return (v - a) / (b - a);
            }

            // Fragment Shader
            float4 frag(Interpolator i) : SV_Target {
                float waves = cos((i.uv.y - (_Time.y * 0.1)) * TAU * 2) * 0.5 + 0.5;

                waves *= 1 - i.uv.y; // Applies the inverse component to flip the gradient fade effect

                waves *= abs(i.normal.y) < 0.999; // Removes from rendering the top and bottom faces. 
                                              // Remember that bools are cast to floats

                return waves * _Color;
            }
            ENDCG
        }
    }
}
