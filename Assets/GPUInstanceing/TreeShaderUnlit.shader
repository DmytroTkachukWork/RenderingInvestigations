Shader "Unlit/TreeShaderUnlit"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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
            // make fog work
            #pragma multi_compile_fog
            #pragma multi_compile_instancing

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                UNITY_SETUP_INSTANCE_ID( v );
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                clip( col.a - 0.1 );
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }

        Pass
        {
          Tags { "LightMode" = "ShadowCaster" }

          CGPROGRAM

          #pragma multi_compile_instancing

          #pragma vertex vert
          #pragma fragment frag
          #pragma target 2.0

          #include "UnityCG.cginc"

          struct v2f
          {
            V2F_SHADOW_CASTER;
          };

          v2f vert( appdata_base v )
          {
            v2f o;
            UNITY_SETUP_INSTANCE_ID( v );
            TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
            return o;
          }

          fixed4 frag( v2f i ) : SV_Target
          {
            SHADOW_CASTER_FRAGMENT( i )
          }
          ENDCG
        }
    }
}
