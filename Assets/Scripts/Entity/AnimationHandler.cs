using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    private static readonly int IsMoving = Animator.StringToHash("IsMove"); // 문자로 비교하는 것보다 숫자로 비교하는 것이 좋으니 Hash값 구해서 쓰기로함.
    private static readonly int IsDamage = Animator.StringToHash("IsDamage");

    protected Animator animator;

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>(); // 자식인 MainSprite에 달려있는 animator를 갖고오는 거니까 InChildren
    }

    public void Move(Vector2 obj)
    {
        animator.SetBool(IsMoving, obj.magnitude > 0.5f);
    }

    public void Damage()
    {
        animator.SetBool(IsDamage, true);
    }

    public void InvincibilityEnd() // 무적이 끝나는 시간을 체크할 거임
    {
        animator.SetBool(IsDamage, false);
    }
}
