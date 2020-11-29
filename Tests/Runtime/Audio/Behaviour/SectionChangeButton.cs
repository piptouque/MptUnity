using UnityEngine;

namespace MptUnity.Test.Audio.Behaviour
{
    public class SectionChangeButton : MonoBehaviour
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
            switch (toggle)
            {
                case Toggle.eUp: ++m_musicCurrent.Section;
                    break;
                case Toggle.eDown: --m_musicCurrent.Section;
                    break;
            }
        }
        
        public enum Toggle
        {
            eUp,
            eDown
        }
    }
}