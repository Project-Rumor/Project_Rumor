using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CyclePanel : MonoBehaviour
{
    [SerializeField] List<Image> CycleSpriteRenderer = new List<Image>();
    [SerializeField] List<Sprite> sprites = new List<Sprite>();

    public void InitializeCycle()
    {
        for (int i = 0; i < CycleSpriteRenderer.Count; i++)
            CycleSpriteRenderer[i].sprite = null;

        for(int i = 0; i < InGameManager.instance.Cycle.Count; i++)
        {
            CycleSpriteRenderer[i].sprite = sprites.Find(x => x.name == InGameManager.instance.Cycle[i]);
        }
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
