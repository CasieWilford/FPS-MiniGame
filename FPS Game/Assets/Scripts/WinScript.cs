using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScript : MonoBehaviour
{
    public GameObject winPanel;

    private bool youHaveWon = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && youHaveWon)
        {
            LoadNextLevel();

            WinConditions.enemiesDead = 0;
            WinConditions.pickups = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("you win");

        if (other.gameObject.name.Equals("FPSPlayer"))
        {
            winPanel.SetActive(true);


            youHaveWon = true;
        }
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
