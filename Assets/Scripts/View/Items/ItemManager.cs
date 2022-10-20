using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace View.Items
{
    public class ItemManager : MonoBehaviour
    {
        [SerializeField] GameObject _itemPrefab;
        private List<Item> _Items = new List<Item>();
        //アイテムテーブルから取得するアイテム数
        private int _numItems = 0;
        // アイテム生成の間隔
        private const float TimeOut = 3;

        // 一定間隔でアイテムを生成する
        public void StartGenerateItems(int numItems)
        {
            _numItems = numItems;
            StartCoroutine("GenerateItems");
        }
        // アイテム生成ストップ
        public void StopGenerateItems()
        {
            StopCoroutine("GenerateItems");
        }
        IEnumerator GenerateItems()
        {
            while (true)
            {
                GenerateItem();
                yield return new WaitForSeconds(TimeOut);
            }
        }
        // アイテム生成
        private void GenerateItem()
        {
            Vector2 position = new Vector2(Random.Range(-14, 14), 10);// ステージのサイズにより変更
            GameObject go = Instantiate(_itemPrefab, position, Quaternion.identity) as GameObject;
            Item item = go.GetComponent<Item>();
            item.SetItemId(_numItems);
            _Items.Add(item);
        }
        // アイテム収集
        public void ItemCollected(GameObject item)
        {
            _Items.Remove(item.GetComponent<Item>());
            item.GetComponent<Item>().Collected();
        }
    }
}

