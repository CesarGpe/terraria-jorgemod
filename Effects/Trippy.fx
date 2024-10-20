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

// wave
const float amplitude = 0.02;
const float frequency = 10.0;

// rainbow
const float colorConst = 0.25;
const float timeMult = 2.5;
const float intensity = 0.25;

float4 MainPS(float2 coords : TEXCOORD0) : COLOR0
{
    // wave
    float wave = sin(coords.y * frequency + uTime) * amplitude;
    coords.x += wave;
    
    // rainbow
    float4 origColor = tex2D(uImage0, coords);
    float r = colorConst + 0.5 * sin(uTime * timeMult + 0.0);
    float g = colorConst + 0.5 * sin(uTime * timeMult + 2.0);
    float b = colorConst + 0.5 * sin(uTime * timeMult + 4.0);
    origColor.rgb = lerp(origColor.rgb, float3(r, g, b), intensity);
    return origColor;
}

technique Technique1
{
    pass MainPS
    {
        PixelShader = compile ps_2_0 MainPS();
    }
}