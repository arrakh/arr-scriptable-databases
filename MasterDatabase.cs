﻿using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Arr.ScriptableDatabases
{
    public class MasterDatabase : ObjectScriptableDatabase<BaseScriptableDatabase>
    {
        [SerializeField] private bool populateOnStart = true;
        
        private const string FILE_NAME = "MasterScriptableDatabase";
        
        #if !MASTER_DB_MANUAL_INIT
        [RuntimeInitializeOnLoadMethod] 
        #endif
        public static void StaticInitialize()
        {
            var masterDb = Resources.Load<MasterDatabase>(FILE_NAME);
            if (masterDb == null) HandleNullMaster();
            
            #if UNITY_EDITOR
            if (masterDb.populateOnStart) masterDb.Populate();
            #endif
                
            masterDb.Initialize();
            
            foreach (var db in _items.Values) db.Initialize();
        }

        private static void HandleNullMaster()
        {
#if UNITY_EDITOR
            Debug.LogError("MasterScriptableDatabase does not exist! will create...");

            var instance = CreateInstance<MasterDatabase>();
            
            instance.Populate();
            
            var folderPath = Path.Combine(Application.dataPath, $"Resources");
            var path = $"Assets/Resources/{FILE_NAME}.asset";

            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            
            AssetDatabase.CreateAsset(instance, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#else
            throw new Exception("Trying to initialize Master Database BUT does not exist in resources!");
#endif
        }
        
#if UNITY_EDITOR
        protected override void OnPopulatePathFound(string path)
        {
            var asset = AssetDatabase.LoadAssetAtPath<BaseScriptableDatabase>(path);
            if (asset is MasterDatabase) return;
            data.Add(asset);
        }
#endif
    }
}