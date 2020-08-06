using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Transform playerBody;
    public float sens = 5.0f;
    public float sming = 2.0f;

    private Vector2 mouseLook;
    private Vector2 smooth;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        Vector2 mouseData = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        mouseData = Vector2.Scale(mouseData,
            new Vector2(sens * sming, sens * sming));

        smooth.x = Mathf.Lerp(smooth.x, mouseData.x, 1f / sming);
        smooth.y = Mathf.Lerp(smooth.y, mouseData.y, 1f / sming);
        mouseLook += smooth;

        mouseLook.y = Mathf.Clamp(mouseLook.y, -90f, 90f);

        transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
        playerBody.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, playerBody.transform.up);
    }
}