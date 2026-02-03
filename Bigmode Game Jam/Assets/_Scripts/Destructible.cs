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
        var explosion = PoolManager.instance.GetItemFromPool("Explosives");
        explosion.transform.position = this.transform.position;
        explosion.GetComponent<Explosion>()?.Play();
        Destroy(gameObject);
    }
}