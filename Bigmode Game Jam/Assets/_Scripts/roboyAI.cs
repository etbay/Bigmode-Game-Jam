using UnityEngine;

public class roboyAI : MonoBehaviour
{
    Vector3 initialPos;
    Vector3 yPos;
    public Transform transform;
    public Vector3 direction;
    public float amplitude = 1f;
    bool disrupted = false; // so you can push them and they wont teleport back to their initial location

    void Start()
    {
        initialPos = transform.position;
    }

    void FixedUpdate()
    {
        if (!disrupted)
            move();
    }

    void move()
    {
        yPos = transform.position.y * Vector3.up;
        transform.position = (Mathf.Sin(Time.fixedTime) * amplitude * direction) + (initialPos.x * Vector3.right) + (initialPos.z * Vector3.right) + yPos;
    }
}
