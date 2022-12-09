using UnityEngine;
using CatLib;

namespace CatLib.Game
{
    public class ProviderFileSystem : IServiceProvider
    {
        public void Init()
        {
            Debug.Log("hello init [ProviderFileSystem]");
        }
        public void Register()
        {
            Debug.Log("hello register [ProviderFileSystem]");
        }
    }
}