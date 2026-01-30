using UnityEngine;

public class Goop : MonoBehaviour
{
    private BoxCollider boxCollider;
    void Awake()
    {
        boxCollider = this.GetComponent<BoxCollider>();
    }
}
