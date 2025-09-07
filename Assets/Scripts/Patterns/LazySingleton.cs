using System;
using UnityEngine;

namespace DOTS_RTS.Patterns
{
    public class LazySingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    new GameObject(typeof(T).Name).AddComponent<T>();
                }
                
                return _instance;
            }
        }
        
        private static T _instance;

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }
    }
}