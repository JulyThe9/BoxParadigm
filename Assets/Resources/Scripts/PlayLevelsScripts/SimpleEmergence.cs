using UnityEngine;

public class SimpleEmergence : MonoBehaviour
{
    public bool effectInProgress;
    public BoxData selBoxData;
    public ObjectTypes.BoxActions latestAction;

    public void Start()
    {
        effectInProgress = false;
        selBoxData = null;
        latestAction = ObjectTypes.BoxActions.Irrelevant;
    }

    public void CleanUpUnfinishedEffects()
    {
        // TODO: legit for quantum select/connect, what about others?
        if (selBoxData != null)
        {
            BoxEntry selBoxEntry = BHWrapper.bHolder.list[selBoxData.xInd][selBoxData.zInd][selBoxData.yInd];
            selBoxEntry.GetBoxGameObj().GetComponent<Renderer>().material = selBoxEntry.GetBoxGameObj().GetComponent<BoxBehavior>().origMaterial;
        }
    }
}
