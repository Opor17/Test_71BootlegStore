namespace _71BootlegStore.Repository.IRepository
{
    public interface IFileUpload
    {
        Task<string> UploadFile(IFormFile file, string directory, string? oldImgUrl = null);

        Task<bool> Unlink(string oldImgUrl);
    }
}
