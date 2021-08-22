using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeScreen : MonoBehaviour
{

    [SerializeField] private Button startGameButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button quitButton;
    // Start is called before the first frame update
    void Start()
    {
        startGameButton.onClick.AddListener(StartGame);
        creditsButton.onClick.AddListener(ShowCredits);
        quitButton.onClick.AddListener(QuitGame);
    }

    private void StartGame()
    {
        SceneManager.LoadScene("tornado-mayhem");
    }

    private void ShowCredits()
    {
        SceneManager.LoadScene("credits");
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
