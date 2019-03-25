﻿using System.Collections;
using UniModule.UnityTools.ObjectPool.Scripts;
using UniModule.UnityTools.UniStateMachine.Interfaces;
using UniStateMachine;
using UniStateMachine.Nodes;
using UnityEngine;

namespace UniModule.UnityTools.ActorEntityModel
{
    public class ActorComponent : EntityComponent, IPoolable
    {
        private Actor _actor = new Actor();

        #region inspector data

        [SerializeField] private bool _launchOnStart = true;

        /// <summary>
        /// behaviour SO
        /// </summary>
        [SerializeField] private UniGraph _stateObject;

        /// <summary>
        /// behaviour component
        /// </summary>
        [SerializeField] private IContextState<IEnumerator> _stateComponent;

        /// <summary>
        /// actor model data
        /// </summary>
        [SerializeField] private ActorInfo _actorInfo;

        #endregion

        public IContextState<IEnumerator> State { get; protected set; }

        public Actor Actor => _actor;

        public void Release()
        {
            Actor.Release();
        }

        protected IContextState<IEnumerator> GetState()
        {
            var model = Context.Get<ActorModel>();
            var parameterBehaviour = _stateObject ? _stateObject : _stateComponent;

            if (model?.Behaviour == null && parameterBehaviour == null)
                return null;

            var state = model?.Behaviour == null ? parameterBehaviour : model.Behaviour;

            return state;
        }

        protected virtual void Activate()
        {
            Actor.SetEnabled(true);
        }

        protected virtual void Deactivate()
        {
            Actor.SetEnabled(false);
        }

        private void OnDisable()
        {
            Deactivate();
        }

        private void OnEnable()
        {
            Activate();
        }

        private void Start()
        {
            if (_launchOnStart)
            {
                var state = GetState();
            }
        }

        // Use this for initialization
        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            var model = _actorInfo?.Create();
            model?.RegisterContext(Context);
            InitializeContext();
            Actor.SetModel(model);
        }

        protected virtual void InitializeContext()
        {
        }

        private void OnDestroy()
        {
            Release();
        }
    }
}