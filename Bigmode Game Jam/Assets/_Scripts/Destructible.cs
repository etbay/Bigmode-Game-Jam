using UnityEngine;

public class Destructible : MonoBehaviour
{
    private bool dead = false;
    public void Kill()
    {
        dead = true;
    }

    void Update()
    {
        if (dead && !Timeslow.IsSlowed)
        {
            Destroy(gameObject);
        }
    }
}