using UnityEngine;
using System.Runtime.CompilerServices;

public class ToolControl : MonoBehaviour
{
    public Tool[] loadout_;
    public Transform toolParent_;

    private Tool curTool_;
    private ObjectTypes.ToolTypes curToolType_;
    private GameObject currentToolObj_;
    private Transform curToolLeftFirePoint_;
    private Transform curToolRightFirePoint_;

    private Camera playerCam_;
    private Vector3 projTravelDest_;

    private int layerMask_;

    private bool dHandedToolLeftUsed_ = false;

    public void Start()
    {
        playerCam_ = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        // MAKE SURE it corresponds to colliding map (otherwise we will have undestroyed objects)
        layerMask_ = LayerMask.GetMask(GlobalVariables.groundLayerName); 
     }

    public void Equip(int toolIdx)
    {
        if (curToolType_ != ObjectTypes.ToolTypes.Undetermined)
        {
            if (curToolType_ != loadout_[toolIdx].type_)
            {
                Destroy(currentToolObj_); // TODO: more efficient with enabling/disabling?

                curTool_ = loadout_[toolIdx];
                curToolType_ = loadout_[toolIdx].type_;
                InstToolPrefab(toolIdx);
            }
        }
        else
        {
            curTool_ = loadout_[toolIdx];
            curToolType_ = loadout_[toolIdx].type_;
            InstToolPrefab(toolIdx);
        }
    }

    public void Unquip()
    {
        if (curToolType_ != ObjectTypes.ToolTypes.Undetermined)
        {
            curTool_ = null;
            curToolType_ = ObjectTypes.ToolTypes.Undetermined;
            Destroy(currentToolObj_);
        }
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
            InstantiateProjectile(projectilePrefab, curToolFirePoint);
        }
        else
        {
            projTravelDest_ = ray.GetPoint(rayCastDist);
            GameObject projectileObj = InstantiateProjectile(projectilePrefab, curToolFirePoint);
            // TODO: temp, level will be enclosed (hits everywhere, otherwise could scale t with level size - glob var)
            Destroy(projectileObj, 15.0f);
        }

        return true;
    }

    private GameObject InstantiateProjectile(GameObject projectile, Transform firePoint) // maybe the second parameter is not needed
    {
        GameObject projectileObj = Instantiate(projectile, firePoint.position,  playerCam_.transform.rotation) as GameObject;
        projectileObj.GetComponent<Rigidbody>().velocity = (projTravelDest_ - firePoint.position).normalized * GlobalVariables.prjctlSpeed;
        return projectileObj;
    }
}
