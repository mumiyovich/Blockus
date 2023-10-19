Shader "Pi/Stencil mask/Stencil mask dither 16"
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

  const static float DITHER_THRESHOLDS[16] =
               {
                            1.0 / 17.0,  9.0 / 17.0,  3.0 / 17.0, 11.0 / 17.0,
                            13.0 / 17.0,  5.0 / 17.0, 15.0 / 17.0,  7.0 / 17.0,
                            4.0 / 17.0, 12.0 / 17.0,  2.0 / 17.0, 10.0 / 17.0,
                            16.0 / 17.0,  8.0 / 17.0, 14.0 / 17.0,  6.0 / 17.0
               };

            fixed4 frag (v2f i) : SV_Target
            {

               i.uv.y *= _ScreenParams.y / _ScreenParams.x;

               float4 col = tex2D(_MainTex, i.uv,0,0);

               float c = (col.r+col.g+col.b)*0.333334;

               //c = max(0, min(1,(c-_min)/(_max-_min)));
               c = smoothstep(_min,_max,c);

               i.uv*=_ScreenParams.x;

               uint index = (uint(i.uv.x) % 4 + uint(i.uv.y) % 4 * 4);
               c = step(DITHER_THRESHOLDS[index],c);


               clip(c-0.5f);

               return 0;
  
            }
            ENDCG
        }
    }
}
