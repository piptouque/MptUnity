

using MptUnity.Audio;
using UnityEngine;
using UnityEngine.Events;

using MptUnity.Audio.Behaviour;

namespace MptUnity.Test.Audio.Behaviour
{
    public class OnSectionChangeEvent : UnityEvent<MusicSection, int> {}
    
    public class MusicCurrentState : MonoBehaviour
    {
        public GameObject musicPlayerObject;

        public IJukebox Player { get; private set; }

        int m_currentSection;

        OnSectionChangeEvent m_partChangeEvent;
        
        public int Section
        {
            get => m_currentSection;
            set
            {
                m_currentSection = (value % Player.NumberSections + Player.NumberSections) % Player.NumberSections;
                // notify
                m_partChangeEvent.Invoke(Player.GetSection(m_currentSection), m_currentSection);
            }

        }
        
        void Awake()
        {
            Player = musicPlayerObject.GetComponent<IJukebox>();
            
            m_partChangeEvent = new OnSectionChangeEvent();
        }

        void Start()
        {
            Player.AddMusicSwitchListener(OnMusicSwitch);
        }

        void OnDestroy()
        {
            Player.RemoveMusicSwitchListener(OnMusicSwitch);
        }

        void OnMusicSwitch(IMusic music)
        {
            // hacky hack.
            Section = Section;
        }


        public void AddSectionChangeListener(UnityAction<MusicSection, int> onSectionChange)
        {
            m_partChangeEvent.AddListener(onSectionChange);
        }

        public void RemoveSectionChangeListener(UnityAction<MusicSection, int> onSectionChange)
        {
            m_partChangeEvent.RemoveListener(onSectionChange);
        }

    }
}