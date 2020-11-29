using System.Collections.Generic;

using FullSerializer;

namespace MptUnity.Audio
{
    /// <summary>
    /// A parser that is able to determine the MusicInfo
    /// of a MOD music file from the text in its Message.
    /// </summary>
    public static class MusicInfoParser
    {
        static readonly fsSerializer s_serialiser = new fsSerializer();
        public static MusicInfo ParseMusicInfo(OpenMpt.ModuleExt moduleExt)
        {
            string message = OpenMptUtility.GetModuleExtMessage(moduleExt);
            string author = OpenMptUtility.GetModuleExtAuthor(moduleExt);
            string title = OpenMptUtility.GetModuleExtTitle(moduleExt);
            
            fsData data = fsJsonParser.Parse(message);

            object deserialised = null;
            fsResult result = s_serialiser.TryDeserialize(data, typeof(SerialisedMusicInfo), ref deserialised);

            if (!result.Failed)
            {
                var serialisedMusicInfo = (SerialisedMusicInfo) deserialised;
                
                return new MusicInfo(
                    ParseAllMusicSections(serialisedMusicInfo), 
                    title, 
                    author, 
                    serialisedMusicInfo.Comment
                    );
            }
            return null;
        }

        public static InstrumentInfo ParseInstrumentInfo(OpenMpt.ModuleExt moduleExt)
        {
            string name = OpenMptUtility.GetModuleExtTitle(moduleExt);
            string message = OpenMptUtility.GetModuleExtMessage(moduleExt);

            fsData data = fsJsonParser.Parse(message);

            object deserialised = null;
            fsResult result = s_serialiser.TryDeserialize(data, typeof(SerialisedInstrumentInfo), ref deserialised);

            if (!result.Failed)
            {
                var serialisedInstrumentInfo = (SerialisedInstrumentInfo) deserialised;
                
                return new InstrumentInfo(
                    name,
                    ParseAllInstruments(serialisedInstrumentInfo),
                    serialisedInstrumentInfo.Comment
                );
            }

            return null;
        }

        [System.Serializable]
        class SerialisedMusicInfo
        {
            public Dictionary<string, int[]> Sections { get; set; }
            public string Comment { get; set; }
        }

        [System.Serializable]
        class SerialisedInstrumentInfo
        {
            // id of the instrument(s) in the MOD.
            public int[] Instruments { get; set;  }
            public string Comment { get; set; }

        }

        static int[] ParseAllInstruments(SerialisedInstrumentInfo serialisedInstrumentInfo)
        {
            var instruments = new int[serialisedInstrumentInfo.Instruments.Length];
            for (int i = 0; i < instruments.Length; ++i)
            {
                instruments[i] = serialisedInstrumentInfo.Instruments[i] - 1;
            }

            return instruments;
        }

        static MusicSection[] ParseAllMusicSections(SerialisedMusicInfo serialisedMusicInfo)
        {
            var sections = new MusicSection[serialisedMusicInfo.Sections.Count];
            int sectionIndex = 0;
            foreach (var pair in serialisedMusicInfo.Sections)
            {
                sections[sectionIndex++] = ParseMusicPart(pair);
            }
            return sections;
        }

        static MusicSection ParseMusicPart(KeyValuePair<string, int[]> serialisedPart)
        {
            // Decrementing the each channel to have it in [0, NumberChannels - 1]
            int[] values = serialisedPart.Value;
            for (int i = 0; i < values.Length; ++i)
            {
                --values[i];
            }
            return new MusicSection(serialisedPart.Key, serialisedPart.Value);
        }

    }
}