using UnityEngine;
using UnityEngine.UI;

public class ExitPanel : MonoBehaviour
{
    [SerializeField] Button CloseButton;
    [SerializeField] Button ExitButton;

    void Start()
    {
        CloseButton.onClick.AddListener(() =>
        {
            ClosePanel();
        });

        ExitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

    public void OpenPanel()
    {
        this.gameObject.SetActive(true);
    }

    public void ClosePanel()
    {
        this.gameObject.SetActive(false);
    }
}
