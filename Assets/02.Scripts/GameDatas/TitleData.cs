using System.Collections.Generic;
using UnityEngine;
using System;

public class TitleData : Singleton<TitleData>
{
    public Dictionary<string, CharData> charDatas = new Dictionary<string, CharData>();
    public Dictionary<string, InteractData> interactDatas = new Dictionary<string, InteractData>();
    public Dictionary<string, DefineData> defineDatas = new Dictionary<string, DefineData>();

    const string path = "Data/";

    public void LoadTitleDatas()
    {
        LoadCharData("CharData");
        LoadInteractData("InteractData");
        LoadInteractData("DefineData");
    }

    void LoadCharData(string _path)
    {
        charDatas.Clear();

        var charDataList = JsonUtilityHelper.FromJson<CharData>(ResourceToJson(_path));
        foreach (var data in charDataList)
        {
            charDatas.Add(data.code, data);
        }
    }

    void LoadInteractData(string _path)
    {
        interactDatas.Clear();

        var interactDataList = JsonUtilityHelper.FromJson<InteractData>(ResourceToJson(_path));
        foreach (var data in interactDataList)
        {
            interactDatas.Add(data.code, data);
        }
    }

    void LoadDefineData(string _path)
    {
        defineDatas.Clear();

        var defineDataList = JsonUtilityHelper.FromJson<DefineData>(ResourceToJson(_path));
        foreach (var data in defineDataList)
        {
            defineDatas.Add(data.code, data);
        }
    }

    string ResourceToJson(string _path)
    {
        return Resources.Load<TextAsset>(path + _path).ToString();
    }
}

[Serializable]
public class CharData
{
    public string code;
    public float speed;
    public float cooltime;
    public float sight;
    public float range;
    public float ability;
    public string name;
    public string abilityName;
    public string ablityDescription;
    public string resource;
    public float skillvalue1;
    public float skillvalue2;
    public float skillvalue3;
    public float skillvalue4;
    public float skillvalue5;
}

[Serializable]
public class InteractData
{
    public string code;
    public int type;
    public float range;
    public float value1;
    public float value2;
    public float value3;
    public float value4;
    public float value5;
}

[Serializable]
public class DefineData
{
    public string code;
    public float value;
}

public class JsonUtilityHelper
{
    public static T[] FromJson<T>(string json)
    {
        string newJson = "";
        if (json[0] == '{')
        {
            newJson = json;
        }
        else
        {
            newJson = "{ \"array\": " + json + "}";
        }
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.array;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.array = array;
        return JsonUtility.ToJson(wrapper);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }
}