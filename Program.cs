using ConcertScannerAppWASM.Data;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;


namespace ConcertScannerAppWASM
{
    public class Program
    {
        private static string PromptText(string label, bool hidden)
        {
            Console.Write(label + ":");
            var input = string.Empty;
            ConsoleKey key;
            do
            {
                var keyInfo = Console.ReadKey(true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && input.Length > 0)
                {
                    Console.Write("\b \b");
                    input = input[0..^1];
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write(hidden ? "*" : keyInfo.KeyChar);
                    input += keyInfo.KeyChar;
                }
            } while (key != ConsoleKey.Enter);
            Console.WriteLine();
            return input;
        }
        public static async Task Main(string[] args)
        {
            Environment.SetEnvironmentVariable("server", PromptText("IP/Host", false));
            Environment.SetEnvironmentVariable("user", PromptText("Username", false));
            Environment.SetEnvironmentVariable("password", PromptText("Password", true));
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");
            builder.Services.AddSingleton<DataService>();
            builder.Services.AddMudServices();

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            await builder.Build().RunAsync();
        }
    }
}
