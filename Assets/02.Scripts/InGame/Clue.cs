using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class Clue : MonoBehaviour
{
    [SerializeField] public GameObject CluePanel;
    PhotonView PV;

    public string clueInfo;

    void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    public void Interactive()
    {
        if(clueInfo == null)
        {
            clueInfo = "꽝일세..";
        }

        CluePanel.SetActive(true);

        CluePanel.GetComponent<Image>().DOFade(0, 1.5f).SetEase(Ease.Linear).SetDelay(TitleData.instance.defineDatas["Info_Show_Time"].value);

        CluePanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = clueInfo;

        CluePanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().DOFade(0, 1.5f).SetEase(Ease.Linear).SetDelay(TitleData.instance.defineDatas["Info_Show_Time"].value)
            .OnComplete(() =>
            {
                CluePanel.SetActive(false);
                PhotonNetwork.Destroy(PV);
            });
    }
}
