using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //CameraFading.CameraFade.In();
    }

    // Update is called once per frame
    bool fading = false;
    void Update()
    {

        if (!fading && Input.GetMouseButtonUp(0))
        {
            fading = true;
            //CameraFading.CameraFade.Out();
            Invoke("LoadGame", 1.1f);
        }
    }
    void LoadGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
