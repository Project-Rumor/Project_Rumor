using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CyclePanel : MonoBehaviour
{
    [SerializeField] List<Image> CycleSpriteRenderer = new List<Image>();
    [SerializeField] List<Sprite> sprites = new List<Sprite>();

    [SerializeField] Image MySpirit;
    [SerializeField] Image targetSpirit;

    public void InitializeCycle()
    {
        Debug.Log("initialize Cycle Panel");

        for (int i = 0; i < CycleSpriteRenderer.Count; i++)
            CycleSpriteRenderer[i].color = new Color(1f, 1f, 1f, 0f);

        for(int i = 0; i < InGameManager.instance.Cycle.Count; i++)
        {
            CycleSpriteRenderer[i].sprite = sprites.Find(x => x.name == InGameManager.instance.Cycle[i]);

            CycleSpriteRenderer[i].color = new Color(1f, 1f, 1f, 1f);
        }

        MySpirit.sprite = sprites.Find(x => x.name == InGameManager.instance.MyChar.chardata.code);
        targetSpirit.sprite = sprites.Find(x => x.name == InGameManager.instance.MyChar.target);
    }

    public void UpdateTarget()
    {
        targetSpirit.sprite = sprites.Find(x => x.name == InGameManager.instance.MyChar.target);
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
