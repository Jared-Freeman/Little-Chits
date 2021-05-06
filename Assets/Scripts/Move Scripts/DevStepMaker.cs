using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DevStepMaker : MonoBehaviour
{
    public List<AudioSource> stepSounds;
    public List<AudioSource> fallSounds;
    //public Transform transformReference;
    int index = 0;

    public void DoFall()
    {
        index += Random.Range(1, 3);
        index %= fallSounds.Count;

        fallSounds[index].Play();
        //AudioSource.PlayClipAtPoint(fallSounds[index].clip, transform.position);
    }

    public void DoStep()
    {
        index += Random.Range(1,3);
        index %= stepSounds.Count;

        stepSounds[index].Play();
        //AudioSource.PlayClipAtPoint(stepSounds[index].clip, transform.position, .4f);
    }
}
