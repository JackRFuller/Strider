using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnButton : MonoBehaviour
{
    private Button m_button;

    private void Start()
    {
        m_button = GetComponent<Button>();
        m_button.onClick.AddListener(delegate { GameManager.Instance.UnitAIManager.InitiateTurn(); });
        m_button.onClick.AddListener(delegate { DisableButton(); });
    }

    private void DisableButton()
    {
        m_button.gameObject.SetActive(false);
    }
}
