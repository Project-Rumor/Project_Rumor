using UnityEngine;

public class GuidePanel : MonoBehaviour
{
    public void OpenPanel()
    {
        this.gameObject.SetActive(true);
    }

    public void ClosePanel()
    {
        this.gameObject.SetActive(false);
    }
}
