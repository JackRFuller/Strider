using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Unit Data", menuName = "Data/Unit",order = 1)]
public class UnitData : ScriptableObject
{
    [Header("Unit Attributes")]
    public string unitName;
    [Range(1, 10)]
    public int healthPoints = 1;
    [Range(1,10)]
    public int strength;
    [Range(1, 10)]
    public int defense;
    [Range(1,10)]
    public int shootingSkill;
    public float unitBaseRadius = 0.5f;
    public float maxMovementDistancePerTurn;

    [Header("Unit Model")]
    public GameObject unitModel;

    [Header("Field of View")]
    public float viewRadius;
    [Range(0,361)]
    public float viewAngle;
    public Material fieldOfViewMaterial;

    [Header("Weapons")]
    public RangedWeaponData rangedWeapon;
}
