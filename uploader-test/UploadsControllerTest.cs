using Moq;
using uploader.Controllers;
using uploader.Models.Post;
using uploader.Models.Response;
using uploader.Services;
using Xunit;

namespace uploader_test
{
    public class UploadsControllerTest
    {
        private readonly Mock<IFileService> _mockFileService;
        UploadsController _uploadsController;

        public UploadsControllerTest()
        {
            _mockFileService = new Mock<IFileService>();
            _uploadsController = new UploadsController(_mockFileService.Object);
        }

        [Fact]
        public void ReturnsFalseStatusWhenSizeIsZero()
        {
            FilePost post = new FilePost
            {
                Size = 0,
                Name = "Test",
                Content = "asd",
                Ext = "txt"
            };
            ResponseModel responseModel = _uploadsController.Post(post);
            Assert.False(responseModel.Status);
        }

        [Fact]
        public void ReturnsTrueStatusWhenSizeIsBiggerThanZero()
        {
            
            FilePost post = new FilePost
            {
                Size = 1000,
                Name = "Test",
                Content = "asd",
                Ext = "txt"
            };
            _mockFileService.Setup(s => s.SaveFile(post)).Returns(ResponseModel.Success("Baþarýlý", ""));
            ResponseModel responseModel = _uploadsController.Post(post);
            Assert.True(responseModel.Status);
        }

        [Fact]
        public void ReturnsFalseStatusWhenExtIsNull()
        {
            FilePost post = new FilePost
            {
                Size = 0,
                Name = "Test",
                Content = "asd",
                Ext = null
            };
            ResponseModel responseModel = _uploadsController.Post(post);

            Assert.False(responseModel.Status);
        }

        [Fact]
        public void ReturnsTrueStatusWhenExtIsNotNull()
        {
            FilePost post = new FilePost
            {
                Size = 1000,
                Name = "Test",
                Content = "asd",
                Ext = "txt"
            };
            _mockFileService.Setup(s => s.SaveFile(post)).Returns(ResponseModel.Success("Baþarýlý", ""));
            ResponseModel responseModel = _uploadsController.Post(post);

            Assert.True(responseModel.Status);
        }
    }
}
