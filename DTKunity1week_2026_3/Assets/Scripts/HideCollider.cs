using UnityEngine;

public class HideCollider : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weed"))
        {
            gameObject.tag = "Untagged";
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Weed"))
        {
            gameObject.tag="Player";
        }
    }
}
