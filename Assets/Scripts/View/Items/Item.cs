using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace View.Items
{
    public class Item : MonoBehaviour
    {
        // ゲーム再開までの時間
        private const int HideTime = 1;
        // Rigidbody
        private Rigidbody2D _rigidbody;
        // Animator
        private Animator _animator;
        // アイテムのID
        private int _ItemId = 0;
        // 無効にするためコライダー取得
        private CircleCollider2D[] _colliders;
        // IDとアイテムアニメーションセット
        public void SetItemId(int numItems)
        {
            // InstantiateでStartが実行されないのでここに入れてる
            _animator = GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _colliders = GetComponents<CircleCollider2D>();

            _ItemId = UnityEngine.Random.Range(1, numItems + 1);
            // _ItemId = 9;
            if(_ItemId == 9){//★★★★★★ダメージアイテムは別クラスにした方が良い
                gameObject.tag = "ItemDamage";
            }
            _animator.SetInteger("ItemNumber", _ItemId);
        }
        // 収集されたとき
        public void Collected()
        {
            _rigidbody.Sleep();
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.angularVelocity = 0;
            _rigidbody.isKinematic = true;
            _rigidbody.gravityScale = 0;
            foreach(CircleCollider2D col in _colliders){
                col.enabled = false;
            }
            _animator.SetInteger("ItemNumber", 0);
            Invoke("Hide", HideTime);
        }
        // 消す
        public void Hide()
        {
            Destroy(gameObject);
        }
        // Item ID取得
        public int GetItemId()
        {
            return _ItemId;
        }
    }
}

