Shader "Pi/Stencil mask/Stencil mask dither 64"
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

    const static float DITHER_THRESHOLDS[64] =
               {
                1.0 / 65.0,49.0 / 65.0,13.0 / 65.0,61.0 / 65.0,4.0 / 65.0,52.0 / 65.0,16.0 / 65.0,64.0 / 65.0,
                33.0 / 65.0,17.0 / 65.0,45.0 / 65.0,29.0 / 65.0,36.0 / 65.0,20.0 / 65.0,48.0 / 65.0,32.0 / 65.0,
                9.0 / 65.0,57.0 / 65.0,5.0 / 65.0,53.0 / 65.0,12.0 / 65.0,60.0 / 65.0,8.0 / 65.0,56.0 / 65.0,
                41.0 / 65.0,25.0 / 65.0,37.0 / 65.0,21.0 / 65.0,44.0 / 65.0,28.0 / 65.0,40.0 / 65.0,24.0 / 65.0,
                3.0 / 65.0,51.0 / 65.0,15.0 / 65.0,63.0 / 65.0,2.0 / 65.0,50.0 / 65.0,14.0 / 65.0,62.0 / 65.0,
                35.0 / 65.0,19.0 / 65.0,47.0 / 65.0,31.0 / 65.0,34.0 / 65.0,18.0 / 65.0,46.0 / 65.0,30.0 / 65.0,
                11.0 / 65.0,59.0 / 65.0,7.0 / 65.0,55.0 / 65.0,10.0 / 65.0,58.0 / 65.0,6.0 / 65.0,54.0 / 65.0,
                43.0 / 65.0,27.0 / 65.0,39.0 / 65.0,23.0 / 65.0,42.0 / 65.0,26.0 / 65.0,38.0 / 65.0,22.0 / 65.0
               };

            fixed4 frag (v2f i) : SV_Target
            {

               i.uv.y *= _ScreenParams.y / _ScreenParams.x;

               float4 col = tex2D(_MainTex, i.uv,0,0);

              // float c = (col.r+col.g+col.b)*0.333334;
              float c = 0.3*col.r+0.59*col.g+0.11*col.b;

               //c = max(0, min(1,(c-_min)/(_max-_min)));
               c = smoothstep(_min,_max,c);

               i.uv*=_ScreenParams.x;

               uint index = (uint(i.uv.x) % 8 + uint(i.uv.y) % 8 * 8);
               c = step(DITHER_THRESHOLDS[index],c);

               clip(c-0.5f);

               return 0;
  
            }
            ENDCG
        }
    }
}
