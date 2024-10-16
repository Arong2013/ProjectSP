using UnityEngine;

public class Box : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("The box is opened, and an item is dropped!");
        OpenBox();
    }

    private void OpenBox()
    {
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Open");
        }

        // 아이템 드롭 로직 (예시)
        DropItem();
    }

    private void DropItem()
    {
        // 간단한 아이템 드롭 예제
        Debug.Log("Item dropped from the box!");
    }
}
