using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] private string displayText;

    void OnTriggerEnter(Collider other) // Use OnTriggerEnter2D for 2D games
    {
        if (other.tag == "Player")
        {
            Debug.Log(displayText);
        }
    }
}
