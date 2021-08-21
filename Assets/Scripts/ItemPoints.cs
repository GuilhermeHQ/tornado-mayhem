using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class ItemPointData
{
    public ItemType itemType;
    public int points;
    [FormerlySerializedAs("pointsToCollect")] 
    public int levelToCollect;
}

[CreateAssetMenu(menuName = "Configs/ItemPoints")]
public class ItemPoints : ScriptableObject
{
    public List<ItemPointData> itemPointData;
}
