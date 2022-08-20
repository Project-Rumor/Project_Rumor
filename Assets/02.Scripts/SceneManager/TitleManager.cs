using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Image LogoImage;
    [SerializeField] InputField nickInput;
    [SerializeField] Button enterButton;

    void Awake()
    {
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        TitleData.instance.LoadTitleDatas();

        Setup();
    }

    void Setup()
    {
        enterButton.onClick.AddListener(() =>
        {
            EnterBtnEvent();
        });

        LogoImage.DOFade(1f, 6.5f).SetEase(Ease.InQuad);
    }

    public void EnterBtnEvent()
    {
        if (nickInput.text == "")
        {
            Debug.LogWarning("Null Nickname");

            return;
        }

        PhotonNetwork.ConnectUsingSettings();
    }

    //Photon Override
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.LocalPlayer.NickName = nickInput.text;

        Eff();
        //PhotonNetwork.JoinLobby();
        //StartCoroutine("LoadingCo");
    }

    //public override void OnJoinedLobby()
    //{
    //    StartCoroutine("LoadingCo");
    //}
    //

    void Eff()
    {
        GameData.instance.isTitle = true;

        LogoImage.DOFade(0, 2f).SetEase(Ease.OutQuad);

        nickInput.image.DOFade(0, 2f).SetEase(Ease.OutQuad);
        nickInput.transform.GetChild(0).GetComponent<Image>().DOFade(0, 2f).SetEase(Ease.OutQuad);
        nickInput.transform.GetChild(1).GetComponent<Text>().DOFade(0, 2f).SetEase(Ease.OutQuad);
        nickInput.transform.GetChild(2).GetComponent<Text>().DOFade(0, 2f).SetEase(Ease.OutQuad);

        enterButton.image.DOFade(0, 2f).SetEase(Ease.OutQuad);
        enterButton.transform.GetChild(0).GetComponent<Image>().DOFade(0, 2f).SetEase(Ease.OutQuad);
        enterButton.transform.GetChild(1).GetComponent<Text>().DOFade(0, 2f).SetEase(Ease.OutQuad);

        nickInput.transform.DOLocalMoveY(0, 2f).SetEase(Ease.OutQuad);
        enterButton.transform.DOLocalMoveY(0, 2f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            NetworkManager.instance.MoveScene(1);
        });
    }
}
