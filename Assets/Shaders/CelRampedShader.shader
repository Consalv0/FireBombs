Shader "Custom/CelRamped"
{
	Properties
	{
		[Header(Colors)]
        [HideInInspector]
		_Color ("Color", Color) = (0.5,0.5,0.5,1.0)
        [HideInInspector]
        _Cutoff ("Alpha Cutoff", Range(0, 1)) = 0.5
        [HideInInspector]
		_HColor ("Highlight Color", Color) = (0.1,0.1,0.1,1.0)
        [HideInInspector]
		_SColor ("Shadow Color", Color) = (0.9,0.9,0.9,1.0)
		
        [Header(Diffuse and Ramp)]
        [HideInInspector]
		_MainTex ("Texture (RGB) Alpha (A)", 2D) = "white" {}    
        [HideInInspector]  
		_Ramp ("Color Ramp (RGB) Alpha (A)", 2D) = "gray" {}

        [Header(Outline)]
        _OutlineColor ("Outline Color", Color) = (0,1,0,1)
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
        #pragma surface surf Custom fullforwardshadows keepalpha
		#pragma target 3.0
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
		};

        fixed _Mode;
		
		//================================================================
		// CUSTOM LIGHTING
		
		//Lighting-related variables
		fixed4 _HColor;
		fixed4 _SColor;
		sampler2D _Ramp;
		
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
			fixed ndl = max(0, dot(s.Normal, lightDir)*0.5 + 0.5);
			
			fixed3 ramp = tex2D(_Ramp, fixed2(ndl,ndl));
		#if !(POINT) && !(SPOT)
			ramp *= atten;
		#endif
			_SColor = lerp(_HColor, _SColor, _SColor.a);
			ramp = lerp(_SColor.rgb,_HColor.rgb,ramp);
			fixed4 c;
			c.rgb = s.Albedo * _LightColor0.rgb * ramp * 2;
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
			o.Albedo = mainTex.rgb * _Color.rgb * (_RimColor.rgb * rim);
                     
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
