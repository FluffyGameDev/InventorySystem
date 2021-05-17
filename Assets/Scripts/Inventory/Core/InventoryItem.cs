using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Inventory/InventoryItem")]
    public class InventoryItem : ScriptableObject
    {
        public string Name;
        public Sprite Sprite;
    }
}
