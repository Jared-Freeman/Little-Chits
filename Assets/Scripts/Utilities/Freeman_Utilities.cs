using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Jared Freeman
//Desc: This class implements some helpful utility methods for use in Unity game design

public static class Freeman_Utilities
{
    public static float MapValueFromRangeToRange(float val_a, float range_a_start, float range_a_end, float range_b_start, float range_b_end)
    {
        float normal = Mathf.InverseLerp(range_a_start, range_a_end, val_a);
        return Mathf.Lerp(range_b_start, range_b_end, normal);
    }
}
