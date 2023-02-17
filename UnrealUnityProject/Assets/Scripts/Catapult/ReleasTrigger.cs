using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ReleasTrigger : MonoBehaviour
{
    [SerializeField] UnityEvent TriggerEvent;
    [SerializeField] Collider target;

    private bool used = false;

    private void OnTriggerEnter(Collider other)
    {
        if(!used && other == target)
        {
            used = true;
            TriggerEvent.Invoke();
        }
    }

    public void ReloadrTrigget()
    {
        used = false;
    }
}
