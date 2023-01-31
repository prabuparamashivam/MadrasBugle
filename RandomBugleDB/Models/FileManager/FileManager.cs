using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;
using PhotoSauce.MagicScaler;

namespace RandomBugleDB.Models.FileManager
{
    public class FileManager:IFileManager
    {
        private string _imagePath;
        public FileManager(IConfiguration config)
        {
            _imagePath = config["Path:Images"];
        }

        public FileStream ImageStream(string image)
        {
            return new FileStream(Path.Combine(_imagePath, image), FileMode.Open, FileAccess.Read);
        }

        public bool RemoveImage(string image)
        {
            try
            {
                var file = Path.Combine(_imagePath, image);
                if (File.Exists(file))
                    File.Delete(file);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
             return false;

            }
        }

        public async Task<string> SaveImage(IFormFile image)
        {
            try
            {
                var save_path = Path.Combine(_imagePath);
                if (!Directory.Exists(save_path))
                {
                    Directory.CreateDirectory(save_path);
                }
                var mime = image.FileName.Substring(image.FileName.LastIndexOf('.'));
                var filename = $"img_{DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss")}{mime}";

                using (var filestream = new FileStream(Path.Combine(save_path, filename), FileMode.Create))
                {
                    await image.CopyToAsync(filestream);
                    MagicImageProcessor.ProcessImage(image.OpenReadStream(), filestream, ImageOptions());
                }

                return filename;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "Error";

            }
        }

        private ProcessImageSettings ImageOptions() => new ProcessImageSettings
        {
            Width = 800,
            Height = 500,
            ResizeMode = CropScaleMode.Crop,
            SaveFormat = FileFormat.Jpeg,
            JpegQuality = 100,
            JpegSubsampleMode = ChromaSubsampleMode.Subsample420
        };
    };
}
