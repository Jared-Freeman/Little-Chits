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
            if(gameObject.tag == "Kill")
            {
                Destroy(other.gameObject);
                Debug.Log("AHHH");
            }
      
            UIMgr.inst.numChit -= 1; 
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //Change Bottle to Chits. Done for testing purposes.
        if (other.gameObject.tag == "Chit")
        {
            if (gameObject.tag != "Kill")
            {
                UIMgr.inst.numChit += 1;
                Debug.Log("Jeez");
            }
        }
    }
}
