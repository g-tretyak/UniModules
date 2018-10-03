﻿using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Tools.UnityTools.Common;
using Assets.Tools.UnityTools.Extension;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.ObjectPool.Scripts;
using Assets.Tools.UnityTools.UniRoutine;
using UnityEngine;

namespace Assets.Tools.UnityTools.StateMachine.UniStateMachine
{
    [CreateAssetMenu(menuName = "States/States/UniParallelState", fileName = "UniParallelState")]
    public class UniParallelState : UniStateBehaviour
    {
        [NonSerialized]
        private IContextExecutor<IEnumerator> _executor;

        [SerializeField]
        private List<UniStateParallelMode> _states = new List<UniStateParallelMode>();

        protected override IEnumerator ExecuteState(IContext context)
        {
            var routineDisposables = ClassPool.Spawn<List<IDisposableItem>>();
            var completionSource = ClassPool.Spawn<CompletionConditionSource>();
            completionSource.Initialize(() => IsComplete(routineDisposables));

            _stateContext.AddValue(context, routineDisposables);
            _stateContext.AddValue(context, completionSource);

            //launch states
            for (int i = 0; i < _states.Count; i++)
            {
                var state = _states[i].StateBehaviour;
                var routine = state.Execute(context);
                var disposable = _executor.Execute(routine);
                routineDisposables.Add(disposable);
            }

            yield return completionSource.RoutineWaitUntil();

        }

        protected bool IsComplete(List<IDisposableItem> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                if (!item.IsDisposed)
                    return false;
            }

            return true;
        }

        protected override void OnInitialize()
        {
            _executor = new UniRoutineExecutor();
            base.OnInitialize();
        }

        protected override void OnExit(IContext context)
        {
            Debug.Log("PARALL EXIT");
            var disposableItems = _stateContext.Get<List<IDisposableItem>>(context);
            for (var i = 0; i < disposableItems.Count; i++)
            {
                var item = disposableItems[i];
                item.Dispose();
            }
            disposableItems.Despawn();

            var completionsSource = _stateContext.Get<CompletionConditionSource>(context);
            completionsSource.Despawn();

            _stateContext.Remove<List<IDisposableItem>>(context);
            _stateContext.Remove<CompletionConditionSource>(context);

            base.OnExit(context);
        }

    }
}
