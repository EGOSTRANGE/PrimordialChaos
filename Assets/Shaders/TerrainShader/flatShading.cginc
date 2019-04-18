#if !defined(FLAT_SHADING_INCLUDED)
#define FLAT_SHADING_INCLUDED

float _BigValue;

struct g2f
{
    float4 vertex : SV_POSITION;
    half fresnel: TEXCOORD0;
    half3 color: COLOR;
};

void MakeTriangles(inout v2g i[3], half3 trgNormal, half colorFactor, inout TriangleStream<g2f> stream ){
    float4 wPos_0 = i[0].wPos;
    float4 wPos_1 = i[1].wPos;
    float4 wPos_2 = i[2].wPos;

    g2f out_0;
    g2f out_1;
    g2f out_2;

    out_0.vertex = UnityWorldToClipPos(wPos_0);
    out_1.vertex = UnityWorldToClipPos(wPos_1);
    out_2.vertex = UnityWorldToClipPos(wPos_2);    

    half3 viewDir_0 = normalize(_WorldSpaceCameraPos.xyz-i[0].wPos);
    half3 viewDir_1 = normalize(_WorldSpaceCameraPos.xyz-i[1].wPos);
    half3 viewDir_2 = normalize(_WorldSpaceCameraPos.xyz-i[2].wPos);
    
    half fresnel_0 = max(0,pow(1-dot(trgNormal, viewDir_0),2));
    half fresnel_1 = max(0,pow(1-dot(trgNormal, viewDir_1),2));
    half fresnel_2 = max(0,pow(1-dot(trgNormal, viewDir_2),2));

    out_0.fresnel = fresnel_0;
    out_1.fresnel = fresnel_1;
    out_2.fresnel = fresnel_2;

    out_0.color = i[0].color*colorFactor;
    out_1.color = i[1].color*colorFactor;
    out_2.color = i[2].color*colorFactor;

    stream.Append(out_0);
    stream.Append(out_1);
    stream.Append(out_2);
}

[maxvertexcount(6)]
void geom_flat_fresnel (
    triangle v2g i[3],
    inout TriangleStream<g2f> stream)
{
    float3 trgNormal = normalize(cross(i[1].wPos - i[0].wPos, i[2].wPos - i[0].wPos));
    MakeTriangles(i, trgNormal, 1, stream);

    half color_0 = (i[0].color.r+i[0].color.g+i[0].color.b)/3;
    half color_1 = (i[1].color.r+i[1].color.g+i[1].color.b)/3;
    half color_2 = (i[2].color.r+i[2].color.g+i[2].color.b)/3;

    if(color_0 > 0.01f || color_1 > 0.01f || color_2 > 0.01f){
        stream.RestartStrip();

        i[0].wPos.xyz += trgNormal*color_0*_BigValue;
        i[1].wPos.xyz += trgNormal*color_1*_BigValue;
        i[2].wPos.xyz += trgNormal*color_2*_BigValue;

        trgNormal = normalize(cross(i[1].wPos - i[0].wPos, i[2].wPos - i[0].wPos));

        MakeTriangles(i, trgNormal, 0, stream);
    }
}

#endif