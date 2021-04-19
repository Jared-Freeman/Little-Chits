using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChitCounter : MonoBehaviour
{
    public GameObject capturedVersion;
    int i = 3;
    private void OnTriggerEnter(Collider other)
    {
        //Change Bottle to Chits. Done for testing purposes.
        if (other.gameObject.tag == "Chit")
        {
            Debug.Log("AHHH");
            Destroy(other.gameObject);
          
                //Instantiate(capturedVersion, new Vector3(7.377F, 0.186f, -5.051f), transform.rotation);

            
            UIMgr.inst.numChit -= 1; 
        }
    }
}
