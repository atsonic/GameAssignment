using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using View.UI;
using View.Items;

namespace View.Character
{
    public class PlayerController : MonoBehaviour
    {

        // 接地判定に使う
        [SerializeField] public ContactFilter2D filter2dGround;
        [SerializeField] public ContactFilter2D filter2dBlock;


        // ジャンプイベント
        private Subject<Unit> _jump = new Subject<Unit>();
        public IObservable<Unit> OnJump { get { return _jump; } }

        // 敵に当たったイベント
        private Subject<Unit> _HitEnemy = new Subject<Unit>();
        public IObservable<Unit> OnHitEnemy { get { return _HitEnemy; } }
        // アイテム取ったイベント
        private Subject<GameObject> _GetItem = new Subject<GameObject>();
        public IObservable<GameObject> OnGetItem { get { return _GetItem; } }
        // ダメージアイテム取ったイベント
        private Subject<GameObject> _GetItemDamage = new Subject<GameObject>();
        public IObservable<GameObject> OnGetItemDamage { get { return _GetItemDamage; } }

        // Animator
        private Animator _animator;
        // プレイヤーのアニメーションステート
        private enum AnimationState
        {
            Idle,
            Right,
            Left,
            Jump,
            Hit
        }
        // 地面に設置しているかどうか
        private bool _isGrounded = false;

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
        private int _jumpCount = 0;
        // ジャンプカウント最大
        private const int JumpCountMax = 10;
        // ジャンプ中かどうか
        private bool _isKeyDownJumping = false;
        // ジャンプ中かどうか
        private bool _isDamaging = false;
        // ゲームオーバーかどうか
        private bool _isGameover = false;

        // 現在のステート
        private AnimationState _animationState = AnimationState.Idle;
        // Rigidbody2D
        private Rigidbody2D _rigidbody;

        private GameObject _prevGameObject;
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
                // 接地しているとき
                if (IsGrounded())
                {
                    _isGrounded = true;
                    _animationState = AnimationState.Idle;
                }
                else
                {
                    _isGrounded = false;
                }

                if(_isDamaging){
                    _animationState = AnimationState.Hit; 
                }

                // キー入力取得で横移動のステートを変更
                float key = Input.GetAxisRaw("Horizontal");
                switch(key)
                {
                    case 1:
                        if (_isGrounded && !_isDamaging) _animationState = AnimationState.Right;
                        _direction = 1;
                        IncreaseSpeed();
                        break;
                    case -1:
                        if (_isGrounded && !_isDamaging) _animationState = AnimationState.Left;
                        _direction = -1;
                        IncreaseSpeed();
                        break;
                    default:
                        if (_isGrounded && !_isDamaging) _animationState = AnimationState.Idle;
                        DecreaseSpeed();
                        break;
                }

                // ジャンプキーが押されてるかどうか
                if (Input.GetKeyDown("space"))
                {
                    _jump.OnNext(Unit.Default);
                    _isKeyDownJumping = true;
                    _animationState = AnimationState.Jump;
                }
                if (Input.GetKeyUp("space"))
                {
                    _isKeyDownJumping = false;
                }
            }
        }
        // 物理演算系更新
        void FixedUpdate()
        {
            Move();
            Jump();
            // ステートに応じてアニメーションを変更
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
                    _animator.SetTrigger("HitTrigger");
                    break;
            }
        }
        // ボタン押してる間はSpeedMaxまで移動スピードを増加させる
        private void IncreaseSpeed()
        {
            _speed += SpeedRate;
            if (_speed > SpeedMax)
            {
                _speed = SpeedMax;
            }
        }
        // ボタン離したらスピードを減少させる
        private void DecreaseSpeed()
        {
            _speed -= SpeedRate;
            if (_speed < 0)
            {
                _speed = 0;
            }
        }
        // 横移動する
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
            // プレイヤーが接地しているときのフラグ管理
            if (IsGrounded())
            {
                _jumpCount = 0;
            }
            // ジャンプキーが押されている間_jumpCountをインクリメントし、_jumpCountMaxになるまで上昇する
            if (_isKeyDownJumping)
            {
                _animationState = AnimationState.Jump;
                _jumpCount++;
                if (_jumpCount > JumpCountMax)
                {
                    _jumpCount = JumpCountMax;
                }
                else
                {
                    _rigidbody.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
                }
            }
        }

        // ゲームオブジェクトとぶつかった時の処理
        private void OnTriggerEnter2D(Collider2D collision)
        {
            GameObject go = collision.gameObject;
            // 接触したオブジェクトのtag名がEnemyの場合イベント発火
            if (go.tag == "Enemy")
            {
                if(go != _prevGameObject){
                    _HitEnemy.OnNext(Unit.Default);
                }
            } 
            // 接触したオブジェクトのtag名がItemの場合はIDを取得してイベント発火
            else if (go.tag == "Item")
            {
                if(go != _prevGameObject){
                    _GetItem.OnNext(go);
                }
            }
            else if(go.tag == "ItemDamage"){
                if(go != _prevGameObject){
                    _GetItemDamage.OnNext(go);
                    StartCoroutine(GetDamage());
                }
            }
            _prevGameObject = go;
        }
        IEnumerator GetDamage()
        {
            while (true)
            {
                _animationState = AnimationState.Hit;
                _isDamaging = true;
                for (int i = 0; i <= 1; i++) {
                    yield return new WaitForSeconds (0.5f);

                    // i==4になったらコルーチン終了  
                    if (i == 1) {
                        _isDamaging = false;
                        StopCoroutine("GetDamage");
                        yield break;
                    }
                }
            }
        }
        // 接地してるかどうかの判定
        private bool IsGrounded()
        {
            bool isGrounded = false;
            bool isGround = _rigidbody.IsTouching(filter2dGround);
            bool isBlock = _rigidbody.IsTouching(filter2dBlock);

            if (isGround || isBlock)
            {
                isGrounded = true;
            }
            return isGrounded;
        }
        // タイムアウトになった時の処理
        public void TimeOut()
        {
            Debug.Log("Stop");
        }
        // ゲームオーバーになった時の処理
        public void GameOver()
        {
            _isGameover = true;
            _isKeyDownJumping = false;
            _rigidbody.Sleep();
            _speed = 0;
            _jumpCount = 0;
            _animationState = AnimationState.Hit;
        }
    }
}