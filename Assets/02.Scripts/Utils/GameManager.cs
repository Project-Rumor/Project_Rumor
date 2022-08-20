using UnityEngine;
using Photon.Pun;
using System;
using System.Linq;

public class GameManager : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
