﻿using ModdingFramework.Backend;
using ModdingFramework.Engines;
using ModdingFramework.Events;
using ModdingFramework.Interfaces;

namespace ModdingFramework
{
    // TODO: Add More Modding Engines? (Wren, Luau, Roslyn) tho that would cost like double the code for supporting the engine properly

    /// <summary>
    /// The heart of the Modding Framework.
    /// </summary>
    public class ModLoader
    {
        private readonly IModEngine _engine = null!;

        /// <inheritdoc cref="IModEngine.LoadedMods"/>
        public IEnumerable<IMod> LoadedMods => _engine.LoadedMods;

        /// <inheritdoc cref="IModEngine.EnabledMods"/>
        public IEnumerable<IMod> EnabledMods => _engine.EnabledMods;

        /// <inheritdoc cref="IModEngine.DisabledMods"/>
        public IEnumerable<IMod> DisabledMods => _engine.DisabledMods;

        /// <inheritdoc cref="IModEngine.ConfigurationProvider"/>
        public string ConfigurationProvider => _engine.ConfigurationProvider;

        /// <summary>
        /// Loads all the <see cref="IMod"/>s available using the provided arguments to be as modular as possible.
        /// </summary>
        /// <param name="folder">The folder to scan, if null or empty it will scan the current folder to scan everything that extends <see cref="IMod"/>.</param>
        /// <param name="engine">The Modding Engine to use in THIS Mod Loader, each engine will load their respective files.</param>
        /// <param name="config">The Modding Configuration Provider to use in the provided <paramref name="engine"/>.</param>
        public ModLoader(string folder = "Mods", string filePrefix = "", IModEngine ?engine = null, IModConfigProvider? config = null)
        {
            string loadPath = Path.Join([AppDomain.CurrentDomain.BaseDirectory, folder]);
            
            // if the folder is empty or null is a risky thing to do depending on the modding engine
            // on the assembly engine its going to scan all the .dlls inside the output folder,
            // so maybe providing a file prefix could help reducing the assemblies to load to look for IMods

            // only check if the folder exists when the provided argument is not empty or null
            if (!string.IsNullOrEmpty(folder) && !Directory.Exists(loadPath))
            {
                // No mods to load
                Directory.CreateDirectory(loadPath);
                return;
            }

            _engine ??= engine ?? new AssemblyEngine(config);
            _engine.LoadMods(loadPath, filePrefix);

            // The mod should handle (?) the hot reloading of the classes they modify, maybe it should be done automatically here but we are letting the mod handle it as freely as it wants
            // bro im so fucking dumb i left over a for loop of the mods in this listener so the handlers would be like O(N)
            // O being the amount of mods and N the amount of handlers active for the event :skull:
            HotReloadHandler.OnCacheClear += (types) => { EventManager.TriggerGlobalEvent(new HRCacheClearEvent(types!)); };
            HotReloadHandler.OnHotReload += (types) => { EventManager.TriggerGlobalEvent(new HRUpdateApplicationEvent(types!)); };
        }

        /// <inheritdoc cref="IModEngine.EnableMod(string)"/>
        public IMod EnableMod(string modName) => _engine.EnableMod(modName);

        /// <inheritdoc cref="IModEngine.DisableMod(string)"/>
        public IMod DisableMod(string modName) => _engine.DisableMod(modName);
    }
}
