using System.Collections.Generic;
using UnityEngine;

namespace peroth
{
    public class ItemIconPoolManager : Singleton<ItemIconPoolManager>
    {
        [SerializeField] private GameObject itemIconPrefab;
        private readonly Queue<GameObject> itemIconPool = new Queue<GameObject>();
        private GameObject instantiateTemp;

        public GameObject CreatItemIcon()
        {
            instantiateTemp = null;
            if (itemIconPool.Count <= 0) instantiateTemp = Instantiate(itemIconPrefab);
            else instantiateTemp = itemIconPool.Dequeue();

            if (instantiateTemp == null) Debug.LogError("Creat Null Object");
            instantiateTemp.SetActive(true);
            return instantiateTemp;
        }

        public void DestroyItemIcon(GameObject obj)
        {
            obj.SetActive(false);
            Debug.Log("ItemIconPoolManager : DestroyItemIcon");
            itemIconPool.Enqueue(obj);
        }
    }
}