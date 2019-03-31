// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/FlatGeometry"
{
    Properties
    {
        _OffsetTex("OffsetTexture", 2D) = "black" {}
        _AnimationSpeed("Speed",Float) = 1
        _TerrainTypeMap("TerrainType", 2D) = "white" {}
        _Height("Height",Float) = 15
        _WaterClip("WaterClip",Float)=.5
        _NoiseMap("NoiseMap",2D)="white"{}
        _SuperHeightMap("SuperHeightMap", 2D) = "bump" {}
        _BigValue("BigValue", Float) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
        	#pragma target 4.0
            #pragma vertex vert_disp
            #pragma fragment frag
			#pragma geometry geom_flat_fresnel
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            
            #include "vertexDisplace.cginc"
            
            #include "flatShading.cginc"
            
            fixed4 frag (g2f i) : SV_Target
            {
            	return fixed4(max(half3(i.fresnel,i.fresnel,i.fresnel),i.color),1);
            }
            ENDCG
        }
    }
}
