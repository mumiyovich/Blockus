Shader "CustomFX/SH_FX_01"
{
	Properties
    {
        _k_y("K_Y",float)=1.0  
        _time("Time",float)=0.0   
        _count("Count",float)=0.0 
        _step("Step",Range(0,1))=0.0  
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

            float _count;
            float _step;
            float _time;
            float _k_y;

            fixed4 frag (v2f i) : SV_Target
            {              
                float2 uv = i.uv - 0.5;
                uv.y *= _k_y;

                float dist = length(uv);

                float c = sin(dist*_count*6.283 - _time)*0.499+0.5;

             //   c = step(_step, c);
                
                if(c > _step)
               {
                    discard;
               }

                return 0;//fixed4(c,c,c,1);
                
            }
            ENDCG
		}
	}
}

