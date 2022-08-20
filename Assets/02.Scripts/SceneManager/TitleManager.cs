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

        LogoImage.transform.DOScale(new Vector2(0.5f, 0.5f), 4f).From().SetEase(Ease.OutBack);
        LogoImage.DOFade(1f, 5f).SetEase(Ease.Linear);
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

        //PhotonNetwork.JoinLobby();
        StartCoroutine("LoadingCo");
    }

    //public override void OnJoinedLobby()
    //{
    //    StartCoroutine("LoadingCo");
    //}
    //

    IEnumerator LoadingCo()
    {
        GameData.instance.isTitle = true;

        yield return new WaitForSeconds(1f);

        NetworkManager.instance.MoveScene(1);
    }
}
