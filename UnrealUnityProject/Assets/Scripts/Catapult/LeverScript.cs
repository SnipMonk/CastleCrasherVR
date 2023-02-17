using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LeverScript : MonoBehaviour
{
    [SerializeField] UnityEvent triggeredEvent;
    [SerializeField] Collider triggerCollider;

    bool enabeled = true;
    public void EnableTrigger()
    {
        enabeled = true;
    }

    public void DisableTrigger()
    {
        enabeled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(enabeled && other == triggerCollider)
        {
            triggeredEvent.Invoke();
        }
    }
}
