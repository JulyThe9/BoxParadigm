using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerChecker : MonoBehaviour
{
    public bool grabbed = false;
    public GameObject grabbedBoxGameObject = null;
    private void OnTriggerEnter(Collider other)
    {
        // TODO: perhaps should be a separate tag name var and in ObjectTypes
        //if (other.gameObject.CompareTag(GlobalVariables.groundLayerName))
        if (other.gameObject.tag.Equals(GlobalVariables.groundLayerName))
        {
            grabbedBoxGameObject = other.transform.parent.gameObject;
            BoxData collidedBoxData = grabbedBoxGameObject.GetComponent<BoxData>();
            // Checking if there is no actual box above the collided box to be able to grab
            if (!BHWrapper.BoxExists(collidedBoxData.xInd, collidedBoxData.zInd, collidedBoxData.yInd + 1))
            {
                grabbed = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (grabbed)
        {
            grabbed = false;
            grabbedBoxGameObject = null;
        }
    }
}
