using SRTPluginBase;
using System;

namespace SRTPluginProviderOri1
{
    internal class PluginInfo : IPluginInfo
    {
        public string Name => "Game Memory Provider (Ori and the Blind Forest DE)";

        public string Description => "A game memory provider plugin for Ori and the Blind Forest DE.";

        public string Author => "VideoGameRoulette";

        public Uri MoreInfoURL => new Uri("https://github.com/VideoGameRoulette");

        public int VersionMajor => assemblyVersion.Major;

        public int VersionMinor => assemblyVersion.Minor;

        public int VersionBuild => assemblyVersion.Build;

        public int VersionRevision => assemblyVersion.Revision;

        private Version assemblyVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
    }
}
