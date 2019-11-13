using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TWeaponComponent : MonoBehaviour
{
    private ItemScriptable m_ItemInHand;    // @TEMP change this into weapon type.
    public ItemScriptable ItemInHand        // @TEMP change this into weapon type.
    {
        get { return m_ItemInHand; }
        set
        {
            m_ItemInHand = value;

            // @TODO when change the weapon in my hand,
            // change shoot total coolDown, range, and other stats.
        }
    }

    private bool m_HoldingWeapon;
    public bool HoldingWeapon
    {
        get { return m_HoldingWeapon; }
        set
        {
            m_HoldingWeapon = value;

            // @TODO if holding weapon get its stats.
            // if holding an item reset default stats.
        }
    }


    [Header("Weapon variables")]
    [SerializeField]
    private float m_InteractionRange;
    public float InteractionRange
    {
        get { return m_InteractionRange * m_InteractionRange; }
        set { m_InteractionRange = value; }
    }

    [SerializeField]
    private float m_TotalCoolDown;
    private float m_CurrentCoolDown;

    private bool m_InCoolDown;
    public bool InCoolDown
    {
        get { return m_InCoolDown; }
        set { m_InCoolDown = value; }
    }


    private void Update()
    {
        // @TODO execute cool down timer.
        ExecuteCoolDownTimer();
    }

    public void CheckInteractionRange()
    {
        // how to check if anything is in player range?

        // Player position = P
        // Clicked Point = C
        // Interaction Range = R

        // (Px - Cx)^2 + (Py - Cy)^2 <= R^2 
        // if Z position is needed, add it to the equation.

        var playerPosition = transform.position;
        var clickedPosition = Input.mousePosition;

        var circle = ((playerPosition.x - clickedPosition.x) * (playerPosition.x - clickedPosition.x)) + 
                     ((playerPosition.y - clickedPosition.y) * (playerPosition.y - clickedPosition.y));

        if (circle <= InteractionRange)
        {
            // Attack
            ExecuteAttack();
        }
    }

    private void CheckItemInhand()
    {
        // if holding weapon...
            // Execute attack.    

        // if holding an item place it.
            // Check if the slot is empty
            // if yes place it.
    }

    private void ExecuteAttack()
    {
        // Check if hit something.
        // if yes do some operations.
    }

    private void ExecuteCoolDownTimer() // This one maybe go inside the player controller.
    {
        if (InCoolDown)
        {
            if (m_CurrentCoolDown > 0)
                m_CurrentCoolDown -= Time.deltaTime;
            else
                InCoolDown = false;
        }
    }
}
