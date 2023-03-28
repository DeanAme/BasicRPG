using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethod
{
     public const float StandardAngle = 0.5f;
    public static bool IsFacingTarget(this Transform transform, Transform target)
{
    var vectorDirection = target.position - transform.position;
    vectorDirection.Normalize();
    float dot = Vector3.Dot(transform.forward, vectorDirection); 
    return dot>= StandardAngle;
}
}
