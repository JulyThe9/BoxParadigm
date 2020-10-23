using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerChecker : MonoBehaviour
{
    public bool grabbed = false;
    private void OnTriggerEnter(Collider other)
    {
        grabbed = true;
    }
    private void OnTriggerExit(Collider other)
    {
        grabbed = false;
    }
}
