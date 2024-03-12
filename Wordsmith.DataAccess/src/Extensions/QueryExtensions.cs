using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Wordsmith.DataAccess.Db.Entities;

namespace Wordsmith.DataAccess.Extensions;

public static class QueryExtensions
{
    public static IQueryable<T> OrderByProperty<T>(this IQueryable<T> query, string propertyName, bool ascending = true)
        where T : class, IEntity
    {
        var entityType = typeof(T);
        var properties = propertyName.Split(".");
        var parameter = Expression.Parameter(entityType, "x");
        Expression propertyAccess = parameter;
        
        foreach (var name in properties)
        {
            var property = entityType.GetProperty(name);
            
            if (property == null)
                throw new ArgumentException($"Property '{name}' not found in type '{entityType.FullName}'");

            propertyAccess = Expression.Property(propertyAccess, property);
            entityType = property.PropertyType;
        }
        
        var lambda = Expression.Lambda(propertyAccess, parameter);
        var methodName = ascending ? "OrderBy" : "OrderByDescending";
        var methodCallExpression = Expression.Call(
            typeof(Queryable),
            methodName,
            new[] { typeof(T), propertyAccess.Type },
            query.Expression,
            Expression.Quote(lambda)
        );

        return query.Provider.CreateQuery<T>(methodCallExpression);
    }
}