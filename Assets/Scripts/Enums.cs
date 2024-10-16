public enum LifecycleEventType
{
    Awake,
    OnEnable,
    Start,
    Update,
    OnDisable,
    OnDestroy,
}
public enum PhysicsEventType
{
    OnTriggerEnter,
    OnTriggerExit,
    OnCollisionEnter,
    OnCollisionExit,
    OnCollisionStay,
    OnTriggerStay,
}


public abstract class EnemyHFSMState
{
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
}

public interface IInteractable
{
    void Interact(); 
}