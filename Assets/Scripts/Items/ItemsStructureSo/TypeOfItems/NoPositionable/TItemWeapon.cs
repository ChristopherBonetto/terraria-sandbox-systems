using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [Flags] ?
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
    [SerializeField]
    public struct PlayerAndEnvInteraction
    {
        public WeaponInteractWithTile interactWithEnv;
        public PlayerAttackType AttackType;
        public Animator PlayerOverrideController;
        public LayerMask InteractableLayer;
    }

    public PlayerAndEnvInteraction VisualAndInteraction;

    [Header("Stats")]

    public int Attack = 1;
    public float AttackSpeed = 1;


    public override bool Use(TPlayerController user, TPointerData inData)
    {
        if (user.CanAttack())
        {
            return true;
        }
        return false;
    }
}
