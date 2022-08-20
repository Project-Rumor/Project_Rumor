//using UnityEngine;
//using Photon.Pun;
//using Boo.Lang;
//using System;
//using System.Linq;

//public class GameManager : MonoBehaviour
//{
//    public string[] SpiritNames = { "Gumiho", "Doggabi", "Reaper", "Dark", "Dungapjwi", "Emugi" };

//    bool initialized;

//    List<string> Cycle;

//    void Awake()
//    {
//        DontDestroyOnLoad(this.gameObject);
//    }
//    void Start()
//    {
//        Cycle = new List<string>();

//        foreach (string s in SpiritNames)
//            Cycle.Push(s);

//        initialized = false;

//        GameInitialize();
//    }

//    public void GameInitialize()
//    {
//        Debug.Log("Initialize");

//        var rnd = new System.Random();
//        var randorder = Cycle.OrderBy(item => rnd.Next());

//        List<string> spirtsTmp = new List<string>();

//        foreach(var cycle in randorder)
//        {
//            spirtsTmp.Push(cycle);
//        }

//        Cycle = spirtsTmp;
//    }
//}
