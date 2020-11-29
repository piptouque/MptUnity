using UnityEngine;

using MptUnity.Audio;

namespace MptUnity.Test.Audio.Behaviour
{
    public class SectionMuteButton : MonoBehaviour
    {
        
        public GameObject musicCurrentStateObject;

        public Material matMute;
        public Material matUnmute;

        MusicCurrentState m_musicCurrent;
        Renderer m_renderer;

        void Start()
        {
            m_musicCurrent = musicCurrentStateObject.GetComponent<MusicCurrentState>();
            
            m_renderer = gameObject.GetComponent<Renderer>();
            
            m_musicCurrent.Player.AddMuteSectionChangeListener(OnMuteSection);
            m_musicCurrent.AddSectionChangeListener(OnSectionChange);
        }

        void OnDestroy()
        {
            m_musicCurrent.Player.RemoveMuteSectionChangeListener(OnMuteSection);
            m_musicCurrent.RemoveSectionChangeListener(OnSectionChange);
        }

        void OnMouseUpAsButton()
        {
            Mute(!m_musicCurrent.Player.IsSectionMuted(m_musicCurrent.Section));
        }

        void Mute(bool muted)
        {
            
            m_musicCurrent.Player.MuteSection(m_musicCurrent.Section, muted);
            // material will be changed in the OnMuteSection callback, through listener.
        }

        void OnMuteSection(MusicSection section, bool muted)
        {
            // button should display the opposite of the current state.
            // so if it's paused, we show the 'resume' material
            m_renderer.material = muted ? matMute : matUnmute;
        }

        void OnSectionChange(MusicSection section, int partIndex)
        {
            bool muted = m_musicCurrent.Player.IsSectionMuted(partIndex); 
            m_renderer.material = muted ? matMute : matUnmute;
        }
    }
}