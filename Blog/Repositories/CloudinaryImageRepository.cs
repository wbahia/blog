using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using dotenv.net;

namespace Blog.Repositories
{
    public class CloudinaryImageRepository : IImageRepository
    {
        
        public Cloudinary _Cloudinary { get; set; }
        public CloudinaryImageRepository()
        {
            DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));
            _Cloudinary = new Cloudinary(Environment.GetEnvironmentVariable("CLOUDINARY_URL"));
            _Cloudinary.Api.Secure = true;
        }

        public async Task<string?> UploadAsync(IFormFile formFile)
        {
            var uploadParams = new ImageUploadParams(){
                File = new FileDescription(formFile.FileName, formFile.OpenReadStream()),
                DisplayName = formFile.FileName
            };

            var uploadResult = await _Cloudinary.UploadAsync(uploadParams);

            if(uploadResult != null && uploadResult.StatusCode == System.Net.HttpStatusCode.OK){
                return uploadResult.SecureUrl.ToString();
            }

            return null;
        }
    }
}