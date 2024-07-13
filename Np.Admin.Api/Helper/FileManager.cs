namespace Np.Admin.WebApi.Helper
{
    public class FileManager
    { // Uploads an image file asynchronously
        public static async Task<Dictionary<string, string>> UploadImageAsync(IFormFile file, string path)
        {
            var res = new Dictionary<string, string>();

            if (file != null)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".svg", ".webp", ".ico" };
                var ext = Path.GetExtension(file.FileName);

                if (file.Length > 2097152)
                {
                    res.Add("status", "error");
                    res.Add("message", "Maximum file upload size is 2MB.");
                }
                else if (allowedExtensions.Contains(ext.ToLower()))
                {
                    string myfile = Guid.NewGuid().ToString() + ext;
                    string fullpath = Path.Combine(path, myfile);

                    try
                    {
                        Directory.CreateDirectory(path);

                        using FileStream stream = new(fullpath, FileMode.Create);
                        await file.CopyToAsync(stream);
                        stream.Close();

                        res.Add("status", "success");
                        res.Add("message", myfile);
                    }
                    catch (Exception ex)
                    {
                        res.Add("status", "error");
                        res.Add("message", ex.Message);
                    }
                }
                else
                {
                    res.Add("status", "error");
                    res.Add("message", "File format not supported.");
                }
            }
            else
            {
                res.Add("status", "error");
                res.Add("message", "File not provided.");
            }

            return res;
        }

        // Uploads an attachment file asynchronously
        public static async Task<Dictionary<string, string>> UploadAttachmentAsync(IFormFile file, string path)
        {
            var res = new Dictionary<string, string>();

            if (file != null)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".svg", ".webp", ".ico", ".pdf" };
                var ext = Path.GetExtension(file.FileName);

                if (file.Length > 2097152)
                {
                    res.Add("status", "error");
                    res.Add("message", "Maximum file upload size is 2MB.");
                }
                else if (allowedExtensions.Contains(ext.ToLower()))
                {
                    string myfile = Guid.NewGuid().ToString() + ext;
                    string fullpath = Path.Combine(path, myfile);

                    try
                    {
                        Directory.CreateDirectory(path);

                        using FileStream stream = new(fullpath, FileMode.Create);
                        await file.CopyToAsync(stream);
                        stream.Close();

                        res.Add("status", "success");
                        res.Add("message", myfile);
                    }
                    catch (Exception ex)
                    {
                        res.Add("status", "error");
                        res.Add("message", ex.Message);
                    }
                }
                else
                {
                    res.Add("status", "error");
                    res.Add("message", "File format not supported.");
                }
            }
            else
            {
                res.Add("status", "error");
                res.Add("message", "File not provided.");
            }

            return res;
        }

        // Deletes a file
        public static void DeleteFile(string existingPath)
        {
            if (!String.IsNullOrEmpty(existingPath))
            {
                try
                {
                    FileInfo existingFile = new(existingPath);
                    if (existingFile.Exists)
                    {
                        existingFile.Delete();
                    }
                }
                catch { }
            }
        }
    }
}
