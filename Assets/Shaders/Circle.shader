Shader "Custom/Circle" {
    Properties {
        _Color ("Color", Color) = (1, 1, 1, 0)
		_EmissionMap ("Emission Map", 2D) = "black" {}
		[HDR] _EmissionColor ("Emission Color", Color) = (0, 0, 0)
    }

    SubShader {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

		// CGPROGRAM
		// #pragma surface surf Lambert

		// half3 _EmissionColor;

		// struct Input 
		// {
		// 	float2 uv_MainTex;
		// };

		// void surf(Input i, inout SurfaceOutput o)
		// {
		// 	o.Emission = _EmissionColor;
		// }

		// ENDCG

        Pass {
            CGPROGRAM
            #include "UnityCG.cginc"

            #pragma vertex vert_img
            #pragma fragment frag
            #pragma target 3.0

            float4 _Color;

            float4 frag(v2f_img i): COLOR {
                fixed4 transparent = float4(_Color.xyz, 0);
                float distance = length(i.uv - float2(0.5, 0.5));
                float delta = fwidth(distance);
                float alpha = smoothstep(0.5, 0.5 - delta, distance);
                return lerp(transparent, _Color, alpha);
            }
            ENDCG
        }

		
    }
}