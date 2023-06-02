using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace EasyMapper.Mapper
{
    public static class EMap
    {
        private static readonly ConcurrentDictionary<(Type, Type), Func<object, object>> _mappings = new ConcurrentDictionary<(Type, Type), Func<object, object>>();

        /// <summary>
        /// maps the values ​​of an object in the destination object
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TDestination Map<TSource, TDestination>(TSource source)
        {
            var mapping = _mappings.GetOrAdd((typeof(TSource), typeof(TDestination)), CreateMapFunction<TSource, TDestination>());
            return (TDestination)mapping(source);
        }

        private static Func<object, object> CreateMapFunction<TSource, TDestination>()
        {
            var sourceProperties = typeof(TSource)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var destinationProperties = typeof(TDestination)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var sourceParameter = System.Linq.Expressions
                .Expression
                .Parameter(typeof(object), "source");

            var sourceCast = System.Linq.Expressions
                .Expression
                .Convert(sourceParameter, typeof(TSource));

            var destinationParameter = System.Linq.Expressions
                .Expression
                .Parameter(typeof(object), "destination");

            var destinationCast = System.Linq.Expressions
                .Expression
                .Convert(destinationParameter, typeof(TDestination));

            var bindings = destinationProperties
                .Where(d => d.CanWrite)
                .Select(d => CreateMapBinding(sourceCast, d, sourceProperties));

            var body = System.Linq.Expressions
                .Expression
                .MemberInit(System.Linq.Expressions.Expression.New(typeof(TDestination)), bindings);

            var lambda = System.Linq.Expressions
                .Expression
                .Lambda<Func<object, object>>(body, sourceParameter);

            return lambda.Compile();
        }

        private static System.Linq.Expressions.MemberBinding CreateMapBinding(System.Linq.Expressions.Expression source, PropertyInfo destinationProperty, PropertyInfo[] sourceProperties)
        {
            var sourceProperty = sourceProperties
                .FirstOrDefault(s => s.Name.Equals(destinationProperty.Name, StringComparison.OrdinalIgnoreCase) && destinationProperty.PropertyType.IsAssignableFrom(s.PropertyType));

            if (sourceProperty == null) return null;

            var sourceAccess = System.Linq.Expressions.Expression
                .Property(source, sourceProperty);

            return System.Linq.Expressions
                .Expression
                .Bind(destinationProperty, sourceAccess);
        }
    }
}
