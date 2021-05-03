using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
      
    private void OnTriggerEnter(Collider other)
    {
        //Change Bottle to Chits. Done for testing purposes.
        if (other.gameObject.tag == "Chit")
        {
            ChitAI chit = other.gameObject.GetComponent<ChitAI>();
            chit.Death();                         
        }
    }

}
