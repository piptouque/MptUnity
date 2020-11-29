namespace MptUnity.Audio
{
    public static class OpenMptUtility
    {
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns>The module extension at path if found, null on failure</returns>
        public static OpenMpt.ModuleExt LoadModuleExt(string path)
        {
            try
            {
                var moduleExt = new OpenMpt.ModuleExt(path);
                // set render parameters
                // Volume ramping to avoid clicking.
                moduleExt.GetModule().SetRenderParam(
                    OpenMpt.Module.RenderParam.eRenderVolumeRampingStrength,
                    MusicConfig.c_renderVolumeRampingStrength
                );
                return moduleExt;
            }
            catch (System.ArgumentException)
            {
                return null;
            }
        }
    
        public static string GetModuleExtMessage(OpenMpt.ModuleExt moduleExt)
        {
            return moduleExt.GetModule().GetMetadata(OpenMpt.Module.c_keyMessage);
        }
        
        public static string GetModuleExtAuthor(OpenMpt.ModuleExt moduleExt)
        {
            return moduleExt.GetModule().GetMetadata(OpenMpt.Module.c_keyAuthor);
        }
        
        public static string GetModuleExtTitle(OpenMpt.ModuleExt moduleExt)
        {
            return moduleExt.GetModule().GetMetadata(OpenMpt.Module.c_keyTitle);
        }

    }
}