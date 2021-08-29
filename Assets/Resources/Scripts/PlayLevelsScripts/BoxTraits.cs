using System.Collections.Generic;
using UnityEngine;

public class BoxTraits : MonoBehaviour
{
    public bool levitating = false;
    public bool traversed = false; // TODO: probably reset if setting traversed to true doesn't lead to GameObject destruction
    public List<BoxData> connectedTo;

    // right hand effects of double handed tools only
    public Dictionary<ObjectTypes.EffectTypes, bool> selectionMade = new Dictionary<ObjectTypes.EffectTypes, bool> // TODO: ņot used at the moment
    {
        { ObjectTypes.EffectTypes.Swapping, false },
        { ObjectTypes.EffectTypes.QuantumConnect, false }
    };
}