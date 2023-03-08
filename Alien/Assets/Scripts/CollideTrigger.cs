using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class CollideTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent uEvent;


    private void OnTriggerEnter(Collider other) {
        uEvent.Invoke();
    }


    

    
}



