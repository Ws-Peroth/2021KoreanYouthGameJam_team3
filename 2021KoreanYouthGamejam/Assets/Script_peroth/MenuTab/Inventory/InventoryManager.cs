using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

namespace peroth
{
    public class InventoryManager : Singleton<InventoryManager>
    {
        private readonly string itemInventoryfilePath = "./Assets/ItemInventory.json";
        private readonly string itemInformationfilePath = "./Assets/ItemInformation.json";

        private List<ItemIcon> itemIconList = new List<ItemIcon>();
        public List<Sprite> imageList = new List<Sprite>();

        [SerializeField] private Transform iconTransform;
        [SerializeField] private Text itemInformationText;
        [SerializeField] private Text itemNameText;
        [SerializeField] private Image itemimage;

        public ItemInformationList itemInformationList;

        private void Start()
        {
            itemInformationText.text = "";
            itemNameText.text = "";

            // 파일 초기화
            if (!File.Exists(itemInformationfilePath)) Debug.LogError("초기화 파일이 존재하지 않음");
            if (!File.Exists(itemInventoryfilePath)) InitializeInventoryJsonFile();

            var loadJson = File.ReadAllText(itemInventoryfilePath);
            itemInformationList = JsonConvert.DeserializeObject<ItemInformationList>(loadJson);

            // Json에 저장되어 있는 아이템의 상태로 초기화
            CreatIcon();
        }

        private void InitializeInventoryJsonFile()
        {
            var loadJson = File.ReadAllText(itemInformationfilePath);
            var tempList = JsonConvert.DeserializeObject<ItemInformationList>(loadJson);

            string json = JsonConvert.SerializeObject(tempList, Formatting.Indented);
            File.WriteAllText(itemInventoryfilePath, json);
        }

        public void ResetInventory()
        {
            var loadJson = File.ReadAllText(itemInformationfilePath);
            var tempList = JsonConvert.DeserializeObject<ItemInformationList>(loadJson);

            string json = JsonConvert.SerializeObject(tempList, Formatting.Indented);
            itemInformationList = JsonConvert.DeserializeObject<ItemInformationList>(loadJson);
            File.WriteAllText(itemInventoryfilePath, json);
        }

        public void AddItem(ItemCode itemCode, int addCount = 1)
        {
            itemInformationList.list[ItemCodeEnumToInt(itemCode)].AddItem(addCount);
        }

        public void SetItemCount(ItemCode itemCode, int setCount)
        {
            itemInformationList.list[ItemCodeEnumToInt(itemCode)].SetItemCount(setCount);
        }

        public void SaveItem()
        {
            string json = JsonConvert.SerializeObject(itemInformationList, Formatting.Indented);
            File.WriteAllText(itemInventoryfilePath, json);
        }

        public void CreatIcon()
        {
            ClearBeforeIcons();

            int haveingCount = 0;
            GameObject icon;

            // 현재 보유 상태에 따른 아이콘 생성
            for (var i = 0; i < itemInformationList.list.Count; i++)
            {
                for (var j = 0; j < itemInformationList.list[i].havingCount; j++)
                {
                    icon = ItemIconPoolManager.instance.CreatItemIcon();
                    InitializeIcon(icon, itemInformationList.list[i].itemCode);
                    haveingCount++;
                }
            }

            // 기본 8칸 생성 : 8 - 총 생성한 아이콘 수
            if (haveingCount < 8)
            {
                for (var i = haveingCount; i < 8; i++)
                {
                    icon = ItemIconPoolManager.instance.CreatItemIcon();
                    InitializeIcon(icon, ItemCodeEnumToInt(ItemCode.BlankItem));
                    haveingCount++;
                }
            }
        }

        private void ClearBeforeIcons()
        {
            for(int i = 0; i < itemIconList.Count; i++)
            {
                if (itemIconList[i].gameObject.activeSelf)
                    itemIconList[i].DestroyIcon();
                else
                {
                    itemIconList[i].transform.SetParent(itemIconList[i].destroyTransform);
                    ItemIconPoolManager.instance.DestroyItemIcon(gameObject);
                }

            }
        }

        private void InitializeIcon(GameObject icon, int itemCode)
        {
            icon.transform.SetParent(iconTransform);
            icon.transform.localScale = Vector3.one;
            ItemIcon tempIcon = icon.GetComponent<ItemIcon>();
            tempIcon.itemCode = itemCode;
            itemIconList.Add(tempIcon);
            icon.GetComponent<Image>().sprite = imageList[itemCode];
        }

        public void IconButtonDown(int iconCode)
        {
            if (iconCode == -1) Debug.LogError("iconCode Initialize Failed");

            itemInformationText.text = itemInformationList.list[iconCode].information;
            itemNameText.text = itemInformationList.list[iconCode].name;

            itemimage.sprite = imageList[iconCode];
        }

        #region EnumConversion
        public int ItemCodeEnumToInt(ItemCode itemCode) =>
            itemCode switch
            {
                ItemCode.ItemA => 1,
                ItemCode.ItemB => 2,
                ItemCode.ItemC => 3,
                ItemCode.ItemD => 4,
                ItemCode.ItemE => 5,
                ItemCode.ItemF => 6,
                ItemCode.ItemG => 7,
                ItemCode.ItemH => 8,
                _ => 0,
            };

        public ItemCode IntToItemCodeEnum(int itemCode) =>
            itemCode switch
            {
                1 => ItemCode.ItemA,
                2 => ItemCode.ItemB,
                3 => ItemCode.ItemC,
                4 => ItemCode.ItemD,
                5 => ItemCode.ItemE,
                6 => ItemCode.ItemF,
                7 => ItemCode.ItemG,
                8 => ItemCode.ItemH,
                _ => 0,
            };
        #endregion
    }

    public class ItemInformationList
    {
        public List<ItemInformation> list = new List<ItemInformation>();
    }

    public struct ItemInformation
    {
        public string name;
        public string information;
        public int havingCount;
        public int itemCode;

        public ItemInformation(string name, string information, int havingCount, int itemCode)
        {
            this.name = name;
            this.information = information;
            this.havingCount = havingCount;
            this.itemCode = itemCode;
        }

        private void SetHavingCount(int value)
        {
            havingCount = value <= 0 ? 0 : value;
        }

        public int AddItem(int addCount)
        {
            SetHavingCount(havingCount + addCount);
            return havingCount;
        }

        public int SetItemCount(int setCount)
        {
            SetHavingCount(setCount);
            return havingCount;
        }
    }

    public enum ItemCode
    {
        BlankItem,
        ItemA,
        ItemB,
        ItemC,
        ItemD,
        ItemE,
        ItemF,
        ItemG,
        ItemH
    }
}