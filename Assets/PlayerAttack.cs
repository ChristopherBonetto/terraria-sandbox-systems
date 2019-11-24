using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerAttackType
{
    Melee,
    Plunge,
    Ranged
}

public class PlayerAttack : MonoBehaviour
{
    /// <summary>
    /// Choose player attack type
    /// </summary>

    public PlayerAttackType AttackType { get; set; } = PlayerAttackType.Melee;

    /// <summary>
    /// Weapon sprite reference.
    /// </summary>

    public SpriteRenderer WeaponIcon { get { return m_WeaponIcon; } }


    // Attack variables

    [Header("variable for every attack type")]
    public LayerMask InteractableLayer;
    public float Damage { get; set; } = 1;


    [Header("Melee Attack")]
    public float MeleeRange;
    public float MeleeAngle;


    [Header("Plunge Attack")]
    public float Range;

    [Header("Visual")]
    [SerializeField] private SpriteRenderer m_WeaponIcon;

    private Collider2D m_DetectedCollider;

    private void Update()
    {
        switch (AttackType)
        {
            case PlayerAttackType.Melee:
                MeleeAttack();
                break;
            case PlayerAttackType.Plunge:
                PlungeAttack();
                break;
            case PlayerAttackType.Ranged:
                break;
            default:
                break;
        }
    }

    private void OnDisable()
    {
        m_DetectedCollider = null;
    }

    private void MeleeAttack()
    {
        var detectedEntity = Physics2D.OverlapCircle(transform.position, MeleeRange, InteractableLayer);

        // 1) Check if entity detected is not equal to the last one.
        // 2) Check the angle.
        // 3) Get multiple component, we don't know what component it has.
        // @TODO : maybe group up common component between entity inside one "interface => IController"
        // 4) Calculate opposite direction of the collision... 
        // Applies effects
        // 6) store enemy hit.


        // 1)
        if (detectedEntity != null && detectedEntity != m_DetectedCollider)
        {
            // 2)
            var angle = Vector3.Angle(-transform.up, (detectedEntity.transform.position - transform.position));
            if (Mathf.Abs(angle) <= MeleeAngle / 2)
            {
                // 3)
                IDefend entity = detectedEntity.GetComponent<IDefend>();
                IKnockBackable ent = detectedEntity.GetComponent<IKnockBackable>();

                // 4)
                Vector2 knockEffect = (-transform.up * GeneralEffects.KbEffect(ent.KbResist)) + Vector3.up * GeneralEffects.KbGlobalEffect * 0.5f;

                entity?.TakeDamage(Damage);
                ent?.KnockBack(knockEffect);

                // 5)
                m_DetectedCollider = detectedEntity;
            }
        }
    }

    private void PlungeAttack()
    {
        // line cast from current position of the hand to -transform.up
        // The hand is animated and rotated, so -transform.up is the right coordinate.

        RaycastHit2D hit = Physics2D.Linecast(transform.position, -transform.up * Range, InteractableLayer);

        if (hit)
        {
            IDefend entity = hit.collider.GetComponent<IDefend>();
            IKnockBackable ent = hit.collider.GetComponent<IKnockBackable>();

            Vector2 knockEffect = (-transform.up * GeneralEffects.KbEffect(ent.KbResist)) + Vector3.up * GeneralEffects.KbGlobalEffect * 0.5f;

            entity?.TakeDamage(Damage);
            ent?.KnockBack(knockEffect);

            m_DetectedCollider = hit.collider;
        }
    }
}
