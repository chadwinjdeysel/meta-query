using RestSharp;

public class GPTService {

    public GPTService(string apiKey)
    {
        API_KEY = apiKey;
    }
    
    private string API_KEY { get; set; }

    public string GetOpenAICompletions(string prompt)
    {
        var client = new RestClient("https://api.openai.com/v1/completions");
        var request = new RestRequest(Method.POST);

        request.AddHeader("Content-Type", "application/json");  
        request.AddHeader("Authorization", $"Bearer {API_KEY}");

        request.AddJsonBody(new {
            model = "text-davinci-003",
            prompt = prompt,
            max_tokens = 256,
            temperature = 0.7,
            top_p = 1,
            frequency_penalty = 0,
            presence_penalty = 0,
        });

        IRestResponse response = client.Execute(request);

        if (response.IsSuccessful)
        {
            if(String.IsNullOrWhiteSpace(response.Content))
            {
                throw new System.Exception("No response from OpenAI");
            }

            GPTResult result = Newtonsoft.Json.JsonConvert.DeserializeObject<GPTResult>(response.Content);

            string generatedText = result.Choices[0].Text;

            return generatedText;   
        }
        else
        {
            throw new System.Exception(response.Content);
        }
    }

    public void PingGPT() {
        var client = new RestClient("https://api.openai.com/v1/engines");
        var request = new RestRequest(Method.GET);

        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Authorization", $"Bearer {API_KEY}");

        IRestResponse response = client.Execute(request);

        if (!response.IsSuccessful)
        {
            throw new System.Exception(response.Content);
        }
    }
}