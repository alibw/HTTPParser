namespace HTTPParser;

public class Request : IEquatable<Request>
{
    public string Type {get;set;}
    public string Url {get;set;}
    public string Protocol {get;set;}
    public List<string> Headers {get;set;}
    public string Body {get;set;}

    public Request()
    {
        Headers = new List<string>();
    }

    public bool Equals(Request? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Type == other.Type && Url == other.Url && Protocol == other.Protocol && (Headers.SequenceEqual(other.Headers)) && Body == other.Body;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Request)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Type, Url, Protocol, Headers, Body);
    }
}