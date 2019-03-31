Shader "Unlit/Brush"
{
    Properties
    {
        _Coordinate("Coordinate", Vector) = (0, 0, 0, 0)
        _Color("Color", Color) = (0, 0, 0, 0)
        _MainTex("Main Texture", 2D) = "white" {}
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _Coordinate;
            float4 _Color;
            sampler2D _MainTex;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                float draw = pow(saturate(1-distance(i.uv, _Coordinate.xy)), 50);
                fixed4 drawCol = _Color * draw;
                return saturate(col+drawCol);
            }
            ENDCG
        }
    }
}
