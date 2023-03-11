using UnityEngine;

public static class UtilsHelper
{
    public static float SqrDistance(Vector3 pos1, Vector3 pos2)
    {
        return (pos2 - pos1).sqrMagnitude;
    }
    /// <summary>
    /// If param is 'true' then stop time
    /// else, flow as usual
    /// </summary>
    /// <param name="value"></param>
    public static void AdjustTimeScale(bool value)
    {
        if(value== true)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}
