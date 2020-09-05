Shader "Moving Background/Background"
{
    Properties
    {
    }

    SubShader
    {
       Lighting Off
       Blend One Zero

       Pass
       {
           CGPROGRAM
           #include "UnityCustomRenderTexture.cginc"
           #pragma vertex CustomRenderTextureVertexShader
           #pragma fragment frag
           #pragma target 3.0

           float4 frag(v2f_customrendertexture IN) : COLOR
           {
               return tex2D(_SelfTexture2D, IN.localTexcoord.xy + fixed2(0, 1 / _CustomRenderTextureHeight));
           }
           ENDCG
       }
    }
}