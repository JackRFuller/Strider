using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Dice : MonoBehaviour
{
    private PhotonView m_photonView;
    private Rigidbody m_rigidBody;
    private Collider m_collider;
    private Transform m_diceTransform;
    private MeshRenderer m_diceMeshRenderer;

    private Action<int> m_diceManagerCallback;

    [Header("Dice Materials")]
    [SerializeField] private Material m_opponentDiceMaterial;

    public PhotonView PhotonView { get { return m_photonView; } }

    private void Start()
    {
        this.enabled = false;

        m_photonView = GetComponent<PhotonView>();
        m_rigidBody = GetComponent<Rigidbody>();
        m_collider = GetComponent<Collider>();

        m_diceMeshRenderer = GetComponentInChildren<MeshRenderer>();
        m_diceTransform = transform;

        //SetDiceMaterial();
        DisableDice();
    }

    private void FixedUpdate()
    {
        //GetDiceValue();
    }   

    public void RollDice(Vector3 rollPosition, Action<int> diceManagerCallback)
    {
        m_diceManagerCallback = diceManagerCallback;

        transform.position = rollPosition;

        EnableDice();

        //Generate Random Speed
        m_rigidBody.AddForce(transform.right * UnityEngine.Random.Range(3, 12), ForceMode.Impulse);
        m_rigidBody.AddTorque(transform.right * UnityEngine.Random.Range(3, 12), ForceMode.Impulse);

        StartCoroutine(WaitBeforeTryingToReturnDiceValue());
    }

    private void GetDiceValue()
    {
        if (m_rigidBody.velocity == Vector3.zero)
        {
            int diceValue = ReturnRolledDiceValue();
            m_diceManagerCallback.Invoke(diceValue);

            this.enabled = false;
        }
    }

    private IEnumerator WaitBeforeTryingToReturnDiceValue()
    {
        yield return new WaitForSeconds(0.5f);
        
        while(m_rigidBody.velocity != Vector3.zero)
        {
            yield return new WaitForSeconds(0.1f);
        }

        GetDiceValue();

    }

    private int ReturnRolledDiceValue()
    {
        int diceValue = 0;

        if (Vector3.Dot(m_diceTransform.forward, Vector3.up) > 0.6f)
            diceValue = 1;
        if (Vector3.Dot(-m_diceTransform.forward, Vector3.up) > 0.6f)
            diceValue = 6;
        if (Vector3.Dot(m_diceTransform.up, Vector3.up) > 0.6f)
            diceValue = 5;
        if (Vector3.Dot(-m_diceTransform.up, Vector3.up) > 0.6f)
            diceValue = 2;
        if (Vector3.Dot(m_diceTransform.right, Vector3.up) > 0.6f)
            diceValue = 4;
        if (Vector3.Dot(-m_diceTransform.right, Vector3.up) > 0.6f)
            diceValue = 3;

        return diceValue;
    }

    private void SetDiceMaterial()
    {
        if (!m_photonView.isMine)
        {
            m_diceMeshRenderer.material = m_opponentDiceMaterial;
        }
    }
   
    public void DisableDice()
    {
        m_collider.enabled = false;
        m_diceMeshRenderer.enabled = false;
        m_rigidBody.velocity = Vector3.zero;
        m_rigidBody.angularVelocity = Vector3.zero;
        m_rigidBody.isKinematic = true;
    }
   
    private void EnableDice()
    {
        m_collider.enabled = true;
        m_rigidBody.isKinematic = false;        
        m_diceMeshRenderer.enabled = true;
    }
}
