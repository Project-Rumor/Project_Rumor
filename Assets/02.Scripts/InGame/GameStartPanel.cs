using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class GameStartPanel : MonoBehaviour
{
    [SerializeField] GameObject NameTextPanel;
    [SerializeField] TextMeshProUGUI NameText;
    //[SerializeField] Image CharImage;

    [SerializeField] InGameUICtrl inGameUICtrl;

    public void Setup(string _code)
    {
        NameTextPanel.gameObject.SetActive(true);
        //CharImage.gameObject.SetActive(true);

        NameText.text = "당신의 혼은 " + TitleData.instance.charDatas[_code].name + "입니다..";
        NameText.text = "\n당신의 목표는 " + TitleData.instance.charDatas[InGameManager.instance.MyChar.target].name + "입니다.. 끝까지 살아남으세요..";
        //CharImage.sprite = Resources.Load<Sprite>(TitleData.instance.charDatas[_code].resource);

        this.GetComponent<Image>().DOFade(0, 1.5f).SetEase(Ease.Linear).SetDelay(TitleData.instance.defineDatas["Info_Show_Time"].value)
            .OnComplete(() =>
            {
                this.gameObject.SetActive(false);
                inGameUICtrl.inGameUIState = InGameUIState.Play;
            });
        SoundManager.instance.PlaySFX("GameStart");
        NameTextPanel.GetComponent<Image>().DOFade(0, 1.5f).SetEase(Ease.Linear).SetDelay(TitleData.instance.defineDatas["Info_Show_Time"].value);
        NameText.DOFade(0, 1.5f).SetEase(Ease.Linear).SetDelay(TitleData.instance.defineDatas["Info_Show_Time"].value);
        //CharImage.DOFade(0, 1.5f).SetEase(Ease.Linear).SetDelay(TitleData.instance.defineDatas["Info_Show_Time"].value);

        SoundManager.instance.PlayBGM("InGame");
    }
}
