using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using DevixonApi.Data.Entities;

namespace DevixonApi.Tests
{
    public static class DbSetExtensions
    {
        public static DbSet<T> CreateMockedDbSet<T>(List <T> users) where T : class
        {
            return CreateMockFromDbSet<T>(users).Object;
        }

        private static Mock<DbSet<T>> CreateMockFromDbSet<T>(List <T> users) where T : class
        {
            var queryable = users.AsQueryable();
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IAsyncEnumerable<T>>()
                .Setup(m => m.GetAsyncEnumerator(CancellationToken.None))
                .Returns(new AsyncEnumerator<T>(queryable.GetEnumerator()));

            mockSet.As<IQueryable<T>>()
                .Setup(m => m.Provider)
                .Returns(new AsyncQueryProvider<T>(queryable.Provider));
            
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
            mockSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>((s) => users.Add(s));

            return mockSet;
        }
    }
}