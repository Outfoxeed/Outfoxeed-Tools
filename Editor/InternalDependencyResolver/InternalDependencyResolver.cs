using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using Newtonsoft.Json;
using UnityEngine;

namespace OutfoxeedTools.InternalDependencyResolver
{
    /// <summary>
    /// Automaticaly add wanted dependencies to the project
    /// manifest.json file on project changed.
    /// Largely inspired/copied from the Originer package from k0dep
    /// https://github.com/k0dep/Originer
    /// </summary>
    public static partial class InternalDependencyResolver
    {
        private const string CONFIG_FILE_PATH = "./Packages/com.outfoxeed.outfoxeed-tools/internal-dependencies.json";
        const string MANIFEST_FILE_PATH = "./Packages/manifest.json";
        
        [InitializeOnLoadMethod]
        public static void Init()
        {
            OnProjectChanged();
            EditorApplication.projectChanged += OnProjectChanged; 
        }

        private static void OnProjectChanged()
        {
            try
            {
                Dictionary<string, string> internalDependencies = GetInternalDependencies(); 
                if (internalDependencies.Count == 0)
                {
                    return;
                }
                
                ManifestInfo manifestData = CollectManifestData();

                // Get dependencies missing in the manifest
                Dictionary<string, string> missingDependencies = new();
                foreach (KeyValuePair<string,string> internalDependency in internalDependencies)
                {
                    if (manifestData.dependencies.ContainsKey(internalDependency.Key))
                    {
                        continue;
                    }
                    missingDependencies.Add(internalDependency.Key, internalDependency.Value);
                }

                if (missingDependencies.Count == 0)
                {
                    return;
                }

                if (!AskModifyManifestPopup(missingDependencies))
                {
                    return;
                }

                // Modify manifest file
                foreach (var missingDependency in missingDependencies)
                {
                    manifestData.dependencies.Add(missingDependency.Key, missingDependency.Value);
                }
                ApplyManifestData(manifestData);
            }
            catch(Exception exception)
            {
                Debug.LogException(exception);
            }
        }

        private static ManifestInfo CollectManifestData()
        {
            string manifestContent = File.ReadAllText(MANIFEST_FILE_PATH);
            ManifestInfo manifestData =
                JsonConvert.DeserializeObject<ManifestInfo>(manifestContent);
            return manifestData;
        }
        private static void ApplyManifestData(ManifestInfo data)
        {
            File.WriteAllText(MANIFEST_FILE_PATH, JsonConvert.SerializeObject(data));   
        }
        
        
        private static Dictionary<string, string> GetInternalDependencies()
        {
            string configFileContent = File.ReadAllText(CONFIG_FILE_PATH);
            InternalDependencyResolver.ConfigInfo configInfo = JsonConvert.DeserializeObject<ConfigInfo>(configFileContent);
            return configInfo.Dependencies;
        }
        
        private static bool AskModifyManifestPopup(IReadOnlyDictionary<string, string> missingDependencies)
        {
            const string title = "Outfoxeed Tools Internal Dependency Resolver - Detecting missing dependencies";
            const string accept = "Yes";
            const string cancel = "No";
            string message = "Do you want to add these dependencies missing to OutfoxeedTools ?\n\n";
            foreach (var missingDependency in missingDependencies)
            {
                message += $" - {missingDependency.Key}\n";
            }
            var canMakeChanges = EditorUtility.DisplayDialog(title, message,accept, cancel);
            return canMakeChanges;
        }

    }
}
