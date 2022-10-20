using System;
using UnityEngine;
using UniRx;
using View.Items;
using Utils;

namespace View.UI{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private GameOver _gameOver;
        [SerializeField] private ButtonStart _buttonStart;
        [SerializeField] private ButtonRestart _buttonRestart;
        [SerializeField] private Score _score;
        [SerializeField] private TimeUI _timeUI;
        [SerializeField] private RankingListUI _rankingListUI;
        [SerializeField] private ItemManager _itemManager;

        // 敵に当たったイベント
        private Subject<Unit> _buttonStartClicked = new Subject<Unit>();
        // スタートボタンクリックイベント
        public IObservable<Unit> OnButtonStartClicked { get { return _buttonStartClicked; } }
        // 敵に当たったイベント
        private Subject<Unit> _buttonRestartClicked = new Subject<Unit>();
        // スタートボタンクリックイベント
        public IObservable<Unit> OnButtonRestartClicked { get { return _buttonRestartClicked; } }

        void Start()
        {
            _gameOver.Hide();
            _rankingListUI.Hide();
            _buttonStart.OnClickAsObservable().Subscribe(_ => {
                _buttonStartClicked.OnNext(Unit.Default);
            }).AddTo(this);
            _buttonRestart.OnClickAsObservable().Subscribe(_ => {
                _buttonRestartClicked.OnNext(Unit.Default);
            }).AddTo(this);
            _buttonRestart.Hide();
        }
        // ゲームスタート
        public void GamePlay(int numItems){
            _buttonStart.Hide();
            _itemManager.StartGenerateItems(numItems);
        }
        // ゲームオーバーになった時の処理
        public void GameOver()
        {
            _itemManager.StopGenerateItems();
            _gameOver.Show();
        }
        public void GameRanking(Score[] ranking, int scoreThisTime){
            
        }
        // スコアアップデート
        public void UpdateScore(int point){
            _score.UpdateText(point);
        }
        // 残り時間アップデート
        public void UpdateTime(int time){
            _timeUI.UpdateText(time);
        }
        // アイテム取得時
        public void CollectItem(GameObject item){
            _itemManager.ItemCollected(item);
        }
        // スコアアップデート
        public void UpdateRanking(int thisId, string ranking){
            Data.Score[] array = ranking.FromJsonArray<Data.Score>();
            // Debug.Log(thisId + " : " +  ranking);
            _rankingListUI.UpdateListView(thisId, array);
            _rankingListUI.Show();
            _buttonRestart.Show();
        }
    }
}

