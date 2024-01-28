using System.Linq.Expressions;
using Wordsmith.Models.Exceptions;

namespace Wordsmith.DataAccess.Extensions;

public static class QueryExtensions
{
    public static IQueryable<T> OrderByProperty<T>(this IQueryable<T> source, string propertyName, bool ascending = true)
    {
        var entityType = typeof(T);
        var property = entityType.GetProperty(propertyName);
        
        if (property == null) throw new AppException("Selected property for orderBy does not exist!");

        var parameter = Expression.Parameter(entityType, "x");
        var propertyAccess = Expression.Property(parameter, property);
        var lambda = Expression.Lambda(propertyAccess, parameter);
        var methodName = ascending ? "OrderBy" : "OrderByDescending";
        var methodCallExpression = Expression.Call(typeof(Queryable), methodName,
            new[] { entityType, property.PropertyType }, source.Expression, Expression.Quote(lambda));

        return source.Provider.CreateQuery<T>(methodCallExpression);
    }
}