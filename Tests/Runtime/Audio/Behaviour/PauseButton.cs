using UnityEngine;

using MptUnity.Audio;

namespace MptUnity.Test.Audio.Behaviour
{
    public class PauseButton : MonoBehaviour
    {
        
        public GameObject musicCurrentStateObject;

        public Material matPause;
        public Material matResume;

        MusicCurrentState m_musicCurrent;
        Renderer m_renderer;

        void Start()
        {
            m_musicCurrent = musicCurrentStateObject.GetComponent<MusicCurrentState>();
            m_renderer = gameObject.GetComponent<Renderer>();
            
            if (m_musicCurrent.Player.IsReady())
            {
                UpdateMat(m_musicCurrent.Player.GetPlaybackState());    
            }
            
            m_musicCurrent.Player.AddPlaybackChangeListener(UpdateMat);
        }

        void OnMouseUpAsButton()
        {
            TogglePause();
        }

        void TogglePause()
        {
            if (m_musicCurrent.Player.IsPaused())
            {
                m_musicCurrent.Player.Play();
            }
            else
            {
                m_musicCurrent.Player.Pause();
            }
        }

        void UpdateMat(AudioPlaybackState state)
        {
            m_renderer.material = state == AudioPlaybackState.ePaused ? matResume : matPause;
        }
    }
}