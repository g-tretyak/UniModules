﻿namespace UniGreenModules.UniCore.Runtime.ObjectPool
{
    using System;
    using System.Collections.Generic;
    using Interfaces;
    using ProfilerTools;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public static class ObjectPoolExtension
    {

        public static TComponent Spawn<TComponent>(this GameObject prototype)
        {
            if (!prototype) return default(TComponent);
            
            var pawn = ObjectPool.Spawn<TComponent>(prototype);
            return pawn;
        }

        public static T Spawn<T>(this T prototype)
            where T : Object
        {
            if (!prototype) return null;
            var pawn = ObjectPool.Spawn<T>(prototype, Vector3.zero, Quaternion.identity, null, false);
            return pawn;
        }
        
        public static T Spawn<T>(this T prototype, Vector3 position,
            Quaternion rotation, Transform parent = null, bool stayWorldPosition = false)
            where T : Object
        {
            if (!prototype) return null;
            var pawn = ObjectPool.Spawn<T>(prototype, position, rotation, parent, stayWorldPosition);
            return pawn;
        }

        
        public static GameObject Spawn(this GameObject prototype, Transform parent = null, bool stayWorldPosition = false)
        {
            if (!prototype) return null;
            var pawn = ObjectPool.Spawn(prototype, Vector3.zero, Quaternion.identity,
                                                    parent, stayWorldPosition);
            return pawn;
        }
        
        public static GameObject Spawn(this GameObject prototype,Vector3 position, Transform parent = null, bool stayWorldPosition = false)
        {
            if (!prototype) return null;
            var pawn = ObjectPool.Spawn(prototype, position, Quaternion.identity,
                                        parent, stayWorldPosition);
            return pawn;
        }
        
        public static GameObject Spawn(this GameObject prototype, Vector3 position, Quaternion rotation, Transform parent = null, bool stayWorldPosition = false)
        {
            if (!prototype) return null;
            var pawn = ObjectPool.Spawn(prototype, position, rotation, parent, stayWorldPosition);
            return pawn;
        }

        public static T Spawn<T>(this GameObject prototype, Transform parent = null, bool stayWorldPosition = false)
            where T : Object
        {
            if (!prototype) return null;
            var pawn = ObjectPool.Spawn<T>(prototype.GetComponent<T>(), Vector3.zero, Quaternion.identity,
                                        parent, stayWorldPosition);
            return pawn;
        }

        public static T Spawn<T>(this Object prototype, Transform parent = null, bool stayWorldPosition = false)
            where T : Object
        {
            if (!prototype) return null;
            var pawn = ObjectPool.Spawn<T>(prototype, Vector3.zero, Quaternion.identity,
                parent, stayWorldPosition);
            return pawn;
        }

        public static T Spawn<T>(Action<T> action = null)
            where T : class, new()
        {
            var item = ClassPool.Spawn( action);
            return item ?? new T();
        }

        public static void DespawnAsset(this Object instance, bool destroy = false)
        {
            if (destroy)
            {
                Object.DestroyImmediate(instance);
                return;
            }
            ObjectPool.Despawn(instance);
        }

        public static void DespawnComponent(this Component data, bool destroy = false)
        {
            if (destroy)
            {
                Object.DestroyImmediate(data.gameObject);
                return;
            }
            ObjectPool.Despawn(data);
        }
        
        public static void Despawn<T>(this T data, bool destroy = false)
            where T:class
        {

            if (data == null) return;

            GameProfiler.BeginSample("PoolExtension_Despawn");

            switch (data) {
                case Component target :
                    DespawnComponent(target, destroy);
                    break;
                case Object target:
                    DespawnAsset(target, destroy);
                    break;
                default:
                    ClassPool.Despawn(data);
                    break;
            }

            GameProfiler.EndSample();
            
        }
    }
}
