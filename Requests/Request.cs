
public class Request
{
}

[Serializable]
public class ResisterRequest
{
    public string UUID { get; set; }
}
[Serializable]
public class AuthRequest
{
    public string JwtToken { get; set; }
}

