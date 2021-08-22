using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditsScreen : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    // Start is called before the first frame update
    void Start()
    {
        mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    private void GoToMainMenu()
    {
        SceneManager.LoadScene("game-start 1");
    }
}
