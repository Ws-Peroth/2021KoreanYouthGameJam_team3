using UnityEngine;

namespace peroth
{
    public class ItemIcon : MonoBehaviour
    {
        public int itemCode = -1;

        public void IconButtonDown()
        {
            InventoryManager.instance.IconButtonDown(itemCode);
        }

        public void DestroyIcon()
        {
            ItemIconPoolManager.instance.DestroyItemIcon(gameObject);
        }
    }
}