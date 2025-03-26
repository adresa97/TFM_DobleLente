Shader "Custom Shaders/Depth Shader" {
    SubShader {
        Tags { "RenderPipeline"="UniversalPipeline" }
        Pass {
            Name "ForwardLit"
            Tags{"LightMode" = "UniversalForward"}

            HLSLPROGRAM
            #pragma vertex Vertex
            #pragma fragment Fragment

            #include "DepthShader.hlsl"
            ENDHLSL
        }
    }
}
