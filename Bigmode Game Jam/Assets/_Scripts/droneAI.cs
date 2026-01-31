using UnityEngine;

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
        transform.position = initialPos + Mathf.Sin(Time.fixedTime) * amplitude * direction;
    }
}
