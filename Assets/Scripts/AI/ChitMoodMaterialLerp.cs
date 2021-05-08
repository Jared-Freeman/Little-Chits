﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// author: Jared Freeman
// idea: map mood range to 0-1, then use this as param t for material lerp
public class ChitMoodMaterialLerp : MonoBehaviour
{
    public Material happy_material;
    public Material angry_material;

    public Renderer rend;

    [Min(.01f)]
    public float realtime_update_interval;

    public ChitAI attached_chit;

    private void Start()
    {
        if (happy_material == null || angry_material == null || rend == null || attached_chit == null)
            Debug.LogError("ChitMoodMaterialLerp: ERROR! Please specify lerp materials, renderer, and attached ChitAI!");
        
        else StartCoroutine(UpdateChitMaterial());
    }

    private IEnumerator UpdateChitMaterial()
    {
        while (true)
        {
            float t = attached_chit.chitHappiness;

            t = Freeman_Utilities.MapValueFromRangeToRange(t, -attached_chit.maxHappiness, attached_chit.maxHappiness, 0, 1);

            rend.material.Lerp(happy_material, angry_material, t);

            yield return new WaitForSecondsRealtime(realtime_update_interval);
        }
    }
}
