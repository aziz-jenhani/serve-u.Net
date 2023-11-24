namespace serve_u.Helper
{
    public class APIResponse
    {
        public int StatusCode { get; set; }
        public bool Success { get; set; }
        public dynamic Data { get; set; }
    }
}
