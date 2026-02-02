using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using Debug = UnityEngine.Debug;

public class PlayerAttackSystem : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private WeaponSFXBank sfxBank;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private ParticleSystem impact;
    [SerializeField] private Light muzzleFlashLight;
    [SerializeField] private BulletTracer tracerPrefab;
    [SerializeField] private BulletTracer timeSlowTracerPrefab;
    [SerializeField] private float tracerDecay = 0.6f;
    [SerializeField] private float tracerWidth = 0.7f;
    [SerializeField] private float shotDelay = 0.05f;
    [SerializeField] private float lightTimer = 0.08f;

    private bool _reqestedAttack = false;
    // private PlayerInputActions _inputActions;
    private int targetsShotInSlow = 0;
    private float delayTimer;
    private Queue<BulletTracer> tracerPool = new Queue<BulletTracer>();
    private Queue<BulletTracer> timeSlowTracerPool = new Queue<BulletTracer>();
    private Queue<Tuple<BulletTracer, Vector3, Vector3, float, float>> tracerTracker = new Queue<Tuple<BulletTracer, Vector3, Vector3, float, float>>();
    
    private void Start()
    {
        for (int i = 0; i < 20; i++)
        {
            // Add tracers to the pool to prevent runtime instantiation
            var tracer = Instantiate(tracerPrefab, bulletSpawn.position, Quaternion.identity);
            tracer.gameObject.SetActive(false);
            tracerPool.Enqueue(tracer);
        }
        for (int i = 0; i < 20; i++)
        {
            // Add tracers to the pool to prevent runtime instantiation
            var tracer = Instantiate(timeSlowTracerPrefab, bulletSpawn.position, Quaternion.identity);
            tracer.gameObject.SetActive(false);
            timeSlowTracerPool.Enqueue(tracer);
        }
        muzzleFlashLight.enabled = false;
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
        else if (_reqestedAttack && delayTimer + (shotDelay / 4) >= shotDelay)
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
        StartCoroutine(MuzzleFlash(lightTimer));
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
                    // Shoot out a timeslow indicator
                    BulletTracer tracer = timeSlowTracerPool.Dequeue();
                    tracer.gameObject.SetActive(true);
                    tracer.FireTracer(bulletSpawn.position, hit.point, tracerWidth);
                    timeSlowTracerPool.Enqueue(tracer);

                    // Wait to display most effects if time is slowed
                    // Add real tracers to the tracker pool
                    tracer = tracerPool.Dequeue();
                    tracer.gameObject.SetActive(true);
                    tracerTracker.Enqueue(new Tuple<BulletTracer, Vector3, Vector3, float, float>(tracer, bulletSpawn.position, hit.point, tracerWidth, tracerDecay));
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
                    // Shoot out a timeslow indicator
                    BulletTracer tracer = timeSlowTracerPool.Dequeue();
                    tracer.gameObject.SetActive(true);
                    tracer.FireTracer(bulletSpawn.position, maxDistance, tracerWidth);
                    timeSlowTracerPool.Enqueue(tracer);

                    // Wait to display most effects if time is slowed
                     tracer = tracerPool.Dequeue();
                    tracer.gameObject.SetActive(true);
                    tracerTracker.Enqueue(new Tuple<BulletTracer, Vector3, Vector3, float, float>(tracer, bulletSpawn.position, maxDistance, tracerWidth, tracerDecay));
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
            AudioManager.instance.PlayOmnicientSoundClip(sfxBank.GunshotSound(), 1f, true, true);
    }

    public IEnumerator FireTrackedTracers()
    {
        while (Timeslow.IsSlowed)
        {
            yield return null;
        }
        while (tracerTracker.Count > 0)
        {
            yield return new WaitForSeconds(1/15f);
            targetsShotInSlow = 0;
            var tracer = tracerTracker.Dequeue();
            tracer.Item1.FireTracer(tracer.Item2, tracer.Item3, tracer.Item4, tracer.Item5);
            // tuple is: bullettracer, its recorded spawn, its recorded end point, its recordedwidth, and its recorded decayrate

            AudioManager.instance.PlaySoundClipFromList(sfxBank.TracerSounds(), tracer.Item2, 1f, true, true);
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
    private IEnumerator MuzzleFlash(float time)
    {
        muzzleFlashLight.enabled = true;
        float initialIntensity = muzzleFlashLight.intensity;
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            muzzleFlashLight.intensity = Mathf.Lerp(initialIntensity, 0f, elapsedTime);
            yield return null;
        }
        muzzleFlashLight.enabled = false;
        muzzleFlashLight.intensity = initialIntensity;
    }
}
