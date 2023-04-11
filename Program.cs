using Microsoft.Extensions.Configuration;

// add database 
Console.WriteLine("Enter server name: ");
string server = Console.ReadLine();
Console.WriteLine("Enter username: ");
string username = Console.ReadLine();
Console.WriteLine("Enter password: ");
string password = Console.ReadLine();
Console.WriteLine("Enter database name: ");
string database = Console.ReadLine();


var db = new MyDatabase(server, username, password, database);

db.GetConnection();

Console.WriteLine("Connection successful.");

// get schema 
db.GetSchema();

// add openai
Console.Clear();
Console.WriteLine("Add your OpenAI API key: ");
string apikey = Console.ReadLine();

GPTService api = new GPTService(apikey);

api.PingGPT();

Console.WriteLine("API key valid.");


// start prompting
Console.Clear();
string schemaText = File.ReadAllText("schema.yaml");

bool running = true;

while(running) {
    Console.WriteLine("Enter a prompt:");
    string prompt = Console.ReadLine();

    string builtPrompt = $@"using: 
    {schemaText}
    t-sql query: {prompt}.";

    try {
    string result = api.GetOpenAICompletions(builtPrompt);
    Console.Write(result); 
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }

    Console.ReadKey();
}
