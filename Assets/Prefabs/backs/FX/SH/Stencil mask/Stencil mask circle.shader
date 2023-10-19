Shader "Pi/Stencil mask/Stencil mask circle"
{
    Properties
    {
        _progress("Progress",Range(0,1))=0.0 
        _count("Count",float)=0 

        _speed("Speed",float)=0.0  



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

            float _count;
            float _progress;
            float _speed;
   

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                float4 screenPos = ComputeScreenPos(o.vertex);
                o.uv = screenPos.xy / screenPos.w;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                
               i.uv -= 0.5;
               i.uv.y *= _ScreenParams.y / _ScreenParams.x;

               float c = sin( length(i.uv) *_count *6.283 - _progress * _speed -1.57075)*0.499+0.5;

               clip(c - _progress);

               return 0;

            }
            ENDCG
        }
    }
}


