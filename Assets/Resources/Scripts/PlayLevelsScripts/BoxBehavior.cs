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

    private void Start()
    {
        groundedChecker = transform.Find(ObjectTypes.bottomJoint).GetComponent<BoxGroundedChecker>();
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
