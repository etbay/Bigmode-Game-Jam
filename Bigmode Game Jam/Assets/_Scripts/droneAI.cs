using UnityEngine;

public class droneAI : MonoBehaviour
{
    Vector3 initialPos;
    public Transform transform;
    public Vector3 direction;
    public float amplitude = 1f;

    void Start() {
        initialPos = transform.position;
    }

    void Update()
    {
        move();
    }

    void move()
    {
        transform.position = initialPos + Mathf.Sin(Time.time) * amplitude * direction;
    }
}
