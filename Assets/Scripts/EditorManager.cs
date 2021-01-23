using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]


public class EditorManager : MonoBehaviour
{
    [Header("Panel references")]
    public GameObject eventsPanel;
    public GameObject propertiesPanel;
    public UnityEngine.UI.Button eventsButton;
    public UnityEngine.UI.Button propsButton;

    [Header("Event layout references")]
    public UnityEngine.UI.Dropdown ddList;
    public UnityEngine.UI.InputField mainText;
    public UnityEngine.UI.InputField nameText;
    public UnityEngine.UI.Dropdown eventTypeDDL;

    [Header("Answer layout references")]
    public UnityEngine.UI.Dropdown answersDDL;
    public UnityEngine.UI.InputField answerName;
    public UnityEngine.UI.InputField answerText;
    public UnityEngine.UI.Dropdown answerLinkDDL;
    public UnityEngine.UI.Dropdown answerBlockDDL;
    public UnityEngine.UI.InputField answerBlockText;

    [Header("Property layout references")]
    public UnityEngine.UI.Dropdown propertiesDDL;
    public UnityEngine.UI.Text propsListText;
    public UnityEngine.UI.Button clearPropsBtn;

    [Header("Property input references")]
    public UnityEngine.UI.InputField propValue;
    public UnityEngine.UI.InputField propOdds;

    //Internal variables
    JSONReader jsonReader; 
    EventList eventList ;  //The event structure to read/write
    public SerializableEvent actualEvent;
    // Start is called before the first frame update
    void Start()
    {
        jsonReader = GetComponent<JSONReader>();
        // Reads and fills the eventList structure
        jsonReader.LoadGameJSON();
        jsonReader.LoadStatJSON();
        eventList = jsonReader.eventList;
        InitEventDDL();
        DoSelectOptionDDL();
        ShowEventPanel();
        InitAnswerTypeDDL();
        FillBlockerDDL();
    }
    //Init answerTypeDDL
    void InitAnswerTypeDDL()
    {
        eventTypeDDL.ClearOptions();

        List<string> optList = new List<string>();
        foreach (answerType tType in System.Enum.GetValues(typeof(answerType)))
        {
            optList.Add(tType.ToString());
        }
        eventTypeDDL.AddOptions(optList);
    }
    /// <EVENT RELATED CODE>
    //Initialise the Event Drop Down List
    void InitEventDDL()
    {
        List<string> optList = new List<string>();
        ddList.ClearOptions();
        for(int i = 0; i<eventList.SerializableEvent.Count; i++)
        {
            optList.Add(eventList.SerializableEvent[i].name);
        }
        optList.Add(" NEW ");
        ddList.AddOptions(optList);
        
    }
    //Selects an Event from the DDL and fills all the panels
    public void DoSelectOptionDDL()
    {
        int tOpt = ddList.value;
        if (tOpt >= eventList.SerializableEvent.Count)
        {
            actualEvent = new SerializableEvent();
            mainText.text = "";
            nameText.text = "NEW";
            actualEvent.id = eventList.SerializableEvent.Count;
            eventTypeDDL.value = 0;
            FillAnswerPanel(actualEvent.SerializableAnswer);
            return;
        }
         actualEvent = eventList.SerializableEvent[tOpt];
        mainText.text = actualEvent.text;
        nameText.text = actualEvent.name;
        eventTypeDDL.value = actualEvent.type;
        FillAnswerPanel(actualEvent.SerializableAnswer);
    }
    //Initialises the answer panel suing the selected reference from event DDL
   void FillAnswerPanel(List<SerializableAnswer> answers)
    {
        answersDDL.ClearOptions();
        List<string> answerNameList = new List<string>();
        for (int i = 0; i < answers.Count; i++)
        {
            answerNameList.Add(answers[i].name);
        }
        answerNameList.Add(" NEW ");
        answersDDL.AddOptions(answerNameList);
        NewAnswerSelected();
    }
    //Saves the creted event as a new one
   public void AddEventAsNew()
    {
        if (UnityEditor.EditorUtility.DisplayDialog("Event  created : " + nameText.text, mainText.text, "OK", "Cancel"))
        {
            actualEvent.id = eventList.SerializableEvent.Count;
            actualEvent.text = mainText.text;
            actualEvent.name = nameText.text;
            actualEvent.type = eventTypeDDL.value;
            //AddAnswersToList();
            eventList.SerializableEvent.Add(actualEvent);
            InitEventDDL();
        }
    }
    //Saves the actual event (does not create new events)

    public void SaveEvent()
    {
        if (actualEvent.id == eventList.SerializableEvent.Count)
            AddEventAsNew();
        else if (UnityEditor.EditorUtility.DisplayDialog("Event  saved : " + nameText.text, mainText.text, "OK", "Cancel"))
        {
            //SerializableEvent eventToSave = new SerializableEvent();
            //actualEvent.id = eventList.SerializableEvent.Count;
            actualEvent.text = mainText.text;
            actualEvent.name = nameText.text;
            actualEvent.type = eventTypeDDL.value;
            //AddAnswersToList();
            eventList.SerializableEvent[ddList.value] = actualEvent;

        }
        InitEventDDL();
    }
    //Deletes the current event and updates the references to this one in other events
    public void DeleteEvent()
    {
        if (UnityEditor.EditorUtility.DisplayDialog("DELETE event: " + nameText.text, "The current event and ALL its references will be deleted", "OK", "Cancel"))
        {
            ResetAllReferences(ddList.value);
            eventList.SerializableEvent.Remove(eventList.SerializableEvent[ddList.value]);
            InitEventDDL();
            DoSelectOptionDDL();
        }
    }
    //Updates the references to the deleted event
    //CRITICAL: This solution might not work on every case, need to test
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

    /// <ANSWER RELATED CODE>
    //Selects a new answer from the DDL and updates the editor content
    public void NewAnswerSelected()
    {
        if (answersDDL.value >= (answersDDL.options.Count - 1)){
            answerName.text = "NEW NAME";
            answerText.text = "NEW TEXT";
            answerBlockDDL.value = 0;
            answerBlockText.text = "0";
            FillAnswerLinkDDL();
            return;
        }
        answerName.text = actualEvent.SerializableAnswer[answersDDL.value].name;
        answerText.text = actualEvent.SerializableAnswer[answersDDL.value].text;
        FillAnswerLinkDDL();
        answerLinkDDL.value = actualEvent.SerializableAnswer[answersDDL.value].next_event;
        answerBlockDDL.value = actualEvent.SerializableAnswer[answersDDL.value].blockCondition.stat_id + 1;
        answerBlockText.text = "" +actualEvent.SerializableAnswer[answersDDL.value].blockCondition.stat_value;
    }
    //Fills the destiny DDL with the events in the event structure (can be heavily optimized in terms of performace)
    void FillAnswerLinkDDL()
    {
        answerLinkDDL.ClearOptions();
        List<string> optList = new List<string>();
        for (int i = 0; i < eventList.SerializableEvent.Count; i++)
        {
            optList.Add(eventList.SerializableEvent[i].name);
        }
        answerLinkDDL.AddOptions(optList);
        FillPropertiesPanel();
        FillPropertiesDLL();
    }
    //Checks if the name of the answer exits and saves it as new if necessary
    public void SaveCurrentAnswer()
    {      
            for (int i = 0; i < actualEvent.SerializableAnswer.Count; i++)
            {
                if (actualEvent.SerializableAnswer[i].name == answerName.text)
                {
                 if (UnityEditor.EditorUtility.DisplayDialog("Answer edited : "+ answerName.text, answerText.text+"\n Link:" + answerLinkDDL.value, "OK", "Cancel"))
                    {
                    //Save as old answer
                    actualEvent.SerializableAnswer[i].text = answerText.text;
                    actualEvent.SerializableAnswer[i].next_event = answerLinkDDL.value;
                    actualEvent.SerializableAnswer[i].blockCondition.stat_id = answerBlockDDL.value - 1;
                    actualEvent.SerializableAnswer[i].blockCondition.stat_value = int.Parse(answerBlockText.text);
                    }
                    return;
                }
            }
        if (UnityEditor.EditorUtility.DisplayDialog("NEW answer: "+answerName.text,answerText.text+"\n Link:"+ answerLinkDDL.value, "OK", "Cancel"))
        {
            //Save as new answer
            SerializableAnswer tAnswer = new SerializableAnswer();
            tAnswer.name = answerName.text;
            tAnswer.text = answerText.text;
            tAnswer.next_event = answerLinkDDL.value;
            tAnswer.blockCondition.stat_id = answerBlockDDL.value - 1;
            tAnswer.blockCondition.stat_value = int.Parse(answerBlockText.text);
            actualEvent.SerializableAnswer.Add(tAnswer);
            FillAnswerPanel(actualEvent.SerializableAnswer);
        }
    }
    void FillPropertiesDLL()
    {
        propertiesDDL.ClearOptions();
        List<string> propsStrings = new List<string>();
        PropertyList tProps = jsonReader.propList;
        for (int i = 0; i < tProps.Property.Count; i++)
        {
            propsStrings.Add(tProps.Property[i].property_name);
        }
        propsStrings.Add("NONE");
        propertiesDDL.AddOptions(propsStrings);

    }
    void FillPropertiesPanel()
    {
        propsListText.text = "";
        if (eventList.SerializableEvent.Count== ddList.value || answersDDL.value == actualEvent.SerializableAnswer.Count)
            return;
        Debug.Log("Debug: EVENTS : " + ddList.value +" ANSWERS : " + answersDDL.value);
        if (actualEvent.SerializableAnswer.Count == 0) return;
         List<SerializableStat> SerializableStat = actualEvent.SerializableAnswer[answersDDL.value].SerializableStat;
         for(int i = 0; i< SerializableStat.Count; i++)
        {
            propsListText.text += "" + jsonReader.propList.Property[SerializableStat[i].stat_id].property_name + " MODIF: " + SerializableStat[i].stat_value + " ODDS: " +SerializableStat[i].odds + " \n";
        }
       
    }

    public void AddNewPropertyToList()
    {
        if( UnityEditor.EditorUtility.DisplayDialog("New property added to event ", jsonReader.propList.Property[propertiesDDL.value].property_name + " modified by " + propValue.text + " with a chance of %" + propOdds.text, "OK", "Cancel")){
            SerializableStat tempStat;
            tempStat.odds = int.Parse(propOdds.text);
            tempStat.stat_value = int.Parse(propValue.text);
            tempStat.stat_id = propertiesDDL.value;
            if (actualEvent.SerializableAnswer.Count == 0)
            {
                SaveCurrentAnswer();
            }
            actualEvent.SerializableAnswer[answersDDL.value].SerializableStat.Add(tempStat); 
        }
        FillPropertiesPanel();
    }
    public void DeleteCurrentAnswer()
    {
        if (UnityEditor.EditorUtility.DisplayDialog("DELETE answer: "+ answerName.text, answerText.text+"\n Link:" + answerLinkDDL.value, "OK", "Cancel"))
        {
            SerializableEvent tEvent = eventList.SerializableEvent[ddList.value];
            for (int i = 0; i < tEvent.SerializableAnswer.Count; i++)
            {
                if (tEvent.SerializableAnswer[i].name == answerName.text)
                {
                    tEvent.SerializableAnswer.RemoveAt(i);
                    FillAnswerPanel(tEvent.SerializableAnswer);
                }
            }
        }
    }
    /// <ANSWER BLOCK CODE>
    
    void FillBlockerDDL()
    {
        answerBlockDDL.ClearOptions();
        List<string> propsStrings = new List<string>();
        propsStrings.Add("NONE");
        PropertyList tProps = jsonReader.propList;
        for (int i = 0; i < tProps.Property.Count; i++)
        {
            propsStrings.Add(tProps.Property[i].property_name);
        }
        answerBlockDDL.AddOptions(propsStrings);
    }

    /// <JSON RELATED CODE>
    public void ExportJSON()
    {
        if (UnityEditor.EditorUtility.DisplayDialog("JSON SAVED", "JSON file is now up to date", "OK")) 
        {
            jsonReader.eventList = eventList;
            jsonReader.SaveJSON();
        }
    }
    public void ReloadJSON()
    {

    }
    /// <PANEL RELATED CODE>
    public void ShowEventPanel()
    {
        eventsButton.enabled = false;
        propsButton.enabled = true;
        eventsPanel.SetActive(true);
        propertiesPanel.SetActive(false);
        FillPropertiesPanel();
    }
    public void ShowPropertiesPanel()
    {
        eventsButton.enabled = true;
        propsButton.enabled = false;
        eventsPanel.SetActive(false);
        propertiesPanel.SetActive(true);
    }
}
