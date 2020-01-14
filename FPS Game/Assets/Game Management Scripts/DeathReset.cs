using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathReset : MonoBehaviour
{
    private Transform player;

    private bool dead = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && dead)
        {
            Debug.Log("Reloading Scene");
            LoadNextLevel();
            GunScript.playerDied = false;
        }
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("FPSPlayer"))
        {
            player.transform.position = new Vector3(-4, 1, -1.5f);
            GunScript.playerDied = true;

            WinConditions.enemiesDead = 0;
            WinConditions.pickups = 0;

            dead = true;
        }
    }
}
