using System.Collections;
using System.Timers;
using UnityEngine;

public class GunEffects : MonoBehaviour
{
    [SerializeField] Transform gunModel;
    [SerializeField] Vector3 recoilDirection;
    [SerializeField] float recoilMultiplier = 1f;
    [SerializeField] float recoilTimer = .1f;
    public void Recoil()
    {
        
    }    
}
