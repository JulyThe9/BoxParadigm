using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxBehavior : MonoBehaviour
{
    public float speed = 3f;
    public float gravity = -20f;
    public Vector3 velocity;

    private bool wasFalling = false;

    private BoxGroundedChecker groundedChecker = null;
    private BoxConstraints boxConstraints = null;
    private BoxTraits boxTraits = null;
    private BoxData boxData = null;
    private BoxEntry boxEntry = null;

    private BoxEntry tempBoxEntry = null;

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
                boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.Swapping] = true;
                boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.QuantumConnect] = true;
                boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.AnalysisAttack] = true;
                boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.TurretAttack] = true;
                break;
            case ObjectTypes.BoxTypes.Stone:
                boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.Swapping] = true;
                boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.QuantumConnect] = true;
                boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.AnalysisAttack] = true; // TODO: temp
                //boxConstraints.attackSueffectSusceptiblesceptible[ObjectTypes.AttackTypes.AnalysisAttack] = false;
                boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.TurretAttack] = true;
                break;
            case ObjectTypes.BoxTypes.Mirror:
                boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.Swapping] = true;
                boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.QuantumConnect] = true;
                boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.AnalysisAttack] = true;
                boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.TurretAttack] = true;
                break;
            case ObjectTypes.BoxTypes.Turret:
                boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.Swapping] = true;
                boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.QuantumConnect] = true;
                boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.AnalysisAttack] = true;
                boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.TurretAttack] = true;
                break;
            case ObjectTypes.BoxTypes.Finish:
                boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.Swapping] = false;
                boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.QuantumConnect] = false;
                boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.AnalysisAttack] = false;
                boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.TurretAttack] = false;
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
        if (groundedChecker.grounded || boxTraits.levitating)
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

                // update indices
                UpdateStructure();
            }
            if (velocity.y < -2f)
            {
                velocity.y = -2f;
            }
        }
        else
        {
            if (!wasFalling)
            {
                tempBoxEntry = boxEntry.ShallowCopy();
                BHWrapper.ClearBoxEntry(boxEntry.xInd, boxEntry.zInd, boxEntry.yInd);
            }
            velocity.y += gravity * Time.deltaTime;
            transform.Translate(velocity * Time.deltaTime, Space.World);
            wasFalling = true;
        }
    }

    void UpdateStructure()
    {
        if (groundedChecker.hitBoxGameObject.tag == ObjectTypes.boxTagName)
        {
            boxData.yInd = groundedChecker.hitBoxGameObject.GetComponent<BoxData>().yInd + 1;
        }
        else if (groundedChecker.hitBoxGameObject.tag == ObjectTypes.floorTagName)
        {
            boxData.yInd = 0;
        }

        if (tempBoxEntry != null)
        {
            BHWrapper.UpdateBoxEntry(boxData.xInd, boxData.zInd, boxData.yInd, tempBoxEntry);
        }
        else
        {
            Debug.Log("tempBoxEntry is NULL, backend error");
        }
        boxEntry = BHWrapper.bHolder.list[boxData.xInd][boxData.zInd][boxData.yInd];
        tempBoxEntry = null;

        if (boxEntry.topInPillar) // TODO: why is this never updated?
        {
            for (int i = BHWrapper.bHolder.list[boxEntry.xInd][boxEntry.zInd].Count - 1; i > boxEntry.yInd; --i)
            {
                BHWrapper.bHolder.list[boxEntry.xInd][boxEntry.zInd].RemoveAt(i);
            }
        }

        DebugMethods.PrintPillar(boxEntry.xInd, boxEntry.zInd);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals(ObjectTypes.projectileTagName))
        {
            ProjectileBoxInteraction(collision.gameObject.GetComponent<GeneralProjectileConstraints>());
        }
    }

    private void ProjectileBoxInteraction(GeneralProjectileConstraints prjctlConstraints)
    {
        if (!boxConstraints.effectSusceptible[prjctlConstraints.effectType])
        {
            return;
        }

        if (!IsTopInPillar())
        {
            BoxEntry upperBoxEntry = GetUpperBoxEntry();
            upperBoxEntry.GetBoxGameObj().GetComponent<BoxBehavior>().SetGrounded(false);

            BHWrapper.ClearBoxEntry(boxEntry.xInd, boxEntry.zInd, boxEntry.yInd);
        }
        else
        {
            BHWrapper.RemoveFromPillar(boxEntry.xInd, boxEntry.zInd, boxEntry.yInd);
        }

        // TODO: delay not too big?
        Destroy(gameObject);
    }

    private bool IsTopInPillar()
    {
        // TODO: return topInPillar
        return boxData.yInd == BHWrapper.bHolder.list[boxData.xInd][boxData.zInd].Count - 1;
    }

    private BoxEntry GetUpperBoxEntry()
    {
        return BHWrapper.bHolder.list[boxData.xInd][boxData.zInd][boxData.yInd + 1];
    }

    private void SetGrounded(bool grounded)
    {
        groundedChecker.grounded = grounded;
    }
}
