using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PlayerAttackSystem : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private AudioClip gunshot;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private ParticleSystem impact;
    [SerializeField] private BulletTracer tracerPrefab;
    [SerializeField] private float tracerDecay = 0.6f;
    [SerializeField] private float tracerWidth = 0.7f;
    [SerializeField] private float shotDelay = 0.05f;

    private bool _reqestedAttack = false;
    // private PlayerInputActions _inputActions;
    private int targetsShotInSlow = 0;
    private float delayTimer;
    private Queue<BulletTracer> tracerPool = new Queue<BulletTracer>();
    private Queue<Tuple<BulletTracer, Vector3, Vector3, float, float>> timeSlowTracers = new Queue<Tuple<BulletTracer, Vector3, Vector3, float, float>>();
    
    private void Start()
    {
        for (int i = 0; i < 25; i++)
        {
            // Add tracers to the pool to prevent runtime instantiation
            var tracer = Instantiate(tracerPrefab, bulletSpawn.position, Quaternion.identity);
            tracer.gameObject.SetActive(false);
            tracerPool.Enqueue(tracer);
        }
    }

    public void updateInput(CharacterInput input)
    {
        _reqestedAttack = input.Attack;
    }

    private void Update()
    {
        if (delayTimer <= shotDelay)
        {
            delayTimer += Time.deltaTime;
        }
        if (_reqestedAttack && delayTimer >= shotDelay)
        {
            Shoot();
        }
        else if (_reqestedAttack && delayTimer + (shotDelay / 3) >= shotDelay)
        {
            StartCoroutine(BufferShoot());
        }
    }

    private void Shoot()
    {
        delayTimer = 0;
        if (Timeslow.IsSlowed)
        {
            targetsShotInSlow += 1;
        }
        RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit))
            {
                var target = hit.collider.gameObject;

                if (target.GetComponent<Destructible>() != null)
                {
                    if (Timeslow.IsSlowed)
                    {
                        Debug.Log(targetsShotInSlow);
                        target.GetComponent<Destructible>().Kill(targetsShotInSlow);
                    }
                    else
                    {
                        target.GetComponent<Destructible>().Kill(0);
                    }
                }
                if (Timeslow.IsSlowed)
                {
                    // Wait to display most effects if time is slowed
                    BulletTracer tracer = tracerPool.Dequeue();
                    tracer.gameObject.SetActive(true);
                    timeSlowTracers.Enqueue(new Tuple<BulletTracer, Vector3, Vector3, float, float>(tracer, bulletSpawn.position, hit.point, tracerWidth, tracerDecay));
                    tracerPool.Enqueue(tracer);
                }
                else
                {
                    BulletTracer tracer = tracerPool.Dequeue();
                    tracer.gameObject.SetActive(true);
                    tracer.FireTracer(bulletSpawn.position, hit.point, tracerWidth, tracerDecay);
                    tracerPool.Enqueue(tracer);
                }

            }
            else
            {
                Vector3 maxDistance = playerCamera.transform.position + playerCamera.transform.forward * 100f;
                if (Timeslow.IsSlowed)
                {
                    // Wait to display most effects if time is slowed
                    BulletTracer tracer = tracerPool.Dequeue();
                    tracer.gameObject.SetActive(true);
                    timeSlowTracers.Enqueue(new Tuple<BulletTracer, Vector3, Vector3, float, float>(tracer, bulletSpawn.position, maxDistance, tracerWidth, tracerDecay));
                    tracerPool.Enqueue(tracer);
                }
                else
                {
                    BulletTracer tracer = tracerPool.Dequeue();
                    tracer.gameObject.SetActive(true);
                    tracer.FireTracer(bulletSpawn.position, maxDistance, tracerWidth, tracerDecay);
                    tracerPool.Enqueue(tracer);
                }
            }
            AudioManager.instance.PlayOmnicientSoundClip(gunshot, 1f, true, true);
    }

    public IEnumerator FireTimeslowTracers()
    {
        while (Timeslow.IsSlowed)
        {
            yield return null;
        }
        while (timeSlowTracers.Count > 0)
        {
            yield return new WaitForSeconds(1/15f);
            targetsShotInSlow = 0;
            var tracer = timeSlowTracers.Dequeue();
            tracer.Item1.FireTracer(tracer.Item2, tracer.Item3, tracer.Item4, tracer.Item5);
        }
    }
    
    private IEnumerator BufferShoot()
    {
        while (delayTimer < shotDelay)
        {
            yield return null;
        }
        Shoot();
    }
}
