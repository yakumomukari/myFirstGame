using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sign : MonoBehaviour
{
    private Animator anim;
    private PlayerInputControl playerInput;
    public GameObject signSprite;
    public Transform playerTrans;
    private PlayerController playerController;
    public IInteractable targetItem;
    public Collider2D targetItemDetail;
    public AudioSource FXsource;
    public AudioClip teleportFX;
    public bool canPress;
    private void Awake()
    {
        // anim = GetComponentInChildren<Animator>();
        anim = signSprite.GetComponent<Animator>();

        playerInput = new PlayerInputControl();
        playerInput.Enable();
        InputSystem.onActionChange += OnActionChange;
        playerInput.Gameplay.Confirm.started += OnConfirm;
    }

    private void Update()
    {
        // signSprite.SetActive(canPress);
        signSprite.GetComponent<SpriteRenderer>().enabled = canPress;
        if (signSprite.transform.localScale.x * playerTrans.localScale.x < 0)
        {
            signSprite.transform.localScale = new Vector3(-signSprite.transform.localScale.x, signSprite.transform.localScale.y, signSprite.transform.localScale.z);
        }
    }
    private void OnConfirm(InputAction.CallbackContext context)
    {
        if (canPress)
        {
            targetItem.TriggerAction();
            if (targetItemDetail.CompareTag("Teleport"))
            {
                FXsource.PlayOneShot(teleportFX);
            }
        }
    }

    private void OnActionChange(object obj, InputActionChange actionChange)
    {
        if (actionChange == InputActionChange.ActionStarted)
        {
            // Debug.Log(((InputAction)obj).activeControl.device);
            var dev = ((InputAction)obj).activeControl.device;
            switch (dev.device)
            {
                case Keyboard:
                    anim.Play("Keyboard-E");
                    break;
                case Gamepad:
                    anim.Play("GamePad-Y");
                    break;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Interactable") || other.CompareTag("Teleport"))
        {
            // 若玩家处于滑铲或攻击状态，禁止交互
            if (playerController == null && playerTrans != null)
            {
                playerController = playerTrans.GetComponent<PlayerController>();
            }

            bool blocked = false;
            if (playerController != null)
            {
                if (playerController.isSlide || playerController.isAttack) blocked = true;
            }

            canPress = !blocked;
            if (canPress)
            {
                targetItem = other.GetComponent<IInteractable>();
                targetItemDetail = other;
            }
        }
        else
        {
            canPress = false;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        canPress = false;
    }
}

