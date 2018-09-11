using Prototype.NetworkLobby;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDController : MonoBehaviour {

    [SerializeField] private GameObject igmPanel;
    private Animator m_AnimIGMPanel;
    private bool openIGM = false;

    private void Awake()
    {
        m_AnimIGMPanel = igmPanel.GetComponent<Animator>();
    }

    public void InGameMenu()
    {
        openIGM = !openIGM;
        m_AnimIGMPanel.SetBool("openIGM", openIGM);
    }
   
}
