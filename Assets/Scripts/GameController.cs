using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    [SerializeField] private float initialPointsToLevelUp = 15;
    [SerializeField] private float progressionMultiplier = 2;
    [SerializeField] private TornadoController tornado;
    [SerializeField] private ScoreScript score;
    [SerializeField] private CountdownTimer countdownTimer;
    [SerializeField] private float gameTimeInSeconds = 30;
    [SerializeField] private Button mainMenuButton;

    private ItemPoints itemPointConfig;
    private Dictionary<ItemType, ItemPointData> ItemPointsDict;
    private int currentLevel = 1;
    private int currentPoints = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        tornado = FindObjectOfType<TornadoController>();
        tornado.onCollideWithDestructibleObject = OnCollideWithDestructibleObject;
        mainMenuButton.onClick.AddListener(GoToMainMenu);

        itemPointConfig = Resources.Load("ItemPointData") as ItemPoints;

        if (itemPointConfig == null)
        {
            Debug.Log("Não consegui encontrar o arquivo de dados de item!!");
            return;
        }
        
        ItemPointsDict = new Dictionary<ItemType, ItemPointData>();

        foreach (ItemPointData itemPointData in itemPointConfig.itemPointData)
        { 
            ItemPointsDict.Add(itemPointData.itemType, itemPointData);
        }
        
        countdownTimer.StartTimer(gameTimeInSeconds, OnFinishTimer);
    }

    private void OnFinishTimer()
    {
        StartCoroutine(EndGame());
    }

    private IEnumerator EndGame()
    {
        tornado.enabled = false;
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("game-start 1");
    }

    private void OnCollideWithDestructibleObject(DestructibleObject destructibleObject)
    {
        if (ItemPointsDict[destructibleObject.itemType].levelToCollect <= currentLevel)
        {
            Debug.Log("Building Destroyed");
            tornado.OrbitObject(destructibleObject);

            currentPoints += ItemPointsDict[destructibleObject.itemType].points;
            score.scoreValue = currentPoints;

            if (ShouldLevelUp())
            {
                currentLevel++;
                tornado.Grow();
            }
        }
    }

    private bool ShouldLevelUp()
    {
        return currentPoints >=  initialPointsToLevelUp * Math.Pow(progressionMultiplier, currentLevel - 1);
    }
    
    private void GoToMainMenu()
    {
        SceneManager.LoadScene("game-start 1");
    }
}
