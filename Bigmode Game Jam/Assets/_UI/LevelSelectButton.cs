using UnityEngine;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour
{
    [SerializeField] private GameObject popup;
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(TogglePopup);
        popup.SetActive(false);
    }
    private void TogglePopup()
    {
        popup.SetActive(!popup.activeInHierarchy);
    }
}
