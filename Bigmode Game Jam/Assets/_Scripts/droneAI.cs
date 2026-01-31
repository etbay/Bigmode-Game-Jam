using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class DroneAI : MonoBehaviour
{
    Vector3 initialPos;
    public Transform transform;
    public Vector3 direction;
    public float amplitude = 1f;

    void Start() {
        initialPos = transform.position;
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        Vector3 newPos = new Vector3(0, 0, 0);
        if (direction.x != 0)
        {
            newPos += (initialPos.x * Vector3.right) + Mathf.Sin(Time.fixedTime) * amplitude * direction.x * Vector3.right;
        }
        else
        {
            newPos += transform.position.x * Vector3.right;
        }

        if (direction.y != 0)
        {
            newPos += (initialPos.y * Vector3.up) + Mathf.Sin(Time.fixedTime) * amplitude * direction.y * Vector3.up;
        }
        else
        {
            newPos += transform.position.y * Vector3.up;
        }

        if (direction.z != 0)
        {
            newPos += (initialPos.z * Vector3.forward) + Mathf.Sin(Time.fixedTime) * amplitude * direction.z * Vector3.forward;
        }
        else
        {
            newPos += transform.position.z * Vector3.forward;
        }
        
        transform.position = newPos;
    }
}
