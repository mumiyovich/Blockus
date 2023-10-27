Shader "Pi/wait"
{
    Properties
    {
       // _MainTex ("Texture", 2D) = "white" {}

        _color("Color", Color) = (1, 1, 1, 1)
        _color_back("Color back", Color) = (1, 1, 1, 0.5)

        _r1("Radius 1",Range(0,0.5))=0.1 
        _r2("Radius 2",Range(0,0.5))=0.4 
        _smooth("Radius smooth",Range(0,0.5))=0.1 
       // _length("Length",Range(0,1))=0.75 
        _speed1("Speed 1",float)=1
        _speed2("Speed 2",float)=1
        
    }

    SubShader
    {
        //Tags { "RenderType"="Opaque" }
        Tags { "RenderType"="Transparent" }
        //Tags { "Queue"="Transparent" }
        LOD 100

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
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

       //     sampler2D _MainTex;
        //    float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv-0.5;//TRANSFORM_TEX(v.uv, _MainTex)- 0.5;
                return o;
            }

            float _r1;
            float _r2;
            float _smooth;
        //    float _length;
            float _speed1;
            float _speed2;

            fixed4 _color;
             fixed4 _color_back;

             static const float PI = 3.14159265f;

            fixed4 frag (v2f i) : SV_Target
            {
                
                fixed4 c = _color;
                float a;

                float _length = sin(_Time.y * PI * 1 * _speed2)*0.5+0.5;

                float _time_offset = _Time.y * PI*2 * _speed1;

                float x =sin(_time_offset);
                float y =cos(_time_offset);

                float2 cir = float2(x,y);

                float len = length(i.uv);
                a = smoothstep(_r1-_smooth,_r1,len) * (1 -smoothstep(_r2-_smooth,_r2,len));

                float pr =dot(normalize(i.uv), cir);

                if(pr > _length*2-1)
                {
                    c = _color_back;
                }

                 c.a *= a;

                return c;
            }
            ENDCG
        }
    }
}
