using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Button function to start the game scene
    public void OnPlay()
    {
        SceneManager.LoadScene("GameScenes", LoadSceneMode.Single);
    }

    //Button function to quit the game
    public void ExitGame()
    {
        Application.Quit();
    }
}
