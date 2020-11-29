using MptUnity.Audio.Behaviour;
using UnityEngine;

namespace MptUnity.Test.Audio.Behaviour
{
    public class BpmChangeButton : MonoBehaviour
    {
        
        public GameObject musicCurrentStateObject;
        
        public Toggle toggle;

        [Range(float.Epsilon, 1)]
        public double value = 10;
        

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
            double dir = toggle == Toggle.eDown ? -value : value;
            m_musicCurrent.Player.MoveTempoFactor(dir);
        }
        
        public enum Toggle
        {
            eUp,
            eDown
        };
    }
}