// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/FlatGeometry"
{
    Properties
    {
        _OffsetTex("Persistent Displace", 2D) = "black" {}
        _Height("Persistent Displace Height",Float) = 15
        _WaterClip("Terrain Type Clip Value",Float)=.5
        _SuperHeightMap("Futile Displace", 2D) = "bump" {}
        _BigValue("Futile Displace Height", Float) = 0.1
        _TerrainTypeMap("Terrain Type Map", 2D) = "white" {}
        _NoiseMap("Sea Waves map",2D)="white"{}
        _AnimationSpeed("Sea animation speed",Float) = 1
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
