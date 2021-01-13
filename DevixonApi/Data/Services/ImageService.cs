namespace DevixonApi.Data.Services
{
    public class ImageService
    {
        private readonly AppDbContext _appDbContext;

        public ImageService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        
        
    }
}