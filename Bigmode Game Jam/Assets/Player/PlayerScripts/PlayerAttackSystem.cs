using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PlayerAttackSystem : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private AudioClip gunshot;
    private bool _reqestedAttack = false;
    private PlayerInputActions _inputActions;
    private int targetsShotInSlow = 0;

    public void updateInput(CharacterInput input)
    {
        _reqestedAttack = input.Attack;
    }

    private void Update()
    {
        if (targetsShotInSlow > 0 && !Timeslow.IsSlowed)
        {
            targetsShotInSlow = 0;
        }
        if (_reqestedAttack)
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit))
            {
                var target = hit.collider.gameObject;
                if (target.GetComponent<Destructible>() != null)
                {
                    if (Timeslow.IsSlowed)
                    {
                        targetsShotInSlow += 1;
                        Debug.Log(targetsShotInSlow);
                        target.GetComponent<Destructible>().Kill(targetsShotInSlow);
                    }
                    else
                    {
                        target.GetComponent<Destructible>().Kill(0);
                    }
                }
            }
            AudioManager.instance.PlayOmnicientSoundClip(gunshot, 1f, true, true);
        }
    }

}
