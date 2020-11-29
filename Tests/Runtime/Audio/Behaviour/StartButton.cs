using UnityEngine;

using MptUnity.Audio;

namespace MptUnity.Test.Audio.Behaviour
{
    public class StartButton : MonoBehaviour
    {
        
        public GameObject musicCurrentStateObject;

        public Material matStop;
        public Material matStart;

        MusicCurrentState m_musicCurrent;
        Renderer m_renderer;

        AudioPlaybackState m_showingState;

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

        void OnDestroy()
        {
            m_musicCurrent.Player.RemovePlaybackChangeListener(UpdateMat);
        }

        void OnMouseUpAsButton()
        {
            ToggleStart();
        }

        void ToggleStart()
        {
            if (m_musicCurrent.Player.IsStopped())
            {
                m_musicCurrent.Player.Play();
            }
            else
            {
                m_musicCurrent.Player.Stop();
            }
        }

        void UpdateMat(AudioPlaybackState state)
        {
            if (m_showingState != state)
            {
                m_renderer.material = state == AudioPlaybackState.eStopped ? matStart : matStop;
                m_showingState = state;
            }
        }
    }
}