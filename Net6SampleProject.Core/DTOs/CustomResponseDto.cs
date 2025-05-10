using System.Text.Json.Serialization;

namespace Net6SampleProject.Core.DTOs
{
    public class CustomResponseDto<T>
    {
        private CustomResponseDto()
        { }

        public T? Data { get; set; }

        public int StatusCode { get; private set; }

        public List<string> Errors { get; private set; } = new List<string>();

        public static CustomResponseDto<T> Success(T data, int statusCode = 200)
        {
            return new CustomResponseDto<T>
            {
                Data = data,
                StatusCode = statusCode
            };
        }

        public static CustomResponseDto<T> Success(int statusCode = 200)
        {
            return new CustomResponseDto<T>
            {
                StatusCode = statusCode
            };
        }

        public static CustomResponseDto<T> Fail(string error, int statusCode = 400)
        {
            return new CustomResponseDto<T>
            {
                StatusCode = statusCode,
                Errors = new List<string> { error }
            };
        }

        public static CustomResponseDto<T> Fail(List<string> errors, int statusCode = 400)
        {
            return new CustomResponseDto<T>
            {
                StatusCode = statusCode,
                Errors = errors
            };
        }

        [JsonIgnore]
        public bool IsSuccess => StatusCode >= 200 && StatusCode < 300;
    }
}
