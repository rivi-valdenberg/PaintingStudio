namespace BL.InterfacesServices
{
    public interface IS3Service
    {
        Task<string> UploadImageAsync(Stream imageStream, string fileName, string contentType);
        Task<bool> DeleteImageAsync(string imageUrl);
        Task<Stream> GetImageAsync(string imageUrl);
    }
}