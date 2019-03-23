using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Unit Data", menuName = "Data/Unit",order = 1)]
public class UnitData : ScriptableObject
{
    public string unitName;
    [Range(1, 10)]
    public int healthPoints = 1;

    public float maxMovementDistancePerTurn;
}
