using Spectre.Console;
using static ApiRequestApp.Models.Settings;

namespace ApiRequestApp.Services
{
    public class UserInteractionService
    {

        //------------------------ Prompt for username/pass -----------------------------------------////

        public string PromptForUsername()
        {
            return AnsiConsole.Prompt(
                new TextPrompt<string>("Enter username:")
                    .PromptStyle("green")
                    .ValidationErrorMessage("[red]That's not a valid username[/]")
                    .Validate(input =>
                    {
                        if (string.IsNullOrWhiteSpace(input))
                        {
                            return ValidationResult.Error("[red]Invalid Username[/]");
                        }
                        if (input.Length < 5)
                        {
                            return ValidationResult.Error("[red]Invalid Username[/]");
                        }
                        return ValidationResult.Success();
                    }));
        }

        public string PromptForPassword()
        {
            return AnsiConsole.Prompt(
                new TextPrompt<string>("Enter password:")
                    .PromptStyle("red")
                    .Secret()
                    .ValidationErrorMessage("[red]That's not a valid password[/]")
                    .Validate(input =>
                    {
                        if (string.IsNullOrWhiteSpace(input))
                        {
                            return ValidationResult.Error("[red]Invalid Password[/]");
                        }
                        if (input.Length < 5)
                        {
                            return ValidationResult.Error("[red]Invalid Password[/]");
                        }
                        return ValidationResult.Success();
                    }));
        }

        //--------------get product details-----------------------------//
        public Overrides.ProductOverrides.ProductDetails PromptForProductDetails()
        {
            var productDetails = new Overrides.ProductOverrides.ProductDetails
            {
                OutputBitDepth = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Choose an [green]Output Bit Depth[/]:")
                        .PageSize(10)
                        .AddChoices(new[] { "8", "16"})),

                Compression = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Choose a [green]Compression[/] method:")
                        .PageSize(10)
                        .AddChoices(new[] { "DEFLATE", "LZW", "None"})),

                ProductOption = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Choose a [green]Product Option[/]:")
                        .PageSize(10)
                        .AddChoices(new[] { "3BandPanSharpen", "4BandPanSharpen" })),

                FileFormat = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Choose a [green]File Format[/]:")
                        .PageSize(10)
                        .AddChoices(new[] { "GeoTIFF", "GeoTIFF-32" }))
            };

            return productDetails;
        }
        public Aoi PromptForAoi()
        {
            var aoi = new Aoi
            {
                Coordinates = new List<List<List<double>>>()
            };

            var polygon = new List<List<double>>();
            AnsiConsole.MarkupLine("[green]Enter the coordinates for each corner of the Polygon (4 corners):[/]");

            for (int i = 0; i < 4; i++)
            {
                double longitude = AnsiConsole.Prompt(
                    new TextPrompt<double>($"Enter Longitude for point {i + 1}:"));

                double latitude = AnsiConsole.Prompt(
                    new TextPrompt<double>($"Enter Latitude for point {i + 1}:"));

                polygon.Add(new List<double> { longitude, latitude });
            }

            // Close the polygon by adding the first point at the end
            polygon.Add(new List<double>(polygon.First()));

            aoi.Coordinates.Add(polygon);

            return aoi;
        }

        public List<string> PromptForInventoryIds()
        {
            var inventoryIds = new List<string>();
            AnsiConsole.MarkupLine("[green]Enter the Catalog IDs. Enter a blank line when done.[/]");

            while (true)
            {
                string inventoryId = AnsiConsole.Prompt(
                    new TextPrompt<string>("Enter Inventory ID (or blank to finish):")
                        .AllowEmpty());

                if (string.IsNullOrWhiteSpace(inventoryId))
                {
                    break;
                }

                inventoryIds.Add(inventoryId);
            }

            return inventoryIds;
        }

        public string PromptForBucket()
        {
            return AnsiConsole.Prompt(
                new TextPrompt<string>("Enter amazon bucket:")
                    .PromptStyle("green"));
        }

        public string PromptForPrefix()
        {
            var prefix = AnsiConsole.Prompt(
                 new TextPrompt<string>("Enter Folder name to save request to:")
                .PromptStyle("green"));


            if (!prefix.EndsWith("/"))
            {
                prefix += "/";
            }

            return prefix;
        }


    }








}

