Shader "Custom/Mask"
{
	Properties{}

	SubShader{

		Tags {
			"RenderType" = "Opaque"
		}

		Pass {
			Blend Zero One
			ZTest NotEqual
			ZWrite Off
		}
	}
}