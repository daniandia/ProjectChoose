using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenBehaviourManager : MonoBehaviour
{
    JSONReader jsonManager;
    int eventId = 0;
    // Start is called before the first frame update
    void Start()
    {
        jsonManager = GetComponent<JSONReader>();
        jsonManager.LoadGameJSON();
        jsonManager.LoadStatJSON();
        InitialiseInGameStats();
        LoadFirstEvent();

        //CameraFading.CameraFade.In();
    }
    public SerializableEvent tEvent;

    private void LoadFirstEvent()
    {
        tEvent = jsonManager.GetFirstEvent();
        SetUpCanvas(tEvent);
    }
    private void LoadActualEvent()
    {
        tEvent = jsonManager.GetActualEvent(eventId);
        SetUpCanvas(tEvent);
    }
    public UnityEngine.UI.Text eventTextCont;
    public UnityEngine.UI.Text [] answerTextConts;
    void SetUpCanvas(SerializableEvent tEvent)
    {
        eventTextCont.text = tEvent.text;
        for (int i = 0; i < tEvent.SerializableAnswer.Count; i++)
        {
            answerTextConts[i].transform.parent.gameObject.SetActive(true);
            answerTextConts[i].text = tEvent.SerializableAnswer[i].text;
            if(CheckBlockCondition(tEvent.SerializableAnswer[i].blockCondition))
            {
                //LockTheAnswer
                answerTextConts[i].transform.parent.GetComponent<Button>().enabled = false;
                answerTextConts[i].transform.parent.GetComponent<Image>().color = Color.grey;
                if (tEvent.SerializableAnswer[i].hideIfBlocked)
                    answerTextConts[i].transform.parent.gameObject.SetActive(false);
            }
            else
            {
                //Unlock the answer
                answerTextConts[i].transform.parent.GetComponent<UnityEngine.UI.Button>().enabled = true;
                answerTextConts[i].transform.parent.GetComponent<Image>().color = Color.white;
            }
        }
        for (int i = tEvent.SerializableAnswer.Count; i < answerTextConts.Length; i++)
            answerTextConts[i].transform.parent.gameObject.SetActive(false);
        UpdateStatText();
    }

    bool CheckBlockCondition(SerializableStat tBlock)
    {
        if (tBlock.stat_id < 0) return false;
        return (inGameStats[tBlock.stat_id].stat_value < tBlock.stat_value);
    }

    public void SelectOption(int optionId)
    {
        eventId = tEvent.SerializableAnswer[optionId].next_event;
        if (eventId < 0) {
            EndGame();
            return;
        }
        ConfirmOption();
        UpdateStats(tEvent.SerializableAnswer[optionId]);
    }

    void EndGame()
    {
        Application.Quit();
    }

    public void ConfirmOption()
    {
        HideOptionCanvas();
        Invoke("RestoreOptionCanvas", scaleInOutSpeeds.x * 1.5f);
    }

    public GameObject backCanvas;
    public Vector2 scaleInOutSpeeds;
    void HideOptionCanvas()
    {
        LeanTween.scaleX(backCanvas, 0f, scaleInOutSpeeds.x);
    }
    void RestoreOptionCanvas()
    {
        if (CheckAnswerType())
        {
            LoadActualEvent();
            LeanTween.scaleX(backCanvas, 0.9f, scaleInOutSpeeds.y);
        }
    }

    public  List<SerializableStat> inGameStats;
    public UnityEngine.UI.Text propsText;
    void InitialiseInGameStats()
    {
        inGameStats = new List<SerializableStat>();
        int i = 0;
        foreach(Property tProp in jsonManager.propList.Property)
        {
            SerializableStat tStat;
            tStat.stat_id = i;
            tStat.stat_value = tProp.initial_value;
            tStat.odds = 0f;
            inGameStats.Add(tStat);
            i++;

        }
    }

    void UpdateStatText()
    {
        propsText.text = "";
        foreach (SerializableStat tStat in inGameStats)
            if(jsonManager.propList.Property[tStat.stat_id].visible)
                propsText.text += (" - " + jsonManager.propList.Property[tStat.stat_id].property_name + " : " + tStat.stat_value+" \n");
    }

    void UpdateStats(SerializableAnswer answer)
    {
        foreach(SerializableStat tStat in answer.SerializableStat)
        {
            if (Random.Range(0, 100) > tStat.odds)
            {
                SerializableStat modif = inGameStats[tStat.stat_id];
                modif.stat_value += tStat.stat_value;
                inGameStats[tStat.stat_id] = modif;
            }
        }
        //We should override this function, call another event or something for specific consequences
    }

    bool CheckAnswerType()
    {
        if (tEvent.type == (int)(answerType.END_NODE))
        {
            //Application.Quit();
            //CameraFading.CameraFade.Out();
            Invoke("LoadMenu", 1.1f);
            return false;
        }
        return true;
    }
    void LoadMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

}
    
   

