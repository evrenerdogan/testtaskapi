using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using uploader.Models.Post;
using uploader.Models.Response;

namespace uploader.Services
{

    public interface IFileService
    {
        public ResponseModel SaveFile(FilePost filePost);
        public ResponseModel ListFiles();
        public bool IsInList(string ext);
        public bool SizeIsCorrect(double size);
    }

    public class FileService : IFileService
    {
        private FileDbContext _fileDbContext { get; set; }
        private IConfiguration _configuration { get; set; }
        private IWebHostEnvironment _environment { get; set; }
        private IHttpContextAccessor _httpContextAccessor { get; set; }

        public FileService(
            FileDbContext fileDbContext, 
            IConfiguration configuration, 
            IWebHostEnvironment environment,
            IHttpContextAccessor httpContextAccessor )
        {
            _fileDbContext = fileDbContext;
            _configuration = configuration;
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
        }


        public ResponseModel SaveFile(FilePost filePost)
        {
            // dosyayı kaydet
            try
            {
                // getting file path from appsettings.json
                string filePath = _configuration.GetSection("File").GetSection("Path").Value;
                string finalDirectory = Path.Combine(_environment.WebRootPath, filePath);

                // create directory if not exists
                if (!Directory.Exists(finalDirectory)) Directory.CreateDirectory(finalDirectory);

                Guid importUid = Guid.NewGuid();
                var firstIndex = filePost.Content.LastIndexOf(":") + 1;
                var length = filePost.Content.LastIndexOf(";") - firstIndex;
                var contentType = filePost.Content.Substring(firstIndex, length);
                var fileContent = Convert.FromBase64String(filePost.Content.Substring(filePost.Content.LastIndexOf(',') + 1));
                string pathFinal = Path.Combine(finalDirectory, importUid.ToString() + "." + filePost.Ext);
                System.IO.File.WriteAllBytes(pathFinal, fileContent);
                Data.File file = new Data.File();
                file.Link = Path.Combine(filePath, importUid.ToString() + "." + filePost.Ext);
                file.Name = filePost.Name;
                file.Size = filePost.Size;
                file.ContentType = contentType;
                file.Ext = filePost.Ext;
                file.UploadedAt = DateTime.Now;
                _fileDbContext.Files.Add(file);
                _fileDbContext.SaveChanges();

                return ResponseModel.Success("File is saved successfully", file.Uid.ToString());
            }
            catch (Exception e)
            {
                return ResponseModel.Error($"There was an error saving file: {e.Message}");
            }
        }


        public bool IsInList(string ext)
        {
            
            List<string> list = _configuration.GetSection("File").GetSection("AllowedTypes").Get<List<string>>();
            list.Add(ext);
            bool r = false;
            int length = list.Count();
            if (length > 1)
            {
                r = list.Where(i => i == list[length - 1]).Count() > 1;
            }

            return r;
        }

        public bool SizeIsCorrect(double size)
        {
            // get max size from appsettings in mb
            double maxSize = _configuration.GetSection("File").GetSection("MaxSize").Get<double>();
            return size <= (maxSize * 1024 * 1024);
        }

        public Data.File GetFile(Guid uid)
        {
            return new Data.File();
        }


        // listing files grouping them by file type
        public ResponseModel ListFiles()
        {
            var req = _httpContextAccessor.HttpContext.Request;
            var baseurl = $"{req.Scheme}://{req.Host}";
            var r = _fileDbContext.Files.
                Where(f => f.Ext != null).
                Select(f => new {
                    f.Ext,
                    Link = Path.Combine(baseurl, f.Link),
                    Name = $"{f.Name}.{f.Ext}",
                    Size = (f.Size / 1024.0).ToString("N2") + " KB",
                    UploadedAt = f.UploadedAt.ToString("dd'/'MM'/'yyyy HH:mm")
                }).
                ToList().
                GroupBy(f => f.Ext).
                Select(g => new
                {
                    Ext = g.Key,
                    Files = g.ToList()
                }).
                ToList();
            return ResponseModel.Success("Dosya Listesi", "", r);
        }
    }
}
