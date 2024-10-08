using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.UIElements;
public interface IUseableItem { public void UseItem(); }
public abstract class Item
{
    public ItemData Data{get; protected set;}
    public abstract Item Clone();
    public abstract bool IsEmpty();
}


[System.Serializable]
public class CountableItem : Item
{
    public int MaxAmount;
    public int Amount;

    public void SetAmount(int amount)
    {
        Amount = Mathf.Clamp(amount, 0, MaxAmount);
    }
    public int AddAmountAndGetExcess(int amount)
    {
        int nextAmount = Amount + amount;
        SetAmount(nextAmount);
        return (nextAmount > MaxAmount) ? (nextAmount - MaxAmount) : 0;
    }
    public CountableItem Seperate(int amount)
    {
        // ������ �Ѱ� ������ ���, ���� �Ұ�
        if (Amount <= 1) return null;

        if (amount > Amount - 1)
            amount = Amount - 1;

        Amount -= amount;

        var newItem = new CountableItem();
        newItem.Amount = amount;
        return newItem;
    }

    public override Item Clone()
    {
        var item = new CountableItem();
        item.Data = this.Data;
        item.Amount = this.Amount;
        item.MaxAmount = this.MaxAmount;
        return item;
    }
    public override bool IsEmpty() { return Amount <= 0; }
}
[System.Serializable]
public class RangedWeaponItem : EquipmentItem
{
    public int MaxAmmo;
    public int curAmmo;
    public ItemData AmmoData;
    public GameObject weaponPrefab;  
  
    public void Reload(int ammoAmount)
    {
        curAmmo = Mathf.Clamp(curAmmo + ammoAmount, 0, MaxAmmo);
    }
  
    public override Item Clone()
    {
        var item = new RangedWeaponItem
        {
            MaxAmmo = this.MaxAmmo,
            curAmmo = this.curAmmo,
            AmmoData = this.AmmoData,
            weaponPrefab = this.weaponPrefab,
            Data = this.Data,
            Durability = this.Durability
        };
        return item;
    }
}

[System.Serializable]
public class MeleeWeaponItem : EquipmentItem
{
    public GameObject weaponPrefab;

    public override Item Clone()
    {
        return new MeleeWeaponItem
        {
            weaponPrefab = this.weaponPrefab,
            Data = this.Data,
            Durability = this.Durability
        };
    }
}

[System.Serializable]
public class ArmorItem : EquipmentItem
{
    public Avatar avatar;

    public override Item Clone()
    {
        return new ArmorItem
        {
            avatar = this.avatar,
            Data = this.Data,
            Durability = this.Durability
        };
    }
}

[System.Serializable]
public abstract class EquipmentItem : Item
{
    public int Durability;

    public override bool IsEmpty()
    {
        return Durability <= 0;
    }

    public void TakeDamage(int damage)
    {
        Durability = Mathf.Max(Durability - damage, 0);
    }
}
