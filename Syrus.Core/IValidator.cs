using System;
using System.Collections.Generic;

namespace Syrus.Core
{
    interface IValidator<T>
    {
        IEnumerable<Predicate<T>> Rules { get; }

        bool IsValid(T objectForValidation);
    }
}
