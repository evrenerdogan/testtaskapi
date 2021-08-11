using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using uploader.Models.Post;
using uploader.Models.Response;
using uploader.Services;

namespace uploader.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadsController : ControllerBase
    {

        private IFileService _fileService { get; set; }

        public UploadsController(IFileService fileService)
        {
            _fileService = fileService;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // Action which saves a new file
        [HttpPost]
        public ResponseModel Post([FromBody] FilePost value)
        {
            // validations
            if (value.Size <= 0) //file size
            {
                return ResponseModel.Error("File size must be greater than zero");
            }
            else if (string.IsNullOrEmpty(value.Ext)) // file extension
            {
                return ResponseModel.Error("File extension must be specified");
            }
            else if (string.IsNullOrEmpty(value.Name)) // file name
            {
                return ResponseModel.Error("File name cannot be empty");
            }
            else if (string.IsNullOrEmpty(value.Content)) // file content
            {
                return ResponseModel.Error("Invalid file content");
            }
            else if (!_fileService.IsInList(value.Ext))
            {
                return ResponseModel.Error("Unsported File Type");
            }
            else if (!_fileService.SizeIsCorrect(value.Size))
            {
                return ResponseModel.Error("File is too big to upload");
            }
            return _fileService.SaveFile(value);
        }

        // action which returns all files
        [HttpPost]
        [Route("[action]")]
        public ResponseModel List([FromBody] dynamic value)
        {
            return _fileService.ListFiles();
        }
    }
}
