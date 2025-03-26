#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

struct Attributes
{
    float3 positionOS : POSITION;
};

struct Interpolators
{
    float4 positionCS : SV_POSITION;
    float4 positionWS : TEXCOORD1;
};

Interpolators Vertex(Attributes input)
{
    Interpolators output;

    // Get clip and world position and pass it to fragment
    VertexPositionInputs posnInputs = GetVertexPositionInputs(input.positionOS);
    output.positionCS = posnInputs.positionCS;
    output.positionWS = float4(posnInputs.positionWS, 1);

    return output;
}

float4 Fragment(Interpolators input) : SV_TARGET
{   
    // Calcule distance from vertex to camera in world space
    float4 distanceVector = float4(_WorldSpaceCameraPos, 1) - input.positionWS;
    float distance = sqrt(pow(distanceVector.x, 2) + pow(distanceVector.y, 2) + pow(distanceVector.z, 2));
    
    float nearPlane = _ProjectionParams.y;
    float farPlane = _ProjectionParams.z;
    
    // Asign a value in according to proximity to camera near and far planes (nearPlane or nearer is 0)
    float value = 0;
    if (distance > nearPlane)
    {
        float depth = farPlane - nearPlane;
        value = (distance - nearPlane) / depth;
    }
    
    float red = clamp(value, 0, 1);
    float green = value - red;

    return float4(red, green, 0, 1);
};