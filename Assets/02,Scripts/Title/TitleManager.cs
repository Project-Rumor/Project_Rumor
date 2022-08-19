using UnityEngine;

public class TitleManager : MonoBehaviour
{
    void Start()
    {
        TitleData.instance.LoadTitleDatas();
    }
}
