using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComponent : MonoBehaviour
{
    protected PlayerView m_playerView;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        m_playerView = GetComponent<PlayerView>();
    }
}
