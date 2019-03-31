#if !defined(TERRAIN_DISPLACE_INCLUDED)
#define TERRAIN_DISPLACE_INCLUDED

sampler2D _TerrainTypeMap;
float4 _TerrainTypeMap_ST;

sampler2D _OffsetTex;
float4 _OffsetTex_ST;

sampler2D _NoiseMap;
float4 _NoiseMap_ST;

float _AnimationSpeed;
float _Height;
float _WaterClip;

sampler2D _SuperHeightMap;
float4 _SuperHeightMap_ST;

struct appdata
{
	float4 vertex : POSITION;
	float2 uv : TEXCOORD0;
};

struct v2g
{
	float4 wPos : SV_POSITION;
	half3 color: TEXCOORD0;
};

bool IsWater(half4 sample){
    return (sample.x < _WaterClip);
}

v2g vert_disp(appdata v){
    v2g o;
    half4 terrainTypeSample = tex2Dlod(_TerrainTypeMap, float4(TRANSFORM_TEX(v.uv, _TerrainTypeMap),1,1));
    float3 wPos = mul(unity_ObjectToWorld, v.vertex);
    o.color = tex2Dlod(_SuperHeightMap, float4(TRANSFORM_TEX(v.uv, _SuperHeightMap),1,1)).xyz;

    if(IsWater(terrainTypeSample)){
        wPos.y=0;
        half4 noiseSample = tex2Dlod(_NoiseMap, half4(TRANSFORM_TEX(v.uv, _NoiseMap)+_Time.x*_AnimationSpeed,1,1));
        half4 noiseSample_1 = tex2Dlod(_NoiseMap, half4(TRANSFORM_TEX(v.uv, _NoiseMap)*3+_Time.x*_AnimationSpeed*2,1,1));
        wPos.y += noiseSample.r*0.5+noiseSample_1.r*0.15;
        o.color = 0;
    }
    else{
        half4 offsetTexSample = tex2Dlod(_OffsetTex, float4(TRANSFORM_TEX(v.uv, _OffsetTex),1,1));
        wPos.y += offsetTexSample.x*_Height;
    }
    o.wPos = float4(wPos,1);
    
    return o;
}

#endif