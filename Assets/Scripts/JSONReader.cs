using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
[System.Serializable]
public struct Property
{
    public string property_name;
    public int initial_value;
    public string description;
}
[System.Serializable]
public struct PropertyList
{
    public List<Property> Property;
}
[System.Serializable]
public struct SerializableStat {
    public int stat_id; //Reference to property ID
    public float stat_value;
    public float odds;
}
[System.Serializable]
public enum answerType
{
    SIMPLE,
    START_POINT,
    END_NODE
}
[System.Serializable]
public class SerializableAnswer
{
    public string name;
    public string text;
    public int icon;
    public int next_event;
    public SerializableStat blockCondition;
    public List<SerializableStat> SerializableStat;
    public SerializableAnswer()
    {
        SerializableStat = new List<SerializableStat>();
        name = "none";
        text = "none";
        blockCondition.stat_id = -1;
    }
}
[System.Serializable]
public class SerializableEvent {
    public string name;
    public int id;
    public int image_id;
    public string text;
    public int type;
    public List<SerializableAnswer> SerializableAnswer;

    public SerializableEvent()
    {
        SerializableAnswer = new List<SerializableAnswer>();
        name = "none";
        text = "none";
    }
}

[System.Serializable]
public class EventList {
    public List<SerializableEvent> SerializableEvent = new List<SerializableEvent>();
}

public class JSONReader : MonoBehaviour
{
    public string jsonName;
    public EventList eventList = new EventList();
    public PropertyList propList = new PropertyList();
    public TextAsset finalJSON;
    public TextAsset propsFINALJSON;
    public string LoadResourceTextfile(string path, bool ismain = true)
    {
        TextAsset targetFile;
#if UNITY_ANDROID
        string filePath = Application.persistentDataPath+"/Json/" + path.Replace(".json", "");
        if (ismain)
            targetFile = finalJSON;
        else
            targetFile = propsFINALJSON;
#endif
#if UNITY_STANDALONE
        string filePath = "Json/" + path.Replace(".json", "");
        Debug.Log("LOAD PATH : " + filePath);
        = Resources.Load<TextAsset>(filePath);
#endif
        Debug.Log(targetFile.text);
        return targetFile.text;
    }

    public void LoadGameJSON() {
        try
        {
            eventList = JsonUtility.FromJson<EventList>(LoadResourceTextfile(jsonName));
        } catch (Exception ex)
        {
            Debug.Log("EVENTS JSON : " + ex.Message);
            return;
        }
    }
    public void LoadStatJSON() {
        try
        {
            propList = JsonUtility.FromJson<PropertyList>(LoadResourceTextfile("PROPS_" + jsonName,false));
        }
        catch (Exception ex)
        {
            Debug.Log("PROPERTIES JSON : " + ex.Message);
            return;
        }
    }
    // Update is called once per frame
    public void SaveJSON()
    {
        string tempText = JsonUtility.ToJson(eventList, true);
        Debug.Log("//////////SAVED JSON////////////////");
        Debug.Log(tempText);
        Debug.Log("////////////////////////////////////");
#if UNITY_ANDROID
        string destination = Application.persistentDataPath+"/Json/" + jsonName + ".json";
#endif
#if UNITY_STANDALONE
        string destination = "Assets/Resources/Json/" + jsonName + ".json";
#endif
        Debug.Log("SAVE PATH EVENT JSON " + destination);
        File.WriteAllText(destination, tempText);
    }
    public void SavePropsJSON()
    {
        string tempText = JsonUtility.ToJson(propList, true);
        Debug.Log("//////////SAVED PROPS JSON////////////////");
        Debug.Log(tempText);
        Debug.Log("////////////////////////////////////");
#if UNITY_ANDROID
        string destination = Application.persistentDataPath+"/Json/PROPS_" + jsonName + ".json";
#endif
#if UNITY_STANDALONE
        string destination = "Assets/Resources/Json/PROPS_" + jsonName + ".json";
#endif
        Debug.Log("SAVE PATH PROP JSON " + destination);
        File.WriteAllText(destination, tempText);
    }
    void PrintDebugEvent()
    {
        Debug.Log("////////////////////////////////////////////");
        foreach (SerializableEvent tEvent in eventList.SerializableEvent)
            print(tEvent.text);
        Debug.Log("////////////////////////////////////////////");

    }

    public SerializableEvent GetActualEvent(int eventId)
    {
        return eventList.SerializableEvent[eventId];
    }

    public SerializableEvent GetFirstEvent(){
        Debug.Log("LOAD FIRST JSON EVENT");
        for (int i = 0; i < eventList.SerializableEvent.Count; i++)
            if (eventList.SerializableEvent[i].type == (int)(answerType.START_POINT))
                return eventList.SerializableEvent[i];
        return eventList.SerializableEvent[0];
    }
}
