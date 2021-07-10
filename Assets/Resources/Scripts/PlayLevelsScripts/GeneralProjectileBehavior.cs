using UnityEngine;

public class GeneralProjectileBehavior : MonoBehaviour
{
    private bool collided_ = false;

    private void OnCollisionStay(Collision collision)
    {
        if (!collided_)
        {
            collided_ = true;
            Destroy(gameObject);
        }
    }
}
