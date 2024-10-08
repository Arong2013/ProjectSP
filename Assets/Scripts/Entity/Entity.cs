using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public abstract class Entity
{
    public Dictionary<LifecycleEventType, Action> lifecycleEventActions =
    Enum.GetValues(typeof(LifecycleEventType)).Cast<LifecycleEventType>()
        .ToDictionary(eventType => eventType, eventType => (Action)(() => { }));

    public Dictionary<PhysicsEventType, Action<Collider>> triggerEventActions =
        Enum.GetValues(typeof(PhysicsEventType)).Cast<PhysicsEventType>()
            .ToDictionary(eventType => eventType, eventType => (Action<Collider>)(collider => { }));

    public Dictionary<PhysicsEventType, Action<Collision>> collisionEventActions =
        Enum.GetValues(typeof(PhysicsEventType)).Cast<PhysicsEventType>()
            .ToDictionary(eventType => eventType, eventType => (Action<Collision>)(collision => { }));


    public abstract void Init(GameObject gameObject);
}
