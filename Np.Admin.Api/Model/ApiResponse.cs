namespace Admin.Np.WebApi.Model
{
    public class ApiResponse<T> where T : class
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public ApiResponse()
        {
            Data = null;
            ErrorMessage = string.Empty;

        }
    }
}
