using System.Collections;
using System.Collections.Generic;
using DungeonArchitect.Samples.GridFlow;
using UnityEngine;

public class Player : Character
{
    [SerializeField] Transform RightHand;
    SurvivalStats survivalStats = new SurvivalStats();
    List<Item> InventoryItems; // 오타 수정
    Joystick joystick;
    Vector3 direction => joystick.Direction;
    Inventory inventory;
    [SerializeField] MeleeWeaponController meleeWeaponController;
    private void Start()
    {
        Init();
    }

    private void Update()
    {
       
    }

    public override void Init()
    {
        base.Init();

        inventory = new Inventory(this, InventoryItems, survivalStats.inventoryCapacity);
        joystick = Utils.GetUI<FloatingJoystick>();
    }

    void Move() { SimpleMove(direction); }
    void MeleeAttack() { base.MeleeAttack(meleeWeaponController); }

    public override bool IsFOVInRange(Transform target)
    {
        return true;
    }

    public override bool IsInAttackRange(Transform target)
    {
        return true;
    }
}