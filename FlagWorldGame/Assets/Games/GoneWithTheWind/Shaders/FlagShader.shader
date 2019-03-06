//https://forum.unity.com/threads/waving-flag-shader-feedback-please.19575/

// Original shader by cboe - Mar, 23, 2009
// Enhanced to 3 axis movement by Seon - Jan, 21, 2010
// Added _WaveSpeed - Jan, 26, 2010 Eric5h5
// Ajustement time function - April, 19, 2010 Fontmaster
//
// Requirements: assumes you are using a subdivided plane created with X (width) * Z (height) where Y is flat.
// Requirements: assumes UV as: left X (U0) is attatched to pole, and Top Z (V1) is at top of pole.
//
// Enjoy!

    Shader "Custom/FlagShader"
    {
     
 
Properties
{
    _Color ("Main Color", Color) = (1,1,1,1)
    _MainTex ("Texture", 2D) = "white" { }
    _WaveSpeed ("Wave Speed", float) = 50.0
}
 
SubShader
{
    Pass
    {
       CULL Off
       
      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag
      #include "UnityCG.cginc"
      #include "AutoLight.cginc"
       
      float4 _Color;
      sampler2D _MainTex;
      float _WaveSpeed;
 
      // vertex input: position, normal
      struct appdata {
          float4 vertex : POSITION;
          float4 texcoord : TEXCOORD0;
      };
       
      struct v2f {
          float4 pos : POSITION;
          float2 uv: TEXCOORD0;
      };
       
      v2f vert (appdata v) {
          v2f o;
 
        float sinOff=v.vertex.x+v.vertex.y+v.vertex.z;
        float t=_Time*_WaveSpeed;
        if(t < 0.0) t *= -1.0;
        float fx=v.texcoord.x;
        float fy=v.texcoord.x*v.texcoord.y;
 
        v.vertex.x+=sin(t*1.45+sinOff)*fx*0.5;
        v.vertex.z=sin(t*3.12+sinOff)*fx*0.5-fy*0.9;
        v.vertex.y-=sin(t*2.2+sinOff)*fx*0.2;
   
          o.pos = UnityObjectToClipPos( v.vertex );
          o.uv = v.texcoord;
 
         return o;
      }
       
      float4 frag (v2f i) : COLOR
      {
         half4 color = tex2D(_MainTex, i.uv);
         return color;
      }
 
      ENDCG
 
      SetTexture [_MainTex] {combine texture}
    }
}
   Fallback "VertexLit"
}