namespace DevixonApi.Data.Helpers
{
    public static class Base64EncodeHelper
    {
        public static string Generate(string password)
        {
            var hash = System.Text.Encoding.UTF8.GetBytes(password);
            return System.Convert.ToBase64String(hash);
        }
    }
}