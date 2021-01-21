using Blazored.Modal.Services;
using System;

namespace RazorShared
{
    public class PunterModalResult<T> : ModalResult
    {
        public PunterModalResult(object data, Type resultType, bool cancelled) : base(data, resultType, cancelled)
        {

        }

        public T GetResult()
        {
            if (Data is T t)
            {
                return t;
            }
            throw new NotSupportedException();
        }
    }
}
