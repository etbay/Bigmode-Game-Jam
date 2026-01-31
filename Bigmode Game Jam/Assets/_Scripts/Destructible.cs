using UnityEngine;

public class Destructible : MonoBehaviour
{
    public void Kill()
    {
        // Create an explosion partical effect
        Destroy(gameObject);
    }
}