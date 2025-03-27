#ifndef IS_VERTEX_IN_PROJECTION_INCLUDED
#define IS_VERTEX_IN_PROJECTION_INCLUDED

// Function that calculates if a input vertex is inside a camera projection by its inputs
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

// Function to check UV points surrounding a certain pixel to check if it is a border vertex behind other object
float IsVertexOccultBorder(float2 pixelPosition, float unitUV, UnityTexture2D cameraTexture, float threshold)
{
    // Get current pixel depth value
    float4 pixelValue = SAMPLE_TEXTURE2D_LOD(cameraTexture, cameraTexture.samplerstate, pixelPosition, 0);
    float pixelDepth = pixelValue.r + pixelValue.g;
    
    // Calcule surroundings pxiels depth values (clock-wise) and compare with current pixel until one difference is greater than threshold
    float deltaX = -1;
    float deltaY = 1;
    bool isBorder = false;
    for (int i = 0; i < 8; i++)
    {
        // Get this surrounding pixel and calculate its depth
        float2 surroundingPixelUV = float2(clamp(pixelPosition.x + deltaX, -1, 1), clamp(pixelPosition.y + deltaY, -1, 1));
        float4 surroundingPixelValues = SAMPLE_TEXTURE2D_LOD(cameraTexture, cameraTexture.samplerstate, pixelPosition, 0);
        float surroundingPixelDepth = surroundingPixelValues.r + surroundingPixelValues.g;
        
        // Substract pixel depth to surrounding depth and check if it is greeater than threshold
        if (surroundingPixelDepth - pixelDepth > threshold)
        {
            return true;
        }
        
        // Update deltas for upcoming loop
        if (i < 2)
        {
            deltaX++;
        }
        else if (i < 4)
        {
            deltaY--;
        }
        else if (i < 6)
        {
            deltaX--;
        }
        else
        {
            deltaY++;
        }
    }
    
    return false;
}

// Function that calculates if a input vertex is inside a camera projection by its inputs
float2 IsVertexInOrBorder(float3 vertexPosition, float4x4 cameraView, UnityTexture2D cameraTexture, float cameraFOV, float cameraFarPlane, float cameraNearPlane, float errorFactor, float borderLength)
{
    // Output variable (x is inside projection and y is inside projection or border)
    float2 output = float2(true, true);
    
    // Calculate position relative to camera
    float4 projectionPosition = mul(cameraView, float4(vertexPosition, 1));
    
    // Get depth from position (z value) and filter out all values too near or too far to camera (behind camera near plane or camera itself and further than far plane)
    float depth = -projectionPosition.z;
    // If outside projection set first output to false
    if (depth < cameraNearPlane)
    {
        output.x = false;
        
        // If outside border set second output to false and return
        if (depth < cameraNearPlane - borderLength)
        {
            output.y = false;
            return output;
        }
    }
    // If outside projection set first output to false
    if (depth > cameraFarPlane)
    {
        output.x = false;
        
        // If outside border set second output to false and return
        if (depth > cameraFarPlane + borderLength)
        {
            output.y = false;
            return output;
        }
    }
    
    // Calculate projection limit (half height and half width) at given depth value
    float projLimit = depth * (tan(radians(cameraFOV / 2)));
    
    // Check if vertex is inside projection limits at given depth
    float projX = abs(projectionPosition.x);
    // If outside projection set first output to false
    if (projX > projLimit)
    {
        output.x = false;
        
        // If outside border set second output to false and return
        if (projX > projLimit + borderLength)
        {
            output.y = false;
            return output;
        }
    }
    
    float projY = abs(projectionPosition.y);
    // If outside projection set first output to false
    if (projY > projLimit)
    {
        output.x = false;
        
        // If outside border set second output to false and return 
        if (projY > projLimit + borderLength)
        {
            output.y = false;
            return output;
        }
    }
    
    // At this point, projection limits are totally defined and do not need to proceed calculating
    if (!output.x)
    {
        return output;
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
    
    if (distance > maxDist)
    {
        output.x = false;
        output.y = false;
        
        /*
        float unitUV = (borderLength + projLimit) / (projLimit * 2);
        output.y = IsVertexOccultBorder(float2(projU, projV), unitUV, cameraTexture, 0.0005);
        */ 
    }

    return output;
}

// Function that outputs if a vertex is inside projection
void IsVertexInProjection_float(in bool isProjectionActive, in float3 vertexPosition, in float4x4 cameraView, in UnityTexture2D cameraTexture, 
    in float cameraFOV, in float cameraFarPlane, in float cameraNearPlane, in float errorFactor,
    out bool isInProjection)
{
    if (!isProjectionActive)
    {
        isInProjection = false;
    }
    else
    {
        isInProjection = IsVertexIn(vertexPosition, cameraView, cameraTexture, cameraFOV, cameraFarPlane, cameraNearPlane, errorFactor);
    }
}

// Function that outputs if a vertex is inside projection or bordered area
void IsVertexInProjectionOrBorder_float(in bool isProjectionActive, in float3 vertexPosition, in float4x4 cameraView, in UnityTexture2D cameraTexture,
    in float cameraFOV, in float cameraFarPlane, in float cameraNearPlane, in float errorFactor,
    out bool isInOrBorder, out bool isInProjection)
{
    if (!isProjectionActive)
    {
        isInProjection = false;
        isInOrBorder = false;
    }
    else
    {
        float2 states = IsVertexInOrBorder(vertexPosition, cameraView, cameraTexture, cameraFOV, cameraFarPlane, cameraNearPlane, errorFactor, 0.1);
        isInProjection = states.x;
        isInOrBorder = states.y;
    }
}

#endif