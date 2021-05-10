using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskWeight : MonoBehaviour
{
    //bruh
    public float weight;
    public float happinessMultiplier;
    public float temptation;
    public float distance;
    public float HappinessWeight;
    public float TimeWeight;
    public float AtTaskWeight;
    public float DistWeight;
    public float DistMultiplier = 8;
    public bool beingUsed;
    public GameObject occupant;
    public Task task;
    private float distanceSq;
    public float RandomWeight;
    public int distInt;
    public bool isRandom;
    public float rsq = 4;

    private void Awake()
    {
        task = GetComponent<Task>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetOccupant(GameObject newOccupant)
    {
        occupant = newOccupant;
    }
    public bool CheckOccupant(GameObject newOccupant)
    {
        return occupant == newOccupant;
    }

    public float GetWeight()
    {
        return weight;
    }
    public bool BeingUsed()
    {
        return task.taskResume;
    }

    public float UpdateTaskWeight(GameObject chit, float timeSince)
    {
        ChitAI chitAI = chit.GetComponent<ChitAI>();
        distanceSq = (transform.position - chit.transform.position).sqrMagnitude;
        HappinessWeight = Mathf.Clamp(happinessMultiplier * chitAI.chitHappiness*2+10, 0, 50);
        TimeWeight = Mathf.Clamp(temptation * Mathf.Log10(timeSince)*10, 0, 50);
        distInt = (int) Mathf.Sqrt(distanceSq);
        DistWeight = DistMultiplier * 10/ Mathf.Sqrt(distInt/10+1);
        AtTaskWeight = 30 *(task.taskTime / task.compTime);
        if (isRandom)
            RandomWeight = Random.value * 30;
        else
            RandomWeight = 0;
        if (AtTask())
        {
            timeSince = 0;
            TimeWeight = 0;
        }
        weight = HappinessWeight + TimeWeight + DistWeight + AtTaskWeight + RandomWeight + AtTaskWeight;
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
