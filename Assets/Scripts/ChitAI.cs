﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChitAI : MonoBehaviour
{
    //commit
    public TaskWeight[] task;
    public int newTask;
    public float[] timeSince;
    public float chitHappiness;
    public float decisionTime;
    public float timePassed;
    public float depressTime;
    public float depression;
    public float bestWeight;
    public Vector3 wander;
    public bool isGrabbed;
    public bool isObsessed;
    public bool isWandering;
    public float chitAttention;
    //private GameObject chit;
    public string assignment;
    private NavMeshAgent agent;
    public Task childTask;

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
        depressTime += Time.deltaTime;
        for (int i = 0; i < timeSince.Length; i++)
            timeSince[i] += Time.deltaTime;
        if (!isGrabbed)
        {
            if (decisionTime < timePassed)
                if (isObsessed)
                {
                    if (assignment == "makeHappy")
                    {
                        chitHappiness++;
                        isObsessed = false;
                    }
                    else if (assignment == "newTask")
                    {
                        Debug.Log("moving to " + childTask);
                        MoveToLocation(childTask.transform.position);
                    }
                    else if (assignment == "die")
                    {
                        Destroy(gameObject);
                        UIMgr.inst.numChit--;
                    }
                    else
                    {
                        isObsessed = false;
                    }

                }
                else if (decisionTime < timePassed)
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
                    decisionTime = Random.value * 4 + 1;
                }
        }
        if (depressTime > depression)
        {
            chitHappiness--;
            depressTime = 0;
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
                if (!task[i].GetComponent<Task>().taskResume || task[i].CheckOccupant(gameObject))
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
            if (other.gameObject.GetComponent<Task>().taskResume == false)
            {
                other.gameObject.GetComponent<Task>().taskResume = true;
                other.gameObject.GetComponent<TaskWeight>().SetOccupant(this.gameObject);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Task")
        {
          

            if (other.gameObject.GetComponent<TaskWeight>().CheckOccupant(this.gameObject))
            {
                other.gameObject.GetComponent<Task>().taskResume = false;
                other.gameObject.GetComponent<TaskWeight>().SetOccupant(null);
            }
        }
    }
}