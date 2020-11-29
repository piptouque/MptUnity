
using UnityEngine.Events;

namespace MptUnity.Audio.Behaviour
{
    /// <summary>
    /// Event which gets called whenever the InstrumentSource starts playing a MusicalNote.
    /// </summary>
    public class OnInstrumentNoteStartEvent : UnityEvent<MusicalNote> { }
    /// <summary>
    /// Event which gets called whenever the InstrumentSource stops playing a MusicalNote.
    /// </summary>
    public class OnInstrumentNoteStopEvent : UnityEvent<MusicalNote> { }
    
    public interface IInstrumentSource : IMusicSource // todo: add IPlayable
    {
        #region Playing
        void StopNote(int voice);
        /// <summary>
        /// Plays the MusicalNote on the instrument.
        /// The instrument will keep playing until StopNote is called.
        /// </summary>
        /// <param name="note"></param>
        /// <returns>voice of the note, -1 on failure. </returns>
        int PlayNote(MusicalNote note);

        int NumberVoices { get; set; }

        #endregion


        #region Playback info
        int GetSpeed();

        int GetCurrentRow();
        MusicalNote GetNote(int voice);
        #endregion

        #region Events
        void AddOnNoteStartListener(UnityAction<MusicalNote> onNoteStart);
        void RemoveOnNoteStartListener(UnityAction<MusicalNote> onNoteStart);
        
        void AddOnNoteStopListener(UnityAction<MusicalNote> onNoteStop);
        void RemoveOnNoteStopListener(UnityAction<MusicalNote> onNoteStop);
        #endregion
    }
}