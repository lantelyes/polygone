Shader "PolyGone/CrossFade"
{
  Properties
  {
    _Blend ( "Blend", Range ( 0, 1 ) ) = 0.5
    _Color ( "Main Color", Color ) = ( 1, 1, 1, 1 )
    _MainTex ( "Texture 1", 2D ) = "white" {}
    _Texture2 ( "Texture 2", 2D ) = ""
  }
  SubShader
  {
  	Lighting Off
    Tags { "RenderType"="Opaque" }
    LOD 300
    Pass
    {
      SetTexture[_MainTex]
      SetTexture[_Texture2]
      {
        ConstantColor ( 0, 0, 0, [_Blend] )
        Combine texture Lerp( constant ) previous
      }    
    }
  
    CGPROGRAM
    #pragma surface surf Unlit
    
    
    sampler2D _MainTex;
    sampler2D _Texture2;
    fixed4 _Color;
    float _Blend;
    
    
    half4 LightingUnlit (SurfaceOutput s, half3 lightDir, half atten) {
    	half4 c;
    	c.rgb = s.Albedo;
        c.a = s.Alpha;
        return c;
    }
    
    struct Input
    {
      float2 uv_MainTex;
      float2 uv_Texture2;
    };
    
    void surf ( Input IN, inout SurfaceOutput o )
    {
      fixed4 t1  = tex2D( _MainTex, IN.uv_MainTex ) * _Color;
      fixed4 t2  = tex2D ( _Texture2, IN.uv_Texture2 ) * _Color;
      o.Albedo  = lerp( t1, t2, _Blend );
    }
    ENDCG
  }
  FallBack "Diffuse"
}