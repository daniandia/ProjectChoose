using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public struct SerializableStat {
    public int stat_id;
    public float stat_value;
}
[System.Serializable]
public class SerializableAnswer
{
    public string text;
    public int icon;
    public int next_event;
    public List<SerializableStat> SerializableStat;
    public SerializableAnswer()
    {
        SerializableStat = new List<SerializableStat>();
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
    }
}

[System.Serializable]
public class EventList {
    public List<SerializableEvent> SerializableEvent = new List<SerializableEvent>();
}

public class JSONReader : MonoBehaviour
{
    //public TextAsset jsonToLoad;
    public string jsonName;
    public EventList eventList = new EventList();
    // Start is called before the first frame update

    public string LoadResourceTextfile(string path)
    {

        string filePath = "Json/" + path.Replace(".json", "");
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
        //Debug.Log(jsonToLoad.text);
        //PrintDebugEvent();
    }
    // Update is called once per frame
    public void SaveJSON()
    {
        string tempText = JsonUtility.ToJson(eventList,true);
        Debug.Log("//////////SAVED JSON////////////////");
        Debug.Log(tempText);
        Debug.Log("////////////////////////////////////");
        FileStream file;
        string destination = "Assets/Resources/Json/" + jsonName + "_B.json";
        //if (File.Exists(destination)) 
        Debug.Log(destination);
        //file = File.OpenWrite(destination);
        //else file = File.Create(destination);
        File.WriteAllText(destination, tempText);
       // BinaryFormatter bf = new BinaryFormatter();
       // bf.Serialize(file, tempText);
        
        //file.Close();
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
