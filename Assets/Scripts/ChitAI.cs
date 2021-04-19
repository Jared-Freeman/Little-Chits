using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChitAI : MonoBehaviour
{
    public TaskWeight[] task;
    public int newTask;
    public float[] timeSince;
    public float chitHappiness;
    public float decisionTime;
    public float timePassed;
    public float bestWeight;
    public bool isGrabbed;
    //private GameObject chit;
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Awake()
    {
        //chit = GetComponent<GameObject>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        for (int i = 0; i < timeSince.Length; i++)
            timeSince[i] += Time.deltaTime;
        if (!isGrabbed)
        {
            if (decisionTime < timePassed)
            {
                NewTask();
                MoveToLocation(task[newTask].GetComponent<Transform>().position);
                timePassed = 0;
            }
        }
    }

    void NewTask()
    {
        bestWeight = 0;
        for (int i = 0; i < task.Length; i++)
        {
            timeSince[i] = task[i].UpdateTaskWeight(this.gameObject, timeSince[i]);
            if (task[i].GetWeight() > bestWeight)
            {
                bestWeight = task[i].GetWeight();
                newTask = i;
            }

        }
    }
    public void MoveToLocation(Vector3 targetPoint)
    {
        agent.destination = targetPoint;
        agent.isStopped = false;
    }
}
