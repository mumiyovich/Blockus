Shader "Pi/Stencil mask/Stencil mask"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        _min("Min",Range(0,1))=0.0 
        _max("Max",Range(0,1))=1.0 



    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Blend Zero One
			ZTest NotEqual
			ZWrite Off

            Stencil
            {
                 Ref 1
                 Comp Always
                 Pass Replace
            }

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
            float _min;
            float _max;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                float4 screenPos = ComputeScreenPos(o.vertex);
                o.uv = screenPos.xy / screenPos.w;

                o.uv = TRANSFORM_TEX(o.uv, _MainTex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {

               i.uv.y *= _ScreenParams.y / _ScreenParams.x;


               float4 col = tex2D(_MainTex, i.uv);

               float c = (col.r+col.g+col.b)*0.333334;

               //c = max(0, min(1,(c-_min)/(_max-_min)));
               c = smoothstep(_min,_max,c);

               
               clip(c-0.5f);

               return 0;

            }
            ENDCG
        }
    }
}
