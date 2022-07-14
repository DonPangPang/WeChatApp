namespace WeChatApp.Test;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        var jwt = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9zaWQiOiI0NWQxNmE0Yy00YTFjLTRjZDktYTBiYS02YWYwYjg2YmJhZjYiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9kYXRlb2ZiaXJ0aCI6IjIwMjItMDctMDYgMTE6NDM6MTIiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoibGl1YmVpIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoi6auY5bGC566h55CG5ZGYIiwiZXhwIjoxNjU3MDg2MTkyLCJpc3MiOiJodHRwOi8vd3d3LmJhaWR1LmNvbS8iLCJhdWQiOiJ4eHh4In0.JeR0iHfwYpPVAF13St-4wKdTYef-PEipHroER6HrzJg";

        var payload = jwt.Split('.')[1];

        var res = ParseBase64WithoutPadding(payload);

        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(res);
    }

    private byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }
}