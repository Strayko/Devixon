using System;
using System.Collections.Generic;
using System.Linq;
using DevixonApi.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace DevixonApi.Tests
{
    public class DbSetFaker
    {
        public DbSet<T> ProvideQuerableDbSet<T>(Mock<DbSet<T>> mockSet, IQueryable<T> data) where T : class
        {
            mockSet.As<IQueryable<T>>().Setup(b => b.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<T>>().Setup(b => b.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<T>>().Setup(b => b.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<T>>().Setup(b => b.GetEnumerator()).Returns(data.GetEnumerator());
            return mockSet.Object;
        }
        
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