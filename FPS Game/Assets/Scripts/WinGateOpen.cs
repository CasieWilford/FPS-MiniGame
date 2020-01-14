using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinGateOpen : MonoBehaviour
{
    public GameObject winBlock1;
    public GameObject winBlock2;

    private Transform endLocation;

    private Vector3 target1;
    private Vector3 target2;

    bool openGates = false;
    bool playOnce = false;

    public AudioSource winSource;
    public AudioClip winClip;
    public GameObject winText;

    // Start is called before the first frame update
    void Start()
    {
        endLocation = GameObject.FindGameObjectWithTag("WinBlock1").transform;

        //Players position.
        target1 = new Vector3(endLocation.position.x, endLocation.position.y, endLocation.position.z);

        winSource.clip = winClip;
    }

    // Update is called once per frame
    void Update()
    {
        if (WinConditions.enemiesDead == 6 && WinConditions.pickups == 3)
        {
            openGates = true;
            winText.SetActive(true);

            if (!playOnce)
            {
                winSource.PlayOneShot(winClip);
                playOnce = true;
            }
        }
        else
            winSource.Stop();

        if(openGates)
        {
            if (winBlock1.transform.position.x < 45.83)
                winBlock1.transform.position = new Vector3(winBlock1.transform.position.x + 0.01f, winBlock1.transform.position.y, winBlock1.transform.position.z);

            if(winBlock2.transform.position.x > 35.92)
                winBlock2.transform.position = new Vector3(winBlock2.transform.position.x - 0.01f, winBlock2.transform.position.y, winBlock2.transform.position.z);
        }
    }
}
