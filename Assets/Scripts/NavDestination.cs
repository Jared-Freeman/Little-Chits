using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavDestination : MonoBehaviour
{
    private NavMeshAgent agent;
    public GameObject[] locations;
    public int currentTarget;
    public float rsq = 16;
    public float distanceSq;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        currentTarget = 0;
        MoveToLocation(locations[currentTarget].transform.position);
    }

    public void MoveToLocation(Vector3 targetPoint)
    {
        agent.destination = targetPoint;
        agent.isStopped = false;
    }

    private void Update()
    {
        if(FoundDestination())
        {
            if (currentTarget < (locations.Length-1))
                currentTarget++;
            else
                currentTarget = 0;
            MoveToLocation(locations[currentTarget].transform.position);
        }
    }

    bool FoundDestination()
    {
        bool atPos = false;
        distanceSq = (locations[currentTarget].transform.position - agent.transform.position).sqrMagnitude;
        if (distanceSq <= rsq)
            atPos = true;
        return atPos;
    }
}
