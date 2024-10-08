using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMono : MonoBehaviour
{
    Entity entity;

    private void Update()
    {
        entity.lifecycleEventActions.GetValueOrDefault(LifecycleEventType.Update)?.Invoke();
    }
    private void HandleEvent(PhysicsEventType eventType, object data)
    {
        if (entity == null)
        {
            Debug.LogWarning("Actor is not initialized.");
            return;
        }
        switch (data)
        {
            case Collider collider when entity.triggerEventActions != null && entity.triggerEventActions.TryGetValue(eventType, out var triggerAction):
                triggerAction?.Invoke(collider);
                break;
            case Collision collision when entity.collisionEventActions != null && entity.collisionEventActions.TryGetValue(eventType, out var collisionAction):
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
