using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoMapper.QueryableExtensions;
using DevixonApi.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using NSubstitute;
using System.Data.Entity.Infrastructure;


namespace DevixonApi.Tests
{
    public class DbSetFaker
    {
        public DbSet<T> ProvideQuerableDbData<T>(Mock<DbSet<T>> mockSet, IQueryable<T> data) where T : class
        {
            mockSet.As<IQueryable<T>>().Setup(b => b.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<T>>().Setup(b => b.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<T>>().Setup(b => b.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<T>>().Setup(b => b.GetEnumerator()).Returns(data.GetEnumerator());
            return mockSet.Object;
        }
        
        public DbSet <TEntity> GetQueryableMockDbSetForAsync <TEntity> (IEnumerable <TEntity> sourceList) where TEntity: class
        {
            var queryable = sourceList as IQueryable<TEntity> ?? sourceList.AsQueryable();
            var mockSet = Substitute.For<DbSet<TEntity>, IQueryable<TEntity>, IDbAsyncEnumerable<TEntity>>();
            var castMockSet = (IQueryable<TEntity>) mockSet;
            var castAsyncEnum = (IDbAsyncEnumerable<TEntity>) mockSet;

            castAsyncEnum.GetAsyncEnumerator().Returns(new TestDbAsyncEnumerator<TEntity>(queryable.GetEnumerator()));
            castMockSet.Provider.Returns(new TestDbAsyncQueryProvider<TEntity>(queryable.Provider));

            castMockSet.Expression.Returns(queryable.Expression);
            castMockSet.ElementType.Returns(queryable.ElementType);
            castMockSet.GetEnumerator().Returns(queryable.GetEnumerator());
            castMockSet.AsNoTracking().Returns(castMockSet);
            mockSet.Include(Arg.Any<string>()).Returns(mockSet);

            return mockSet;
        }
        
        public static void AddToDbSetForAsync<TEntity>(DbContext context, IEnumerable<TEntity> queryableEnumerable) where TEntity : class
        {
            var set = queryableEnumerable.
            context.Set<TEntity>().Returns(set);
        }
        
        // public DbSet <T> GetQueryableMockDbSet <T> (List <T> sourceList) where T: class 
        // {
        //     var queryable = sourceList.AsQueryable();
        //     var dbSet = new Mock<DbSet<T>>();
        //     dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
        //     dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
        //     dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        //     dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
        //     dbSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>((s) => sourceList.Add(s));
        //     return dbSet.Object;
        // }
        
        public IQueryable<User> GetUser()
        {
            IQueryable<User> user = new List<User>
            {
                new User { Id = 1, FirstName = "Moamer", LastName = "Jusupovic", Email = "moamer@live.com", Password = "171wuM6+JTaYbYq9IOLKw3IxTwn5w3am8DLHrnB/I/k=", PasswordSalt = "bW9hbWVyMTIzIw==", FacebookUser = false, Active = true, Blocked = false, ImageId = 1, CreatedAt = DateTime.Now}
            }.AsQueryable();
            return user;
        }
    }
}