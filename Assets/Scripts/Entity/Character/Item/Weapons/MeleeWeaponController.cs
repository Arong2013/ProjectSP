using System;
using UnityEngine;

public class MeleeWeaponController : Entity
{
    [SerializeField] ItemData testWeaponData;
    MeleeWeaponItem meleeWeaponItem;
    private BoxCollider weaponCollider;
    private Character ownerCharacter; // 자신의 캐릭터 (부모)

    private void Start()
    {
        Init();
        meleeWeaponItem = testWeaponData.item.Clone() as MeleeWeaponItem;
    }

    public override void Init()
    {
        weaponCollider = GetComponent<BoxCollider>();
        weaponCollider.enabled = false;
        ownerCharacter = GetComponentInParent<Character>();
    }
    public void EnableCollider(bool isEnabled)
    {
        weaponCollider.enabled = isEnabled;
    }
    public void SetDamge(Collider collider)
    {
        if (collider.TryGetComponent<Character>(out Character hitCharacter) && hitCharacter != ownerCharacter)
        {
            hitCharacter.TakeDamage(meleeWeaponItem.combatStats.attack.Value);
        }
    }
}
