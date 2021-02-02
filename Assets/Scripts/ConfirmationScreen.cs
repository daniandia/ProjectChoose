using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmationScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = Vector3.zero;
    }    
    public UnityEngine.UI.Text titleText;
    public UnityEngine.UI.Text infoText;
    string okFunction;
    string denyFunction;
    GameObject callerObj;
    public void ShowConfirmMessage(string title, string text, string callbackOk, string callbackCancel, GameObject caller)
    {
        transform.localScale = Vector3.one;
        titleText.text = title;
        infoText.text = text;
        okFunction = callbackOk;
        denyFunction = callbackCancel;
        callerObj = caller;
    }
    public void ConfirmOption() {
        if(okFunction!="")
            callerObj.SendMessage(okFunction);
        transform.localScale = Vector3.zero;
    }
    public void DenyOption() {
        if(denyFunction!="")
            callerObj.SendMessage(denyFunction);
        transform.localScale = Vector3.zero;
    }
}
