using Prototype.NetworkLobby;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {

    [SerializeField] private GameObject igmPanel;
    [Header("Menu Buttons")]
    [SerializeField] private Button backButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button musicButton;
    private Animator m_AnimIGMPanel;
    private bool openIGM = false;

    private void Start()
    {
        SetOnClickMenuButtons();
        m_AnimIGMPanel = igmPanel.GetComponent<Animator>();        
    }
    
    public void InGameMenu()
    {
        openIGM = !openIGM;
        m_AnimIGMPanel.SetBool("openIGM", openIGM);
    }
   
    private void SetOnClickMenuButtons()
    {
        backButton.onClick.AddListener(GoBackButton);
        restartButton.onClick.AddListener(ResetLevel);
        //Music button works by itself
    }

    private void GoBackButton()
    {
        LobbyManager.s_Singleton.GoBackButton();
    }

    private void ResetLevel()
    {
        GameObject.FindGameObjectWithTag("ResetLevel").GetComponent<ResetLevel>().ResetScene();
    }
}
