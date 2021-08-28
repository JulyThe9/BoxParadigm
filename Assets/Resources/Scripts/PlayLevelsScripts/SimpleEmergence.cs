using UnityEngine;

public class SimpleEmergence : MonoBehaviour
{
    public bool effectInProgress;
    public BoxData selBoxData;
    public ObjectTypes.BoxActions latestAction;
    // TODO: the fact it's int seems super sketchy, 
    // CHECK how simultaneous projectiles get resolved
    public int dHandedToolRightUsed; 

    public void Start()
    {
        effectInProgress = false;
        selBoxData = null;
        latestAction = ObjectTypes.BoxActions.Irrelevant;
        dHandedToolRightUsed = 0;
    }

    public void CleanUpUnfinishedEffects()
    {
        // TODO: legit for quantum select/connect, what about others?
        if (selBoxData != null)
        {
            BoxEntry selBoxEntry = BHWrapper.bHolder.list[selBoxData.xInd][selBoxData.zInd][selBoxData.yInd];
            selBoxEntry.GetBoxGameObj().GetComponent<Renderer>().material = selBoxEntry.GetBoxGameObj().GetComponent<BoxBehavior>().curMaterial;
        }
    }
}
