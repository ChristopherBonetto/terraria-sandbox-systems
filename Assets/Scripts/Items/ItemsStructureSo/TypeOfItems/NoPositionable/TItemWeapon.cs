using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [Flags] ?
/// <summary>
/// Type of environment that weapon can interact.
/// </summary>
public enum WeaponInteractWithTile
{
    None = 0,
    Foreground = 1,
    Background = 2,
    Both = 3,
}

[CreateAssetMenu(fileName = "WeaponItem", menuName = "Item/OtherItems/NoPositionable/Weapon")]
public class TItemWeapon : TItem
{
    [System.Serializable]
    public struct PlayerAndEnvInteraction
    {
        [Tooltip("Type of environment that weapon can interact")]
        public List<TWorldGroupID> DamageableWorldGroups;

        [Space]

        [Tooltip("Describe player attack type: Melee, Plunge, Range")]
        public PlayerAttackType AttackType;

        [Tooltip("This controller going to update the player's one, change the attack animation")]
        public AnimatorOverrideController PlayerOverrideController;

        [Tooltip("This one it's used to detect enemy, but it can be extended")]
        public LayerMask InteractableLayer;
    }

    /// <summary>
    /// Describe all type of interaction between weapon-player and weapon-environment
    /// </summary>
    /// 
    public PlayerAndEnvInteraction VisualAndInteraction;

    [Header("Stats")]

    public int Attack = 1;

    [Tooltip("This value change the animation speed")]
    public float AttackSpeed = 1;


    public override bool Use(TPlayerController user, TPointerData inData)
    {
        if (user.CanAttack())
        {
            if (VisualAndInteraction.DamageableWorldGroups.Count > 0 && user.IsInActionRange(inData.GridPosition))
            {
                RaycastHit2D hit = Physics2D.GetRayIntersection(CameraFollow.MainCamera.ScreenPointToRay(inData.ScreenPosition));

                if (hit.collider)
                {
                    TWorldItem hitItem = hit.collider.GetComponentInParent<TWorldItem>();

                    if (hitItem && VisualAndInteraction.DamageableWorldGroups.Contains(hitItem.GroupID))
                    {
                        hitItem.TakeDamage(Attack);
                        return true;
                    }
                }

                TTilemapManager.SharedInstance.TryDamageTile(inData.GridPosition, VisualAndInteraction.DamageableWorldGroups, Attack);
                return true;
            }
        }

        return false;
    }
}
