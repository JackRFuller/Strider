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
    [Range(1,10)]
    public int strength;
    [Range(1, 10)]
    public int defense;
    [Range(1,10)]
    public int shootingSkill;

    public float maxMovementDistancePerTurn;

    [Header("Field of View")]
    public float viewRadius;
    [Range(0,361)]
    public float viewAngle;
    public Material fieldOfViewMaterial;

    [Header("Weapons")]
    public RangedWeaponData rangedWeapon;
}
