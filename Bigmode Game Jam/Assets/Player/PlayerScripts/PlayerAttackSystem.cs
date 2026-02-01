using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PlayerAttackSystem : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
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
            //Debug.Log("Attack requested");
            // Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            // if (Physics.Raycast(ray,out var hitInfo, 100))
            // {
            //     Renderer renderer = hitInfo.collider.GetComponent<Renderer>();
            //     //HealthSystem healthSystem;
            //     //if (hitInfo.collider.TryGetComponent<HealthSystem>(out healthSystem))
            //     //{
            //     //    healthSystem?.onHit?.Invoke();
            //     //}
            //     Debug.Log($"Hit: { hitInfo.collider.name} || Texture: {renderer.material}");
            // }
            
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
                    Player.SlickValue += 0.5f;
                } 
                Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward, Color.red);
            }
        }
    }

}
