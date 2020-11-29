using UnityEngine;

namespace MptUnity.Test.Audio.Behaviour
{
    public class MusicSwitchButton : MonoBehaviour
    {
        public GameObject musicCurrentStateObject;
        
        public Toggle toggle;

        public Material matSign;

        MusicCurrentState m_musicCurrent;
        
        Renderer m_renderer;

        void Start()
        {
            m_musicCurrent = musicCurrentStateObject.GetComponent<MusicCurrentState>();
            
            m_renderer = gameObject.GetComponent<Renderer>();
            m_renderer.material = matSign;
        }

        void OnMouseUpAsButton()
        {
            int currentMusicIndex = m_musicCurrent.Player.GetCurrentMusicIndex();
            int numberLoaded = m_musicCurrent.Player.GetLoadedNumber();
            int way = toggle == Toggle.eUp ? 1 : -1;

            currentMusicIndex = (currentMusicIndex + way + numberLoaded) % numberLoaded;
            m_musicCurrent.Player.SwitchMusic(currentMusicIndex);
        }
        
        public enum Toggle
        {
            eUp,
            eDown
        }
    }
}