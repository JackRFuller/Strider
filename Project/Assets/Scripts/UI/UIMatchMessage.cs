using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMatchMessage : MonoBehaviour
{
    [SerializeField] private GameObject matchMessageObj;
    [SerializeField] private TMP_Text matchMessageHeaderText;
    [SerializeField] private TMP_Text matchMessageSubText;

    private void Start()
    {
        matchMessageObj.SetActive(false);
    }

    public void ShowMatchMessage(MatchMessage matchMessage)
    {
        matchMessageHeaderText.text = matchMessage.matchMessageHeader;
        matchMessageSubText.text = matchMessage.matchMessageSubHeader;

        matchMessageObj.SetActive(true);

        StartCoroutine(WaitToTurnOffMatchMessage());
    }

    private IEnumerator WaitToTurnOffMatchMessage()
    {
        yield return new WaitForSeconds(3.0f);

        matchMessageObj.SetActive(false);
    }
}

public struct MatchMessage
{
    public string matchMessageHeader;
    public string matchMessageSubHeader;

    public MatchMessage(string _matchMessageHeader, string _matchMessageSubHeader)
    {
        matchMessageHeader = _matchMessageHeader;
        matchMessageSubHeader = _matchMessageSubHeader;
    }
}
