using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class MaterialChanger : NetworkInteractiveObject, IPointerClickHandler {


    [SerializeField] private Material[] materials;
    [SerializeField] public Material currMaterial;

    private Material startMaterial;
    private int materialsCount = 0;

    public Action networkRpcAction;
    public Action networkCmdAction;
    public Action networkNmCmdAction;


    public Action OnLeftClick;
    public Action OnRightClick;

    public Action OnMaterialChange;

    private void Start()
    {
        startMaterial = materials[0];
        this.GetComponent<Renderer>().material = startMaterial;
        currMaterial = this.GetComponent<Renderer>().material;

        OnLeftClick += nextMaterial;
        OnRightClick += previousMaterial;
    }
    
    private void OnDisable()
    {
        OnLeftClick -= nextMaterial;
        OnRightClick -= previousMaterial;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (OnLeftClick != null)
                OnLeftClick();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (OnRightClick != null)
                OnRightClick();
        }
    }

    //Set Next Material
    #region nextMaterial
    private void nextMaterial()
    {
        if (isServer)
        {
            RpcNextMaterial();
        }
        else
        {
            if (!hasAuthority)
            {
                this.OnInteraction += NmCmdNextMaterial;
                setLocalAuthority();
            }
            else
            {
                CmdNextMaterial();
            }
        }
    }

    public void NmCmdNextMaterial()
    {
        CmdNextMaterial();
    }
    [Command]
    public void CmdNextMaterial()
    {
        RpcNextMaterial();
    }
    [ClientRpc]
    private void RpcNextMaterial()
    {
        if (materialsCount >= (materials.Length - 1))
        {
            materialsCount = 0;
        }
        else
        {
            materialsCount++;
        }
        this.GetComponent<Renderer>().material = materials[materialsCount];
        currMaterial = materials[materialsCount];

        if(OnMaterialChange != null)
            OnMaterialChange();
    }
    #endregion

    #region previousMaterial
    private void previousMaterial()
    {
        if (isServer)
        {
            RpcPreviousMaterial();
        }
        else
        {
            if (!hasAuthority)
            {
                this.OnInteraction += NmCmdPreviousMaterial;
                setLocalAuthority();
            }
            else
            {
                CmdPreviousMaterial();
            }
        }
    }
    public void NmCmdPreviousMaterial()
    {
        CmdPreviousMaterial();
    }
    [Command]
    public void CmdPreviousMaterial()
    {
        RpcPreviousMaterial();
    }
    [ClientRpc]
    private void RpcPreviousMaterial()
    {
        if (materialsCount <= 0)
        {
            materialsCount = materials.Length - 1;
        }
        else
        {
            materialsCount--;
        }
        this.GetComponent<Renderer>().material = materials[materialsCount];
        currMaterial = materials[materialsCount];

        if (OnMaterialChange != null)
            OnMaterialChange();
    }
    #endregion
}
