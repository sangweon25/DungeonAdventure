using UnityEngine;

public class MeleeWeapon : WeaponHandler
{
    [Header("Melee Attack")]
    public Vector2 collideBoxSize = Vector2.one;

    protected override void Start()
    {
        base.Start();
        collideBoxSize *= WeaponSize;
    }

    public override void Attack()
    {
        base.Attack();

        RaycastHit2D hit = Physics2D.BoxCast(
            transform.position + (Vector3)Controller.LookDirection * collideBoxSize.x,
            collideBoxSize,
            0,
            Vector2.zero,
            0,
            target);

        if (hit.collider != null)
        {
            ResourceController resourceController = hit.collider.GetComponent<ResourceController>();
            if (resourceController != null)
            {
                resourceController.ChangeHealth(-AtkPower);
                if (IsOnKnockback)
                {
                    BaseController baseController = hit.collider.GetComponent<BaseController>();
                    if (baseController != null)
                    {
                        Controller.ApplyKnockback(transform, KnockbackPower, KnockbackTime);
                    }
                }
            }
        }
    }

    public override void FlipWeapon(bool isLeft)
    {
        if (isLeft)
            transform.eulerAngles = new Vector3(0, 0, 180);
        else
            transform.eulerAngles = new Vector3(0, 0, 0);
    }
}
