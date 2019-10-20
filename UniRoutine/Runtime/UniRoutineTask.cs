﻿namespace UniTools.UniRoutine.Runtime
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UniGreenModules.UniCore.Runtime.DataFlow;
    using UniGreenModules.UniCore.Runtime.ObjectPool.Interfaces;

    public class UniRoutineTask : IUniRoutineTask
    {
        private readonly Stack<IEnumerator> awaiters = new Stack<IEnumerator>();
        private readonly LifeTimeDefinition lifeTimeDefinition = new LifeTimeDefinition();
        
        public readonly ILifeTime lifeTime;
        
        private IEnumerator rootEnumerator;
        private RoutineState state = RoutineState.Complete;
        
        public UniRoutineTask()
        {
            lifeTime = lifeTimeDefinition.LifeTime;
        }

        public ILifeTime LifeTime => lifeTime;

        public bool IsCompleted => state == RoutineState.Complete;
        
        public IEnumerator Current { get; private set; }

        object IEnumerator.Current => Current;

        public void Initialize(
            IEnumerator enumerator,
            bool moveNextImmediately = false)
        {
            Release();
                        
            rootEnumerator   = enumerator;
            Current          = enumerator;

            state          = RoutineState.Active;

            if (moveNextImmediately) MoveNext();
        }

        /// <summary>
        /// iterate all enumerator steps with inner iterators
        /// </summary>
        /// <returns>is iteration completed</returns>
        public bool MoveNext()
        {
            if (IsCompleted) return false;
            
            if (state == RoutineState.Paused)
                return true;

            var moveNext = MoveNextInner();
            if (!moveNext) {
                Complete();
            }

            return moveNext;
        }

        public void Pause()
        {
            if (IsCompleted) return;
            state = RoutineState.Paused;
        }

        public void Unpause()
        {
            if (IsCompleted) return;
            state = RoutineState.Active;
        }

        public void Complete()
        {
            if (IsCompleted) return;
            Release();
        }
        
        public void Release()
        {
            rootEnumerator = null;
            state          = RoutineState.Complete;
            awaiters.Clear();
            lifeTimeDefinition.Release();
        }
        
        public void Reset()
        {
            rootEnumerator?.Reset();
            Current = rootEnumerator;
            state = RoutineState.None;
            awaiters.Clear();
        }

        public void Dispose()
        {
            Release();
        }

        private bool MoveNextInner()
        {
            //if current already null - stop execution
            if (Current == null)
            {
                Dispose();
                return false;
            }

            //cacl nect execution step
            var moveNext = Current.MoveNext();

            //if current enumerator motion finished try get next one from stack
            if (!moveNext)
            {
                if (awaiters.Count == 0){
                    return false;
                }
                Current = awaiters.Pop();
                return true;
            }

            while (moveNext && Current.Current is IEnumerator awaiter)
            {
                //add new inner enumerator to stack
                awaiters.Push(Current);
                Current = awaiter;
                //for new root enumerator calculate first step
                moveNext = Current.MoveNext();
            }

            return true;
        }
        
    }

}