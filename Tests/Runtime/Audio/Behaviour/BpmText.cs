
using UnityEngine;

namespace MptUnity.Test.Audio.Behaviour
{
    [RequireComponent(typeof(TMPro.TMP_Text))]
    public class BpmText : MonoBehaviour
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
                OnBpmChange(m_musicCurrent.Player.GetPlayingTempo());
            }

            m_musicCurrent.Player.AddBpmChangeListener(OnBpmChange);
        }

        void Update()
        {
            if (m_musicCurrent.Player.IsReady())
            {
                OnBpmChange(m_musicCurrent.Player.GetPlayingTempo());
            }
        }

        void OnBpmChange(double bpm)
        {
            m_text.text = bpm.ToString("F");
        }
        
        void OnDestroy()
        {
            m_musicCurrent.Player.RemoveBpmChangeListener(OnBpmChange);
        }
    }
}