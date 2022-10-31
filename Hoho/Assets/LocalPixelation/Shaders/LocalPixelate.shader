Shader "Unlit/LocalPixelate"
{
	Properties
	{
		_ResolutionScale ("Resolution Scale", range(0,1)) = 0.5
		[Enum(UnityEngine.Rendering.CullMode)] _cull ("Face Culling Mode", Float) = 2
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		Cull [_cull]
		LOD 100

		GrabPass{
			"_Background"
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 grabPos : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _Background;
			float _ResolutionScale;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.grabPos = ComputeScreenPos(o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float4 coord = i.grabPos;
				coord.xy /= coord.w;
				float2 scale = _ScreenParams.xy * _ResolutionScale;
				coord.xy = floor( coord.xy * scale ) / scale;
				fixed4 col = tex2D(_Background, coord.xy);
				return col;
			}
			ENDCG
		}
	}
}
