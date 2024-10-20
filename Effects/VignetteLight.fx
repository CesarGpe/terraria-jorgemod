sampler uImage0 : register(s0);
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

const float Radius = 0;
const float FadeDistance = 900;
const float DesatLevel = 0.5;

float4 MainPS(float2 coords : TEXCOORD0) : COLOR0
{
    float4 origColor = tex2D(uImage0, coords);

    // Apply vignette on the screen
    float2 targetCoords = (uTargetPosition - uScreenPosition) / uScreenResolution;
    float2 vec = (targetCoords - coords) * uScreenResolution;
    float dist = sqrt(pow(vec.x, 2) + pow(vec.y, 2));
    float lerpStrength = 0;
    
    if (dist > Radius)
    {
        float DistFromRadius = dist - Radius;
        lerpStrength = clamp(DistFromRadius / FadeDistance, 0, 1);
    }

    origColor = lerp(origColor, float4(uColor, 0), lerpStrength * uOpacity * uIntensity);
    
    // Calculate the grayscale value using the luminance formula
    float gray = dot(origColor.rgb, float3(0.299, 0.587, 0.114));

    // Mix the original color with the grayscale color in a new color
    //float3 desatColor = lerp(origColor.rgb, float3(gray, gray, gray), DesatLevel);
    
    // interpolate between the original color and the new one
    //origColor = lerp(origColor, float4(desatColor, 0), lerpStrength * -1);
    
    origColor.rgb = lerp(origColor.rgb, float3(gray, gray, gray), DesatLevel);
    
    return origColor;
}

technique Technique1
{
    pass MainPS
    {
        PixelShader = compile ps_2_0 MainPS();
    }
}