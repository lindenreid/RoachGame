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

float level(float2 p)
{
    if (p.x == 1.0 && p.y == 1.0)
    {
        return 1.0;
    }
    return 0.0;
}

float VoronoiClusters(float2 uv)
{
    float2 f_uv = floor(uv);

    float md = 1e10;
    for(int i = -1; i <= 1; i++)
    {
        for(int j = -1; j <= 1; j++)
        {
            float2 g1 = f_uv + float2(i, j);
            float3 rr = random3(g1);
            float2 o = g1 + rr.xy;
            float d = length(o - uv);
            float z = rr.z;

            if(z < 0.75)
            {
                md = min(md, d);
            }
            else 
            {
                for(int k = 0; k <= 1; k++)
                {
                    for(int l = 0; l <= 1; l++)
                    {
                        float2 g2 = g1 + float2(k, l)/2.0;
                        rr = random3(g2);
                        o = g2 + rr.xy/2.0;
                        d = length(o - uv);
                        z = rr.z;

                        if(z < 0.75)
                        {
                            md = min(md, d);
                        }
                        else 
                        {
                            for(int n = 0; n <= 1; n++)
                            {
                                for(int m = 0; m <= 1; m++)
                                {
                                    float2 g3 = g2 + float2(m, n)/4.0;
                                    rr = random3(g3);
                                    o = g3 + rr.xy/4.0;
                                    d = length(o - uv);
                                    z = rr.z;

                                    md = min(md, d);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    return md;
}

void VoronoiClusters_float(float2 UV, float AngularOffset, float Density, out float Noise)
{
    // TODO: use angular offset to animate
    float2 v = UV * Density;
    Noise = VoronoiClusters(v);
}

void VoronoiSimple_float(float2 UV, float Speed, float Density, out float Noise)
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