using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenChit : MonoBehaviour
{
    private bool flag_debug = false;
    private static readonly float realtime_update_interval = .1f; // ten of these checks per second is all we really need

    public ChitAI chit;
    public float duration = 10f; //sec
    public GameObject ice;

    void Start()
    {
        chit = GetComponent<ChitAI>();
        if (chit == null) Destroy(this);
        else if (ice == null) Destroy(this);
        duration = Mathf.Abs(duration);

        StartCoroutine(ContinueFreeze());
    }

    private IEnumerator ContinueFreeze()
    {
        if (flag_debug) Debug.Log("starting freeze");
        chit.Grabbed();
        chit.body.velocity = Vector3.zero;
        GameObject newIce = Instantiate(ice, chit.transform);


        float cur_time = Time.time;
        float start_time = Time.time;

        while(Mathf.Abs(cur_time - start_time) < duration)
        {
            cur_time = Time.time;

            if (flag_debug) Debug.Log(Mathf.Abs(cur_time - start_time));
            if (flag_debug) Debug.Log(duration);
            yield return new WaitForSecondsRealtime(realtime_update_interval);
        }

        if (flag_debug) Debug.Log("ending freeze");
        Destroy(newIce);
        chit.Released();

        Destroy(this);
    }
}
