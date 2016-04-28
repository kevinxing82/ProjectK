using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KevinX.Debugger;
using System.Collections;

namespace KevinX.Manager
{
    public class ResourceManager
    {
        public string name;
        private static ResourceManager _ins;
        public static ResourceManager Instance
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new ResourceManager();
                }
                return _ins;
            }
        }

        private ResourceManager()
        {

        }

        private Dictionary<string, object> _dicAsset = new Dictionary<string, object>();
        private Dictionary<string, WWW> _dicLoadingReq = new Dictionary<string, WWW>();

        public object GetResource(string name)
        {
            object obj = null;
            if (_dicAsset.TryGetValue(name, out obj) == false)
            {
                KXDebugger.LogWarnning("<GetResource Failed> Res not exist,res.Name=" + name);
                if (_dicLoadingReq.ContainsKey(name))
                {
                    KXDebugger.LogWarnning("<GetResource Failed>The res is still loading");
                }
            }
            return obj;
        }

        public void LoadAsync(string name)
        {
            LoadAsync(name, typeof(object));
        }

        public void LoadAsync(string name, System.Type type)
        { 
            if(_dicAsset.ContainsKey(name))
            {
                return;
            }
            if(_dicLoadingReq.ContainsKey(name))
            {
                return;
            }
            RefAsset(name);

            CoroutineProvider.instance.StartCoroutine(AsyncLoadCoroutine(name,type));
        }

        private IEnumerator AsyncLoadCoroutine(string name, System.Type type)
        {
            string assetBundleName = GlobalSetting.ConvertToAssetBundleName(name);
            string url =GlobalSetting.ConvertToFtpPath(assetBundleName);
            int   verNum = GameApp.GetVersionNumber();

            WWW www = WWW.LoadFromCacheOrDownload(url, verNum);
            _dicLoadingReq.Add(name, www); 
            while(www.isDone==false)
            {
                yield return null;
            }
            AssetBundleRequest req = www.assetBundle.LoadAssetAsync(GetAssetName(name));
            while(req.isDone==false)
            {
                yield return null;
            }

            _dicAsset.Add(name, req.asset);
            _dicLoadingReq.Remove(name);
            www.assetBundle.Unload(false);
            www = null;
        }

        public bool IsResLoading(string name)
        {
            return _dicLoadingReq.ContainsKey(name);
        }

        public bool IsResLoaded(string name)
        {
            return _dicAsset.ContainsKey(name);
        }

        public WWW GetLoadingWWW(string name)
        {
            WWW www = null;
            _dicLoadingReq.TryGetValue(name, out www);
            return www;
        }

        public void UnrefAsset(string name)
        {
            _dicAsset.Remove(name);
        }

        private string GetAssetName(string name)
        {
            int index = name.LastIndexOf('/');
            return name.Substring(index + 1, name.Length - index - 1);
        }

        public void UnloadUnuseAsset()
        {
            Resources.UnloadUnusedAssets();
        }

        private void RefAsset(string name)
        {

        }
    }
}
