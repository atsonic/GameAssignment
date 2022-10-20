using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Data;
using System;
using Utils;

namespace View.UI
{
    public class RankingListUI : MonoBehaviour
    {
        [SerializeField] private RectTransform _content;
        [SerializeField] private GameObject _cellPrefab;
        [SerializeField] private Scrollbar _scrollBar;
        private List<GameObject> _cells = new List<GameObject>();
        private int _scrollPosition;
        private int _listNum;

        public void UpdateListView(int thisId, Data.Score[] ranking)
        {
            ranking.Sort( c => c.score, c => c.id );
            Array.Reverse(ranking);
            RemoveAllListViewItem();

            int i = 1; // スクロール一用の変数
            int rank  = 0; // ランク表示用の変数
            int tempScore = 0; // スコア保持用の変数
            _listNum = ranking.Length;
            foreach (var rankingData in ranking)
            {
                int id = rankingData.id; // 各データのプロパティ
                int score = rankingData.score; // 各データのプロパティ

                GameObject cell = Instantiate(_cellPrefab, _content);
                Text[] scoreText = cell.GetComponentsInChildren<Text>();
                Image backGround = cell.GetComponentInChildren<Image>();

                if(tempScore != score)
                {
                    rank += 1;
                    tempScore = score;
                    scoreText[0].text = "No." + rank.ToString();
                }else{
                    scoreText[0].text = "";
                }

                scoreText[1].text = tempScore.ToString();
                if (id == thisId)
                {
                    backGround.color = Color.yellow;
                    _scrollPosition = i;
                }
                i++;

                RectTransform itemTransform = (RectTransform)cell.transform;
                itemTransform.SetParent(_content, false);

                _cells.Add(cell); // 更新用にListに追加。後で全削除。
            }
        }
        // 一度全て削除
        private void RemoveAllListViewItem()
        {
            foreach (var cell in _cells)
            {
                Destroy(cell);
            }
            _cells.Clear();
        }
        // 表示
        public void Show()
        {
            gameObject.SetActive(true);
            // 表示のタイミングでスクロール位置を変更。非アクティブ時には効かない。
            gameObject.GetComponent<ScrollRect>().verticalNormalizedPosition = 1.0f - (float)_scrollPosition/_listNum;
        }
        // 非表示
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}

