using System.Collections.Generic;
using UnityEngine;

public class SimpleEmergence : MonoBehaviour
{
    public bool effectInProgress;
    public BoxData selBoxData;
    public ObjectTypes.BoxActions latestAction;
    // TODO: the fact it's int seems super sketchy, 
    // CHECK how simultaneous projectiles get resolved
    public int dHandedToolRightUsed;

    public GameObject replayBoxObj;
    public BoxTraits replayBoxTraits;

    public List<GameObject> boxesToCleanUp;

    public int swapXIndDiff;
    public int swapZIndDiff;
    public int swapYIndDiff;

    public void Start()
    {
        effectInProgress = false;
        selBoxData = null;
        latestAction = ObjectTypes.BoxActions.Irrelevant;
        dHandedToolRightUsed = 0;
        swapXIndDiff = 0;
        swapZIndDiff = 0;
        swapYIndDiff = 0;
    }

    public void CleanUpUnfinishedEffects()
    {
        // TODO: legit for quantum select/connect, what about others?
        if (selBoxData != null)
        {
            BoxEntry selBoxEntry = BHWrapper.bHolder.list[selBoxData.xInd][selBoxData.zInd][selBoxData.yInd];
            selBoxEntry.GetBoxGameObj().GetComponent<Renderer>().material = selBoxEntry.GetBoxGameObj().GetComponent<BoxBehavior>().curMaterial;

            BoxBehavior selBoxBehavior = selBoxEntry.GetBoxGameObj().GetComponent<BoxBehavior>();
            if (selBoxBehavior.secondaryEffect != null)
            {
                Destroy(selBoxBehavior.secondaryEffect);
            }
        }
    }

    public void CleanUpFinishedEffects()
    {
        for (int i = boxesToCleanUp.Count - 1; i >= 0; i--)
        {
            boxesToCleanUp[i].GetComponent<BoxTraits>().traversed = false;
            boxesToCleanUp.RemoveAt(i);
        }

        swapXIndDiff = 0;
        swapZIndDiff = 0;
        swapYIndDiff = 0;
    }
}
