using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetZone : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            AudioManager.instance.PlayPersistentSoundClip(AudioManager.instance.restart, 1f, false, false);
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }
    }
}
