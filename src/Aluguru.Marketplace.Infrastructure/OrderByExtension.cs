using Aluguru.Marketplace.Domain;
using PampaDevs.Utils;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Aluguru.Marketplace.Infrastructure
{
    public static class OrderByExtensions
    {
        public static IOrderedQueryable<TEntity> OrderByPropertyName<TEntity>(
            this IQueryable<TEntity> source,
            string propertyName,
            bool isDescending) where TEntity : IEntity
        {
            Ensure.Argument.NotNull(source);
            Ensure.Argument.NotNull(propertyName);

            var type = typeof(TEntity);
            var arg = Expression.Parameter(type, "x");

            var parts = propertyName.Split('.');

            Expression expression = arg;

            foreach(var part in parts)
            {
                type = type.GetProperty(part.FirstCharToUpper()).PropertyType;
                expression = Expression.Property(expression, part.FirstCharToUpper());
            }

            var delegateType = typeof(Func<,>).MakeGenericType(typeof(TEntity), type);
            var lambda = Expression.Lambda(delegateType, expression, arg);
            
            var methodName = isDescending ? "OrderByDescending" : "OrderBy";
            var result = typeof(Queryable).GetMethods().Single(
                    method => method.Name == methodName
                              && method.IsGenericMethodDefinition
                              && method.GetGenericArguments().Length == 2
                              && method.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(TEntity), type)
                .Invoke(null, new object[] { source, lambda });

            return (IOrderedQueryable<TEntity>)result;
        }
    }
}
