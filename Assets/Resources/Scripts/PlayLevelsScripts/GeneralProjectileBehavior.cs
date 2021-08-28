using UnityEngine;

public class GeneralProjectileBehavior : MonoBehaviour
{
    private bool collided_ = false;
    private Rigidbody prjctlRigidbody_;

    private Vector3 initVelocity_;
    private float prjctlMult_;
    private bool changingVelocity_;

    public bool markedABox_;

    public SimpleEmergence simplEmergence_;

    private void OnCollisionEnter(Collision collision)
    {
        if (!collided_)
        {
            collided_ = true;
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        prjctlRigidbody_ = gameObject.GetComponent<Rigidbody>();
        initVelocity_ = prjctlRigidbody_.velocity;
        prjctlMult_ = 0f;
        changingVelocity_ = true;
        markedABox_ = false;
    }

    public void Update()
    {
        if (changingVelocity_)
        {
            if (prjctlMult_ < GlobalVariables.maxPrjctlSpeedPercent * GlobalVariables.prjctlSpeed)
            {
                prjctlMult_ += GlobalVariables.prjctlSpeedChangePercent * GlobalVariables.prjctlSpeed;
                prjctlRigidbody_.velocity = initVelocity_ + initVelocity_.normalized * prjctlMult_;
            }
            else
            {
                changingVelocity_ = false;
            }    
        }
    }

    public void OnDestroy()
    {
        if (!markedABox_)
        {
            simplEmergence_.CleanUpUnfinishedEffects();
        }
        --simplEmergence_.dHandedToolRightUsed;
        Debug.Assert(simplEmergence_.dHandedToolRightUsed >= 0);
    }
}
