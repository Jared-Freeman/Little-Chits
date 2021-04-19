using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChitCounter : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        //Change Bottle to Chits. Done for testing purposes.
        if (other.gameObject.name == "Bottle")
        {
            Destroy(other.gameObject);
            UIMgr.inst.numChit -= 1; 
        }
    }
}
