using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

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
        float horizontal = Input.GetAxisRaw("Horizontal"); // ProjectSettings > InputManager�� ���ǵ� ��� ��� 
        float vertical = Input.GetAxisRaw("Vertical");
        movementDirection = new Vector2(horizontal, vertical).normalized;

        Vector2 mousePosition = Input.mousePosition;
        Vector2 worldPos = camera.ScreenToWorldPoint(mousePosition); // ȭ��� ��ǥ�� �츮�� ���ϴ� ������ǥ�� ��ȯ
        lookDirection = (worldPos - (Vector2)transform.position);

        if(lookDirection.magnitude/*������ ũ��*/ < 0.9f)
        {
            lookDirection = Vector2.zero;
        }
        else
        {
            lookDirection = lookDirection.normalized;
        }

        isAttacking = Input.GetMouseButton(0);
    }

    public override void Death()
    {
        base.Death();
        gameManager.GameOver();
    }
}
