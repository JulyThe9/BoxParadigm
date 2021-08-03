using UnityEngine;

public class GeneralProjectileBehavior : MonoBehaviour
{
    private bool collided_ = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (!collided_)
        {
            collided_ = true;
            Destroy(gameObject);
        }
    }
}
