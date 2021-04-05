using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class dumdevflickerlight : MonoBehaviour
{
    
    public List<Light> lts;
    float change = 0f;

    // Start is called before the first frame update
    void Start()
    {
        lts = GetComponentsInChildren<Light>().ToList();
    }

    // Update is called once per frame
    void Update()
    {
        float add = Random.Range(0, 50);
        change += add * Time.deltaTime;

        if (change > 200f)
        {
            foreach(Light lt in lts)
            {
                lt.enabled = !lt.enabled;

                if (lt.enabled == false) change = 198.5f;
            }
            float lol = Random.Range(0, 100);
            
            if(lol < 8)
                change = 0f;
        }
    }
}
