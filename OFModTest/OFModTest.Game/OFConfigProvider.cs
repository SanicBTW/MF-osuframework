using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using ModdingFramework;
using ModdingFramework.Interfaces;
using osu.Framework.Configuration;
using osu.Framework.Platform;

namespace OFModTest.Game
{
    // osu!framework configuration provider, made to use IniConfigManager and being able to provide it to the Modding Framework
    public class OFConfigProvider : IniConfigManager<OFConfigEnum>, IModConfigProvider
    {
        private const char delimeter = '|';

        private ModRegistry registry;

        private string configFile = "";
        protected override string Filename => configFile;

        protected override void InitialiseDefaults()
        {
            SetDefault(OFConfigEnum.ENABLED_MODS, "");
            SetDefault(OFConfigEnum.DISABLED_MODS, "");
        }

        public OFConfigProvider(Storage storage, IDictionary<OFConfigEnum, object> defaultOverrides = null) : base(storage, defaultOverrides) { }

        public void Setup(string configFile, ModRegistry registry)
        {
            this.configFile = configFile;
            this.registry = registry;

            // what is this cast lmfao
            ((IModConfigProvider)this).Save();
            Load();
        }

        // Instead of calling Save directly (from ConfigManager) we set the value on the bindables and those call the Save function directly (which writes to disk)
        void IModConfigProvider.Save()
        {
            IEnumerable<string> enabledMods = registry.GetEnabledMods().Select((mod) => mod.Manifest.Name);
            IEnumerable<string> disabledMods = registry.GetDisabledMods().Select((mod) => mod.Manifest.Name);

            SetValue(OFConfigEnum.ENABLED_MODS, string.Join(delimeter, enabledMods));
            SetValue(OFConfigEnum.DISABLED_MODS, string.Join(delimeter, disabledMods));
        }

        // gets called twice because of the constructor but since the filename is null until setup it doesnt do anything
        protected override void PerformLoad()
        {
            base.PerformLoad();

            if (string.IsNullOrEmpty(Filename))
                return;

            Type type = typeof(ModRegistry);
            MethodInfo method = type.GetMethod("dynamicSetList", BindingFlags.Instance | BindingFlags.NonPublic, [typeof(string), typeof(IEnumerable<string>)])!;

            Debug.Assert(method != null, "Reflection failed.");

            method!.Invoke(registry, ["enabledMods", Get<string>(OFConfigEnum.ENABLED_MODS).Split(delimeter, StringSplitOptions.RemoveEmptyEntries)]);
            method!.Invoke(registry, ["disabledMods", Get<string>(OFConfigEnum.DISABLED_MODS).Split(delimeter, StringSplitOptions.RemoveEmptyEntries)]);
        }

        public string GetConfigType() => "osu!framework configuration provider";

    }

    public enum OFConfigEnum
    {
        ENABLED_MODS,
        DISABLED_MODS
    }
}
