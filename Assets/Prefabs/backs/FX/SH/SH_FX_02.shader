Shader "CustomFX/SH_FX_02"
{
	Properties
    {

        _MainTex ("Texture", 2D) = "white" {}

        _time("Time",float)=0.0   

        _size_x("SizeX",float)=10.0 
        _size_y("SizeY",float)=10.0 
        _shift("Shift",Range(0,1))=0.0  
        _blur("Blur",Range(0,1))=0.0 
        _min("Min",Range(0,1))=0.0 
        _max("Max",Range(0,1))=1.0 

        [Toggle] _use_min_max("UseMinMax",int)=0
        _min_max("MinMax",Range(0,1))=0.0 

        [Toggle] _use_dither("UseDither",int)=0
        _dither_size("DitherSize",float)=5
        

        _step("Step",Range(0,1))=0.5  
    }

	SubShader
    {

		Tags
        {
			"RenderType" = "Opaque"
		}

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

            sampler2D _MainTex;

            struct vertex_data
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (vertex_data v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            /*
            float3 hash3( float2 p )
            {
                   float3 q = float3( dot(p,float2(127.1,311.7)), 
				   dot(p,float2(269.5,183.3)), 
				   dot(p,float2(419.2,371.9)) );
	               return frac(sin(q)*43758.5453);
            }

           float voronoise( in float2 p, float u, float v )
           {
	            float k = 1.0+63.0*pow(1.0-v,6.0);

                float2 i = floor(p);
                float2 f = frac(p);
    
	            float2 a = float2(0.0,0.0);
                for( int y=-2; y<=2; y++ )
                for( int x=-2; x<=2; x++ )
                {
                    float2  g = float2( x, y );
		            float3  o = hash3( i + g )*float3(u,u,1.0);
		            float2  d = g - f + o.xy;
		            float w = pow( 1.0-smoothstep(0.0,1.414,length(d)), k );
		            a += float2(o.z*w,w);
                }
	
                return a.x/a.y;
           }



           */
           
            
            float _time;

            float _step;

            float _blur;
            float _shift;
            float _size_x;
            float _size_y;
            float _min;
            float _max;

            bool _use_min_max;
            float _min_max;

            bool _use_dither;
            float _dither_size;

           float dither(float2 uv, float color)
           {

               static float DITHER_THRESHOLDS[16] =
               {
                            1.0 / 17.0,  9.0 / 17.0,  3.0 / 17.0, 11.0 / 17.0,
                            13.0 / 17.0,  5.0 / 17.0, 15.0 / 17.0,  7.0 / 17.0,
                            4.0 / 17.0, 12.0 / 17.0,  2.0 / 17.0, 10.0 / 17.0,
                            16.0 / 17.0,  8.0 / 17.0, 14.0 / 17.0,  6.0 / 17.0
               };
 
 
               //uint index = abs(uint(uv.x * 100) % 4 + uint(uv.y * 250) % 4 * 4);
               uint index = (uint(uv.x * _size_x*2 * _dither_size) % 4 + uint(uv.y * _size_y*2 * _dither_size ) % 4 * 4);

               float d = DITHER_THRESHOLDS[index];


               float closestColor = (color < 0.5) ? 0 : 1;
               float secondClosestColor = 1 - closestColor;
               float distance = abs(closestColor - color);
               return (distance < d) ? closestColor : secondClosestColor;
           }

            fixed4 frag (v2f i) : SV_Target
            {      

                
                //float2 uv = float2((i.uv.x-0.5)*_size_x,(i.uv.y-0.5)*_size_y);

                /*
                float2 uv = float2((i.uv.x)*_size_x,(i.uv.y)*_size_y);
                float2 p = float2(_shift,_blur);
	            p = p*p*(3.0-2.0*p);
	            p = p*p*(3.0-2.0*p);
	            p = p*p*(3.0-2.0*p);
	            float f = voronoise( uv, p.x, p.y );//24
                */

                float f = tex2D(_MainTex, i.uv).r;

                if(_use_min_max)
                {
                    _min = smoothstep(0.25,1,_min_max);
                    _max = smoothstep(0,0.75,_min_max);
                }

                f = smoothstep(_min, _max, f);

                if(_use_dither)
                {
                    f = dither(i.uv, f);
                }

               if(f > _step)
               {
                  discard;
               }

               return 0;

               
	
	            //fixed4 fragColor = fixed4( f, f, f, 1.0 );
                //return fragColor;//fixed4(c,c,c,1);
                
                
            }
            ENDCG
		}
	}
}





