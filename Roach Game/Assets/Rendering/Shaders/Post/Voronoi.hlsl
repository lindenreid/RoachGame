//UNITY_SHADER_NO_UPGRADE
#ifndef REID_VORONOI_INCLUDED
#define REID_VORONOI_INCLUDED

// thank you https://sangillee.com/_posts/2025-04-18-Cellular-noises/
// Created by inigo quilez - iq/2013
// License Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// http://www.iquilezles.org/www/articles/voronoilines/voronoilines.htm
// Edited by Sangil Lee
// Translated to HLSL and further edited by Travis Reid

half random1 (half2 value, half2 dotDir = half2(12.9898, 78.233))
{
    half2 smallValue = sin(value);
    half random = dot(smallValue, dotDir);
    random = frac(sin(random) * 143758.5453);
    return random;
}

half3 random3 (half2 p)
{
    half3 q = half3( dot(p, half2(127.1,311.7)), 
        dot(p, half2(269.5,183.3)), 
        dot(p, half2(419.2,371.9)) );
    return frac(sin(q) * 43758.5453);
}

half weightedDist(half2 x, half2 y, half w)
{
    return 1.0/w * length(x-y);
}

half level(half2 p)
{
    if (p.x == 1.0 && p.y == 1.0)
    {
        return 1.0;
    }
    return 0.0;
}

half VoronoiClusters(half2 uv, half ClusterPropability, half Time, half MoveSpeed, half GrowSpeed)
{
    // quantize to create the tiles
    half2 cellCoords = floor(uv);

    // big numbah
    half minDist = 1e10;
    
    half offsetAmt = 2.0;

    // loop through self + 8 neighbos - self is at (0,0)
    // 3 nested for loops to create clusters inside individual cells
    for(int i = -1; i <= 1; i++)
    {
        for(int j = -1; j <= 1; j++)
        {
            // get distance from this pixel to neighbor cell's blob center
            half2 neighborCoords1 = cellCoords + half2(i, j);
            half3 cellRand = random3(neighborCoords1);
            // must animate blob's center location BEFORE applying neighbor coords
            half2 cellRandCenter = 0.5 + 0.5 * sin(Time * MoveSpeed + 6.2831 * cellRand);
            half2 blobCenter = neighborCoords1 + cellRandCenter.xy;

            // animate size of blob by factoring a weight into the distance equation
            //      and animating the weight value
            // the random start offset is based off of a neighboring cell's random value
            //      because i'm running out of random values i can generate from one 2d position lol
            half weight = random1(neighborCoords1) + 0.8;
            half randomStartOffset = random1(neighborCoords1 - half2(1,1));
            weight = weight + sin(randomStartOffset*offsetAmt + Time * GrowSpeed) * weight/6.0;
            half dist = weightedDist(blobCenter, uv, weight);

            // random value to determine if this cell should create a cluster
            // keep original random value- NOT animated- as the chance of clustering should remain the same over time
            half cluster = cellRand.z;

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
                        half2 neighborCoords2 = neighborCoords1 + half2(k, l)/2.0;
                        cellRand = random3(neighborCoords2);
                        cellRandCenter = 0.5 + 0.5 * sin(Time * MoveSpeed + 6.2831 * cellRand);
                        blobCenter = neighborCoords2 + cellRandCenter.xy/2.0;

                        // weight inner clusters to be smaller
                        // cluster 1 min is 0.5, max is 0.8
                        weight = random1(neighborCoords2) * 0.8 + 0.5;
                        randomStartOffset = random1(neighborCoords2 - half2(1,1));
                        weight = weight + sin(randomStartOffset*offsetAmt + Time * GrowSpeed) * weight/4.0;
                        dist = weightedDist(blobCenter, uv, weight);

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
                                    half2 neighborCoords3 = neighborCoords2 + half2(m, n)/4.0;
                                    cellRand = random3(neighborCoords3);
                                    cellRandCenter = 0.5 + 0.5 * sin(Time * MoveSpeed + 6.2831 * cellRand);
                                    blobCenter = neighborCoords3 + cellRandCenter.xy/4.0;

                                    weight = random1(neighborCoords3) * 0.5 + 0.2;
                                    randomStartOffset = random1(neighborCoords3 - half2(1,1));
                                    weight = weight + sin(randomStartOffset*offsetAmt + Time * GrowSpeed) * weight/4.0;
                                    dist = weightedDist(blobCenter, uv, weight);

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

void VoronoiClusters_half(half2 UV, half Time, half MoveSpeed, half GrowSpeed, half Density, half ClusterPropability, out half Noise)
{
    half2 v = UV * Density;
    Noise = VoronoiClusters(v, ClusterPropability, Time, MoveSpeed, GrowSpeed);
}

void VoronoiWeighted_half(half2 UV, half Time, half MoveSpeed, half Density, out half Noise)
{
    half2 value = UV * Density;
    
    half2 i_st = floor(value);
    half2 f_st = frac(value);

    half minDist = 100;

    for(int y = -2; y <= 2; y++)
    {
        for(int x = -2; x <= 2; x++)
        {
            half2 neighborCoords = half2(x, y);

            half2 blobCenter = random3(i_st + neighborCoords).xy;
            blobCenter = 0.5 + 0.5 * sin(Time * MoveSpeed + 6.2831 * blobCenter);

            half weight = 0.2 + 0.8 * random3(i_st + neighborCoords).z;
            half dist = weightedDist(neighborCoords + blobCenter, f_st, weight);

            minDist = min(minDist, dist);
        }
    }

    Noise = minDist;
}

void VoronoiSimple_half(half2 UV, half Time, half MoveSpeed, half Density, out half Noise)
{
    // UV is 0-1 value, so make it larger based on how many tiles we want to have
    half2 value = UV * Density;
    
    // quantize to create the tiles
    half2 cellCoords = floor(value);

    // get this pixel's exact position inside the cell
    half2 innerCellPos = frac(value);

    // big numbah
    half minDist = 1e10;

    // loop through self + 8 neighbors (9 cells total)
    // self is at (0,0)
    for(int y = -1; y <= 1; y++)
    {
        for(int x = -1; x <= 1; x++)
        {
            // get coordinates for neighbor tile
            half2 neighborCoords = half2(x, y);

            // get center of tile
            // center is a randomized value using the UV as the seed
            half2 blobCenter = random3(cellCoords + neighborCoords).xy;

            // animate center of blob based on time 
            // for some reason, _Time isn't working properly inside this custom ShaderGraph function,
            // so that's why i'm passing it in from function arguments >~>
            blobCenter = 0.5 + 0.5 * sin(Time * MoveSpeed + 6.2831 * blobCenter);

            // get the distance from this pixel to the neighboring blob center
            half dist = length(neighborCoords + blobCenter - innerCellPos);

            // keep the closest distance value from all cell comparisons
            minDist = min(minDist, dist);
        }
    }

    Noise = minDist;
}

#endif //REID_VORONOI_INCLUDED