#ifndef IS_VERTEX_IN_PROJECTION_INCLUDED
#define IS_VERTEX_IN_PROJECTION_INCLUDED

float IsVertexIn(float3 vertexPosition, float4x4 cameraView, UnityTexture2D cameraTexture, float cameraFOV, float cameraFarPlane, float cameraNearPlane, float errorFactor)
{
    // Calculate position relative to camera
    float4 projectionPosition = mul(cameraView, float4(vertexPosition, 1));
    
    // Get depth from position (z value) and filter out all values too near to camera (behind camera near plane or camera itself)
    float depth = -projectionPosition.z;
    if (depth < cameraNearPlane)
    {
        return false;
    }
    
    // Calculate projection limit (half height and half width) at given depth value
    float projLimit = depth * (tan(radians(cameraFOV / 2)));
    
    // Check if vertex is inside projection limits at given depth
    float projX = abs(projectionPosition.x);
    if (projX > projLimit)
    {
        return false;
    }
    
    float projY = abs(projectionPosition.y);
    if (projY > projLimit)
    {
        return false;
    }
    
    // Now only have front vertexes inside projection are left, filter out vertexes outside range or behind objects
    // Check maximum distance value at corresponding pixel from camera depth texture
    float projU = (projectionPosition.x + projLimit) / (projLimit * 2);
    float projV = (projectionPosition.y + projLimit) / (projLimit * 2);
    float4 depthTexValue = SAMPLE_TEXTURE2D_LOD(cameraTexture, cameraTexture.samplerstate, float2(projU, projV), 0);
    float maxDist = depthTexValue.r + depthTexValue.g;
    
    // Apply ranges and error factor to maximum distance
    maxDist = maxDist * (cameraFarPlane - cameraNearPlane) + cameraNearPlane + errorFactor;

    // Check if vertex is viewed by camera or not
    float distance = sqrt(pow(projectionPosition.x, 2) + pow(projectionPosition.y, 2) + pow(projectionPosition.z, 2));
    return distance < maxDist;
}

void IsVertexInProjection_float(in float3 vertexPosition, in float4x4 cameraView, in UnityTexture2D cameraTexture, in float cameraFOV, in float cameraFarPlane, in float cameraNearPlane, in float errorFactor,
    out bool isInProjection)
{
    isInProjection = IsVertexIn(vertexPosition, cameraView, cameraTexture, cameraFOV, cameraFarPlane, cameraNearPlane, errorFactor);
}

#endif