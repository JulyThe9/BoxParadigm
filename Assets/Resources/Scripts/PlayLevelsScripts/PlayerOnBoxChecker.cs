using UnityEngine;

public class PlayerOnBoxChecker : MonoBehaviour
{
    private BoxBehavior boxBehavior;
    private void Start()
    {
        boxBehavior = gameObject.GetComponentInParent(typeof(BoxBehavior)) as BoxBehavior;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (boxBehavior != null && other.gameObject.CompareTag(ObjectTypes.playerBottomTagName))
        {
            boxBehavior.OnPlayerEnterBox();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (boxBehavior != null && other.gameObject.CompareTag(ObjectTypes.playerBottomTagName))
        {
            boxBehavior.OnPlayerExitBox();
        }
    }
}
