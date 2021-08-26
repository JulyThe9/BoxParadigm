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

    private SimpleEmergence simpleEmergence;
    public Material origMaterial;

    private void Start()
    {
        origMaterial = GetComponent<Renderer>().material;

        simpleEmergence = GameObject.Find(ObjectTypes.simpleEmergenceName).GetComponent<SimpleEmergence>();

        groundedChecker = transform.Find(ObjectTypes.bottomJoint).GetComponent<BoxGroundedChecker>();

        // constraint initialization
        boxConstraints = transform.GetComponent<BoxConstraints>();
        boxTraits = transform.GetComponent<BoxTraits>();
        boxData = transform.GetComponent<BoxData>();
        boxEntry = BHWrapper.bHolder.list[boxData.xInd][boxData.zInd][boxData.yInd];

        switch (boxEntry.type)
        {
            case ObjectTypes.BoxTypes.Wood:
                boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.SwapSelect] = true;
                boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.Swapping] = true;
                boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.QuantumSelect] = true;
                boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.QuantumConnect] = true;
                boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.AnalysisAttack] = true;
                boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.TurretAttack] = true;
                break;
            case ObjectTypes.BoxTypes.Stone:
                boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.SwapSelect] = true;
                boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.Swapping] = true;
                boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.QuantumSelect] = true;
                boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.QuantumConnect] = true;
                boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.AnalysisAttack] = true; // TODO: temp
                //boxConstraints.attackSueffectSusceptiblesceptible[ObjectTypes.AttackTypes.AnalysisAttack] = false;
                boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.TurretAttack] = true;
                break;
            case ObjectTypes.BoxTypes.Mirror:
                boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.SwapSelect] = true;
                boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.Swapping] = true;
                boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.QuantumSelect] = true;
                boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.QuantumConnect] = true;
                boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.AnalysisAttack] = true;
                boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.TurretAttack] = true;
                break;
            case ObjectTypes.BoxTypes.Turret:
                boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.SwapSelect] = true;
                boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.Swapping] = true;
                boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.QuantumSelect] = true;
                boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.QuantumConnect] = true;
                boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.AnalysisAttack] = true;
                boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.TurretAttack] = true;
                break;
            case ObjectTypes.BoxTypes.Finish:
                boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.SwapSelect] = false;
                boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.Swapping] = false;
                boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.QuantumSelect] = false;
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
            if (!collision.gameObject.GetComponent<GeneralProjectileBehavior>().markedABox_)
            {
                collision.gameObject.GetComponent<GeneralProjectileBehavior>().markedABox_ = true;
                ProjectileBoxInteraction(collision.gameObject.GetComponent<GeneralProjectileConstraints>());
            }
        }
    }

    private void ProjectileBoxInteraction(GeneralProjectileConstraints prjctlConstraints)
    {
        switch(prjctlConstraints.effectType)
        {
            case ObjectTypes.EffectTypes.AnalysisAttack:
                if (boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.AnalysisAttack])
                {
                    OnAttack();
                }
                else
                {

                }
                break;
            case ObjectTypes.EffectTypes.TurretAttack:
                if (boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.AnalysisAttack])
                {
                    OnAttack();
                }
                else
                {

                }
                break;
            case ObjectTypes.EffectTypes.SwapSelect:
                if (boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.AnalysisAttack])
                {
                    OnSwapSelect();
                }
                else
                {

                }
                break;
            case ObjectTypes.EffectTypes.Swapping:
                if (boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.AnalysisAttack])
                {
                    OnSwapping();
                }
                else
                {

                }
                break;
            case ObjectTypes.EffectTypes.QuantumSelect:
                if (boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.AnalysisAttack])
                {
                    OnQuantumSelect();
                }
                else
                {

                }
                break;
            case ObjectTypes.EffectTypes.QuantumConnect:
                if (boxConstraints.effectSusceptible[ObjectTypes.EffectTypes.AnalysisAttack])
                {
                    if (simpleEmergence.effectInProgress)
                    {
                        OnQuantumConnect();
                    }
                    else
                    {

                    }
                }
                else
                {
                    // TODO: potentially OnQuantumConnectFailed();
                    // visual 

                    // structural
                    simpleEmergence.CleanUpUnfinishedEffects();
                }
                break;
        }
        simpleEmergence.latestAction = ObjectTypes.effectTypesToBoxActions[prjctlConstraints.effectType];
        if (simpleEmergence.latestAction != ObjectTypes.BoxActions.Irrelevant)
        {
            ReplayActionsOnConnected(boxTraits);
        }
    }

    private void OnAttack()
    {
        if (!IsTopInPillar())
        {
            BoxEntry upperBoxEntry = GetUpperBoxEntry();
            if (upperBoxEntry.type != ObjectTypes.BoxTypes.Undetermined)
            {
                upperBoxEntry.GetBoxGameObj().GetComponent<BoxBehavior>().SetGrounded(false);
            }
            BHWrapper.ClearBoxEntry(boxEntry.xInd, boxEntry.zInd, boxEntry.yInd);
        }
        else
        {
            BHWrapper.RemoveFromPillar(boxEntry.xInd, boxEntry.zInd, boxEntry.yInd);
        }
        // TODO: delay not too big?
        boxEntry.Clear();
        Destroy(gameObject);
    }

    private void OnSwapSelect()
    {

    }

    private void OnSwapping()
    {

    }

    private void OnQuantumSelect()
    {
        // TODO: parametrize potentially
        // TODO: consts for "Materials/" and "_"
        // visual
        GetComponent<Renderer>().material = Resources.Load("Materials/" + 
            ObjectTypes.boxTypesToMaterialNames[boxEntry.type] + "_" + ObjectTypes.effectTypesToSelMaterialNames[ObjectTypes.EffectTypes.QuantumSelect],
            typeof(Material)) as Material;

        // structural
        boxTraits.selectionMade[ObjectTypes.EffectTypes.QuantumConnect] = true;
        simpleEmergence.effectInProgress = true;
        simpleEmergence.selBoxData = boxData;
    }

    private void OnQuantumConnect()
    {
        // visual
        GetComponent<Renderer>().material = Resources.Load("Materials/" +
           ObjectTypes.boxTypesToMaterialNames[boxEntry.type] + "_" + ObjectTypes.effectTypesToSelMaterialNames[ObjectTypes.EffectTypes.QuantumConnect],
           typeof(Material)) as Material;

        // structural
        BoxEntry selBoxEntry = BHWrapper.bHolder.list[simpleEmergence.selBoxData.xInd][simpleEmergence.selBoxData.zInd][simpleEmergence.selBoxData.yInd];
        BoxBehavior selBoxBehavior = selBoxEntry.GetBoxGameObj().GetComponent<BoxBehavior>();

        boxTraits.connectedTo.Add(selBoxBehavior.boxData);
        selBoxBehavior.boxTraits.connectedTo.Add(boxData);

        simpleEmergence.effectInProgress = false;
        simpleEmergence.selBoxData = null;
    }

    private void ReplayActionsOnConnected(BoxTraits curBoxTraits)
    {
        foreach (BoxData curBoxData in curBoxTraits.connectedTo)
        {
            if (!BHWrapper.BoxExists(curBoxData.xInd, curBoxData.zInd, curBoxData.yInd))
            {
                continue;
            }

            BoxBehavior connectBoxBeh = BHWrapper.GetBoxEntry(curBoxData.xInd, curBoxData.zInd, curBoxData.yInd).GetBoxGameObj().GetComponent<BoxBehavior>();
            if (!connectBoxBeh.boxTraits.traversed)
            {
                // TODO: possibly arguments related to boxAction (by how many spaces to move)
                connectBoxBeh.boxTraits.traversed = true; // TODO: temp
                ReplayActionsOnConnected(connectBoxBeh.boxTraits);
                ReplayAction(connectBoxBeh, simpleEmergence.latestAction);
            }
        }
    }

    private void ReplayAction(BoxBehavior boxBehavior, ObjectTypes.BoxActions boxAction)
    {
        switch (boxAction)
        {
            case ObjectTypes.BoxActions.Destroyed:
                boxBehavior.OnAttack();
                break;
        }
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
