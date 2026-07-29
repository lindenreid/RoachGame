//UNITY_SHADER_NO_UPGRADE
#ifndef MYHLSLINCLUDE_INCLUDED
#define MYHLSLINCLUDE_INCLUDED

// thank you https://sangillee.com/_posts/2025-04-18-Cellular-noises/
// Created by inigo quilez - iq/2013
// License Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// http://www.iquilezles.org/www/articles/voronoilines/voronoilines.htm
// Edited by Sangil Lee
// Translated to HLSL and further edited by Travis Reid

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

float VoronoiClusters(float2 uv, float ClusterPropability, float Time, float MoveSpeed)
{
    // quantize to create the tiles
    float2 cellCoords = floor(uv);

    // big numbah
    float minDist = 1e10;

    // loop through self + 8 neighbos - self is at (0,0)
    // 3 nested for loops to create clusters inside individual cells
    for(int i = -1; i <= 1; i++)
    {
        for(int j = -1; j <= 1; j++)
        {
            // get distance from this pixel to neighbor cell's blob center
            float2 neighborCoords1 = cellCoords + float2(i, j);
            float3 cellRand = random3(neighborCoords1);
            // must animate blob's center location BEFORE applying neighbor coords
            float2 cellRandCenter = 0.5 + 0.5 * sin(Time * MoveSpeed + 6.2831 * cellRand);
            float2 blobCenter = neighborCoords1 + cellRandCenter.xy;
            float dist = length(blobCenter - uv);

            // random value to determine if this cell should create a cluster
            // keep original random value- NOT animated- as the chance of clustering should remain the same over time
            float cluster = cellRand.z;

            if(cluster < 1.0 - ClusterPropability)
            {
                // if we don't need an inner cluster, stop here
                minDist = min(minDist, dist);
            }
            else 
            {
                // otherwise, loop again to create a cluster inside this cell
                for(int k = 0; k <= 1; k++)
                {
                    for(int l = 0; l <= 1; l++)
                    {
                        float2 neighborCoords2 = neighborCoords1 + float2(k, l)/2.0;
                        cellRand = random3(neighborCoords2);
                        cellRandCenter = 0.5 + 0.5 * sin(Time * MoveSpeed + 6.2831 * cellRand);
                        blobCenter = neighborCoords2 + cellRandCenter.xy/2.0;

                        // weight inner clusters to be smaller
                        dist = weightedDist(blobCenter, uv, 0.5);

                        cluster = cellRand.z;

                        if(cluster < 1.0 - ClusterPropability)
                        {
                            minDist = min(minDist, dist);
                        }
                        else 
                        {
                            for(int n = 0; n <= 1; n++)
                            {
                                for(int m = 0; m <= 1; m++)
                                {
                                    float2 neighborCoords3 = neighborCoords2 + float2(m, n)/4.0;
                                    cellRand = random3(neighborCoords3);
                                    cellRandCenter = 0.5 + 0.5 * sin(Time * MoveSpeed + 6.2831 * cellRand);
                                    blobCenter = neighborCoords3 + cellRandCenter.xy/4.0;
                                    dist = weightedDist(blobCenter, uv, 0.25);
                                    cluster = cellRand.z;

                                    minDist = min(minDist, dist);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    return minDist;
}

void VoronoiClusters_float(float2 UV, float Time, float MoveSpeed, float Density, float ClusterPropability, out float Noise)
{
    // TODO: use angular offset to animate
    float2 v = UV * Density;
    Noise = VoronoiClusters(v, ClusterPropability, Time, MoveSpeed);
}

void VoronoiWeighted_float(float2 UV, float Time, float MoveSpeed, float Density, out float Noise)
{
    float2 value = UV * Density;
    
    float2 i_st = floor(value);
    float2 f_st = frac(value);

    float minDist = 100;

    for(int y = -2; y <= 2; y++)
    {
        for(int x = -2; x <= 2; x++)
        {
            float2 neighborCoords = float2(x, y);

            float2 blobCenter = random3(i_st + neighborCoords).xy;
            blobCenter = 0.5 + 0.5 * sin(Time * MoveSpeed + 6.2831 * blobCenter);

            float weight = 0.2 + 0.8 * random3(i_st + neighborCoords).z;
            float dist = weightedDist(neighborCoords + blobCenter, f_st, weight);

            minDist = min(minDist, dist);
        }
    }

    Noise = minDist;
}

void VoronoiSimple_float(float2 UV, float Time, float MoveSpeed, float Density, out float Noise)
{
    // UV is 0-1 value, so make it larger based on how many tiles we want to have
    float2 value = UV * Density;
    
    // quantize to create the tiles
    float2 cellCoords = floor(value);

    // get this pixel's exact position inside the cell
    float2 innerCellPos = frac(value);

    // big numbah
    float minDist = 1e10;

    // loop through self + 8 neighbors (9 cells total)
    // self is at (0,0)
    for(int y = -1; y <= 1; y++)
    {
        for(int x = -1; x <= 1; x++)
        {
            // get coordinates for neighbor tile
            float2 neighborCoords = float2(x, y);

            // get center of tile
            // center is a randomized value using the UV as the seed
            float2 blobCenter = random3(cellCoords + neighborCoords).xy;

            // animate center of blob based on time 
            // for some reason, _Time isn't working properly inside this custom ShaderGraph function,
            // so that's why i'm passing it in from function arguments >~>
            blobCenter = 0.5 + 0.5 * sin(Time * MoveSpeed + 6.2831 * blobCenter);

            // get the distance from this pixel to the neighboring blob center
            float dist = length(neighborCoords + blobCenter - innerCellPos);

            // keep the closest distance value from all cell comparisons
            minDist = min(minDist, dist);
        }
    }

    Noise = minDist;
}

#endif //MYHLSLINCLUDE_INCLUDED