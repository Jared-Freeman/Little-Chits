using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChitCounter : MonoBehaviour
{
      
    private void OnTriggerEnter(Collider other)
    {
        //Change Bottle to Chits. Done for testing purposes.
        if (other.gameObject.tag == "Chit")
        {            
            if (gameObject.tag == "Kill")
            {               
                Destroy(other.gameObject);
                ChitAI.inst.chitDeathSound.Play();
            }      
            UIMgr.inst.numChit -= 1;
            ChitAI.inst.chitCageSound.Play();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Chit")
        {
            if (gameObject.tag != "Kill")
            {
                UIMgr.inst.numChit += 1;               
            }
        }
    }
}
