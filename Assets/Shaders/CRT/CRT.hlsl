#ifndef CRT_INCLUDED
#define CRT_INCLUDED

void GetDimentions_float(UnityTexture2D _texture, out float2 dimentions)
{
#if SHADERGRAPH_PREVIEW
	dimentions = float2(0,0);
#else
    float width, height;
    _texture.tex.GetDimensions(width, height);
    
    dimentions = float2(width,height);
    
#endif
}

void RandomRange(float2 Seed, float Min, float Max, out float Out)
{
    float randomno = frac(sin(dot(Seed, float2(12.9898, 78.233))) * 43758.5453);
    Out = lerp(Min, Max, randomno);
}

float2 unity_gradientNoise_dir(float2 p)
{
    p = p % 289;
    float x = (34 * p.x + 1) * p.x % 289 + p.y;
    x = (34 * x + 1) * x % 289;
    x = frac(x / 41) * 2 - 1;
    return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
}

float unity_gradientNoise(float2 p)
{
    float2 ip = floor(p);
    float2 fp = frac(p);
    float d00 = dot(unity_gradientNoise_dir(ip), fp);
    float d01 = dot(unity_gradientNoise_dir(ip + float2(0, 1)), fp - float2(0, 1));
    float d10 = dot(unity_gradientNoise_dir(ip + float2(1, 0)), fp - float2(1, 0));
    float d11 = dot(unity_gradientNoise_dir(ip + float2(1, 1)), fp - float2(1, 1));
    fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
    return lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x);
}

void Unity_GradientNoise(float2 UV, float2 Scale, out float Out)
{
    Out = unity_gradientNoise(float2(UV.x * Scale.x, UV.y * Scale.y))+0.5;
}


void GetLineIndex_float(float2 ScreenSize, float2 UV, int lineSize, float randomOffset, float2 noiseScale, float noiseIntensity, out float ans)
{
    #if SHADERGRAPH_PREVIEW
	ans = 0;
#else
    float width, height;
    width = ScreenSize.x;
    height = ScreenSize.y;
    
    float UVy = ((UV.y % 1)+1)%1;
    
    height = height / lineSize;
    int pixel = height * UVy;
    
    float rand, rand2;
    float offsetTexel = randomOffset / height;
    RandomRange(float2(_Time.y, pixel), -offsetTexel, offsetTexel, rand);
    
    Unity_GradientNoise(UV + float2(0, rand), noiseScale, rand2);
    rand2 = (rand2 - 0.5) * noiseIntensity;
    
    float newUV = (((UVy + rand + rand2) % 1) + 1) % 1;
    
    ans = floor(height * newUV);
#endif
}

void GetDistortedUV_float(float2 UV, float distortionIntensity, float vignetteWidth, float2 windowSize, out float2 DistortedUV, out float blackSides)
{
#if SHADERGRAPH_PREVIEW
	DistortedUV = UV;
    blackSides = 1;
#else
    
    float2 uv = UV * 2.0f - 1.0f;
    float2 offset = uv.yx / distortionIntensity;
    uv = uv + uv * offset * offset;
    DistortedUV = uv * 0.5f + 0.5f;
    
    blackSides = 1;
    
    float2 vignette = vignetteWidth / windowSize;
    vignette = smoothstep(0, vignette, 1 - abs(uv));
    vignette = saturate(vignette);
    
    blackSides = blackSides * vignette.x * vignette.y;
    
    blackSides = (DistortedUV.x < 0 || DistortedUV.x > 1 || DistortedUV.y < 0 || DistortedUV.y > 1) ? 0 : blackSides;
    
#endif
}

void GetTextureColorWithChannelOffset_float(UnityTexture2D _texture, float2 UV, float ChromaticAberrationMiddle, float ChromaticAberrationEnd, out float4 color)
{
    float2 newUV = UV - 0.5;
    float t = saturate((newUV.x * newUV.x + newUV.y * newUV.y) / 0.5);
    
    float val = lerp(ChromaticAberrationMiddle, ChromaticAberrationEnd, t);
    
    float2 redOffset = float2(-1, -1) * val;
    float2 greenOffset = float2(0, 0);
    float2 blueOffset = float2(1, 1) * val;
    
    float redSample = SAMPLE_TEXTURE2D(_texture, _texture.samplerstate, UV + redOffset).r;
    float greenSample = SAMPLE_TEXTURE2D(_texture, _texture.samplerstate, UV + greenOffset).g;
    float blueSample = SAMPLE_TEXTURE2D(_texture, _texture.samplerstate, UV + blueOffset).b;
    
    color = float4(redSample, greenSample, blueSample, 1);
    
}


void AddScanLines_float(float4 color, float2 UV, float intensity, float lineIndex, out float4 res)
{
    //SamplerState samplerTexture;
#if SHADERGRAPH_PREVIEW
	res = float4(1,1,1,1);
#else
    
    float val = max(0, min(1, (lineIndex % 2) + (1.0f - max(0, min(1, intensity)))));
    
    res = color * val;
    
#endif
}

#endif