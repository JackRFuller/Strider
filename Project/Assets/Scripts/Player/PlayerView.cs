using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS_Cam;

public class PlayerView : MonoBehaviour
{
    private Camera m_playerCamera;
    private PhotonView m_photonView;
    private RTS_Camera m_playerCameraMovement;

    public Camera PlayerCamera { get { return m_playerCamera; } }
    public PhotonView PhotonView { get { return m_photonView; } }
    public RTS_Camera PlayerCameraMovement { get { return m_playerCameraMovement; } }

    private void Awake()
    {
        m_playerCamera = GetComponent<Camera>();
        m_photonView = GetComponent<PhotonView>();
        m_playerCameraMovement = GetComponent<RTS_Camera>();

        if (!m_photonView.isMine)
            DisablePlayerComponents();
    }

    private void DisablePlayerComponents()
    {
        m_playerCameraMovement.enabled = false;
        m_playerCamera.enabled = false;
    }
}
