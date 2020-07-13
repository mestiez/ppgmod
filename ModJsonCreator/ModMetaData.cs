namespace ppgmod
{
    [System.Serializable]
    public class ModMetaData
    {
        [Entry("mod name", "Untitled mod")]
        public string Name;
        [Entry("author name (your name)", "Unknown individual")]
        public string Author;
        [Entry("mod description", "No description given")]
        public string Description;
        [Entry("mod version", "1.0")]
        public string ModVersion;
        [Entry("game version", "1.8")]
        public string GameVersion;
        [Entry("thumbnail path name", "thumb.png")]
        public string ThumbnailPath;
        [Entry("entry point", "Mod.Mod")]
        public string EntryPoint;
        [Entry("tags, separated by comma", "Fun")]
        public string[] Tags = { };
        [Entry("scripts, separated by comma", "script.cs")]
        public string[] Scripts = { };

        public ModMetaData()
        {
        }
    }
}
