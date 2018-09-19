﻿using System;
using System.Collections.Generic;
using Assets.Scripts.Extensions;
using UniRx;

namespace Assets.Scripts.Tools.StateMachine
{
    public class StateManager<TStateType, TAwaiter> : IStateManager<TStateType>
    {
        protected List<IDisposable> _disposables;
        protected IStateValidator<TStateType> _validator;
        protected IStateMachine<IStateBehaviour<TAwaiter>> _stateMachine;
        private IStateFactory<TStateType, TAwaiter> _stateFactory;

        public StateManager(
            IStateMachine<IStateBehaviour<TAwaiter>> stateMachine,
            IStateFactory<TStateType, TAwaiter> stateFactory,
            IStateValidator<TStateType> validator = null)
        {
            _disposables = new List<IDisposable>();
            _stateMachine = stateMachine;
            _stateFactory = stateFactory;
            _validator = validator;

        }

        public TStateType CurrentState { get; protected set; }
        public TStateType PreviousState { get; protected set; }

        public virtual void Dispose()
        {
            _disposables.Cancel();
        }

        public virtual void SetState(TStateType state)
        {
            ExecuteState(state);
        }

        protected void ExecuteState(TStateType state)
        {
            if (!ValidateTransition(state))
                return;
            PreviousState = CurrentState;
            CurrentState = state;
            ChangeState(state);
        }

        protected virtual void ChangeState(TStateType state)
        {
            var stateBehaviour = _stateFactory.Create(state);
            _stateMachine.Execute(stateBehaviour);
        }

        protected virtual bool ValidateTransition(TStateType nextState)
        {
            return _validator == null || 
                   _validator.Validate(CurrentState, nextState);
        }
    }
}