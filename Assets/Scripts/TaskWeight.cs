using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskWeight : MonoBehaviour
{

    public float weight;
    public float happinessMultiplier;
    public float temptation;
    public float distance;
    public float HappinessWeight;
    public float TimeWeight;
    public float AtTaskWeight;
    public float DistWeight;
    public float DistMultiplier = 10;
    public bool beingUsed;
    private float distanceSq;
    public int distInt;
    public float rsq = 4;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetWeight()
    {
        return weight;
    }

    public float UpdateTaskWeight(GameObject chit, float timeSince)
    {
        ChitAI chitAI = chit.GetComponent<ChitAI>();
        distanceSq = (transform.position - chit.transform.position).sqrMagnitude;
        HappinessWeight = happinessMultiplier * chitAI.chitHappiness/4;
        TimeWeight = temptation * Mathf.Log10(timeSince);
        distInt = (int) Mathf.Sqrt(distanceSq);
        DistWeight = 5 * 10/ Mathf.Sqrt(distInt/10+1);
        if (AtTask())
        {
            timeSince = 0;
            TimeWeight = 0;
        }
        weight = HappinessWeight + TimeWeight + DistWeight + AtTaskWeight;
        return timeSince;
    }

    bool AtTask()
    {
        bool atPos = false;
        if (distanceSq <= rsq)
            atPos = true;
        return atPos;
    }
}
