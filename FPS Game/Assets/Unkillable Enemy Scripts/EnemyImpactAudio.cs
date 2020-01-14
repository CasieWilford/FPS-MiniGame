using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyImpactAudio : MonoBehaviour
{ 
    public AudioClip enemyImpactClip;
    public  AudioSource enemyImpactSource;

    public bool turnImpactAudioOn = false;

    public static void playImpactAudio()
    {
       
    }

    // Start is called before the first frame update
    void Start()
    {
        enemyImpactSource.clip = enemyImpactClip;
    }

    // Update is called once per frame
    void Update()
    {
        if (turnImpactAudioOn)
        {
            enemyImpactSource.clip = enemyImpactClip;

            enemyImpactSource.Play();
            Debug.Log("hit");
            turnImpactAudioOn = false;
        }
    }
}
