using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RollDiceButton : UIButton
{
    [SerializeField] private Image m_diceIconImage;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        m_diceIconImage = GetComponentInChildren<Image>();

        m_button.onClick.AddListener(delegate { GameManager.Instance.DiceManager.RollDice(); });

        DisableButton();

        GameManager.Instance.DiceManager.DiceEventSetup += EnableButton;
        GameManager.Instance.DiceManager.DiceEventEnded += DisableButton;
    }

    protected override void EnableButton()
    {
        base.EnableButton();

        m_diceIconImage.enabled = true;
    }

    protected override void DisableButton()
    {
        base.DisableButton();

        m_diceIconImage.enabled = false;

        Debug.Log("Hit");
    }
}
