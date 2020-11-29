namespace MptUnity.Audio
{
    
    /// <summary>
    /// Info on a MOD music file.
    /// Includes number of sections, their associated channels,
    /// and the patterns.
    /// </summary>
    public class MusicInfo
    {
        public readonly string title;
        public readonly string author;
        
        readonly MusicSection[] m_sections;

        public readonly string comment;

        public MusicInfo(MusicSection[] sections, string a_title, string a_author, string a_comment)
        {
            title = a_title;
            author = a_author;
            // shallow copy will be enough here
            m_sections = (MusicSection[]) sections.Clone();
            comment = a_comment;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns> Number of sections for this music</returns>
        public int GetNumberSections()
        {
            return m_sections.Length;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns> array of all the channels owned by section at partIndex.</returns>
        public int[] GetSectionChannels(int sectionIndex)
        {
            return GetSection(sectionIndex).channels;
        }

        public MusicSection GetSection(int sectionIndex)
        {
            return m_sections[sectionIndex];
        }

    }
}