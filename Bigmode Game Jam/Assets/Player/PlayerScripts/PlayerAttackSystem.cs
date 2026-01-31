using UnityEngine;

public class PlayerAttackSystem : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    private bool _reqestedAttack = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCamera = gameObject.GetComponentInParent<Camera>();
        
    }

    public void updateInput(CharacterInput input)
    {
        _reqestedAttack = input.Attack;
    }

    

    private void Update()
    {
        if (_reqestedAttack)
        {
            //Debug.Log("Attack requested");
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            if (Physics.Raycast(ray,out var hitInfo, 100))
            {
                Renderer renderer = hitInfo.collider.GetComponent<Renderer>();
                //HealthSystem healthSystem;
                //if (hitInfo.collider.TryGetComponent<HealthSystem>(out healthSystem))
                //{
                //    healthSystem?.onHit?.Invoke();
                //}
                Debug.Log($"Hit: { hitInfo.collider.name} || Texture: {renderer.material}");
            }
        }
    }

}
