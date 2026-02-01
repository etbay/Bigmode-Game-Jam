using UnityEngine;

public class Gun : MonoBehaviour
{
    public Camera cam;
    // Update is called once per frame
    void Update()
    {
        
    }

    void Shoot()
    {
        // RaycastHit hit;
        // if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
        // {
        //     hit.collider.gameObject.GetComponent<Destructible>()?.Kill();
        //     Debug.DrawRay(cam.transform.position, cam.transform.forward, Color.red);
        // }
    }
}
