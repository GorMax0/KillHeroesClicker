using System;
using UnityEngine;

public static class MathfExtension
{
  public static double MoveTowards(this Mathf mathf, double current, double target, double maxDelta)
    {
        if (Math.Abs(target - current) <= maxDelta)
        {
            return target;
        }

        return current + Math.Sign(target - current) * maxDelta;
    }
}