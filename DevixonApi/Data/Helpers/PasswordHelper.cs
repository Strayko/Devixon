namespace DevixonApi.Data.Helpers
{
    public static class PasswordHelper
    {
        public static string EncodeAndHash(string password, out string passwordHash)
        {
            var base64Encode = Base64EncodeHelper.Generate(password);
            passwordHash = HashingHelper.HashUsingPbkdf2(password, base64Encode);
            return base64Encode;
        }
    }
}