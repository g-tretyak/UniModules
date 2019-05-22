﻿using System;
using System.Collections;
using System.Collections.Generic;
using UniModule.UnityTools.UniStateMachine.Interfaces;

namespace UniModule.UnityTools.UniStateMachine
{
    using UniGreenModules.UniCore.Runtime.Extension;

    public class StateBehaviour : IStateBehaviour<IEnumerator>
    {
        protected readonly List<IDisposable> _disposables = new List<IDisposable>();


        public bool IsActive { get; protected set; }

        
        #region public methods

        public IEnumerator Execute()
        {

            if (IsActive)
                yield return Wait();
            
            IsActive = true;
            
            Initialize();

            yield return ExecuteState();

            OnPostExecute();
        }

        public void Exit() {
            
            IsActive = false;
            _disposables.Cancel();
            OnExite();
            
        }



        #endregion

        protected virtual IEnumerator Wait()
        {
            while (IsActive)
            {
                yield return null;
            }
        }
        
        protected virtual void OnExite()
        {
        }
        
        protected virtual void Initialize()
        {
        }

        protected virtual void OnPostExecute() {
            
        }

        protected virtual IEnumerator ExecuteState()
        {
            yield break;
        }

    }
}
