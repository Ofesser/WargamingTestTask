using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Some auxiliary scripts. This metods tested in TestScript

public static class Utils
{
    public static float CalculateAngleOfArc(float radius, float pointsDistance)
    {
        float angle = Mathf.Acos(1f - (Mathf.Pow(pointsDistance, 2f) / (2 * Mathf.Pow(radius, 2f))));
        angle *= Mathf.Rad2Deg;
        return angle;
    }

    public static float CalculateAngVelByLinVel(float linearVelocity, float radius)
    {
        float angularVelocity = linearVelocity / radius;
        return angularVelocity;
    }

    public static float GetMiddleValue(float valMin, float valMax)
    {
        float result = valMax - (valMax - valMin) / 2f;
        return result;
    }

    public static float FindReturnTime(float radius, float linearVelocity)
    {
        float returnTime = (2 * Mathf.PI * radius) / linearVelocity;
        return returnTime;
    }
}
