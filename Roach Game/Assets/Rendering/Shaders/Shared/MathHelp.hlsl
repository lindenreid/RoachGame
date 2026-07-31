//UNITY_SHADER_NO_UPGRADE
#ifndef REID_MATHHELP_INCLUDED
#define REID_MATHHELP_INCLUDED

// god bless IQ as usual
// biplanar and triplanar mapping algorithms referenced from https://iquilezles.org/articles/biplanar/

void BiplanarWeights_float(float3 normal, float3 objPos, float blend, out float2 uvA, out float2 uvB, out float2 weights)
{
    normal = abs(normal);

    // axes are used for creating UVs for texture sampling
    float3 majorAxis = (normal.x > normal.y && normal.x > normal.z) ? float3(0, 1, 2) :
                       (normal.y > normal.z)                        ? float3(1, 2, 0) :
                                                                      float3(2, 0, 1) ;

    float3 minorAxis = (normal.x < normal.y && normal.x < normal.z) ? float3(0, 1, 2) :
                       (normal.y < normal.z)                        ? float3(1, 2, 0) :
                                                                      float3(2, 0, 1) ;   

    float3 medianAxis = float3(3, 3, 3) - minorAxis - majorAxis;
    
    uvA = float2(objPos[majorAxis.y], objPos[majorAxis.z]);
    uvB = float2(objPos[medianAxis.y], objPos[medianAxis.z]);

    // weights are used for blending texture samples
    weights = float2(normal[majorAxis.x], normal[medianAxis.x]);
    weights = clamp((weights - 0.5773)/(1.0 - 0.5773), 0.0, 1.0);
    weights = pow(weights, float2(blend/8.0, blend/8.0));
}

void BiplanarBlending_float(float3 colorA, float3 colorB, float2 weights, out float3 outColor)
{
    outColor = (colorA * weights.x + colorB * weights.y) / (weights.x + weights.y);
}

void TriplanarWeights_float(float3 normal, out float3 weights)
{
    float3 triW = abs(normal);
	weights = triW / (triW.x + triW.y + triW.z);
}

void SinTime01_float(float Time, float Speed, out float Result)
{
    Result = sin(Time*Speed)*0.5 + 0.5;
}

#endif //REID_MATHHELP_INCLUDED