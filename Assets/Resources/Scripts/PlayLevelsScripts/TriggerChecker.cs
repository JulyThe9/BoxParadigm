using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerChecker : MonoBehaviour
{
    public bool entered = false;
    private void OnTriggerEnter(Collider other)
    {
        entered = true;
    }
    private void OnTriggerExit(Collider other)
    {
        entered = false;
    }
}
