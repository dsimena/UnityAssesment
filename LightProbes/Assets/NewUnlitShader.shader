Shader "MyShader/Diffuse With LightProbes" {
    Properties { 
        [NoScaleOffset] 
        _MainTex ("Texture", 2D) = "red" {} }
  
    SubShader {
        Pass {
            Tags {
                "LightMode"="ForwardBase"
            }
        CGPROGRAM
            #pragma vertex v
            #pragma fragment f
            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"

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
                UNITY_FOG_COORDS(1)
                float3 worldPos : TEXCOORD2;
                float3 worldNormal : TEXCOORD3;
                fixed4 diff : COLOR0;
            };

            v2f v (appdata_t vertex_data) {
                v2f o;
                o.vertex = UnityObjectToClipPos(vertex_data.vertex);
                o.worldNormal = UnityObjectToWorldNormal(vertex_data.normal);
                o.worldPos = mul(unity_ObjectToWorld, vertex_data.vertex).xyz;
                o.uv = vertex_data.texcoord;
                
                half nl = max(0, dot(o.worldNormal, _WorldSpaceLightPos0.xyz));
                o.diff = nl * _LightColor0;
               
                //o.texcoord = TRANSFORM_TEX(vertex_data.texcoord,_MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                o.diff.rgb += ShadeSH9(half4(o.worldNormal,1));
                return o;
            }

            fixed4 f (v2f input_fragment) : SV_Target {
               fixed4 col = tex2D(_MainTex, input_fragment.uv);
                col *= input_fragment.diff;
                return col;
            }

            ENDCG
        }
    }
}