using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
public class ChitAI : MonoBehaviour
{
    public static bool flag_debug = false; //addition by Jared to limit Debug.Log()'s 

    public static ChitAI inst;

    //commit
    public GameObject[] taskObj;
    public TaskWeight[] task;
    public int newTask;
    public float[] timeSince;
    public float chitHappiness;
    public float maxHappiness = 5;
    public float decisionTime;
    public float timePassed;
    public float depressTime;
    public float depression;
    public float bestWeight;
    public Vector3 wander;
    public bool isGrabbed;
    public bool isObsessed;
    public bool isWandering;
    public int escapeChance;
    public int escapeGrowth;
    public int defaultEscapeChance;
    public bool isTrapped;
    public float chitAttention;

    //private GameObject chit;
    public string assignment;
    private NavMeshAgent agent;
    public Task childTask;
    public Rigidbody body;

    //Sounds
    public AudioClip chitJumpSound;
    public AudioClip chitDeathSound;
    public AudioClip chitCagedSound;
    public AudioClip chitHappySound;
    public AudioClip chitBadSound;
    public AudioClip chitGrabbedSound;
    private AudioSource audioSource;


    #region EVENTS
    public static event System.EventHandler<ChitAIEventArgs> event_chit_spawn;
    public static event System.EventHandler<ChitAIEventArgs> event_chit_despawn;
    #endregion

    #region INIT
    void Start()
    {
        event_chit_spawn?.Invoke(this, new ChitAIEventArgs(this));
    }
    #endregion

    private void OnDestroy()
    {
        event_chit_despawn?.Invoke(this, new ChitAIEventArgs(this));
    }

    private void Awake()
    {
        inst = this;
        //chit = GetComponent<GameObject>();
        taskObj = GameObject.FindGameObjectsWithTag("Task");
        agent = GetComponent<NavMeshAgent>();
        escapeChance = defaultEscapeChance;
        body = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        timeSince = new float[taskObj.Length];
        task = new TaskWeight[taskObj.Length];
        for(int i = 0; i< taskObj.Length; i++)
        {
            task[i] = taskObj[i].GetComponent<TaskWeight>();
        }
    }

    [System.Serializable]
    public class HappyEvent : UnityEvent { }
    public HappyEvent onHappy = new HappyEvent();

    public void Happy()
    {
        audioSource.clip = chitHappySound;
        audioSource.Play();
        MoreHappy(1);
        isObsessed = false;
        onHappy.Invoke();
    }

    [System.Serializable]
    public class JumpEvent : UnityEvent { }
    public JumpEvent onJump = new JumpEvent();

    public void Jump()
    {
        audioSource.clip = chitJumpSound;
        audioSource.Play();
        DisableAI();
        Vector3 launch = new Vector3(Random.value * 200 - 100, 100, Random.value * 200 - 100);
        if(flag_debug) Debug.Log(launch);
        body.AddForce(launch);
        onJump.Invoke();
    }

    [System.Serializable]
    public class DeathEvent : UnityEvent { }
    public DeathEvent onDeath = new DeathEvent();

    IEnumerator Remove()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);

    }
    public void Death()
    {
        audioSource.clip = chitDeathSound;
        audioSource.Play();
        StartCoroutine("Remove");
        UIMgr.inst.numChit -= 1;
        onDeath.Invoke();
    }

    [System.Serializable]
    public class CagedEvent : UnityEvent { }
    public CagedEvent onCaged = new CagedEvent();

    public void Caged()
    {
        audioSource.clip = chitCagedSound;
        if (!SoundMgr.inst.played)
        {
            audioSource.Play();
            SoundMgr.inst.played = true;
        }
        UIMgr.inst.numChit -= 1;
        isTrapped = true;
        escapeChance = defaultEscapeChance;
        onCaged.Invoke();
    }

    [System.Serializable]
    public class GrabbedEvent : UnityEvent { }
    public GrabbedEvent onGrabbed = new GrabbedEvent();

    public void Grabbed()
    {
        audioSource.clip = chitGrabbedSound;
        audioSource.Play();        
        onGrabbed.Invoke();
        isGrabbed = true;
        DisableAI();
    }

    [System.Serializable]
    public class ReleasedEvent : UnityEvent { }
    public ReleasedEvent onReleased = new ReleasedEvent();

    public void Released()
    {
        audioSource.clip = chitGrabbedSound;
        audioSource.Play();
        onGrabbed.Invoke();
        isGrabbed = false;
        decisionTime = 0;
    }

    [System.Serializable]
    public class BadEvent : UnityEvent { }
    public BadEvent onBad = new BadEvent();

    public void Bad()
    {
        audioSource.clip = chitBadSound;
        audioSource.Play();        
        onBad.Invoke();
    }
    public void DisableAI()
    {
        agent.enabled = false;
        body.isKinematic = false;
    }

    public void EnableAI()
    {
        agent.enabled = true;
        body.isKinematic = true;
    }
    public void MoreHappy(int val)
    {
        chitHappiness += val;
        if (chitHappiness > maxHappiness)
            chitHappiness = maxHappiness;
    }

    public void LessHappy(int val)
    {
        chitHappiness -= val;
        if (chitHappiness < -maxHappiness)
            chitHappiness = -maxHappiness;
    }
    public void SetHappiness(int val)
    {
        chitHappiness = val;
        if (chitHappiness > maxHappiness)
            chitHappiness = maxHappiness;
        else if (chitHappiness < -maxHappiness)
            chitHappiness = -maxHappiness;
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
            {
                EnableAI();
                agent.enabled = true;
                body.isKinematic = true;
                if (isObsessed)
                {
                    if (assignment == "makeHappy")
                    {
                        Happy();               
                        
                    }
                    else if (assignment == "newTask")
                    {
                        if (flag_debug) Debug.Log("moving to " + childTask);
                        MoveToLocation(childTask.transform.position);
                    }
                    else if (assignment == "die")
                    {
                        Death();                        
                    }
                    else
                    {
                        isObsessed = false;
                    }

                }
                else if (isTrapped)
                {
                    wander = transform.position + new Vector3(Random.value * 4 - 2, Random.value * 4 - 2);
                    float rollEscape = Random.value * 100;
                    if (rollEscape < escapeChance)
                    {
                        audioSource.clip = chitJumpSound;
                        audioSource.Play();
                        DisableAI();
                        Vector3 launch = new Vector3(Random.value * 300 - 150, 250, Random.value * 300 - 150);
                        if (flag_debug) Debug.Log(launch);
                        body.AddForce(launch);
                        //agent.enabled = true;
                    }
                    else
                    {
                        MoveToLocation(wander);
                        escapeChance += escapeGrowth;
                    }
                }
                else if (decisionTime < timePassed)
                {
                    EnableAI();
                    NewTask();
                    if (isWandering)
                    {
                        
                        float jumproll = Random.value * 100;
                        if (jumproll <= 75)
                        {                            
                            wander = transform.position + new Vector3(Random.value * 4 - 2, Random.value * 4 - 2);
                            MoveToLocation(wander);
                            if (flag_debug) Debug.Log(agent.pathStatus + "[" + wander + "]");
                        }
                        else
                        {
                            Jump();
                            
                        }
                        chitAttention -= 5;

                    }
                    else
                    {
                        MoveToLocation(task[newTask].GetComponent<Transform>().position);
                        if (flag_debug) Debug.Log(agent.pathStatus + "[" + task[newTask].GetComponent<Transform>().position + "]");
                        chitAttention += 5;
                    }
                    if (agent.pathStatus != NavMeshPathStatus.PathComplete && !isTrapped)
                    {
                        Jump();
                        isObsessed = false;
                    }
                }
                timePassed = 0;
                decisionTime = Random.value * 4 + 1;
            }
        }

        if (depressTime > depression)
        {
            LessHappy(1);
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
            if (bestWeight < chitAttention)
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
        if (other.gameObject.tag == "Cage")
        {
            //isTrapped = true;
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
        if (other.gameObject.tag == "Cage")
        {
            ///isTrapped = false;
        }
    }
}