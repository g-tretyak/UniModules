﻿
namespace UniGreenModules.UniCore.Runtime.Interfaces
{
    using System;
    using ObjectPool.Interfaces;
    using UniRx;

    public interface IContext : 
        IMessageBroker,
        IPoolable, 
        IReadOnlyContext,
        IDisposable,
        ILifeTimeContext,
        IContextWriter
    {
        
    }
}
