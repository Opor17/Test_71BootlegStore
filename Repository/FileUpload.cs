using _71BootlegStore.Repository.IRepository;

namespace _71BootlegStore.Repository
{
    public class FileUpload : IFileUpload
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileUpload(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> UploadFile(IFormFile file, string directory, string? oldImgUrl = null)
        {
            string wwwRootPath = this._webHostEnvironment.WebRootPath;
            string path = "";
            try
            {
                if (file.Length > 0)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    path = Path.GetFullPath(Path.Combine(wwwRootPath, @"Storage\" + directory));

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    if (!string.IsNullOrEmpty(oldImgUrl))
                    {
                        // delete old image
                        var oldImagePath = Path.Combine(wwwRootPath, oldImgUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    return @"\Storage\" + directory + @"\" + fileName;
                }
                else
                {
                    return "File Copy Failed";
                }
            }
            catch (Exception ex)
            {
                throw new Exception("File Copy Failed", ex);
            }
        }

        public async Task<bool> Unlink(string oldImgUrl)
        {
            string wwwRootPath = this._webHostEnvironment.WebRootPath;
            if (!string.IsNullOrEmpty(oldImgUrl))
            {
                // delete old image
                var oldImagePath = Path.Combine(wwwRootPath, oldImgUrl.TrimStart('\\'));

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }

                return true;
            }

            return false;
        }
    }
}
