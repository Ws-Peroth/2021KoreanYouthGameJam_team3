using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace peroth
{
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
        ItemA,  // 투명화
        ItemB,  // 영상
        ItemC,  // 편지
        ItemD,
        ItemE,
        ItemF,
        ItemG,
        ItemH
    }
}
