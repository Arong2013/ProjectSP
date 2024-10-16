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
    [SerializeField] float jumpForce = 5f; // 추가된 필드

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        lifecycleEventActions[LifecycleEventType.Update]?.Invoke();
        HandleTouchInput();
    }

    public override void Init()
    {
        base.Init();

        inventory = new Inventory(this, InventoryItems, survivalStats.inventoryCapacity);
        joystick = Utils.GetUI<FloatingJoystick>();

        lifecycleEventActions[LifecycleEventType.Update] = Move;
    }

    void Move() { SimpleMove(direction); }
    void MeleeAttack() { base.MeleeAttack(meleeWeaponController); }

    void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                    interactable?.Interact();
                }
            }
        }
    }
}