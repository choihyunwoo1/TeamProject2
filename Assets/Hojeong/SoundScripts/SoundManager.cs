using System;
using UnityEngine;
using UnityEngine.Audio;
using Choi;

namespace HJ
{
    public class SoundManager : Singleton<SoundManager>
    {
        #region

        //Audio Resource
        //public AudioSource bGMSource;
        //public AudioSource sFXSource;

        //public Sound[] bgmSounds;
        //public Sound[] sfxSounds;

        public Sound[] allsounds;

        private string bgmSound = "";    //현재 플레이 되고 있는 배경음

        public AudioMixer audioMixer;

        #endregion

        #region Unity Event Method
        protected override void Awake()
        {
            base.Awake();

            //AudioMixerGroup 목록 가져오기 0:Master, 1:BGM, 2:SFX
            AudioMixerGroup[] mixerGroups = audioMixer.FindMatchingGroups("Master");

            foreach (var sound in allsounds)
            {
                sound.source = gameObject.AddComponent<AudioSource>();

                sound.source.clip = sound.clip;

                sound.source.volume = sound.volume;
                sound.source.loop = sound.loop;
                sound.source.playOnAwake = sound.playOnAwake;

                //배경음
                if (sound.source.playOnAwake)
                {
                    sound.source.outputAudioMixerGroup = mixerGroups[1];    //BGM
                    sound.source.Play();
                    // 만약 배경음이면 현재 배경음 변수에도 이름 등록
                    if (sound.playOnAwake) bgmSound = sound.name;
                }
                else
                {
                    sound.source.outputAudioMixerGroup = mixerGroups[2];    //SFX
                }

                if (sound.playOnAwake)
                {
                    sound.source.Play();
                    // 만약 배경음이면 현재 배경음 변수에도 이름 등록
                    if (sound.loop) bgmSound = sound.name;
                }
            }
        }
        #endregion

        #region Custom Method

        //사운드 플레이 시작
        public void Play(string name)
        {
            //플레이할 사운드
            Sound sound = null;

            foreach (var s in allsounds)
            {
                if (s.name == name)
                {
                    sound = s;
                    break;      //찾았으면 반복문 정지
                }
            }

            //못 찾았으면
            if (sound == null)
            {
                //Debug.Log($"Cannot Find {name} Play Sound");
                return;
            }


            sound.source.Play();
            //sound.source.PlayOneShot(sound.clip, sound.volume);
            //Play 대신 PlayOndShot (연격 소리 끊기지 않음) 설정
        }


        //Pitch 자동 조절 기능
        public void SetPitch(string name, float newPitch)
        {
            //이름으로 sound 찾기
            Sound s = Array.Find(allsounds, sound => sound.name == name);

            //일치 확인 후 pitch 조정
            if (s != null && s.source != null)
            {
                s.source.pitch = newPitch;
            }
        }

        //사운드 플레이 정지
        public void Stop(string name)
        {
            //정지할 사운드
            Sound sound = null;

            foreach (var s in allsounds)
            {
                if (s.name == name)
                {
                    sound = s;
                    break;      //찾았으면 반복문 정지
                }
            }

            //못 찾았으면
            if (sound == null)
            {
                //Debug.Log($"Cannot Find {name} Stop Sound");
                return;
            }

            sound.source.Stop();
        }

        //배경음 플레이
        public void PlayBGM(string name)
        {
            //배경음 이름 체크
            if (bgmSound == name)
            {
                return;
            }

            //기존 배경음 정지 - sound 정지할 배경음            
            foreach (var s in allsounds)
            {
                if (s.name == bgmSound)
                {
                    //찾았으면 찾은 audioSource 플레이 정지
                    s.source.Stop();
                    break;
                }
            }

            //새로운 배경음 플레이
            Sound sound = null;
            foreach (var s in allsounds)
            {
                if (s.name == name)
                {
                    sound = s;
                    bgmSound = s.name;  //배경음 이름 저장
                    break;      //찾았으면 반복문 정지
                }
            }

            //못 찾았으면
            if (sound == null)
            {
                Debug.Log($"Cannot Find {name} BGM Sound");
                return;
            }

            sound.source.Play();
        }

        //배경음 종료
        public void StopBGM()
        {
            Stop(bgmSound);
        }
        #endregion
    }
}
