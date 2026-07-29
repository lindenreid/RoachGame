//UNITY_SHADER_NO_UPGRADE
#ifndef MYHLSLINCLUDE_INCLUDED
#define MYHLSLINCLUDE_INCLUDED

// thank you https://sangillee.com/_posts/2025-04-18-Cellular-noises/

float3 random3 (float2 p)
{
    float3 q = float3( dot(p, float2(127.1,311.7)), 
        dot(p, float2(269.5,183.3)), 
        dot(p, float2(419.2,371.9)) );
    return frac(sin(q) * 43758.5453);
}

void Voronoize_float(float2 UV, float Speed, float Density, out float Noise)
{
    float2 value = UV * Density;
    
    float2 i_st = floor(value);
    float2 f_st = frac(value);

    float minDist = 100;

    for(int y = -1; y <= 1; y++)
    {
        for(int x = -1; x <= 1; x++)
        {
            float2 neighbor = float2(x, y);
            float2 pt = random3(i_st + neighbor).xy;
            float dist = length(neighbor + pt - f_st);
            minDist = min(minDist, dist);
        }
    }

    Noise = minDist;
}
#endif //MYHLSLINCLUDE_INCLUDED