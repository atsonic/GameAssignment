using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sound
{
    public class SoundManager : MonoBehaviour
    {
        public enum Sound
        {
            OPENING,
            GAMESTART,
            GAMEOVER,
            HIT,
            COLLECT,
            JUMP,
            DAMAGE
        }
        private AudioSource _sourceSE;
        private AudioSource _sourceBGM;
        [SerializeField] private AudioClip _bgm;
        [SerializeField] private AudioClip _soundOpening;
        [SerializeField] private AudioClip _soundGameStart;
        [SerializeField] private AudioClip _soundGameOver;
        [SerializeField] private AudioClip _soundHit;
        [SerializeField] private AudioClip _soundCollect;
        [SerializeField] private AudioClip _soundJump;
        [SerializeField] private AudioClip _soundDamage;

        void Start(){
            _sourceSE = GetComponents<AudioSource>()[0];
            _sourceBGM = GetComponents<AudioSource>()[1];
        }
        public void PlayBGM(){
            _sourceBGM.clip = _bgm;
            _sourceBGM.volume = 0.05f;
            _sourceBGM.Play();
        }
        public void StopBGM(){
            _sourceBGM.Stop();
        }
        public void PlaySound(Sound state)
        {
            _sourceSE = GetComponent<AudioSource>();
            switch (state)
            {
                case Sound.OPENING:
                    _sourceSE.PlayOneShot(_soundOpening);
                    break;
                case Sound.GAMESTART:
                    _sourceSE.PlayOneShot(_soundGameStart);
                    break;
                case Sound.GAMEOVER:
                    _sourceSE.PlayOneShot(_soundGameOver);
                    break;
                case Sound.HIT:
                    _sourceSE.PlayOneShot(_soundHit);
                    break;
                case Sound.COLLECT:
                    _sourceSE.PlayOneShot(_soundCollect);
                    break;
                case Sound.JUMP:
                    _sourceSE.PlayOneShot(_soundJump);
                    break;
                case Sound.DAMAGE:
                    _sourceSE.PlayOneShot(_soundDamage);
                    break;
            }
        }
    }
}
