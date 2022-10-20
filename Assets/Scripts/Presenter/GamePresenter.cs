using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Data;
using View.Character;
using View.UI;
using View.Items;
using Sound;
using UniRx;
using Presenter;

public class GamePresenter : MonoBehaviour
{
    // プレイヤー
    [SerializeField] private PlayerController _playerController;
    // UI管理
    [SerializeField] private UIController _uiController;
    // サウンド管理
    [SerializeField] private SoundManager _soundManager;
    [SerializeField] private EnemyManagePresenter _enemyManagePresenter;
    // モデル
    [SerializeField] private GameModel _gameModel;
    // ステート
    private GameState _gameState;
    // 一個前のステート（★★★★★★Update内で管理しないようにすると要らない）
    private GameState _preGameState;
    // ゲーム再開までの時間
    private const int GameOverTime = 2;
    void Start()
    {
        // モデルのスコアが更新されたらUIに反映
        _gameModel.ReactiveTotalPoint.Subscribe(score => _uiController.UpdateScore(score)).AddTo(this);
        // ランキングリストが更新されたらUIに反映
        _gameModel.ListRanking.SkipLatestValueOnSubscribe().Subscribe(ranking => _uiController.UpdateRanking(_gameModel.ThisId, ranking)).AddTo(this);
        // カウントダウンタイマーの購読
        _gameModel.OnCoundDown.Subscribe(time => {
            _uiController.UpdateTime(time);
            if(time <= 0){
                _gameState = GameState.GameOver;
            }
        }).AddTo(this);


        // ゲームステート初期値
        _gameState = GameState.GameStart;
        _preGameState = GameState.Opening;

        // オープニングのサウンド再生
        _soundManager.PlaySound(SoundManager.Sound.OPENING);
    }
    void Update()
    {
        // スペースキー入力でゲームスタート
        if(_gameState == GameState.GameStart){
            if(Input.GetKeyDown(KeyCode.Space)){
                _gameState = GameState.GamePlay;
            }
        }
        if(_gameState == GameState.GameRanking){
            if(Input.GetKeyDown(KeyCode.Space)){
                GameRestart();
            }
        }
        
        // State管理
        if(_gameState != _preGameState){
            if(_gameState == GameState.GameStart)
            {
                GameStart();
            } 
            else if (_gameState == GameState.GamePlay)
            {
                GamePlay();
            } 
            else if (_gameState == GameState.GameOver)
            {
                GameOver();
            } 
            else if (_gameState == GameState.GameRanking)
            {
                GameRanking();
            }
            _preGameState = _gameState;
        }
    }
    // ゲーム開始初期化
    void GameStart(){
        // プレイヤーの各イベント購読
        _playerController.OnJump.Subscribe(_ => { CharacterJump();}).AddTo(this);
        _playerController.OnHitEnemy.Subscribe(_ => { _gameState = GameState.GameOver;}).AddTo(this);
        _playerController.OnGetItem.Subscribe(item => { GetItem(item);}).AddTo(this);
        _playerController.OnGetItemDamage.Subscribe(item => { GetItemDamage(item);}).AddTo(this);

        // UIコントローラーのイベント購読
        _uiController.OnButtonStartClicked.Subscribe(_ => {
            _gameState = GameState.GamePlay;
        }).AddTo(this);
        _uiController.OnButtonRestartClicked.Subscribe(_ => {
            // _gameState = GameState.GameStart;
            GameRestart();
        }).AddTo(this);

        Time.timeScale = 0;// GamePlayになるまでゲームを止める
    }
    // プレイ開始
    void GamePlay(){
        _soundManager.PlaySound(SoundManager.Sound.GAMESTART);
        _soundManager.PlayBGM();
        _uiController.GamePlay(_gameModel.GetNumItems());// アイテムテーブルからアイテムの数を渡す
        _gameModel.GamePlay();
        Time.timeScale = 1;// ゲームを再開
    }
    // ゲームオーバー時の処理
    void GameOver(){
        _soundManager.PlaySound(SoundManager.Sound.GAMEOVER);
        _soundManager.StopBGM();
        _playerController.GameOver();
        _uiController.GameOver();
        _gameModel.Gameover();
        _enemyManagePresenter.GameOver();
        Invoke("OnGameOverFinished", GameOverTime);
    }
    // ゲームリスタートの処理
    public void GameRestart()
    {
        // 現在のシーンを取得してロードする
        Scene activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.name);
    }
    public void OnGameOverFinished(){
        _gameState = GameState.GameRanking;
    }
    // スコア登録＞ランキング表示
    public void GameRanking(){
        _gameModel.PostScoreAsync(_gameModel.ReactiveTotalPoint.Value);
    }
    // プレイヤーがジャンプした時の処理（音再生）
    void CharacterJump(){
        _soundManager.PlaySound(SoundManager.Sound.JUMP);
    }
    // プレイヤーがポイントゲットの処理
    private void GetItem(GameObject item){
        _soundManager.PlaySound(SoundManager.Sound.COLLECT);
        GetitemCommon(item);
    }
    // プレイヤーがダメージアイテムを取った時
    private void GetItemDamage(GameObject item){
        _soundManager.PlaySound(SoundManager.Sound.DAMAGE);
        GetitemCommon(item);
    }
    // アイテム取得時の得点に絡む処理
    private void GetitemCommon(GameObject item){
        int id = item.GetComponent<Item>().GetItemId();
        _gameModel.AddPoint(id);
        _uiController.CollectItem(item);
    }
}
