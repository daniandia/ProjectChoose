using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorEventStatsController : MonoBehaviour
{
    [Header("Properties editor references")]
    public UnityEngine.UI.InputField nameInput;
    public UnityEngine.UI.InputField valueInput;
    public UnityEngine.UI.InputField descriptionInput;
    public Transform propListPanel;
    
    public GameObject instantiableProperty;
    [Header("Visual editor offsets")]
    public Vector2 layoutOffset = new Vector2(0f, -90f);
    const int maxrows = 10;
    List<GameObject> propsList = new List<GameObject>();

    JSONReader jsonReader;

    void Start()
    {
        jsonReader = GetComponent<JSONReader>();
    }
    void InitPropertyList()
    {
        for (int i = 0; i < propsList.Count; i++)
        {
            Destroy(propsList[i]);
        }
        propsList.Clear();
        for (int i=0; i < jsonReader.propList.Property.Count; i++)
        {
            Property tProp = jsonReader.propList.Property[i];
            GameObject tGO = Instantiate(instantiableProperty);
            tGO.transform.parent = propListPanel;
            tGO.GetComponent<EditorProperty>().Initialise(tProp.property_name,tProp.description,tProp.initial_value, new Vector3((int)(i/maxrows)*layoutOffset.x, (i%maxrows) * layoutOffset.y, 0f)); 
            propsList.Add(tGO);
        }
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
