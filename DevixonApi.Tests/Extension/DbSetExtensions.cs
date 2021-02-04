using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DevixonApi.Data.Entities;
using DevixonApi.Tests.AsyncSupport;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;

namespace DevixonApi.Tests.Extension
{
    public static class DbSetExtensions
    {
        private static List<User> _users;
        private static List<Image> _images;

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
                .Callback<T, CancellationToken>((entity, cancellationToken) => { data.Add(entity); })
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

        public static List<Image> InMemoryImagesData()
        {
            _images = new List<Image>
            {
                new Image
                {
                    Id = 1, Name = "picture.jpeg", CreatedAt = DateTime.Now
                }
            };
            return _images;
        }
    }
}