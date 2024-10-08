﻿sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
sampler uImage2 : register(s2);
sampler uImage3 : register(s3);
float3 uColor;
float3 uSecondaryColor;
float2 uScreenResolution;
float2 uScreenPosition;
float2 uTargetPosition;
float2 uDirection;
float uOpacity;
float uTime;
float uIntensity;
float uProgress;
float2 uImageSize1;
float2 uImageSize2;
float2 uImageSize3;
float2 uImageOffset;
float uSaturation;
float4 uSourceRect;
float2 uZoom;

float4 MainPS(float2 coords : TEXCOORD0) : COLOR0
{
    float4 origColor = tex2D(uImage0, coords);

    // Calculate the grayscale value using the luminance formula
    float gray = dot(origColor.rgb, float3(0.299, 0.587, 0.114));

    // Mix the original color with the grayscale color (50% desaturation)
    origColor.rgb = lerp(origColor.rgb, float3(gray, gray, gray), 0.5);
    
    return origColor;
}

technique Technique1
{
    pass MainPS
    {
        PixelShader = compile ps_2_0 MainPS();
    }
}