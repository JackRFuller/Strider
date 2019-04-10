using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitAIAction : UnitComponent
{
    protected Action m_actionFinished;

    public virtual void DoAction(Action turnFinished)
    {
        if (m_actionFinished == null)
            m_actionFinished = turnFinished;
    }
}
