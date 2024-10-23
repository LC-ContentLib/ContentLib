using System;
using System.Collections.Generic;
using System.Linq;
using ContentLib.API.Model.Event;
using ContentLib.API.Model.Mods;
using UnityEngine;

namespace ContentLib.Core.Loader;

public class CustomContentLoader
    {
        private static readonly Lazy<CustomContentLoader> instance = new Lazy<CustomContentLoader>(() => new CustomContentLoader());
        public static CustomContentLoader Instance => instance.Value;
        
        private readonly Dictionary<string, ICustomContent?> _contentCache = new Dictionary<string, ICustomContent?>();
        private readonly List<ICustomContentProperties> _propertiesCache = new List<ICustomContentProperties>();

        /// <summary>
        /// Loads an asset bundle and searches for IContentProperties SOs.
        /// </summary>
        /// <param name="bundlePath">The path to the asset bundle.</param>
        public void LoadAssetBundle(string bundlePath)
        {
            AssetBundle assetBundle = AssetBundle.LoadFromFile(bundlePath);
            if (assetBundle == null)
            {
                Debug.LogError($"Failed to load AssetBundle: {bundlePath}");
                return;
            }

            // Load all assets that implement IContentProperties
            var contentProperties = assetBundle.LoadAllAssets<ScriptableObject>()
                .OfType<ICustomContentProperties>()
                .ToList();

            _propertiesCache.AddRange(contentProperties);
            Debug.Log($"Loaded {contentProperties.Count} IContentProperties from {bundlePath}");

            // Clean up the asset bundle after loading assets
            assetBundle.Unload(false);
        }

        /// <summary>
        /// Caches the IContent implementations.
        /// </summary>
        /// <param name="content">The IContent instance to cache.</param>
        public void CacheContent(ICustomContent? content)
        {
            if (content == null)
            {
                Debug.LogWarning("Attempted to cache null content or properties.");
                return;
            }

            // Use the class path as a unique identifier
            var classPath = content.Properties.ClassPath;
            if (!_contentCache.ContainsKey(classPath))
            {
                _contentCache[classPath] = content;
                Debug.Log($"Cached content for class path: {classPath}");
            }
            else
            {
                Debug.LogWarning($"Content for class path {classPath} is already cached.");
            }
        }

        /// <summary>
        /// Matches loaded IContentProperties with cached IContent implementations.
        /// </summary>
        public void MatchPropertiesWithContent()
        {
            foreach (ICustomContentProperties? properties in _propertiesCache)
            {
                var propertiesClassPath = properties.ClassPath;
                if (!_contentCache.TryGetValue(propertiesClassPath, out ICustomContent? content))
                {
                    Debug.LogWarning($"No matching content found for properties class path: {propertiesClassPath}");
                    continue;
                }

                if (content == null)
                {
                    Debug.LogWarning($"The cached content for properties class path '{propertiesClassPath}" +
                                     $"' is unexpectedly null. Please check initialization.");
                    continue;
                }
                if (!content.ArePropertiesValid(properties))
                {
                    Debug.LogWarning("Content properties are invalid for Content with class path:" +
                                         $" {propertiesClassPath}");
                }
                Debug.Log($"Matched properties for {propertiesClassPath} with cached content.");
                content.Properties = properties;
            }
        }

        /// <summary>
        /// Clears the cached content and properties.
        /// </summary>
        public void ClearCache()
        {
            _contentCache.Clear();
            _propertiesCache.Clear();
            Debug.Log("Cleared content and properties cache.");
        }
    }