using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIButton : MonoBehaviour
{
    protected Button m_button;
    protected Image m_buttonImage;
    protected TMP_Text m_buttonText;

    protected virtual void Start()
    {
        m_button = GetComponent<Button>();
        m_buttonImage = GetComponent<Image>();
        m_buttonText = GetComponentInChildren<TMP_Text>();
    }

    protected virtual void DisableButton()
    {
        if (m_button)
            m_button.enabled = false;
        if (m_buttonImage)
            m_buttonImage.enabled = false;
        if (m_buttonText)
            m_buttonText.enabled = false;
    }

    protected virtual void EnableButton()
    {
        if (m_button)
            m_button.enabled = true;
        if (m_buttonImage)
            m_buttonImage.enabled = true;
        if (m_buttonText)
            m_buttonText.enabled = true;
    }
}
