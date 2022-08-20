using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameStartPanel : MonoBehaviour
{
    [SerializeField] Text NameText;
    [SerializeField] Image CharImage;

    [SerializeField] InGameUICtrl inGameUICtrl;

    public void Setup(string _code)
    {
        this.gameObject.SetActive(true);

        NameText.text = TitleData.instance.charDatas[_code].name;
        CharImage.sprite = Resources.Load<Sprite>(TitleData.instance.charDatas[_code].resource);

        this.GetComponent<Image>().DOFade(0, 1.5f).SetEase(Ease.Linear).SetDelay(TitleData.instance.defineDatas["Info_Show_Time"].value)
            .OnComplete(() =>
            {
                this.gameObject.SetActive(false);
                inGameUICtrl.inGameUIState = InGameUIState.Play;
            });
        NameText.DOFade(0, 1.5f).SetEase(Ease.Linear).SetDelay(TitleData.instance.defineDatas["Info_Show_Time"].value);
        CharImage.DOFade(0, 1.5f).SetEase(Ease.Linear).SetDelay(TitleData.instance.defineDatas["Info_Show_Time"].value);
    }
}
