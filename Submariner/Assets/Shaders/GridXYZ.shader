Shader "Custom/GridXYZ"
{
	Properties
	{
		_LightColor("Light Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Radius("Radius", Float) = 10
		_FadeRadius("Fade Radius", Float) = 1

		_ScanColor("Scan Color", Color) = (1,1,1,1)
		_Emission("Emission", Range(0,1)) = 0.5
		_GridStep("Grid size", Float) = 10
		_GridWidth("Grid width", Float) = 1

		_ScanWidth("Scan Width", Float) = 10

		_FirstLineWidth("First Line Width", Float) = .1
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 200

			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			#pragma surface surf Standard fullforwardshadows

			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0

			sampler2D _MainTex;

			struct Input
			{
				float2 uv_MainTex;
				float3 worldPos;
				float3 localPos;
			};

			half _Emission;
			fixed4 _LightColor;
			fixed4 _ScanColor;
			float _GridStep;
			float _GridWidth;

			float _ScanWidth;
			float _FirstLineWidth;

			float3 _WorldSpaceScannerPos;
			float _ScanDistance;

			float _Radius;
			float _FadeRadius;

			void surf(Input IN, inout SurfaceOutputStandard o) {

				// Albedo comes from a texture tinted by color
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _LightColor;

				float dst = distance(IN.worldPos, _WorldSpaceScannerPos);
				if (dst < _Radius + _FadeRadius)
				{
					if (dst > _Radius)
						c = c * .5;

					o.Emission = c.rgb * .7; // * _Color.a;
				}
				else
				{
					c = fixed4(0, 0, 0, 0);
					o.Emission = 0;
				}
				//else
				{
					fixed4 c2 = _ScanColor;

					if (dst < _ScanDistance && dst > _ScanDistance - _ScanWidth)
					{
						float grid = 1;

						if (IN.worldPos.y - _WorldSpaceScannerPos.y < _FirstLineWidth && IN.worldPos.y - _WorldSpaceScannerPos.y > 0)
							grid = 1;
						else if (dst < _ScanDistance - _FirstLineWidth)
						{
							// grid overlay
							float3 pos = IN.worldPos.xyz / _GridStep;
							float3 f = abs(frac(pos) - .5);
							float3 df = fwidth(pos) * _GridWidth;
							float3 g = smoothstep(-df, df, f);
							grid = 1.0 - saturate(g.x * g.y * g.z);
						}

						if (grid > .1)
						{
							c2.rgb *= lerp(float3(0,0,0), c2.rgb, grid);
							c = c2;

							float diff = 1 - (_ScanDistance - dst) / (_ScanWidth);

							c.rgb *= diff;

							o.Emission = c.rgb * grid * _Emission * 10;
						}
					}
					else c.rgba = float4(0, 0, 0, 0);
				}

				o.Albedo = c.rgb;
				o.Alpha = c.a;
			}
			ENDCG
		}
			FallBack "Diffuse"
}