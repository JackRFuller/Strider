using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    private List<CombatEncounter> combatEncounters;

    private void Start()
    {
        combatEncounters = new List<CombatEncounter>();
    }

    public void CreateCombatEncounter(UnitView attacker, List<UnitView> defenders)
    {
        CombatEncounter combatEncounter = new CombatEncounter(attacker, defenders);
        combatEncounters.Add(combatEncounter);
    }
}

public class CombatEncounter
{
    public UnitView attacker;
    public List<UnitView> defenders;

    public CombatEncounter(UnitView _attacker, List<UnitView> _defenders)
    {
        attacker = _attacker;
        defenders = _defenders;
    }

}

