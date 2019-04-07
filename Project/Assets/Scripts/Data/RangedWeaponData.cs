using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ranged Weapon",menuName = "Data/Ranged Weapon")]
public class RangedWeaponData : WeaponData
{
    [Range(1,100)]
    public int weaponRange;
    [Range(1, 5)]
    public int numberOfShots;
}
