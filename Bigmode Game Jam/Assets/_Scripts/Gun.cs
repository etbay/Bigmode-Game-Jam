using UnityEngine;

public class Gun : MonoBehaviour
{
    public Camera cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (true)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
        {
            // if (hit.collider.gameObject.GetComponent<Destructible>() != null)
            // {
            //     Debug.Log(hit.transform.name);
            // }
            hit.collider.gameObject.GetComponent<Destructible>()?.Kill();
            Debug.DrawRay(cam.transform.position, cam.transform.forward, Color.red);
        }
    }
}
