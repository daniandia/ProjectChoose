using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenBehaviourManager : MonoBehaviour
{
    JSONReader jsonManager;
    int eventId = 0;
    // Start is called before the first frame update
    void Start()
    {
        jsonManager = GetComponent<JSONReader>();
        jsonManager.LoadGameJSON();
        LoadActualEvent();
    }
    SerializableEvent tEvent;
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
        }
        for (int i = tEvent.SerializableAnswer.Count; i < answerTextConts.Length; i++)
            answerTextConts[i].transform.parent.gameObject.SetActive(false);
    }

    public void SelectOption(int optionId)
    {
        eventId = tEvent.SerializableAnswer[optionId].next_event;
        if (eventId < 0) {
            EndGame();
            return;
        }
        ConfirmOption();
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
        LoadActualEvent();
        LeanTween.scaleX(backCanvas, 0.9f, scaleInOutSpeeds.y);
    }
}
