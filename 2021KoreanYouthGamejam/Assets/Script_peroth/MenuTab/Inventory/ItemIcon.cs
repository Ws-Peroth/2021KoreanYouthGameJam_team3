using UnityEngine;

namespace peroth
{
    public class ItemIcon : MonoBehaviour
    {
        public Transform destroyTransform;
        public int itemCode = -1;

        public void IconButtonDown()
        {
            InventoryManager.instance.IconButtonDown(itemCode);
        }

        public void DestroyIcon()
        {
            transform.SetParent(destroyTransform);
            ItemIconPoolManager.instance.DestroyItemIcon(gameObject);
        }
    }
}