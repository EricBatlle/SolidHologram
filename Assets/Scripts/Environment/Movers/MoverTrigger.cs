using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverTrigger : MonoBehaviour, IInteractable
{
    [SerializeField] Mover mover;
    [SerializeField] private Action triggerAction;

    public enum Action
    {
        Open,
        Close
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerInteraction();
        }
    }

    public void PlayerInteraction()
    {
        if (triggerAction == Action.Open)
        {
            mover.Open();
        }
        else if(triggerAction == Action.Close)
        {
            mover.Close();
        }
    }
}
