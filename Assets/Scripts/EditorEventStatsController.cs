using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorEventStatsController : MonoBehaviour
{
    [Header("Properties editor references")]
    public UnityEngine.UI.InputField nameInput;
    public UnityEngine.UI.InputField valueInput;
    public UnityEngine.UI.InputField descriptionInput;
    [Header("Property list  references")]
    public UnityEngine.UI.Dropdown propsDDL;
    public UnityEngine.UI.Text pName;
    public UnityEngine.UI.Text pVal;
    public UnityEngine.UI.Text pDesc;
    public UnityEngine.UI.Text pRefs;
    JSONReader jsonReader;


    void Start()
    {
        jsonReader = GetComponent<JSONReader>();
        InitPropertyList();
    }
    void InitPropertyList()
    {
        propsDDL.ClearOptions();
        List<string> propsStrings = new List<string>();
        PropertyList tProps = jsonReader.propList;
        for (int i = 0; i < tProps.Property.Count; i++)
        {
            propsStrings.Add(tProps.Property[i].property_name);
        }
        propsStrings.Add("NONE");
        propsDDL.AddOptions(propsStrings);
    }
    public void SelectNewProperty()
    {
        Property prop = jsonReader.propList.Property[propsDDL.value];
        pName.text = prop.property_name;
        pVal.text = ""+prop.initial_value;
        pDesc.text = prop.description;
        string referencesInEvents = "";
        for(int i = 0; i < jsonReader.eventList.SerializableEvent.Count; i++)
        {
            for (int j=0;j< jsonReader.eventList.SerializableEvent[i].SerializableAnswer.Count; j++)
            {
                for(int k = 0;k < jsonReader.eventList.SerializableEvent[i].SerializableAnswer[j].SerializableStat.Count; k++){
                    if (jsonReader.eventList.SerializableEvent[i].SerializableAnswer[j].SerializableStat[k].stat_id == propsDDL.value)
                    {
                        referencesInEvents += "" + jsonReader.eventList.SerializableEvent[i].name + " - " + jsonReader.eventList.SerializableEvent[i].SerializableAnswer[j].name + " : " + jsonReader.eventList.SerializableEvent[i].SerializableAnswer[j].SerializableStat[k].stat_value + " \n";
                    }
                }
            }
        }
        pRefs.text = referencesInEvents;
    }

    public void DeleteProperty()
    {
        if (propsDDL.options.Count == 0)
            return;
        for (int i = 0; i < jsonReader.eventList.SerializableEvent.Count; i++)
        {
            for (int j = 0; j < jsonReader.eventList.SerializableEvent[i].SerializableAnswer.Count; j++)
            {
                for (int k = 0; k < jsonReader.eventList.SerializableEvent[i].SerializableAnswer[j].SerializableStat.Count; k++)
                { 
                    SerializableStat tStat = jsonReader.eventList.SerializableEvent[i].SerializableAnswer[j].SerializableStat[k];
                    if (tStat.stat_id == propsDDL.value)
                    {
                        tStat.stat_id = -1;
                    }
                    if (tStat.stat_id > propsDDL.value)
                    {
                        tStat.stat_id--;
                    }
                    jsonReader.eventList.SerializableEvent[i].SerializableAnswer[j].SerializableStat[k] = tStat;
                }
            }
        }
        jsonReader.propList.Property.RemoveAt(propsDDL.value);
        InitPropertyList();
    }

    public void CreateNewProperty()
    {
        Property tProperty;
        tProperty.property_name = nameInput.text;
        tProperty.description = descriptionInput.text;
        tProperty.initial_value = int.Parse(valueInput.text);
        jsonReader.propList.Property.Add(tProperty);
        InitPropertyList();
    }
    public void ExportPropsJSON()
    {

    }


}
