using System;
using MptUnity.Audio;
using MptUnity.Audio.Behaviour;
using UnityEngine;

namespace MptUnity.Test.Audio.Behaviour
{
    [RequireComponent(typeof(TMPro.TMP_Text))]
    public class CommentText : MonoBehaviour
    {
        public GameObject currentMusicStateObject;
        
        TMPro.TMP_Text m_text;
        MusicCurrentState m_musicCurrent;

        void Start()
        {
            m_text = GetComponent<TMPro.TMP_Text>();
            m_musicCurrent = currentMusicStateObject.GetComponent<MusicCurrentState>();

            bool isReady = m_musicCurrent.Player.IsReady();
            if (isReady)
            {
                SetComment(m_musicCurrent.Player.GetComment());     
            }
            
            m_musicCurrent.Player.AddMusicSwitchListener(OnMusicSwitch);
        }
        
        void OnMusicSwitch(IMusic music)
        {
            SetComment(music.GetComment());
        }

        void SetComment(string comment)
        {
            m_text.text = comment;
        }

        void OnDestroy()
        {
            m_musicCurrent.Player.RemoveMusicSwitchListener(OnMusicSwitch);
        }
    }
}