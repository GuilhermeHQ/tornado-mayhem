using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{

    [SerializeField] private float initialPointsToLevelUp = 15;
    [SerializeField] private float progressionMultiplier = 2;
    [SerializeField] private TornadoController tornado;
    [SerializeField] private ScoreScript score;
    [SerializeField] private CountdownTimer countdownTimer;
    
    private ItemPoints itemPointConfig;
    private Dictionary<ItemType, ItemPointData> ItemPointsDict;
    private int currentLevel = 1;
    private int currentPoints = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        tornado = FindObjectOfType<TornadoController>();
        tornado.onCollideWithDestructibleObject = OnCollideWithDestructibleObject;
        
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
    }
    
    private void OnCollideWithDestructibleObject(DestructibleObject destructibleObject)
    {
        if (ItemPointsDict[destructibleObject.itemType].levelToCollect <= currentLevel)
        {
            Debug.Log("Building Destroyed");
            Destroy(destructibleObject.gameObject);

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


    // Update is called once per frame
    void Update()
    {
        
    }
}
