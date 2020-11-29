namespace MptUnity.Audio
{
    public interface ILoadable
    {
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Success or failure</returns>
        bool Load();
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Success or failure</returns>
        bool Unload();
        
        bool IsLoaded();
    }
}