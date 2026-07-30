//UNITY_SHADER_NO_UPGRADE
#ifndef REID_MATHHELP_INCLUDED
#define REID_MATHHELP_INCLUDED

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