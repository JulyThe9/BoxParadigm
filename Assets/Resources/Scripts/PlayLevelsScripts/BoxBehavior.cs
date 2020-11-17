using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxBehavior : MonoBehaviour
{
    public float speed = 3f;
    public float gravity = -20f;
    public Vector3 velocity;

    private bool wasFalling = false;

    public bool allowFalling = false;
    private BoxGroundedChecker groundedChecker = null;
    private BoxConstraints boxConstraints = null;
    private BoxTraits boxTraits = null;
    private BoxData boxData = null;
    private BoxEntry boxEntry = null;

    private void Start()
    {
        groundedChecker = transform.Find(ObjectTypes.bottomJoint).GetComponent<BoxGroundedChecker>();

        // constraint initialization
        boxConstraints = transform.GetComponent<BoxConstraints>();
        boxTraits = transform.GetComponent<BoxTraits>();
        boxData = transform.GetComponent<BoxData>();
        boxEntry = BHWrapper.bHolder.list[boxData.xInd][boxData.zInd][boxData.yInd];

        switch (boxEntry.type)
        {
            case ObjectTypes.BoxTypes.Wood:
                boxConstraints.swappable = true;
                boxConstraints.quantumConnectable = true;
                boxConstraints.attackSusceptible[ObjectTypes.AttackTypes.AnalysisAttack] = true;
                boxConstraints.attackSusceptible[ObjectTypes.AttackTypes.TurretAttack] = true;
                break;
            case ObjectTypes.BoxTypes.Stone:
                boxConstraints.swappable = true;
                boxConstraints.quantumConnectable = true;
                boxConstraints.attackSusceptible[ObjectTypes.AttackTypes.AnalysisAttack] = false;
                boxConstraints.attackSusceptible[ObjectTypes.AttackTypes.TurretAttack] = true;
                break;
            case ObjectTypes.BoxTypes.Mirror:
                boxConstraints.swappable = true;
                boxConstraints.quantumConnectable = true;
                boxConstraints.attackSusceptible[ObjectTypes.AttackTypes.AnalysisAttack] = true;
                boxConstraints.attackSusceptible[ObjectTypes.AttackTypes.TurretAttack] = true;
                break;
            case ObjectTypes.BoxTypes.Turret:
                boxConstraints.swappable = true;
                boxConstraints.quantumConnectable = true;
                boxConstraints.attackSusceptible[ObjectTypes.AttackTypes.AnalysisAttack] = true;
                boxConstraints.attackSusceptible[ObjectTypes.AttackTypes.TurretAttack] = true;
                break;
            case ObjectTypes.BoxTypes.Finish:
                boxConstraints.swappable = false;
                boxConstraints.quantumConnectable = false;
                boxConstraints.attackSusceptible[ObjectTypes.AttackTypes.AnalysisAttack] = false;
                boxConstraints.attackSusceptible[ObjectTypes.AttackTypes.TurretAttack] = false;
                break;
            default:
                break;
        }

        // trait initialization
        if (boxData.yInd > 0)
        {
            if (BHWrapper.bHolder.list[boxData.xInd][boxData.zInd][boxData.yInd - 1].type == ObjectTypes.BoxTypes.Undetermined)
            {
                // TODO: also add an option to editor to make a box levitating (with visual effects)
                boxTraits.levitating = true;
            }
        }
    }

    void FixedUpdate()
    {
        if (allowFalling)
        {
            if (groundedChecker.grounded)
            {
                if (wasFalling)
                {
                    float additDist = 0f;
                    if (groundedChecker.hitBoxGameObject.tag == ObjectTypes.floorTagName)
                    {                 
                        additDist = GlobalDimensions.minDifDistance_;
                    }
                    transform.position = Vector3.MoveTowards(transform.position,
                        new Vector3(transform.position.x,
                                    groundedChecker.hitBoxGameObject.transform.position.y + GlobalDimensions.margin_ + additDist,
                                    transform.position.z),
                        0.5f);

                    wasFalling = false;
                }
                if (velocity.y < -2f)
                {
                    velocity.y = -2f;
                }
            }
            else
            {
                velocity.y += gravity * Time.deltaTime;
                transform.Translate(velocity * Time.deltaTime, Space.World);
                wasFalling = true;
            }
        }
    }
}
