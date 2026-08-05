//UNITY_SHADER_NO_UPGRADE
#ifndef REID_MATHHELP_INCLUDED
#define REID_MATHHELP_INCLUDED

// god bless IQ as usual
// biplanar and triplanar mapping algorithms referenced from https://iquilezles.org/articles/biplanar/

void BiplanarWeights_half(half3 normal, half3 objPos, half blend, out half2 uvA, out half2 uvB, out half2 weights)
{
    normal = abs(normal);

    // axes are used for creating UVs for texture sampling
    half3 majorAxis = (normal.x > normal.y && normal.x > normal.z) ? half3(0, 1, 2) :
                       (normal.y > normal.z)                        ? half3(1, 2, 0) :
                                                                      half3(2, 0, 1) ;

    half3 minorAxis = (normal.x < normal.y && normal.x < normal.z) ? half3(0, 1, 2) :
                       (normal.y < normal.z)                        ? half3(1, 2, 0) :
                                                                      half3(2, 0, 1) ;   

    half3 medianAxis = half3(3, 3, 3) - minorAxis - majorAxis;
    
    uvA = half2(objPos[majorAxis.y], objPos[majorAxis.z]);
    uvB = half2(objPos[medianAxis.y], objPos[medianAxis.z]);

    // weights are used for blending texture samples
    weights = half2(normal[majorAxis.x], normal[medianAxis.x]);
    weights = clamp((weights - 0.5773)/(1.0 - 0.5773), 0.0, 1.0);
    weights = pow(weights, half2(blend/8.0, blend/8.0));
}

void BiplanarBlending_half(half3 colorA, half3 colorB, half2 weights, out half3 outColor)
{
    outColor = (colorA * weights.x + colorB * weights.y) / (weights.x + weights.y);
}

void TriplanarWeights_half(half3 normal, out half3 weights)
{
    half3 triW = abs(normal);
	weights = triW / (triW.x + triW.y + triW.z);
}

void SinTime01_half(half Time, half Speed, out half Result)
{
    Result = sin(Time*Speed)*0.5 + 0.5;
}

#endif //REID_MATHHELP_INCLUDED