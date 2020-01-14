using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashScreen : MonoBehaviour
{
    public GameObject flashPanel;

    public static bool flashScreen = false;

    float timer = .1f;
    float time = 0f;


    // Start is called before the first frame update
    void Start()
    {
        flashPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (flashScreen)
        {
            flashPanel.SetActive(true);

            if (flashPanel)
            {
                if (time < timer)
                    time += Time.deltaTime;
                else
                {
                    flashScreen = false;
                    time = 0;
                    flashPanel.SetActive(false);
                }
            }
        }
    }
}
