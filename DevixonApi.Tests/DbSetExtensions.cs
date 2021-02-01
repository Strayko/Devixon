using System;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DevixonApi.Data.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DevixonApi.Tests
{
    public static class DbSetExtensions
    {
        private static List<User> _users;
        
        public static DbSet<T> CreateMockedDbSetAsync<T>(List<T> data) where T : class
        {
            return CreateMockFromDbSetAsync<T>(data).Object;
        }

        private static Mock<DbSet<T>> CreateMockFromDbSetAsync<T>(List<T> data) where T : class
        {
            var queryable = data.AsQueryable();
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IAsyncEnumerable<T>>()
                .Setup(m => m.GetAsyncEnumerator(CancellationToken.None))
                .Returns(new AsyncEnumerator<T>(queryable.GetEnumerator()));

            mockSet.As<IQueryable<T>>()
                .Setup(m => m.Provider)
                .Returns(new AsyncQueryProvider<T>(queryable.Provider));

            mockSet.Setup(s => s.AddAsync(It.IsAny<T>(), It.IsAny<CancellationToken>()))
                .Callback((T model, CancellationToken token) => { data.Add(model); })
                .Returns((T model, CancellationToken token) => new ValueTask<EntityEntry<T>>());

            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
            
            return mockSet;
        }

        public static List<User> InMemoryUsersData()
        {
            _users = new List<User>
            {
                new User
                {
                    Id = 1, FirstName = "Moamer", LastName = "Jusupovic", Email = "moamer@live.com",
                    Password = "171wuM6+JTaYbYq9IOLKw3IxTwn5w3am8DLHrnB/I/k=", PasswordSalt = "bW9hbWVyMTIzIw==",
                    FacebookUser = false, Active = true, Blocked = false, ImageId = 1, CreatedAt = DateTime.Now
                }
            };
            return _users;
        }
    }
}