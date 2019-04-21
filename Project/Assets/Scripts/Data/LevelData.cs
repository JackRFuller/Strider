using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Level",menuName = "Data/Level")]
public class LevelData : ScriptableObject
{
    public string levelName;
    public GameObject levelPrefab;

    public Transform[] teamOneSpawnPoints;
    public Transform[] teamTwoSpawnPoints;
}
