using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using NSubstitute;

namespace DevixonApi.Tests
{
    public static class MockExtension
    {
        public static DbSet<TEntity> GenerateMockDbSet<TEntity>(this IEnumerable<TEntity> queryableEnumerable) where TEntity : class
        {
            var queryable = queryableEnumerable as IQueryable<TEntity> ?? queryableEnumerable.AsQueryable();

            var mockSet = Substitute.For<DbSet<TEntity>, IQueryable<TEntity>>();
            var castMockSet = (IQueryable<TEntity>)mockSet;

            castMockSet.Provider.Returns(queryable.Provider);
            castMockSet.Expression.Returns(queryable.Expression);
            castMockSet.ElementType.Returns(queryable.ElementType);
            castMockSet.GetEnumerator().Returns(queryable.GetEnumerator());
            castMockSet.AsNoTracking().Returns(castMockSet);
            mockSet.Include(Arg.Any<string>()).Returns(mockSet);
            return mockSet;
        }
        
        public static DbSet<TEntity> GenerateMockDbSetForAsync<TEntity>(this IEnumerable<TEntity> queryableEnumerable) where TEntity : class
        {
            var queryable = queryableEnumerable as IQueryable<TEntity> ?? queryableEnumerable.AsQueryable();

            var mockSet = Substitute.For<DbSet<TEntity>, IQueryable<TEntity>, IDbAsyncEnumerable<TEntity>>();

            // async support
            var castMockSet = (IQueryable<TEntity>)mockSet;
            var castAsyncEnum = (IDbAsyncEnumerable<TEntity>)mockSet;
            castAsyncEnum.GetAsyncEnumerator().Returns(new TestDbAsyncEnumerator<TEntity>(queryable.GetEnumerator()));
            castMockSet.Provider.Returns(new TestDbAsyncQueryProvider<TEntity>(queryable.Provider));

            castMockSet.Expression.Returns(queryable.Expression);
            castMockSet.ElementType.Returns(queryable.ElementType);
            castMockSet.GetEnumerator().Returns(queryable.GetEnumerator());
            castMockSet.AsNoTracking().Returns(castMockSet);
            mockSet.Include(Arg.Any<string>()).Returns(mockSet);

            return mockSet;
        }
        
        public static void AddToDbSetForAsync<TEntity>(this DbContext context, IEnumerable<TEntity> queryableEnumerable) where TEntity : class
        {
            var set = queryableEnumerable.GenerateMockDbSetForAsync();
            context.Set<TEntity>().Returns(set);
        }
        
        public static void AddToDbSet<TEntity>(this DbContext context, IEnumerable<TEntity> queryableEnumerable) where TEntity : class
        {
            var set = queryableEnumerable.GenerateMockDbSetForAsync();
            context.Set<TEntity>().Returns(set);
        }
        
        public static IQueryable<T> MockInclude<T, TProperty>(this IQueryable<T> source, Expression<Func<T, TProperty>> path)
        {
            source.Include(path).Returns(source);
            return source;
        }
    }
}