namespace HTTPParser;

public class Request
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
}