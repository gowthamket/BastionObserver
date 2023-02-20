using System.Collections;
using UnityEngine;

public class PlayerAttackState : PlayerBaseState, IRootState
{

    public PlayerAttackState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory)
    {
        IsRootState = true;
    }

    public override void EnterState()
    {
        Ctx.AppliedMovementX = 0;
        Ctx.AppliedMovementZ = 0;
        Ctx.Animator.SetBool(Ctx.IsAttackingHash, true);
        HandleGravity();
        Ctx.IsAttackComplete = false;
    }

    public override void UpdateState()
    {
        HandleGravity();
        Attack();
        CheckSwitchStates();
    }

    public override void CheckSwitchStates()
    {
        if (Ctx.IsAttackComplete)
        {
            SwitchState(Factory.Grounded());
        }
    }

    public override void ExitState()
    {
        Ctx.AttackCount = 0;
        Ctx.Animator.SetInteger(Ctx.AttackCountHash, 0);
        Ctx.Animator.SetBool(Ctx.IsAttackingHash, false);
    }

    public void HandleGravity()
    {
        Ctx.CurrentMovementY = Ctx.Gravity;
        Ctx.AppliedMovementY = Ctx.Gravity;
    }

    private void Attack()
    {
        if (Ctx.IsAttackSphereEnabled)
        {
            Vector3 attackPosition = Ctx.transform.position + Ctx.transform.forward * 1.5f;
            Collider[] colliders = Physics.OverlapSphere(Ctx.transform.position, 1.5f, Ctx.AttackableLayer);
            foreach (Collider currentCollider in colliders)
            {
              Slime currentEnemy = currentCollider.GetComponent<Slime>();
              Debug.Log(currentEnemy);
              currentEnemy.TakeDamage(40, Ctx.transform.forward);
            }
        }
    }

    public override void InitializeSubState() { }
}
