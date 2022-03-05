using System;
using System.Collections.Generic;

namespace BetaLixT.Logger
{
    internal class LoggerScope : IDisposable
    {
        private readonly List<object> _scopes = new List<object>();

        public IDisposable Push(object scope)
        {
            this._scopes.Add(scope);
            return this;
        }

        public object[] GetScopes()
        {
            return this._scopes.ToArray();
        }

        public void Dispose()
        {
            this._scopes.RemoveAt(this._scopes.Count - 1);
        }
    }
}