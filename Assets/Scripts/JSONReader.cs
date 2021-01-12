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
public class SerializableAnswer
{
    public string name;
    public string text;
    public int icon;
    public int next_event;
    public List<SerializableStat> SerializableStat;
    public SerializableAnswer()
    {
        SerializableStat = new List<SerializableStat>();
        name = "none";
        text = "none";
    }
}
[System.Serializable]
public class SerializableEvent {
    public string name;
    public int id;
    public int image_id;
    public string text;
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

    public string LoadResourceTextfile(string path)
    {
    #if UNITY_ANDROID
            string filePath = Application.persistentDataPath+"/Json/" + path.Replace(".json", "");
    #endif
    #if UNITY_STANDALONE
        string filePath = "Json/" + path.Replace(".json", "");
    #endif
        Debug.Log(filePath);
        TextAsset targetFile = Resources.Load<TextAsset>(filePath);
        Debug.Log(targetFile.text);
        return targetFile.text;
    }

    public void LoadGameJSON() {
        try
        {
            eventList = JsonUtility.FromJson<EventList>(LoadResourceTextfile(jsonName));
        }catch(Exception ex)
        {
            Debug.Log(ex.Message);
            return;
        }
    }
    // Update is called once per frame
    public void SaveJSON()
    {
        string tempText = JsonUtility.ToJson(eventList,true);
        Debug.Log("//////////SAVED JSON////////////////");
        Debug.Log(tempText);
        Debug.Log("////////////////////////////////////");
    #if UNITY_ANDROID
        string destination = Application.persistentDataPath+"/Json/" + jsonName + ".json";
    #endif
    #if UNITY_STANDALONE
        string destination =  "Json/" + jsonName + ".json";
    #endif
        Debug.Log(destination);
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
}
