using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatEncounter
{
    public List<UnitView> units;

    public CombatEncounter(List<UnitView> _units)
    {
        units = _units;
    }
}
