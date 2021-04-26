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
    public Vector3 wander;
    public bool isGrabbed;
    public bool isObsessed;
    public bool isWandering;
    public float chitAttention;
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
                if (isWandering)
                {
                    wander = transform.position + new Vector3(Random.value * 4 - 2, Random.value * 4 - 2);
                    MoveToLocation(wander);
                    chitAttention -= 5;
                }
                else
                {
                    MoveToLocation(task[newTask].GetComponent<Transform>().position);
                    
                    chitAttention += 5;
                }
                timePassed = 0;
                decisionTime = Random.value * 7 + 3;
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
                if (task[i].BeingUsed() == false || task[i].CheckOccupant(gameObject))
                {
                    bestWeight = task[i].GetWeight();
                    newTask = i;
                }
            }
            if(bestWeight < chitAttention)
                isWandering = true;
            else
                isWandering = false;

        }
    }
    public void MoveToLocation(Vector3 targetPoint)
    {
        agent.destination = targetPoint;
        agent.isStopped = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Task")
        {
            other.gameObject.GetComponent<Task>().taskResume = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Task")
        {
            other.gameObject.GetComponent<Task>().taskResume = false;
        }
    }
}
