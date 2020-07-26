﻿using UniModules.UniGame.UniBuild.Editor.ClientBuild.Commands.PreBuildCommands;
using UniModules.UniGame.UniBuild.Editor.ClientBuild.Interfaces;
using UnityEngine;

namespace UniModules.UniGame.BuildCommands.Editor.Addressables
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UniGreenModules.UniCore.EditorTools.Editor.AssetOperations;
    using UnityEditor.AddressableAssets.Settings;

    [CreateAssetMenu(menuName = "UniGame/UniBuild/Commands/AddressablesActivateProfile", fileName = nameof(AddressablesActivateProfileCommand))]
    public class AddressablesActivateProfileCommand : UnityPreBuildCommand
    {
        private AddressableAssetSettings addressableAssetSettings;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ValueDropdown("GetProfiles")]
#endif
        public string targetProfileName = string.Empty;

        public AddressableAssetSettings AddressableAssetSettings => addressableAssetSettings =
            addressableAssetSettings == null ? 
                AssetEditorTools.GetAsset<AddressableAssetSettings>() : 
                addressableAssetSettings;
        
        public override void Execute(IUniBuilderConfiguration buildParameters)
        {
            Execute();
        }

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void Execute()
        {
            var settings = AddressableAssetSettings;
            var names = settings.profileSettings.GetAllProfileNames();
            if (!names.Contains(targetProfileName)) {
                Debug.LogError($"Target profile name doesn't exists for Addressables Settings");
            }

            var targetProfileId = settings.profileSettings.GetProfileId(targetProfileName);
            settings.activeProfileId = targetProfileId;
            
        }

        private List<string> GetProfiles()
        {
            var settings = AddressableAssetSettings;
            return settings.profileSettings.GetAllProfileNames();
        }

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(targetProfileName))
                targetProfileName = GetProfiles()?.FirstOrDefault();
        }
    }
}
