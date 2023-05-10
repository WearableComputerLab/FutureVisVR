Shader "Custom/RadiusShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        // _CutOff("CutOff", Range(0,1)) = 0.5

        _Center("Center", Vector) = (0,0,0,0)
_Radius("Radius", Float) = 0.5
_RadiusColor("Radius Color", Color) = (1,0,0,1)
_RadiusWidth("Radius Width", Float) = 2
_BackgroundColor("Radius Color", Color) = (0,0,0,0)

    }
    SubShader
    {
        // Tags { "RenderType"="Opaque" }
        Tags { "RenderType"="Opaque" "Queue"="Transparent" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

 float3 _Center;
 float _Radius;
 fixed4 _RadiusColor;
 float _RadiusWidth;
 fixed4 _BackgroundColor;

#pragma surface surf Lambert alpha
        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

 struct Input
 {
   float2 uv_MainTex; // The UV of the terrain texture
   float3 worldPos; // The in-world position
 };

        // struct Input
        // {
        //     float2 uv_MainTex;
        // };

        // half _Glossiness;
        // half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        // UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        // UNITY_INSTANCING_BUFFER_END(Props)

        // void surf (Input IN, inout SurfaceOutputStandard o)
        // {
        //     // Albedo comes from a texture tinted by color
        //     fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
        //     o.Albedo = c.rgb;
        //     // Metallic and smoothness come from slider variables
        //     o.Metallic = _Metallic;
        //     o.Smoothness = _Glossiness;
        //     o.Alpha = c.a;
        // }
    
 void surf(Input IN, inout SurfaceOutputStandard o)
 {
   float d = distance(_Center, IN.worldPos);
   if (d > _Radius && d < _Radius + _RadiusWidth){
      o.Albedo = _RadiusColor;
   }

   else
     o.Albedo = _BackgroundColor;
    
    // o.Alpha = tex2D(_MainTex, IN.uv_MainTex)*_Color;
 }  
        ENDCG
    }
    FallBack "Diffuse"
}
