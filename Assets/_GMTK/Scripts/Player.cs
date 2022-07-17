using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;

public class Player : DicePlayer
{
    [SerializeField]
    private float interactionMaxDistance = 10f;

    public void OnPlayerAction(InputValue inputValue)
    {
        if(inputValue.isPressed)
        {
            if(GameplayManager.Instance.State == GameState.PlayerTurn)
            {
                diceThrowing.ThrowDices();
                GameplayManager.Instance.PlayerThrewDices();
            }
            if(GameplayManager.Instance.State == GameState.PlayerCanInteract)
            {
                if(!TryToInteract())
                {
                    PrepareForGame();
                    GameplayManager.Instance.ChangeState(GameState.PlayerTurn);
                }
            }
            else
            {
                TryToInteract();
            }
        }
    }

    private bool TryToInteract()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        int interactabeLayerMask = 1 << (int)Layers.Interactable;
        if(Physics.Raycast(cameraRay, out RaycastHit hit, interactionMaxDistance, interactabeLayerMask))
        {
            if(hit.collider.TryGetComponent(out IInteractable interactable))
            {
                interactable.Interact(this);
                return true;
            }
        }
        
        return false;
    }
}