using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ApiRequestApp.Models
{
    public class OrderRequest
    {
        [JsonPropertyName("output_config")]
        public OutputConfig? OutputConfig { get; set; }

        [JsonPropertyName("settings")]
        public Settings Settings { get; set; }

        [JsonPropertyName("notifications")]
        public List<Notification> Notifications { get; set; } = new List<Notification>();

        [JsonPropertyName("metadata")]
        public dynamic Metadata { get; set; } = new ExpandoObject();
    }

    public class Notification
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "email";

        [JsonPropertyName("target")]
        public string? Email { get; set; } = "";

        [JsonPropertyName("level")]
        public string Level { get; set; } = "FINAL_ONLY";
    }




    public class OutputConfig
    {
        [JsonPropertyName("amazon_s3")]
        public AmazonS3Config? AmazonS3 { get; set; }

        public class AmazonS3Config
        {
            [JsonPropertyName("bucket")]
            public string? Bucket { get; set; }

            [JsonPropertyName("prefix")]
            public string? Prefix { get; set; }
        }
    }


    public class Settings
    {
        [JsonPropertyName("customer_description")]
        public string CustomerDescription { get; set; } = "";

        [JsonPropertyName("inventory_ids")]
        public List<string> InventoryIds { get; set; }

        [JsonPropertyName("aoi")]
        public Aoi aoi { get; set; }

        [JsonPropertyName("overrides")]
        public Overrides overrides { get; set; }


        public class Aoi
        {
            [JsonPropertyName("type")]
            public string Type { get; set; } = "Polygon";

            [JsonPropertyName("coordinates")]
            public List<List<List<double>>> Coordinates { get; set; }
        }

        public class Overrides
        {
            [JsonPropertyName("product")]
            public ProductOverrides Product { get; set; }


            public class ProductOverrides
            {
                [JsonPropertyName("product_details")]
                public ProductDetails productdetails { get; set; }

                public class ProductDetails
                {
                    [JsonPropertyName("output_bit_depth")]
                    public string? OutputBitDepth { get; set; }

                    [JsonPropertyName("compression")]
                    public string Compression { get; set; }

                    [JsonPropertyName("product_option")]
                    public string ProductOption { get; set; }

                    [JsonPropertyName("file_format")]
                    public string FileFormat { get; set; }
                }

            }


        }
    }


}

