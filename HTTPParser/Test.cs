using NUnit.Framework;

namespace HTTPParser;

[TestFixture]
public class Test
{
    private string script = @"GET https://example.com/comments/1 HTTP/1.1

###

GET https://example.com/topics/1 HTTP/1.1

###

POST https://example.com/comments HTTP/1.1
content-type: application/json

{
    'name': 'sample',
    'time': 'Wed, 21 Oct 2015 18:27:50 GMT'
}";
    [Test]
    public void EqualityTest()
    {
        List<Request> requests = new ()
        {
            new ()
            {
                Type = "GET",
                Url = "https://example.com/comments/1",
                Protocol = "HTTP/1.1"
            },
            new ()
            {
                Type = "GET",
                Url = "https://example.com/topics/1",
                Protocol = "HTTP/1.1"
            },
            new ()
            {
            Type = "POST",
            Url = "https://example.com/comments",
            Protocol = "HTTP/1.1",
            Headers = new List<string>()
            {
                "content-type:application/json"
            },
            Body = @"{ 'name': 'sample', 'time': 'Wed, 21 Oct 2015 18:27:50 GMT' }"
            }

        };
        Parser parser = new Parser();
        parser.Input = script;
        var result = parser.Parse();
        Assert.AreEqual(requests,result);
    }
    
}