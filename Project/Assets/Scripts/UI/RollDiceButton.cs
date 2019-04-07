using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RollDiceButton : MonoBehaviour
{
    private Button m_button;
    private Image m_buttonImage;

    // Start is called before the first frame update
    void Start()
    {
        m_button = GetComponent<Button>();
        m_buttonImage = GetComponent<Image>();
        m_button.onClick.AddListener(delegate { GameManager.Instance.DiceManager.RollDice(); });

        DisableButton();

        GameManager.Instance.DiceManager.DiceEventSetup += EnableButton;
        GameManager.Instance.DiceManager.DiceEventEnded += DisableButton;
    }

    private void DisableButton()
    {
        m_button.enabled = false;
        m_buttonImage.enabled = false;
    }

    private void EnableButton()
    {
        m_button.enabled = true;
        m_buttonImage.enabled = true;
    }
   
}
