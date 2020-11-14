using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxGroundedChecker : MonoBehaviour
{
    public bool grounded = false;
    public GameObject hitBoxGameObject = null;
    private void OnTriggerEnter(Collider other)
    {
        grounded = true;
        hitBoxGameObject = other.gameObject;
    }
    private void OnTriggerExit(Collider other)
    {
        grounded = false;
        hitBoxGameObject = null;
    }
}
