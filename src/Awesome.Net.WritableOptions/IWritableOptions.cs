using System;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace Awesome.Net.WritableOptions
{
    public interface IWritableOptions<T> : IOptionsSnapshot<T> where T : class, new()
    {
        void Update(Func<T,T> updateAction, bool reload = true, JsonSerializerOptions serializerOptions = null);
        void Reload();
    }
}