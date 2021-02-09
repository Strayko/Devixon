using DevixonApi.Data.Models;
using DevixonApi.Data.Requests;
using DevixonApi.Data.Responses;
using NUnit.Framework;

namespace DevixonApi.Tests.TestHelper
{
    public class UserHelper
    {
        public LoginRequest Login(string email, string password)
        {
            return new LoginRequest
            {
                Email = email,
                Password = password
            };
        }

        public void AssertResult(string firstName, string lastName, string email, LoggedUserResponse result)
        {
            Assert.AreEqual(firstName, result.FirstName);
            Assert.AreEqual(lastName, result.LastName);
            Assert.AreEqual(email, result.Email);
            Assert.IsNotEmpty(result.Token);
        }

        public UserModel Model(string userModelValues)
        {
            var values = userModelValues.Split(',');
            var id = int.Parse((string) values.GetValue(0));
            var firstName = (string) values.GetValue(1);
            var lastName = (string) values.GetValue(2);
            var email = (string) values.GetValue(3);
            var setImage = (string) values.GetValue(4);
            if (setImage == "null") setImage = null;
            var password = (string) values.GetValue(5);
            if (password == "null") password = null;
            
            var userModel = new UserModel
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                SetImage = setImage,
                Password = password
            };
            return userModel;
        }
    }
}