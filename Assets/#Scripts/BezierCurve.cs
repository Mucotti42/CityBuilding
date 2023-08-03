using System;
using UnityEngine;
public static class BezierUtils
{
    public static Vector3 CalculateCubicBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 point = uuu * p0; // (1-t)^3 * p0
        point += 3 * uu * t * p1; // 3 * (1-t)^2 * t * p1
        point += 3 * u * tt * p2; // 3 * (1-t) * t^2 * p2
        point += ttt * p3; // t^3 * p3

        return point;
    }
}
[ExecuteInEditMode]
public class BezierCurve : MonoBehaviour
{
    public Transform controlPoint1;
    public Transform controlPoint2;
    public Transform controlPoint3;

    [Range(0f, 1f)]
    public float resolution = 0.1f;

    private void Awake()
    {
        controlPoint1 = transform.GetChild(0);
        controlPoint2 = transform.GetChild(1);
        controlPoint3 = transform.GetChild(2);
    }

    private void OnDrawGizmos()
    {
        DrawBezierCurve();
        DrawControlPoints();
    }

    private void DrawBezierCurve()
    {
        Vector3 lastPoint = controlPoint1.position;

        Gizmos.color = Color.white;
        for (float t = resolution; t <= 1f; t += resolution)
        {
            Vector3 point = BezierUtils.CalculateCubicBezierPoint(
                controlPoint1.position,
                controlPoint2.position,
                controlPoint3.position,
                controlPoint3.position,
                t
            );

            Gizmos.DrawLine(lastPoint, point);
            lastPoint = point;
        }

        Gizmos.DrawLine(lastPoint, controlPoint3.position);
    }

    private void DrawControlPoints()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(controlPoint1.position, 0.1f);
        Gizmos.DrawSphere(controlPoint2.position, 0.1f);
        Gizmos.DrawSphere(controlPoint3.position, 0.1f);

        Gizmos.color = Color.gray;
        Gizmos.DrawLine(controlPoint1.position, controlPoint2.position);
        Gizmos.DrawLine(controlPoint2.position, controlPoint3.position);
    }
}
