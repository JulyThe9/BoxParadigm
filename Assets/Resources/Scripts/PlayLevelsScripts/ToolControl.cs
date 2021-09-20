using UnityEngine;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

public class ToolControl : MonoBehaviour
{
    public SO_Tool[] loadout_;
    public Transform toolParent_;

    private SO_Tool curTool_;
    private int curToolIdx_;
    private ObjectTypes.ToolTypes curToolType_;
    private GameObject currentToolObj_;
    private Transform curToolLeftFirePoint_;
    private Transform curToolRightFirePoint_;

    private Camera playerCam_;
    private SimpleEmergence simpleEmergence_;
    private Vector3 projTravelDest_;

    private int layerMask_;

    private bool dHandedToolLeftUsed_ = false;

    public Dictionary<ObjectTypes.ToolTypes, int> toolCounts = new Dictionary<ObjectTypes.ToolTypes, int>
    {
        { ObjectTypes.ToolTypes.Analysis, 0 },
        { ObjectTypes.ToolTypes.SpaceWarp, 0 },
        { ObjectTypes.ToolTypes.Synthesis, 0 },
        { ObjectTypes.ToolTypes.Levitator, 0 }
    };

    public void Start()
    {
        playerCam_ = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        simpleEmergence_ = GameObject.Find(ObjectTypes.simpleEmergenceName).GetComponent<SimpleEmergence>();
        // MAKE SURE it corresponds to colliding map (otherwise we will have undestroyed objects)
        layerMask_ = LayerMask.GetMask(GlobalVariables.groundLayerName);

        curToolIdx_ = 0;
     }

    public void Equip(int toolIdx)
    {
        if (simpleEmergence_.dHandedToolRightUsed == 0)
        {
            simpleEmergence_.CleanUpUnfinishedEffects();
        }

        dHandedToolLeftUsed_ = false;

        if (curToolType_ != ObjectTypes.ToolTypes.Undetermined)
        {
            if (curToolType_ != loadout_[toolIdx].type_)
            {
                Destroy(currentToolObj_); // TODO: more efficient with enabling/disabling?

                curTool_ = loadout_[toolIdx];
                curToolIdx_ = toolIdx;
                curToolType_ = loadout_[toolIdx].type_;
                InstToolPrefab(toolIdx);
            }
        }
        else
        {
            curTool_ = loadout_[toolIdx];
            curToolIdx_ = toolIdx;
            curToolType_ = loadout_[toolIdx].type_;
            InstToolPrefab(toolIdx);
        }
    }

    public void Unquip()
    {
        if (curToolType_ != ObjectTypes.ToolTypes.Undetermined)
        {
            curTool_ = null;
            curToolIdx_ = 0;
            curToolType_ = ObjectTypes.ToolTypes.Undetermined;
            Destroy(currentToolObj_);
        }
    }

    public void ReduceToolCount(ObjectTypes.ToolTypes toolType)
    {
        if (!toolCounts.ContainsKey(toolType))
        {
            return; // nothing to reduce
        }

        if (toolCounts[toolType] > 0)
        {
            if (toolCounts[toolType] == 1)
            {
                TriggerToolDepletion(curToolIdx_, true);
                Destroy(currentToolObj_);
                InstToolPrefab(curToolIdx_);
            }
            --toolCounts[toolType];
        }
    }

    private void TriggerToolDepletion(int toolIdx, bool depleted)
    {
        GameObject tempPrefab = loadout_[toolIdx].prefab_;
        loadout_[toolIdx].prefab_ = loadout_[toolIdx].secondaryPrefab_;
        loadout_[toolIdx].secondaryPrefab_ = tempPrefab;
        loadout_[toolIdx].SetDepleted(depleted);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void InstToolPrefab(int toolIdx)
    {
        GameObject newTool = Instantiate(loadout_[toolIdx].prefab_, toolParent_.position, toolParent_.rotation, toolParent_) as GameObject;
        newTool.transform.localPosition = Vector3.zero;
        newTool.transform.localEulerAngles = Vector3.zero;
        currentToolObj_ = newTool;
        curToolLeftFirePoint_ = currentToolObj_.transform.Find("Anchor").Find("Effects").Find("LeftFirePoint");
        curToolRightFirePoint_ = currentToolObj_.transform.Find("Anchor").Find("Effects").Find("RightFirePoint"); // TODO: consts
    }

    // TODO: create consts
    public bool LaunchProjectile()
    {
        if (curToolType_ == ObjectTypes.ToolTypes.Undetermined)
        {
            return false;
        }
        if (toolCounts[curToolType_] == 0)
        {
            // effects
            return false;
        }

        GameObject projectilePrefab = curTool_.rightProjectilePrefab_;
        Transform curToolFirePoint = curToolRightFirePoint_;
        if (curTool_.doubleHanded_)
        {
            if (dHandedToolLeftUsed_) // TODO: make it SATISFYING - using tool (left mb and right mb)
            {
                dHandedToolLeftUsed_ = false;
            }
            else
            {
                curToolFirePoint = curToolLeftFirePoint_;
                projectilePrefab = curTool_.leftProjectilePrefab_;
                dHandedToolLeftUsed_ = true;
            }
        }
        

        Ray ray = playerCam_.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        float rayCastDist = 1000f;
        if (Physics.Raycast(ray, out hit, rayCastDist, layerMask_))
        {
            projTravelDest_ = hit.point;
            if (hit.transform.gameObject != null)
            {
                Debug.Log(hit.transform.gameObject.tag);
            }
            ++simpleEmergence_.dHandedToolRightUsed;
            InstantiateProjectile(projectilePrefab, curToolFirePoint);
        }
        else
        {
            projTravelDest_ = ray.GetPoint(rayCastDist);
            ++simpleEmergence_.dHandedToolRightUsed;
            GameObject projectileObj = InstantiateProjectile(projectilePrefab, curToolFirePoint);
            // TODO: temp, level will be enclosed (hits everywhere, otherwise could scale t with level size - glob var)
            Destroy(projectileObj, 3.0f);
        }

        return true;
    }

    private GameObject InstantiateProjectile(GameObject projectile, Transform firePoint) // maybe the second parameter is not needed
    {
        GameObject projectileObj = Instantiate(projectile, firePoint.position,  playerCam_.transform.rotation) as GameObject;
        projectileObj.GetComponent<Rigidbody>().velocity = (projTravelDest_ - firePoint.position).normalized * GlobalVariables.prjctlSpeed;
        projectileObj.GetComponent<GeneralProjectileBehavior>().simplEmergence_ = simpleEmergence_;
        return projectileObj;
    }

    private void OnDestroy()
    {
        for (int i = 0; i < loadout_.Length; ++i)
        {
            if (loadout_[i].GetDepleted())
            {
                TriggerToolDepletion(i, false);
            }
        }
    }
}
