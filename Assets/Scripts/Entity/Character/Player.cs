using System.Collections;
using System.Collections.Generic;
using DungeonArchitect.Samples.GridFlow;
using UnityEngine;

public class Player : Character
{
    
    [SerializeField] Transform RightHand;
    SurvivalStats survivalStats = new SurvivalStats();
    List<Item> IvnentoryItems;
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
        lifecycleEventActions[LifecycleEventType.Update]?.Invoke();
    }
    public override void Init()
    {
        base.Init();

        inventory = new Inventory(this, IvnentoryItems, survivalStats.inventoryCapacity);
        joystick = Utils.GetUI<FloatingJoystick>();


        lifecycleEventActions[LifecycleEventType.Update] = Move;
    }
    void Move() { SimpleMove(direction); }
    void MeleeAttack()
    {
        base.MeleeAttack(meleeWeaponController);
    }
}
