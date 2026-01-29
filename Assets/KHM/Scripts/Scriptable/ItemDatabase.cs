using System.Collections.Generic;
using UnityEngine;

namespace hm
{
    [CreateAssetMenu(menuName = "Game/Item/ItemDatabase")]
    public class ItemDatabase : ScriptableObject
    {
        public List<ItemData> items;

        private Dictionary<int, ItemData> idMap;
        private Dictionary<string, ItemData> nameMap;

        public void Init()
        {
            idMap = new Dictionary<int, ItemData>();
            nameMap = new Dictionary<string, ItemData>();

            foreach (var item in items)
            {
                idMap[item.id] = item;
                nameMap[item.devName] = item;
            }
        }

        public ItemData GetById(int id)
        {
            return idMap.TryGetValue(id, out var item) ? item : null;
        }

        public ItemData GetByName(string devName)
        {
            return nameMap.TryGetValue(devName, out var item) ? item : null;
        }
    }
}
