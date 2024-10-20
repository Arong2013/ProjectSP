using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public Dictionary<PhysicsEventType, Action<Collider>> triggerEventActions =
        Enum.GetValues(typeof(PhysicsEventType)).Cast<PhysicsEventType>()
            .ToDictionary(eventType => eventType, eventType => default(Action<Collider>));

    public Dictionary<PhysicsEventType, Action<Collision>> collisionEventActions =
        Enum.GetValues(typeof(PhysicsEventType)).Cast<PhysicsEventType>()
            .ToDictionary(eventType => eventType, eventType => default(Action<Collision>));
    public abstract void Init();
    private void HandleEvent(PhysicsEventType eventType, object data)
    {
        switch (data)
        {
            case Collider collider when triggerEventActions != null && triggerEventActions.TryGetValue(eventType, out var triggerAction):
                triggerAction?.Invoke(collider);
                break;
            case Collision collision when collisionEventActions != null && collisionEventActions.TryGetValue(eventType, out var collisionAction):
                collisionAction?.Invoke(collision);
                break;
        }
    }
    private void OnTriggerEnter(Collider other) => HandleEvent(PhysicsEventType.OnTriggerEnter, other);
    private void OnTriggerExit(Collider other) => HandleEvent(PhysicsEventType.OnTriggerExit, other);
    private void OnTriggerStay(Collider other) => HandleEvent(PhysicsEventType.OnTriggerStay, other);
    private void OnCollisionEnter(Collision collision) => HandleEvent(PhysicsEventType.OnCollisionEnter, collision);
    private void OnCollisionExit(Collision collision) => HandleEvent(PhysicsEventType.OnCollisionExit, collision);
    private void OnCollisionStay(Collision collision) => HandleEvent(PhysicsEventType.OnCollisionStay, collision);
}
