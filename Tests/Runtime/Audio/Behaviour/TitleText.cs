using UnityEngine;

using MptUnity.Audio;
using MptUnity.Audio.Behaviour;

namespace MptUnity.Test.Audio.Behaviour
{
    [RequireComponent(typeof(TMPro.TMP_Text))]
    public class TitleText : MonoBehaviour
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
                SetTitle(m_musicCurrent.Player.GetTitle());
            }
            
            m_musicCurrent.Player.AddMusicSwitchListener(OnMusicSwitch);
        }
        
        void OnMusicSwitch(IMusic music)
        {
            SetTitle(music.GetTitle());
        }

        void SetTitle(string title)
        {
            m_text.text = title;
        }
        
        void OnDestroy()
        {
            m_musicCurrent.Player.RemoveMusicSwitchListener(OnMusicSwitch);
        }
    }
}