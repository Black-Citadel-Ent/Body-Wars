using UnityEngine;

public static class Extensions
{
    public static bool NearZero(this float f)
    {
        return Mathf.Abs(f) < 0.0001;
    }

    public static bool NearZero(this float f, float tolerance)
    {
        return Mathf.Abs(f) < tolerance;
    }

    public static float Lerp(this float t, float t0, float t1, float x0, float x1)
    {
        return x0 + (x1 - x0) * (t - t0) / (t1 - t0);
    }

    public static float NormalizedAngle(this float f)
    {
        var ret = f;
        while (ret >= 180.0f)
            ret -= 360.0f;
        while (ret < -180.0f)
            ret += 360.0f;
        return ret;
    }

    public static Vector2 DegreesToVector2(this float f)
    {
        var rad = f * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
    }

    public static float ToDegrees(this Vector2 v)
    {
        var n = v.normalized;
        return Mathf.Atan2(n.y, n.x) * Mathf.Rad2Deg;
    }

    public static Vector2 Random(this Vector2 v)
    {
        var randAngle = UnityEngine.Random.Range(-180, 180) * Mathf.Deg2Rad;
        v.Set(Mathf.Cos(randAngle), Mathf.Sin(randAngle));
        return v;
    }
}

public abstract class BaseObject : MonoBehaviour
{
    
}