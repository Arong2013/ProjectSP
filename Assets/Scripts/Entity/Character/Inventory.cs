using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Inventory
{
    Stat InventoryStat;
    List<Item> items;
    Player player;
    public Inventory(Player _player,List<Item> _items,Stat _InventoryStat)
    {
        player = _player;
        items = _items;
        InventoryStat = _InventoryStat;
    }
    public int MaxAmount => (int)InventoryStat.Value;
    public bool AddItem(Item _item)
    {
        if (_item is CountableItem countItem)
        {
            var matchingItem = items.OfType<CountableItem>().FirstOrDefault(i => i.Data == _item.Data);
            if (matchingItem != null)
            {
                countItem.SetAmount(-matchingItem.AddAmountAndGetExcess(countItem.Amount));
                if (countItem.Amount <= 0) return true;
            }
        }
        if (items.Count >= MaxAmount)
            return false;
        items.Add(_item);
        return true;
    }
}
