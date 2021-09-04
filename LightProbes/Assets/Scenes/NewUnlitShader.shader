Shader "MyShader/Diffuse With LightProbes" {
    Properties { [NoScaleOffset] _MainTex ("Texture", 2D) = "white" {} }
    SubShader {
        Pass {
            Tags {
                "LightMode"="ForwardBase"
            }
        CGPROGRAM
            #pragma vertex v
            #pragma fragment f
            #include "UnityCG.cginc"
            sampler2D _MainTex;

            struct appdata_t {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;                
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                UNITY_FOG_COORDS(1)
                float3 worldPos : TEXCOORD2;
                float3 worldNormal : TEXCOORD3;
            };

            v2f v (appdata_base vertex_data) {
                v2f o;
                o.vertex = UnityObjectToClipPos(vertex_data.vertex);
                o.worldNormal = UnityObjectToWorldNormal(vertex_data.normal);
                o.worldPos = mul(unity_ObjectToWorld, vertex_data.vertex).xyz;
                o.uv = vertex_data.texcoord;
                //o.texcoord = TRANSFORM_TEX(vertex_data.texcoord,_MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 f (v2f input_fragment) : SV_Target {
                half3 currentAmbient = half3(0, 0, 0);
                //half3 ambient = ShadeSHPerPixel(input_fragment.worldNormal, currentAmbient, input_fragment.worldPos);
                fixed4 col = tex2D(_MainTex, input_fragment.uv);
                //col.xyz += ambient;
                UNITY_APPLY_FOG_COLOR(iinput_fragment.fogCoord, col, fixed4(0,0,0,0)); // fog towards black due to our blend mode
                return col;
            }

            ENDCG
        }
    }
}