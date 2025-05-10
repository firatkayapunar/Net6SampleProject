namespace Net6SampleProject.MVC.Models.Responses
{
    public class CustomResponse<T>
    {
        public T? Data { get; set; }
        public int StatusCode { get; set; }
        public List<string>? Errors { get; set; }
    }
}
