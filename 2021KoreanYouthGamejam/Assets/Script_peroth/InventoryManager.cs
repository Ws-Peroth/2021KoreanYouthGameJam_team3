using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

namespace peroth
{
    public class InventoryManager : Singleton<InventoryManager>
    {
        public List<Sprite> imageList = new List<Sprite>();

        [SerializeField] private Transform iconTransform;
        [SerializeField] private Text itemInformationText;
        [SerializeField] private Text itemNameText;
        [SerializeField] private Image itemimage;

        private readonly string filePath = "./Assets/ItemInformation.json";
        private ItemInformationList itemInformationList;

        private void Start()
        {
            itemInformationText.text = "";
            itemNameText.text = "";

            if (!File.Exists(filePath))
            {
                var initJson = JsonConvert.SerializeObject(new ItemInformationList {list = new List<ItemInformation>()},
                    Formatting.Indented);
                File.WriteAllText(filePath, initJson);
            }

            var loadJson = File.ReadAllText(filePath);
            itemInformationList = JsonConvert.DeserializeObject<ItemInformationList>(loadJson);

            GameObject icon;
            for (var i = 0; i < itemInformationList.list.Count; i++)
            {
                icon = ItemIconPoolManager.instance.CreatItemIcon();
                icon.transform.SetParent(iconTransform);
                icon.transform.localScale = Vector3.one;
                icon.GetComponent<ItemIcon>().itemCode = i;
                icon.GetComponent<Image>().sprite = imageList[i];
            }
        }

        public void IconButtonDown(int iconCode)
        {
            if (iconCode == -1) Debug.LogError("iconCode Initialize Failed");

            itemInformationText.text = itemInformationList.list[iconCode].information;
            itemNameText.text = itemInformationList.list[iconCode].name;

            itemimage.sprite = imageList[iconCode];
        }
    }

    public class ItemInformationList
    {
        public List<ItemInformation> list = new List<ItemInformation>();
    }

    public struct ItemInformation
    {
        public string name;
        public string information;

        public ItemInformation(string name, string information)
        {
            this.name = name;
            this.information = information;
        }
    }
}