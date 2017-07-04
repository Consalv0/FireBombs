Shader "Custom/CelRamped Shader"
{
	Properties
	{
		[Header(Colors)]
        [HideInInspector]
		_Color ("Color", Color) = (0.5,0.5,0.5,1.0)
        [HideInInspector]
        _Cutoff ("Alpha Cutoff", Range(0, 1)) = 0.5
        [HideInInspector]
		_HColor ("Highlight Color", Color) = (0.9,0.9,0.9,1.0)
        [HideInInspector]
		_SColor ("Shadow Color", Color) = (0.1,0.1,0.1,1.0)
        [HideInInspector]
        _Roughness ("Roughness", Range(0, 1)) = 0.5
        [HideInInspector]
        _Specular ("Specular", Range(0, 1)) = 0
        [HideInInspector] 
        _RampSaturation("Ramp Gray Scale", Float) = 0
		
        [Header(Diffuse and Ramp)]
        [HideInInspector]
		_MainTex ("Texture (RGB) Alpha (A)", 2D) = "white" {}    
        [HideInInspector]  
		_Ramp ("Color Ramp (RGB) Alpha (A)", 2D) = "gray" {}

        [Header(Outline)]
        [HideInInspector]
        _OutlineColor ("Outline Color", Color) = (0,1,0,1)
        [HideInInspector]
        _OutlineWeight ("Outline Weight", Range (0.002, 0.03)) = 0.01
		
        [Header(Rim)]
		_RimColor ("Rim Color", Color) = (0.8,0.8,0.8,0.6)
		_RimMin ("Rim Min", Range(0,1)) = 0.5
		_RimMax ("Rim Max", Range(0,1)) = 1.0      

        // BLENDING STATE
        [HideInInspector]
        _Mode("Render Mode", Float) = 0
        [HideInInspector]
        _SrcBlend ("__src", Float) = 1.0
        [HideInInspector]
        _DstBlend ("__dst", Float) = 0.0
        [HideInInspector]
        _ZWrite ("__zw", Float) = 1.0
	}
	
	SubShader
	{
		Tags { "RenderType" = "Transparent" }

        Blend [_SrcBlend] [_DstBlend]
        ZWrite [_ZWrite]
		
		CGPROGRAM
        #pragma surface surf Custom keepalpha fullforwardshadows
		#pragma target 2.0
		#pragma glsl
		
		
		//================================================================
		// VARIABLES
		
		fixed4 _Color;
        half _Cutoff;
		sampler2D _MainTex;
		
		fixed4 _RimColor;
		fixed _RimMin;
		fixed _RimMax;
		float4 _RimDir;

		struct Input
		{
			half2 uv_MainTex;
			float3 viewDir;
            float3 worldRefl;
		};

        fixed _Mode;
		
		//================================================================
		// CUSTOM LIGHTING
		
		//Lighting-related variables
		fixed4 _HColor;
		fixed4 _SColor;
		sampler2D _Ramp;
        float _RampSaturation;
        half _Roughness;
        half _Specular;
		
		//Custom SurfaceOutput
		struct SurfaceOutputCustom
		{
			fixed3 Albedo;
			fixed3 Normal;
			fixed3 Emission;
			half Specular;
			fixed Alpha;
		};
		
		inline half4 LightingCustom (SurfaceOutputCustom s, half3 lightDir, half3 viewDir, half atten)
		{
			fixed ndl = pow(max(0, dot(s.Normal, lightDir) * (1 - _Roughness * 0.5)), .015 + _Specular * 5);
            
            fixed3 ramp = tex2D(_Ramp, fixed2(ndl, 0.5));;
            if (_RampSaturation > 0) {
                ramp = lerp(ramp, dot(ramp, float3(.222, .707, .071)), _RampSaturation);
            }
		#if !(POINT) && !(SPOT)
			ramp *= atten;
		#endif
			ramp = lerp(_SColor.rgb, _HColor.rgb, ramp);
			fixed4 c;
			c.rgb = s.Albedo * _LightColor0.rgb * ramp * (atten * 2) + ramp - 0.35;
			c.a = s.Alpha;
		#if (POINT || SPOT)
			c.rgb *= atten;
		#endif
			return c;
		}
		
		//================================================================
		// SURFACE FUNCTION
		
		void surf (Input IN, inout SurfaceOutputCustom o)
		{
			fixed4 mainTex = tex2D(_MainTex, IN.uv_MainTex);
            
            half rim = dot(IN.viewDir, o.Normal);
            rim = smoothstep(_RimMin, _RimMax, rim);
			o.Emission = rim * _RimColor.a * _RimColor.rgb;
            o.Albedo = mainTex.rgb * _Color.rgb;
                     
            half alpha = mainTex.a * _Color.a;
            if (_Mode == 1)
              if (alpha <= _Cutoff)
                alpha = 0;
              else
                alpha = 1;

            o.Alpha = alpha;
		}
		
		ENDCG
	}
	
	Fallback "Diffuse"
    CustomEditor "CelRampedShaderGUI"
}
