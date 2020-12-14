using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 3f;
    public float gravity = -20f;
    public float jumpHeight = 0.8f;

    public Transform groundCheck;
    public float groundDistance = 0.1f;
    public LayerMask groundMask;

    public Vector3 velocity;
    public bool isGrounded = true;
    public bool isHanging = false;

    private TriggerChecker triggerChecker = null;
    private ToolControl toolControl = null;

    void Start()
    {
        triggerChecker = transform.Find(ObjectTypes.ledgeGrabber).GetComponent<TriggerChecker>();
        toolControl = transform.GetComponent<ToolControl>();
    }

    void Update()
    {
        // Grounding and movements
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < -2f)
        {
            velocity.y = -2f;
        }
        else if (/*Input.GetButtonDown("Jump") &&*/ triggerChecker.grabbed)
        {
            if (velocity.y < -2f)
            {
                velocity.y = -2f;
                isHanging = true;
                toolControl.Unquip();
            }
        }
        else if (isHanging)
        {
            isHanging = false;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButton("Jump"))
        {
            if (isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                isGrounded = false;
            }
            else if (isHanging)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    // TODO: start from here
                    velocity.y = Mathf.Sqrt((jumpHeight + 0.5f) * -2f * gravity);
                    isHanging = false;
                }
                else
                {
                    velocity.y = -2f;
                    isHanging = false;
                }
            }
        }
        if (!isGrounded && !isHanging)
        {
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }

        // Tool controlling // TODO: mayve it's better to do this in a separate script
        // TODO: binding
        if (Input.GetKey(KeyCode.Alpha1))
        {
            toolControl.Equip(0);
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            toolControl.Equip(1);
        }
        if (Input.GetKey(KeyCode.T))
        {
            toolControl.Unquip();
        }
    }
}