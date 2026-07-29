//UNITY_SHADER_NO_UPGRADE
#ifndef REID_MATHHELP_INCLUDED
#define REID_MATHHELP_INCLUDED

void SinTime01_float(float Time, float Speed, out float Result)
{
    Result = sin(Time*Speed)*0.5 + 0.5;
}

#endif //REID_MATHHELP_INCLUDED