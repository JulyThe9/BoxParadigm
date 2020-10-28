using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerChecker : MonoBehaviour
{
    public bool grabbed = false;
    public GameObject grabbedBoxGameObject = null;
    private void OnTriggerEnter(Collider other)
    {
        grabbed = true;
        grabbedBoxGameObject = other.gameObject;
    }
    private void OnTriggerExit(Collider other)
    {
        grabbed = false;
        grabbedBoxGameObject = null;
    }
}
