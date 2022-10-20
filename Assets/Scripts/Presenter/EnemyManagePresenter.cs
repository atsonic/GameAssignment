using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View.Character;

namespace Presenter
{
    public class EnemyManagePresenter : MonoBehaviour
    {
        [SerializeField] GameObject _enemy1;
        [SerializeField] GameObject _enemy2;

        private EnemyController _enemyController1;
        private EnemyController _enemyController2;

        void Start(){
            _enemyController1 = _enemy1.GetComponent<EnemyController>();
            _enemyController2 = _enemy2.GetComponent<EnemyController>();
        }
        public void GameOver(){
            _enemyController1.GameOver();
            _enemyController2.GameOver();
        }
    }
}

