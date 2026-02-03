using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Destructible : MonoBehaviour
{
    private int order = 0;
    private bool dead = false;
    public void Kill(int num)
    {
        if (!dead)
        {
            dead = true;
            if (Timeslow.IsSlowed)
            {
                order = num;
            }
        }
    }

    void Update()
    {
        if (dead && !Timeslow.IsSlowed)
        {
            StartCoroutine(DeathScript(order));
        }
    }

    private IEnumerator DeathScript(int timeWait)
    {
        yield return new WaitForSeconds(timeWait / 15f); // controls deletion after resuming time
        Player.SlickValue += SlickometerData.DestructibleSlickGain;
        // Create an explosion from an explosion pool. 
        // Where is the pool stored:
        // - general game manager singleton
        // - unique explosionpool script singleton
        // - objectpool pool script to hold a hash of each object pool by indifier <- most efficient?
        Destroy(gameObject);
    }
}