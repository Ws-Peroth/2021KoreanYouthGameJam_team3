using UnityEngine;

namespace peroth
{
    public class ItemIcon : MonoBehaviour
    {
        public GameObject selfGameObject;
        // public Transform selfTransform;
        // public Transform destroyTransform;
        public int itemCode = -1;

        public void IconButtonDown()
        {
            InventoryManager.instance.IconButtonDown(itemCode);
        }

        public void DestroyIcon()
        {
            Transform destroyTransform = InventoryManager.instance.selfTransform;
            Debug.Log($"destroyTransform = {destroyTransform.name}\nnow Transform = {transform.parent.name}");
            
            transform.parent = destroyTransform;
            transform.SetParent(destroyTransform);
            
            Debug.Log($"now Transform = {transform.parent.name}");
            ItemIconPoolManager.instance.DestroyItemIcon(gameObject);
        }

        private void OnDestroy()
        {
            Debug.Log("Icon Destroy");
        }
    }
}