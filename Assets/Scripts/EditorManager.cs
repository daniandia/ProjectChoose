using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OptionsLayouts {
    public UnityEngine.UI.Dropdown dList;
    public UnityEngine.UI.InputField text;
    public UnityEngine.UI.Toggle selector;
}

public class EditorManager : MonoBehaviour
{
    public UnityEngine.UI.Dropdown ddList;
    public UnityEngine.UI.InputField mainText;
    public UnityEngine.UI.InputField nameText;

    //options
    public OptionsLayouts[] answers;
    JSONReader jsonReader;
    EventList eventList ;
    // Start is called before the first frame update
    void Start()
    {
        jsonReader = GetComponent<JSONReader>();
        jsonReader.LoadGameJSON();
        eventList = jsonReader.eventList;
        InitEventDDL();
        DoSelectOptionDDL();
    }

    void InitEventDDL()
    {
        List<string> optList = new List<string>();
        ddList.ClearOptions();
        for(int i = 0; i<eventList.SerializableEvent.Count; i++)
        {
            optList.Add(eventList.SerializableEvent[i].name);
        }
        optList.Add(" END ");
        ddList.AddOptions(optList);
        for(int i = 0; i<answers.Length; i++)
        {
            answers[i].dList.ClearOptions();
            answers[i].dList.AddOptions(optList);
        }
    }

    public void DoSelectOptionDDL()
    {
        int tOpt = ddList.value;
        if (tOpt >= eventList.SerializableEvent.Count)
            return;
        SerializableEvent tempEvent = eventList.SerializableEvent[tOpt];
        mainText.text = tempEvent.text;
        nameText.text = tempEvent.name;
        int i = 0;
        for ( ; i<tempEvent.SerializableAnswer.Count; i++)
        {
            SerializableAnswer tAnswer = tempEvent.SerializableAnswer[i];
            answers[i].text.text = tAnswer.text;
            answers[i].dList.value = tAnswer.next_event;
            answers[i].selector.isOn = true;
        }
        for(;i<answers.Length;i++)
            answers[i].selector.isOn = false;
    }
  
   public void AddEventAsNew()
    {
        SerializableEvent eventToSave = new SerializableEvent();
        eventToSave.id = eventList.SerializableEvent.Count;
        eventToSave.text = mainText.text;
        eventToSave.name = nameText.text;
        for(int i = 0; i< answers.Length; i++)
        {
            if (answers[i].selector.isOn)
            {
                SerializableAnswer tAnswer = new SerializableAnswer();
                tAnswer.text = answers[i].text.text;
                tAnswer.next_event = answers[i].dList.value;
                eventToSave.SerializableAnswer.Add(tAnswer);
                //PROPERTIES MISSING
            }
        }
        eventList.SerializableEvent.Add(eventToSave);
        InitEventDDL();
    }

    public void SaveEvent()
    {
        SerializableEvent eventToSave = new SerializableEvent();
        eventToSave.id = eventList.SerializableEvent.Count;
        eventToSave.text = mainText.text;
        eventToSave.name = nameText.text;
        for (int i = 0; i < answers.Length; i++)
        {
            if (answers[i].selector.isOn)
            {
                SerializableAnswer tAnswer = new SerializableAnswer();
                tAnswer.text = answers[i].text.text;
                tAnswer.next_event = answers[i].dList.value;
                eventToSave.SerializableAnswer.Add(tAnswer);
                //PROPERTIES MISSING
            }
        }
        eventList.SerializableEvent[ddList.value] = eventToSave;
        InitEventDDL();
    }

    public void DeleteEvent()
    {

        ResetAllReferences(ddList.value);
        eventList.SerializableEvent.Remove(eventList.SerializableEvent[ddList.value]);
        InitEventDDL();
        DoSelectOptionDDL();
    }

    public void ResetAllReferences(int deletedReference)
    {
        for(int i=0;i< eventList.SerializableEvent.Count; i++)
        {
            for(int j = 0; j < eventList.SerializableEvent[i].SerializableAnswer.Count; j++)
            {
                if (eventList.SerializableEvent[i].SerializableAnswer[j].next_event > deletedReference)
                    eventList.SerializableEvent[i].SerializableAnswer[j].next_event--;
                else if (eventList.SerializableEvent[i].SerializableAnswer[j].next_event == deletedReference)
                    eventList.SerializableEvent[i].SerializableAnswer[j].next_event = eventList.SerializableEvent.Count - 1;
            }
        }
    }
    public void ExportJSON()
    {
        jsonReader.SaveJSON();
    }
    public void ReloadJSON()
    {

    }
}
