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
        public Transform selfTransform;

        public bool isDebug;
        private string itemInformationfilePath;
        private string itemInventoryfilePath;

        private readonly List<ItemIcon> itemIconList = new List<ItemIcon>();

        public ItemInformationList itemInformationList;

        private void Start()
        {
            itemInformationfilePath = $"{Application.persistentDataPath}/ItemInformation.json";
            itemInventoryfilePath = $"{Application.persistentDataPath}/ItemInventory.json";

            itemInformationText.text = "";
            itemNameText.text = "";

            // 파일 초기화
            if (!File.Exists(itemInformationfilePath)) InitializeInformationJsonFile();
            if (!File.Exists(itemInventoryfilePath)) InitializeInventoryJsonFile();

            UpdateInventory();

            // Json에 저장되어 있는 아이템의 상태로 초기화
            CreatIcon();

            Debug.Log("StartManager");
        }

        private void InitializeInformationJsonFile()
        {
            var loadJson = Resources.Load("Information") as TextAsset;
            var tempList = JsonConvert.DeserializeObject<ItemInformationList>(loadJson.ToString());

            var json = JsonConvert.SerializeObject(tempList, Formatting.Indented);
            File.WriteAllText(itemInformationfilePath, json);
        }

        private void InitializeInventoryJsonFile()
        {
            var loadJson = File.ReadAllText(itemInformationfilePath);
            var tempList = JsonConvert.DeserializeObject<ItemInformationList>(loadJson);

            var json = JsonConvert.SerializeObject(tempList, Formatting.Indented);
            File.WriteAllText(itemInventoryfilePath, json);
        }

        public void ResetInventory()
        {
            var loadJson = File.ReadAllText(itemInformationfilePath);
            var tempList = JsonConvert.DeserializeObject<ItemInformationList>(loadJson);

            var json = JsonConvert.SerializeObject(tempList, Formatting.Indented);
            itemInformationList = JsonConvert.DeserializeObject<ItemInformationList>(loadJson);
            File.WriteAllText(itemInventoryfilePath, json);
        }

        public int GetItemHavingCount(ItemCode itemCode) => itemInformationList.list[ItemCodeEnumToInt(itemCode)].havingCount;

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
            var json = JsonConvert.SerializeObject(itemInformationList, Formatting.Indented);
            File.WriteAllText(itemInventoryfilePath, json);
        }

        public void UpdateInventory()
        {
            var loadJson = File.ReadAllText(itemInventoryfilePath);
            itemInformationList = JsonConvert.DeserializeObject<ItemInformationList>(loadJson);
        }

        public void CreatIcon()
        {
            ClearBeforeIcons();

            if (isDebug) UpdateInventory();

            var haveingCount = 0;
            GameObject icon;

            // 현재 보유 상태에 따른 아이콘 생성
            for (var i = 0; i < itemInformationList.list.Count; i++)
            for (var j = 0; j < itemInformationList.list[i].havingCount; j++)
            {
                icon = ItemIconPoolManager.instance.CreatItemIcon();
                InitializeIcon(icon, itemInformationList.list[i].itemCode);
                haveingCount++;
            }

            // 기본 8칸 생성 : 8 - 총 생성한 아이콘 수
            if (haveingCount < 8)
                for (var i = haveingCount; i < 8; i++)
                {
                    icon = ItemIconPoolManager.instance.CreatItemIcon();
                    InitializeIcon(icon, ItemCodeEnumToInt(ItemCode.BlankItem));
                    haveingCount++;
                }
        }

        private void ClearBeforeIcons()
        {
            for (var i = 0; i < itemIconList.Count; i++)
                if (itemIconList[i].selfGameObject.activeSelf)
                    itemIconList[i].DestroyIcon();
        }

        private void InitializeIcon(GameObject icon, int itemCode)
        {
            icon.transform.SetParent(iconTransform);
            icon.transform.localScale = Vector3.one;
            var tempIcon = icon.GetComponent<ItemIcon>();
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

        public int ItemCodeEnumToInt(ItemCode itemCode)
        {
            return itemCode switch
            {
                ItemCode.ItemA => 1,
                ItemCode.ItemB => 2,
                ItemCode.ItemC => 3,
                ItemCode.ItemD => 4,
                ItemCode.ItemE => 5,
                ItemCode.ItemF => 6,
                ItemCode.ItemG => 7,
                ItemCode.ItemH => 8,
                _ => 0
            };
        }

        public ItemCode IntToItemCodeEnum(int itemCode)
        {
            return itemCode switch
            {
                1 => ItemCode.ItemA,
                2 => ItemCode.ItemB,
                3 => ItemCode.ItemC,
                4 => ItemCode.ItemD,
                5 => ItemCode.ItemE,
                6 => ItemCode.ItemF,
                7 => ItemCode.ItemG,
                8 => ItemCode.ItemH,
                _ => 0
            };
        }

        #endregion
    }
}