using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    private List<CombatEncounter> m_combatEncounters;

    private void Start()
    {
        m_combatEncounters = new List<CombatEncounter>();
    }

    public void SetupCombatEncounter(List<UnitView> enemies, UnitView instigator)
    {
        instigator.UnitCombat.SetToInCombat();

        Enemies targetEnemies = new Enemies(enemies);

        if (targetEnemies.enemiesInCombat.Count > 0)
        {
            //Search Through And Add
            for (int i = 0; i < m_combatEncounters.Count; i++)
            {
                for (int j = 0; j < targetEnemies.enemiesInCombat.Count; j++)
                {
                    if(m_combatEncounters[i].units.Contains(targetEnemies.enemiesInCombat[j]))
                    {
                        m_combatEncounters[i].units.Add(instigator);
                        break;                       
                    }                        
                }
            }

            CreateCombatEncounter(instigator, targetEnemies.enemiesNotInCombat);
        }
        else //If ALl Units Are Not In Combat
        {
            CreateCombatEncounter(instigator, enemies);      
        }

        //Find Mid Point
        Vector3 centerPoint = instigator.transform.position;

        for(int i = 0; i < enemies.Count;i++)
        {
            centerPoint += enemies[i].transform.position;
        }

        centerPoint = centerPoint / (enemies.Count + 1);

        //Set Units To Look At Center Point
        Vector3 lookAT = new Vector3(centerPoint.x, 0, centerPoint.z);
        instigator.transform.LookAt(lookAT);

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].transform.LookAt(lookAT);
        }
    }

    private void CreateCombatEncounter(UnitView instigator, List<UnitView> enemies)
    {
        List<UnitView> units = new List<UnitView>();
        units.Add(instigator);

        for (int i = 0; i < enemies.Count; i++)
        {
            units.Add(enemies[i]);
        }

        CombatEncounter combatEncounter = new CombatEncounter(units);
    }
}

public class Enemies
{
    public List<UnitView> enemiesInCombat;
    public List<UnitView> enemiesNotInCombat;

    public Enemies(List<UnitView> enemies)
    {
        enemiesInCombat = new List<UnitView>();
        enemiesNotInCombat = new List<UnitView>();

        for(int i = 0; i < enemies.Count; i++)
        {
            if (!enemies[i].UnitCombat.IsInCombat)
                enemiesNotInCombat.Add(enemies[i]);
            else
                enemiesInCombat.Add(enemies[i]);
        }
    }
}


