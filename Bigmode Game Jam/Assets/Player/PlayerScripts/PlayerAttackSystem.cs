using UnityEngine;

public class PlayerAttackSystem : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    private bool _reqestedAttack = false;
    private PlayerInputActions _inputActions;

    public void updateInput(CharacterInput input)
    {
        _reqestedAttack = input.Attack;
    }

    private void Update()
    {
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
                hit.collider.gameObject.GetComponent<Destructible>()?.Kill();
                Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward, Color.red);
            }
        }
    }

}
