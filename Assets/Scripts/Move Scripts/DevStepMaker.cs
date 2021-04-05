using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DevStepMaker : MonoBehaviour
{
    public List<AudioSource> stepSounds;
    public List<AudioSource> fallSounds;
    public Transform transformReference;
    int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void DoFall()
    {
        index += Random.Range(1, 3);
        index %= fallSounds.Count;
        AudioSource.PlayClipAtPoint(fallSounds[index].clip, transform.position);
    }

    public void DoStep()
    {
        index += Random.Range(1,3);
        index %= stepSounds.Count;
        AudioSource.PlayClipAtPoint(stepSounds[index].clip, transform.position);
    }
}
