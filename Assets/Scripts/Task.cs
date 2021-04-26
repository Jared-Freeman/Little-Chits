using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Task : MonoBehaviour
{

    public bool taskResume;
    public bool taskComplete;
    public float taskTime;
    public float compTime;
    public TaskWeight weigher;

    // Start is called before the first frame update
    void Start()
    {
        weigher = GetComponent<TaskWeight>();
    }

    // Update is called once per frame
    void Update()
    {
        if(taskResume)
        {
            weigher.beingUsed = true;
            taskTime += Time.deltaTime;
        }
        else
        {
            weigher.beingUsed = true;
            if(taskTime > 0)
            {
                taskTime -= Time.deltaTime / 50;
            }
        }

        if(taskTime >= compTime)
        {
            Debug.Log("TASK COMPLETE!");
            taskTime = 0;
        }
    }


}
