using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tool", menuName = "Tool")]
public class SO_Tool : ScriptableObject
{
    public string name_;
    public ObjectTypes.ToolTypes type_;
    public bool doubleHanded_;
    public GameObject prefab_;
    public GameObject secondaryPrefab_;
    public GameObject leftProjectilePrefab_;
    public GameObject rightProjectilePrefab_;

    private bool depleted_;
    public void SetDepleted(bool depleted)
    {
        depleted_ = depleted;
    }
    public bool GetDepleted()
    {
        return depleted_;
    }
}
