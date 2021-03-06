﻿// OnionFruit.Status Copyright 2021 DragonFruit Network <inbox@dragonfruit.network>
// Licensed under MIT. Please refer to the LICENSE file for more info

using System;
using System.IO;
using System.Linq;
using Bia.Countries.Iso3166;
using DragonFruit.Common.Data;
using DragonFruit.Common.Data.Services;

namespace DragonFruit.OnionFruit.Status.Generator
{
    internal static class Program
    {
        private static readonly ApiClient Client = new();

        private static void Main(string[] args)
        {
            Console.WriteLine("Fetching data...");
            var nodes = Client.GetServerInfo().Relays;
            var countries = nodes.GroupBy(x => x.CountryCode.ToUpper()).Where(x => Countries.GetCountryByAlpha2(x.Key) is not null).ToArray();

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"Downloaded {nodes.Length} relay metadata packets over {countries.Length} countries");
            var response = (OnionFruitCountriesList)countries;

            Console.WriteLine($"Response contains {response.In.Length} entry countries and {response.Out.Length} exit countries");

            var saveLocation = Environment.GetEnvironmentVariable("OUTPUT_FILE") ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "countries.json");
            Console.Write($"Writing to {saveLocation}...");

            FileServices.WriteFile(saveLocation, response);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Complete");
        }
    }
}
