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
    ObjectPool<BulletTracer> tracerPool = new ObjectPool<BulletTracer>();
    ObjectPool<BulletTracer> timeSlowTracerPool = new ObjectPool<BulletTracer>();
    ObjectPool<ParticleSystem> impactParticles = new ObjectPool<ParticleSystem>();
    private Queue<Tuple<BulletTracer, Vector3, Vector3, float, float>> tracerTracker = new Queue<Tuple<BulletTracer, Vector3, Vector3, float, float>>();
    
    private void Start()
    {
        tracerPool.GeneratePool(20, tracerPrefab.gameObject);
        timeSlowTracerPool.GeneratePool(20, timeSlowTracerPrefab.gameObject);
        impactParticles.GeneratePool(20, impact.gameObject);
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
                    var tracer = timeSlowTracerPool.RequestAndReturnToPool(timeSlowTracerPrefab.gameObject);
                    tracer.gameObject.SetActive(true);
                    tracer.FireTracer(bulletSpawn.position, hit.point, tracerWidth);

                    // Wait to display most effects if time is slowed
                    // Add real tracers to the tracker pool
                    tracer = tracerPool.RequestAndReturnToPool(tracerPrefab.gameObject);
                    tracerTracker.Enqueue(new Tuple<BulletTracer, Vector3, Vector3, float, float>(tracer, bulletSpawn.position, hit.point, tracerWidth, tracerDecay));
                }
                else
                {
                    var tracer = tracerPool.RequestAndReturnToPool(tracerPrefab.gameObject);
                    tracer.gameObject.SetActive(true);
                    tracer.FireTracer(bulletSpawn.position, hit.point, tracerWidth, tracerDecay);
                    ActivateImpactParticles(hit);
                }

            }
            else
            {
                Vector3 maxDistance = playerCamera.transform.position + playerCamera.transform.forward * 100f;
                if (Timeslow.IsSlowed)
                {
                    // Shoot out a timeslow indicator
                    var tracer = timeSlowTracerPool.RequestAndReturnToPool(timeSlowTracerPrefab.gameObject);
                    tracer.gameObject.SetActive(true);
                    tracer.FireTracer(bulletSpawn.position, maxDistance, tracerWidth);

                    // Wait to display most effects if time is slowed
                    tracer = tracerPool.RequestAndReturnToPool(tracerPrefab.gameObject);
                    tracerTracker.Enqueue(new Tuple<BulletTracer, Vector3, Vector3, float, float>(tracer, bulletSpawn.position, maxDistance, tracerWidth, tracerDecay));
                }
                else
                {
                    var tracer = tracerPool.RequestAndReturnToPool(tracerPrefab.gameObject);
                    tracer.gameObject.SetActive(true);
                    tracer.FireTracer(bulletSpawn.position, maxDistance, tracerWidth, tracerDecay);
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
            tracer.Item1.gameObject.SetActive(true);
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
    private void ActivateImpactParticles(RaycastHit hit)
    {
        ParticleSystem impactFX = impactParticles.RequestAndReturnToPool(impact.gameObject);
        impactFX.gameObject.transform.position = hit.point;
        impactFX.gameObject.transform.forward = hit.normal;
        impactFX.Play();
    }
}
