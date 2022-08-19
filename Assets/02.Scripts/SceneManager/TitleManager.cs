using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviourPunCallbacks
{
    [SerializeField] InputField nickInput;
    [SerializeField] Button enterButton;
    [SerializeField] GameObject loadingImage;

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

        loadingImage.SetActive(false);
        Color color = new Color(255f, 255f, 255f, 0f) / 255f;
        loadingImage.GetComponent<Image>().color = color;
    }

    public void EnterBtnEvent()
    {
        if (nickInput.text == "")
        {
            Debug.LogWarning("Null Nickname");

            return;
        }

        loadingImage.SetActive(true);
        loadingImage.GetComponent<Image>().DOFade(1f, 1f).SetEase(Ease.OutQuad).SetLoops(-1, LoopType.Yoyo);

        PhotonNetwork.ConnectUsingSettings();
    }

    //Photon Override
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.LocalPlayer.NickName = nickInput.text;

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        StartCoroutine("LoadingCo");
    }
    //

    IEnumerator LoadingCo()
    {
        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene("Scene_01_Lobby_R");
    }
}
