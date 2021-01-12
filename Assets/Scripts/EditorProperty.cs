using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorProperty : MonoBehaviour
{
    string Name;
    string Description;
    int initialValue;
    public UnityEngine.UI.Text uiText;
    // Start is called before the first frame update
    public void Initialise(string _name, string _desc, int _inval, Vector3 pos)
    {
        Name = _name;
        Description = _desc;
        initialValue = _inval;
        uiText.text = Name + " " + initialValue + " " + _desc;
        Debug.Log(pos);
        GetComponent<RectTransform>().localPosition = pos;
    }

}
