using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : BaseController
{
    private Camera camera;

    private GameManager gameManager;

    public void Init(GameManager gameManager)
    {
        this.gameManager = gameManager;
        camera = Camera.main;
    }

    protected override void HandleAction()
    {

        
    }

    public override void Death()
    {
        base.Death();
        gameManager.GameOver();
    }

    void OnMove(InputValue inputValue)
    {
        movementDirection = inputValue.Get<Vector2>();
        movementDirection = movementDirection.normalized;
    }

    void OnLook(InputValue inputValue)
    {
        Vector2 mousePosition = inputValue.Get<Vector2>();
        Vector2 worldPos = camera.ScreenToWorldPoint(mousePosition); // 화면상 좌표를 우리가 원하는 월드좌표로 변환
        lookDirection = (worldPos - (Vector2)transform.position);

        if (lookDirection.magnitude/*벡터의 크기*/ < 0.9f)
        {
            lookDirection = Vector2.zero;
        }
        else
        {
            lookDirection = lookDirection.normalized;
        }
    }

    void OnFire(InputValue inputValue)
    {
        if (EventSystem.current.IsPointerOverGameObject()) return; // UI에 마우스가 올라가있으면 공격체 쏘지 않게.
        isAttacking = inputValue.isPressed;
    }
}
