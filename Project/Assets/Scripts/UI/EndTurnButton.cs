using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnButton : UIButton
{
    private TurnManager m_turnManager;

    protected override void Start()
    {
        base.Start();

        m_turnManager = GameManager.Instance.TurnManager;

        m_button.onClick.AddListener(delegate { m_turnManager.EndTurn(); });
        m_turnManager.TurnPhaseUpdated += DetermineIfEndTurnButtonShouldBeDisplayed;

        DetermineIfEndTurnButtonShouldBeDisplayed();
    }

    private void DetermineIfEndTurnButtonShouldBeDisplayed()
    {
        if(m_turnManager.IsPlayersTurn())
        {
            EnableButton();
        }
        else
        {
            DisableButton();
        }
    }


}
