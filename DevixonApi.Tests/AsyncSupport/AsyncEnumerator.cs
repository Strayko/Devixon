using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevixonApi.Tests.AsyncSupport
{
    public class AsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;

        public AsyncEnumerator(IEnumerator<T> inner)
        {
            _inner = inner;
        }

        public void Dispose()
        {
            _inner.Dispose();
        }

        public T Current => _inner.Current;

        public async ValueTask<bool> MoveNextAsync()
        {
            await Task.CompletedTask;
            return _inner.MoveNext();
        }

        public ValueTask DisposeAsync()
        {
            _inner.Dispose();
            return new ValueTask();
        }
    }
}