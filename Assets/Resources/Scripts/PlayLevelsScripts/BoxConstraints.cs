using System.Collections.Generic;
using UnityEngine;

public class BoxConstraints : MonoBehaviour
{
    public Dictionary<ObjectTypes.EffectTypes, bool> effectSusceptible = new Dictionary<ObjectTypes.EffectTypes, bool>
    {
        { ObjectTypes.EffectTypes.AnalysisAttack, false },
        { ObjectTypes.EffectTypes.TurretAttack, false },
        { ObjectTypes.EffectTypes.Swapping, false },
        { ObjectTypes.EffectTypes.QuantumConnect, false }
    };
}