using FantasyTown.Saving;
using System.Collections.Generic;
using UnityEngine;

namespace FantasyTown.Inventory
{
    public class Inventory : MonoBehaviour, ISaveable
    {
        private Dictionary<string, int> items = new Dictionary<string, int>();

        public List<string> keys;
        public List<int> values;

        public void CollectItem(Item newItem)
        {
            if (items.ContainsKey(newItem.itemName))
            {
                items[newItem.itemName]++;
            }
            else
            {
                items.Add(newItem.itemName, 1);
            }
        }

        public void CaptureState()
        {
            SavingWrapper.Instance.Data.savedItems = items;

           
        }

        public void RestoreState()
        {
            items = SavingWrapper.Instance.Data.savedItems;


            foreach (var ele in items)
            {
                keys.Add(ele.Key);
                values.Add(ele.Value);
            }
        }

      
    }
}


