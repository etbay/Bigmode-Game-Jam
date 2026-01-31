using UnityEngine;

public class Destructible : MonoBehaviour
{
    void Kill()
    {
        // Create an explosion partical effect
        Destroy(gameObject);
    }
}