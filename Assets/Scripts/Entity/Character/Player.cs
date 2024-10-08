using System.Collections;
using System.Collections.Generic;
using DungeonArchitect.Samples.GridFlow;
using UnityEngine;

public class Player : Character
{
    SurvivalStats survivalStats = new SurvivalStats();
    List<Item> IvnentoryItems;
    Joystick joystick;
    Vector3 direction => joystick.Direction;
    Inventory inventory;
    public Player()
    {

    }
    public override void Init(GameObject gameObject)
    {
        base.Init(gameObject);
  
  
        inventory = new Inventory(this,IvnentoryItems,survivalStats.inventoryCapacity);
        joystick = Utils.GetUI<FloatingJoystick>();
  
  
        lifecycleEventActions[LifecycleEventType.Update] = Move;
    }
    void Move() { SimpleMove(direction); }

    public void EquipmentWeapon()
    {

    }
}
