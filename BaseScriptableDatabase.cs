using UnityEditor;
using UnityEngine;

namespace Arr.ScriptableDatabases
{
    public class BaseScriptableDatabase : ScriptableObject, IScriptableKey
    {
        public string Id => name;

        public virtual void Initialize(){}
        
#if UNITY_EDITOR
        [ContextMenu("Populate Data")]
        protected void Populate()
        {
            if (!CanFilter) return;
            OnPrePopulate();
            
            string[] guids = AssetDatabase.FindAssets(Filter);
            Debug.Log($"FILTER {Filter} FOUND {guids.Length} GUID");

            for (var i = 0; i < guids.Length; i++)
            {
                string guid = guids[i];
                string path = AssetDatabase.GUIDToAssetPath(guid);

                OnPopulatePathFound(path);
            }
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        protected virtual bool CanFilter { get; }

        protected virtual string Filter { get; }
        protected virtual void OnPopulatePathFound(string path){}
        protected virtual void OnPrePopulate(){}
#endif
    }
}