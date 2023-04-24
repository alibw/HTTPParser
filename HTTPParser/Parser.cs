namespace HTTPParser;

public class Parser
{
    public string Input;
        private IEnumerable<char> splitters = new []
        {
            ' ',
            '\t',
            '\r',
            '\n'
        };
        public List<string> Split()
        {
            return Input.Split((char[]?)splitters, StringSplitOptions.RemoveEmptyEntries).ToList();
        }
        public List<Request> Parse()
        {
            var requests = new List<Request>();
            var splitted = Split();
            var request = new Request();
            int bodyStartIndex = 0;
            bool typeFilled = false;
            bool urlFilled = false;
            bool protocolFilled = false;

            for (int i = 0; i < splitted.Count ; i++)
            {
                if(splitted[i] == "###")    
                {
                    if(!string.IsNullOrEmpty(request.Url) && !string.IsNullOrEmpty(request.Type))
                       requests.Add(request);
                    request = new Request();
                    typeFilled = false;
                    urlFilled = false;
                    protocolFilled = false;
                    continue;
                }
            
                if(!typeFilled)
                {
                    request.Type = splitted[i];
                    typeFilled = true;
                    continue;
                }

                if(typeFilled && !urlFilled)
                {
                    request.Url = splitted[i];
                    urlFilled = true;
                    continue;
                }

                if(typeFilled && urlFilled && !protocolFilled)
                {
                    request.Protocol = splitted[i];
                    protocolFilled = true;
                    continue;
                }

                if (i > 0 && splitted[i - 1].ToCharArray().Last() == ':' && bodyStartIndex != 0)
                {
                    request.Headers.Add($"{splitted[i - 1]}{splitted[i]}");
                }

                if(splitted[i] == "{")            
                    bodyStartIndex = i;

                if (splitted[i] == "}")
                {
                    var body = splitted.Skip(bodyStartIndex).Take(i - bodyStartIndex + 1);
                    request.Body = String.Join(" ",body);
                    bodyStartIndex = 0;
                    requests.Add(request);
                    request = new Request();
                }
            }
            return requests;
        }
}