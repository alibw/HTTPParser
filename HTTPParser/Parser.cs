using System.Text;

namespace HTTPParser;

public class Parser
{
    public string Input;
    public List<string> Split()
        {
            return Input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToList();
        }
        public List<Request> Parse()
        {
            var requests = new List<Request>();
            var splitted = Split();
            var request = new Request();
            bool readingBody = false;
            bool firstLineRead = false;
            bool readingCodeBlock = false;
            string body = "";

            for (int i = 0; i < splitted.Count ; i++)
            {
                if(splitted[i] == "###")    
                {
                    if(!string.IsNullOrEmpty(request.Url) && !string.IsNullOrEmpty(request.Type))
                       requests.Add(request);
                    request = new Request();
                    firstLineRead = false;
                    continue;
                }

                if (!firstLineRead)
                {
                    var firstLine = splitted[i].Split(new[] {' '}, 3);
                    request.Type = firstLine[0];
                    request.Url = firstLine[1];
                    request.Protocol = 2 < firstLine.Length ?  firstLine[2] : null;
                    firstLineRead = true;
                    continue;
                }
                
                if (firstLineRead && !readingBody && !readingCodeBlock)
                {
                    var header = splitted[i].Split(':');
                    if(header.Length == 2)
                    request.Headers.Add(new Tuple<string, string>(header[0],header[1]));
                }

                if (splitted[i] == "< {%")
                {
                    readingCodeBlock = true;
                    continue;
                }
                if (splitted[i] == "%}")
                {
                    readingCodeBlock = false;
                    continue;
                }

                if (readingCodeBlock)
                {
                    if (splitted[i].Contains("assert"))
                        request.CodeblockLines.Add(splitted[i]);
                    continue;
                }

                if (splitted[i] == "{")
                    readingBody = true;
                if (readingBody)
                    body += $"{splitted[i]}{Environment.NewLine}";
                

                if (splitted[i] == "}")
                {
                    request.Body = body;
                    readingBody = false;
                    requests.Add(request);
                    request = new Request();
                }
            }
            return requests;
        }

        public string SendRequest(Request request)
        {
            var client = new HttpClient();
            
            foreach (var header in request.Headers)
            {
                client.DefaultRequestHeaders.Add(header.Item1,header.Item2);
            }
            
            return Convert.ToString(request.Type switch 
            {
              "GET" => client.GetAsync(request.Url),
              "POST" =>
                  () =>
                  {
                      var body = new StringContent(request.Body, Encoding.UTF8, "application/json");
                      return client.PostAsync(request.Url, body).Result.ToString();
                  }
                  
            });
        }
        
     
        
}