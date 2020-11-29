
using UnityEngine;

using MptUnity.Audio;

namespace MptUnity.Test.Audio.Behaviour
{
    [RequireComponent(typeof(TMPro.TMP_Text))]
    public class SectionText : MonoBehaviour
    {
        public GameObject currentMusicStateObject;
        
        TMPro.TMP_Text m_text;
        
        MusicCurrentState m_musicCurrent;

        void Start()
        {
            m_text = GetComponent<TMPro.TMP_Text>();
            m_musicCurrent = currentMusicStateObject.GetComponent<MusicCurrentState>();

            if (m_musicCurrent.Player.IsReady())
            {
                OnSectionChange(m_musicCurrent.Player.GetSection(m_musicCurrent.Section), m_musicCurrent.Section);
            }

            m_musicCurrent.AddSectionChangeListener(OnSectionChange);
            m_musicCurrent.Player.AddMusicSwitchListener(OnMusicSwitch);
        }

        void OnSectionChange(MusicSection section, int partIndex)
        {
            m_text.text = partIndex + " -> " + section.name;
        }

        void OnMusicSwitch(IMusic music)
        {
            OnSectionChange(music.GetSection(0), 0);
        }

        void OnDestroy()
        {
            m_musicCurrent.RemoveSectionChangeListener(OnSectionChange);
            m_musicCurrent.Player.RemoveMusicSwitchListener(OnMusicSwitch);
        }
    }
}