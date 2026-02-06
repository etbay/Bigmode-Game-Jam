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
        StartCoroutine(recoil());
    }

    private IEnumerator recoil()
    {
        float elapsedTime = 0;
        while (elapsedTime < recoilTimer / 2)
        {
            gunModel.position = gunModel.position + Vector3.Lerp(Vector3.zero, recoilDirection * recoilMultiplier, elapsedTime);
            yield return null;
        }
        while (elapsedTime < recoilTimer)
        {
            gunModel.position = gunModel.position + Vector3.Lerp(recoilDirection * recoilMultiplier, Vector3.zero, elapsedTime);
            yield return null;
        }
    }
    
}
