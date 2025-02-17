using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    private static readonly int IsMoving = Animator.StringToHash("IsMove"); // ���ڷ� ���ϴ� �ͺ��� ���ڷ� ���ϴ� ���� ������ Hash�� ���ؼ� �������.
    private static readonly int IsDamage = Animator.StringToHash("IsDamage");

    protected Animator animator;

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>(); // �ڽ��� MainSprite�� �޷��ִ� animator�� ������� �Ŵϱ� InChildren
    }

    public void Move(Vector2 obj)
    {
        animator.SetBool(IsMoving, obj.magnitude > 0.5f);
    }

    public void Damage()
    {
        animator.SetBool(IsDamage, true);
    }

    public void InvincibilityEnd() // ������ ������ �ð��� üũ�� ����
    {
        animator.SetBool(IsDamage, false);
    }
}
