using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DiceManager : MonoBehaviour
{
    [SerializeField] private GameObject m_dicePrefab;

    public Action DiceEventSetup;
    public Action DiceEventEnded;

    private Camera m_playerCamera;

    

    private List<Dice> m_dice;
    private List<int> m_diceRollResults;
    private List<Dice> m_diceUsedDuringEvent;
    private DiceRollEvent m_diceRollEvent;

    private void Start()
    {
        PoolDice();
    }

    private void PoolDice()
    {
        m_dice = new List<Dice>();

        for (int i = 0; i < 10; i++)
        {
            GameObject dicePrefab = Instantiate(m_dicePrefab, Vector3.zero, Quaternion.identity);
            Dice diceScript = dicePrefab.GetComponent<Dice>();
            m_dice.Add(diceScript);
        }
    }

    public void InitiateDiceEvent(DiceRollEvent diceRollEvent)
    {
        Debug.Log("Event Initiated");

        m_diceRollEvent = diceRollEvent;
        m_diceRollResults = new List<int>();

        if (DiceEventSetup != null)
            DiceEventSetup();
    }

    public void RollDice()
    {
        m_diceUsedDuringEvent = new List<Dice>();

        Vector3 diceSpawnPosition = Vector3.zero;

        for(int i = 0; i < m_diceRollEvent.numberOfDiceRequired;i++)
        {
            Ray ray = m_diceRollEvent.playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if(hit.collider.CompareTag("Ground"))
                {
                    diceSpawnPosition = new Vector3(hit.point.x,
                                                    hit.point.y + 2,
                                                    hit.point.z);

                    m_dice[i].RollDice(diceSpawnPosition, RecieveDiceRollValue);

                    m_diceUsedDuringEvent.Add(m_dice[i]);
                }
            }
        }
    }

    public void RecieveDiceRollValue(int rollValue)
    {
        Debug.Log("Dice Manager " + rollValue);

        m_diceRollResults.Add(rollValue);

        if(m_diceRollResults.Count == m_diceRollEvent.numberOfDiceRequired)
        {
            if (DiceEventEnded != null)
                DiceEventEnded();

            //Disable Dice
            for (int i = 0; i < m_diceUsedDuringEvent.Count; i++)
            {
                m_diceUsedDuringEvent[i].DisableDice();
            }

            m_diceRollEvent.diceRollEventCallback.Invoke(m_diceRollResults);           
        }
    }
}

public class DiceRollEvent
{
    public Camera playerCamera;
    public int numberOfDiceRequired;  
    public Action<List<int>> diceRollEventCallback;

    public DiceRollEvent(Camera _playerCamera, int _numberOfDiceRequired, Action<List<int>> _diceRollEventCallback)
    {
        playerCamera = _playerCamera;
        numberOfDiceRequired = _numberOfDiceRequired;       
        diceRollEventCallback = _diceRollEventCallback;
    }
}
