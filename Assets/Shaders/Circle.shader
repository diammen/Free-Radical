Shader "Custom/Circle" {
    Properties {
        _MainTex ("Main Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 0)
		_EmissionMap ("Emission Map", 2D) = "black" {}
		[HDR] _EmissionColor ("Emission Color", Color) = (0, 0, 0, 0)
    }

    SubShader {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

		// CGPROGRAM
		// #pragma surface surf Lambert

        // sampler2D _MainTex;

        // sampler2D _EmissionMap;
        // float4 _EmissionColor;

		// struct Input
		// {
		// 	float2 uv_MainTex;
		// };

		// void surf(Input i, inout SurfaceOutput o)
		// {
		// 	//o.Emission = _EmissionColor * tex2D(_MainTex, i.uv_MainTex).a;
		// 	//o.Emission = _EmissionColor;
		// }

		// ENDCG

        Pass {
            CGPROGRAM
            #include "UnityCG.cginc"

            #pragma vertex vert_img
            #pragma fragment frag
            #pragma target 3.0

            sampler2D _MainTex;
            sampler2D _EmissionMap;

            float4 _Color;

            float4 frag(v2f_img i): COLOR {
                fixed4 transparent = float4(_Color.xyz, 0);
                float distance = length(i.uv - float2(0.5, 0.5));
                float delta = fwidth(distance);
                float alpha = smoothstep(0.5, 0.5 - delta, distance);

                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 emission = tex2D(_EmissionMap, i.uv);

                //return fixed4(0,0,0,0);
                return lerp(transparent, emission * _Color + col, alpha);
            }
            ENDCG
        }

		
    }
}