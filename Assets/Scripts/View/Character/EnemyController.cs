using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace View.Character
{
    public class EnemyController : MonoBehaviour
    {
        // 接地判定に使う
        [SerializeField] public ContactFilter2D filter2dGround;
        [SerializeField] public ContactFilter2D filter2dBlock;
        // Animator
        private Animator _animator;
        // プレイヤーのステート
        private enum AnimationState
        {
            Idle,
            Right,
            Left,
            Jump,
            Hit
        }
        //地面に設置しているかどうか
        private bool _isGrounded = true;

        // 横移動のスピード
        private float _speed = 0.0f;
        // 横移動のスピード増加/減少量
        private const float SpeedRate = 0.5f;
        // 横移動の最大スピード
        private const float SpeedMax = 5.0f;
        // 横移動の方向
        private float _direction = 1;
        // ジャンプ力
        private const float JumpPower = 1.1f;
        // ジャンプカウント
        // private int _jumpCount = 0;
        // ジャンプカウント最大
        private const int JumpCountMax = 10;
        //ジャンプ中かどうか
        // private bool _isKeyDownJumping = false;
        // ゲームオーバーかどうか
        private bool _isGameover = false;

        // 現在のステート
        private AnimationState _animationState = AnimationState.Idle;

        // Rigidbody2D
        private Rigidbody2D _rigidbody;

        // 移動タイマー
        private float _timeOut = 3.0f;
        private float _timeElapsed = 0.0f;
        void Start()
        {
            //　Animator取得
            _animator = GetComponent<Animator>();
            //　Rigidbody2D取得
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            // プレイ中の処理（ゲームオーバーじゃないなら）
            if(!_isGameover)
            {
                // 一定時間経過で移動処理
                _timeElapsed += Time.deltaTime;
                if (_timeElapsed >= _timeOut)
                {
                    // キー入力取得で横移動のステートを変更
                    float key = UnityEngine.Random.Range(-1, 2);// SystemとUnityEngineのRandomでエラーが出る
                    switch(key)
                    {
                        case 1:
                            if (_isGrounded) _animationState = AnimationState.Right;
                            _direction = 1;
                            IncreaseSpeed();
                            break;
                        case -1:
                            if (_isGrounded) _animationState = AnimationState.Left;
                            _direction = -1;
                            IncreaseSpeed();
                            break;
                        default:
                            if (_isGrounded) _animationState = AnimationState.Idle;
                            DecreaseSpeed();
                            break;
                    }
                    _timeElapsed = 0.0f;
                }
            }
        }
        void FixedUpdate()
        {
            Move();
            Jump();
            // ステートに応じて移動方向を変更
            switch (_animationState)
            {
                case AnimationState.Idle:
                    _animator.SetTrigger("IdleTrigger");
                    break;
                case AnimationState.Right:
                    _animator.SetTrigger("RunTrigger");
                    break;
                case AnimationState.Left:
                    _animator.SetTrigger("RunTrigger");
                    break;
                case AnimationState.Jump:
                    _animator.SetTrigger("JumpTrigger");
                    break;
                case AnimationState.Hit:
                    break;
            }
        }
        // スピードを増加させる
        private void IncreaseSpeed()
        {
            _speed += SpeedRate;
            if (_speed > SpeedMax)
            {
                _speed = SpeedMax;
            }
        }
        // スピードを減少させる
        private void DecreaseSpeed()
        {
            _speed = 0;
        }
        // 横移動
        private void Move()
        {
            Vector3 scale = transform.localScale;
            scale.x = _direction;
            transform.localScale = scale;
            _rigidbody.velocity = new Vector2(_speed * _direction, _rigidbody.velocity.y);
        }
        // ジャンプ
        private void Jump()
        {
        }
        // タイムアウトになった時の処理
        public void TimeOut(){
            Debug.Log("Stop");
        }
        // ゲームオーバーになった時の処理
        public void GameOver(){
            _isGameover = true;
            _rigidbody.Sleep();
            _speed = 0;
            _animationState = AnimationState.Idle;
        }
    }
}
