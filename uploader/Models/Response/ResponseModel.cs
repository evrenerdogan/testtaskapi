
namespace uploader.Models.Response
{
    public class ResponseModel
    {
        public bool Status { get; set; }
        public dynamic Data { get; set; }
        public string Description { get; set; }
        public string Id { get; set; }

        public static ResponseModel Error(string description)
        {
            return new ResponseModel
            {
                Status = false,
                Description = description
            };
        }

        public static ResponseModel Success(string description, string id, dynamic data = null)
        {
            return new ResponseModel
            {
                Status = true,
                Description = description,
                Data = data,
                Id = id,
            };
        }
    }
}
