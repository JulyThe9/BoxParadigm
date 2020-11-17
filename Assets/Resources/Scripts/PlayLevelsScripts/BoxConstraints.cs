using System.Collections.Generic;
using UnityEngine;

public class BoxConstraints : MonoBehaviour
{
    public bool swappable = false;
    public bool quantumConnectable = false;
    public Dictionary<ObjectTypes.AttackTypes, bool> attackSusceptible = new Dictionary<ObjectTypes.AttackTypes, bool>
    {
        { ObjectTypes.AttackTypes.AnalysisAttack, false },
        { ObjectTypes.AttackTypes.TurretAttack, false }
    };
}