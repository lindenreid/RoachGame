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

float weightedDist(float2 x, float2 y, float w)
{
    return 1.0/w * length(x-y);
}

void Voronoize_float(float2 UV, float Speed, float Density, out float Noise)
{
    float2 value = UV * Density;
    
    float2 i_st = floor(value);
    float2 f_st = frac(value);

    float minDist = 100;

    for(int y = -2; y <= 2; y++)
    {
        for(int x = -2; x <= 2; x++)
        {
            float2 neighbor = float2(x, y);
            float2 pt = random3(i_st + neighbor).xy;

            float weight = 0.2 + 0.8 * random3(i_st + neighbor).z;
            float dist = weightedDist(neighbor + pt, f_st, weight);

            minDist = min(minDist, dist);
        }
    }

    Noise = minDist;
}
#endif //MYHLSLINCLUDE_INCLUDED