﻿using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Extensions;

namespace Assets.Scripts.Tools.StateMachine
{
    public class StateBehaviour : IStateBehaviour<IEnumerator>
    {
        protected readonly List<IDisposable> _disposables = new List<IDisposable>();
        protected bool _isActive;
        
        #region public methods

        public IEnumerator Execute()
        {

            if (_isActive)
                yield return Wait();
            
            _isActive = true;
            
            Initialize();

            yield return ExecuteState();

        }

        public void Stop() {
            
            _isActive = false;
            _disposables.Cancel();
            OnStateStop();
            
        }



        #endregion

        protected virtual IEnumerator Wait()
        {
            while (_isActive)
            {
                yield return null;
            }
        }
        
        protected virtual void OnStateStop()
        {
        }
        
        protected virtual void Initialize()
        {
        }

        protected virtual IEnumerator ExecuteState()
        {
            yield break;
        }

    }
}
