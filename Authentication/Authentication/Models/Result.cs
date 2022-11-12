namespace Auth.Controllers;

public class Result<T>
{
    public T? Data { get; set; }
    public Error? Error { get; set;}
}

public class Error
{
    public string? Message { get; set; }
    public int Code { get; set; }
}
