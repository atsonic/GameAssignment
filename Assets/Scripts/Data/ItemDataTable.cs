using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class ItemDataTable
    {
        // 各アイテムのデータテーブル（★★★★★★ScriptableObjectでやった方が良いか）
        private Dictionary<int, int> _dic = new Dictionary<int, int>(){
            {1, 100},//Apple
            {2, 200},//Banana
            {3, 250},//Cherry
            {4, 150},//Kiwi
            {5, 250},//Melon
            {6, 200},//Pinapple
            {7, 50},//Orange
            {8, 50},//Strawberry
            {9, -100},//Saw
        };
        // ID（ItemのItemId）からポイントを取得
        public int SearchPoint(int id)
        {
            int point = _dic[id];
            return point;
        }
        // アイテムテーブルの個数を取得（アイテム決定のランダムに必要）
        public int GetNumItems()
        {
            return _dic.Count;
        }
    }
}