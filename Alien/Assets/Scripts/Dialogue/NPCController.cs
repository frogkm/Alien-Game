using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public bool inRange = false;

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Person") {
            inRange = true;
        }
        
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Person") {
            inRange = false;
        }
    }

    
}
