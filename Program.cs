using ApiRequestApp.Models;
using ApiRequestApp.Services;
using Spectre.Console;
using System.Dynamic;


HttpClient httpClient = new HttpClient();
UserInteractionService interactionService = new UserInteractionService();
TokenService tokenService = new TokenService(httpClient);
ApiService apiService;
string? bearerToken = null;
bool tokenObtained = false;


//-------------------------get token ------------------//
while (!tokenObtained)
{
    try
    {
        // Get username/pass from input
        var username = interactionService.PromptForUsername();
        var password = interactionService.PromptForPassword();
        Console.Clear();
        // Try to get token from username/password
        await AnsiConsole.Status()
        .StartAsync("Getting Token...", async ctx =>
        {
            bearerToken = await tokenService.GetToken(username, password);

        });
        //if no exception was thrown then we can break out 
        tokenObtained = true;
    }
    catch (Exception ex)
    {
        Console.Clear();
        AnsiConsole.MarkupLine("[red]Error retrieving token:[/] " + ex.Message);
        AnsiConsole.MarkupLine("[yellow]Try again...[/]");
        Console.WriteLine();


    }
}


//-------------------start api requests using token ----------------//

if (tokenObtained)
{
    if (bearerToken != null)
    {

        Console.WriteLine("Token obtained successfully.");
        apiService = new ApiService(httpClient, bearerToken);

        //prompts to build request body
        var productDetails = interactionService.PromptForProductDetails();
        var aoi = interactionService.PromptForAoi();
        var catalogIDs = interactionService.PromptForInventoryIds();
        var bucket = interactionService.PromptForBucket();
        var prefix = interactionService.PromptForPrefix();



        var outputConfig = new OutputConfig
        {
            AmazonS3 = new OutputConfig.AmazonS3Config
            {
                Bucket = bucket,
                Prefix = prefix
            }
        };

        // Create the Overrides
        var overrides = new Settings.Overrides
        {
            Product = new Settings.Overrides.ProductOverrides
            {
                productdetails = productDetails
            }
        };

        // Create the Settings
        var settings = new Settings
        {
            CustomerDescription = "",
            InventoryIds = catalogIDs,
            aoi = aoi,
            overrides = overrides
        };

        // Create the OrderRequest
        var orderRequest = new OrderRequest
        {
            OutputConfig = outputConfig,
            Settings = settings,
            Notifications = new List<Notification>(), 
            Metadata = new ExpandoObject() 
        };


        string response = "";

        //send request
        await AnsiConsole.Status()
           .StartAsync("Getting Token...", async ctx =>
           {
                response =  await apiService.SendOrderRequest(orderRequest);

           });

        Console.WriteLine(response);
        //string jsonString = System.Text.Json.JsonSerializer.Serialize(orderRequest, new JsonSerializerOptions { WriteIndented = true });
        //Console.WriteLine(jsonString);










    }



}

