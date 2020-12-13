using UnityEngine;

// SOURCE: https://www.youtube.com/watch?v=nlcIz-czKyI&ab_channel=WeltonKing

public class ToolSway : MonoBehaviour
{
    public float intensity_ = 1f;
    public float smooth_ = 10f;

    private Quaternion originRotation_;

    private void Start()
    {
        originRotation_ = transform.localRotation;
    }

    void Update()
    {
        UpdateSway();
    }

    private void UpdateSway()
    {
        // TODO: have input readings in one place - MouseLook or ideally a separate class
        float t_x_mouse = Input.GetAxis("Mouse X");
        float t_y_mouse = Input.GetAxis("Mouse Y");
         
        Quaternion t_x_adj = Quaternion.AngleAxis(-intensity_ * t_x_mouse, Vector3.up);
        Quaternion t_y_adj = Quaternion.AngleAxis(intensity_ * t_y_mouse, Vector3.right);
        Quaternion targetRotation = originRotation_ * t_x_adj * t_y_adj;

        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * smooth_);
    }
}
