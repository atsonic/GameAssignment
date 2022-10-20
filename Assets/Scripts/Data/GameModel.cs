using System;
using UnityEngine;
using UniRx;
using Utils;
using NetWorking;
using System.Threading.Tasks;

namespace Data{
    public class GameModel : MonoBehaviour
    {
        // 1プレイの制限時間
        [SerializeField] private CountDownTimer _countDownTimer;// ★★★★★★newでやりたい（Monobehaviour継承してないとAddTo(this)ができない）
        // サーバー通信用RestClient
        [SerializeField] private RestClient _restClient;
        // 1プレイの取得スコア
        public readonly IntReactiveProperty ReactiveTotalPoint = new IntReactiveProperty(0);
        // アイテムのスコアテーブル
        private ItemDataTable _itemDataTable = new ItemDataTable();
        // スコアリスト
        // public ReactiveCollection<Score> ScoreList;
        public readonly StringReactiveProperty ListRanking = new StringReactiveProperty("");

        private int _thisId;
        public int ThisId{
            get{
                return _thisId;
            }
        }
        // カウントダウンイベント
        private Subject<int> _countDown = new Subject<int>();
        public IObservable<int> OnCoundDown { get { return _countDown; } }
        
        // ゲームスタート
        public void GamePlay(){
            _countDownTimer.CountDownTime.Subscribe(time => {
                if(time < 0) time = 0;
                _countDown.OnNext(time);
            }).AddTo(this);
            _countDownTimer.StartTimer();
        }
        // ゲームオーバー
        public void Gameover(){
            _countDownTimer.StopTimer();
        }
        // ポイント獲得
        public void AddPoint(int id){
            ReactiveTotalPoint.Value += _itemDataTable.SearchPoint(id);
        }
        // アイテムテーブルからアイテム数を取得
        public int GetNumItems()
        {
            return _itemDataTable.GetNumItems();
        }
        // RestClientからランキングを取得
        public async void GetRankingAsync()
        {
            var result = await _restClient.Get();
            ListRanking.Value = result;
        }
        // RestClientからランキングを取得
        public async void PostScoreAsync(int score)
        {
            var result = await _restClient.Post(score);
            var response = result.FromJson<Response>();
            if(response.result =="ok" ){
                _thisId = response.id;
                GetRankingAsync();
            }
        }
    }
}