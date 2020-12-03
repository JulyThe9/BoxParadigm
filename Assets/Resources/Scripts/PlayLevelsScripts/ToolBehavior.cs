using UnityEngine;

public class ToolBehavior : MonoBehaviour
{
    private GameObject mainCamera = null;

    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

   
    void Update()
    {
        transform.localRotation = mainCamera.transform.localRotation;
    }
}
