using System.Linq.Expressions;
using System.Reflection;

namespace Syrus.Core
{
    internal static class ObjectActivator
    {
        private delegate object Activator(params object[] args);

        public static T CreateInstance<T>(ConstructorInfo ci) where T : class => CreateExpression<Activator>(ci)() as T;

        private static TDelegate CreateExpression<TDelegate>(ConstructorInfo ctor) where TDelegate : class
        {
            var ctorParams = ctor.GetParameters();
            var paramExp = Expression.Parameter(typeof(object[]), "args");

            var expArr = new Expression[ctorParams.Length];

            for (var i = 0; i < ctorParams.Length; i++)
            {
                var ctorType = ctorParams[i].ParameterType;
                var argExp = Expression.ArrayIndex(paramExp, Expression.Constant(i));
                var argExpConverted = Expression.Convert(argExp, ctorType);
                expArr[i] = argExpConverted;
            }

            var newExp = Expression.New(ctor, expArr);
            var lambda = Expression.Lambda<TDelegate>(newExp, paramExp);
            return lambda.Compile();
        }
    }
}
