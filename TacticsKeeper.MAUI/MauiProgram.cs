using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using SQLitePCL;
using System;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Controls.Hosting;
using MudBlazor.Services;
using TacticsKeeper.Shared.Models;

using TacticsKeeper.Shared.Services;

namespace TacticsKeeper.MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

            #if DEBUG
    		    builder.Services.AddBlazorWebViewDeveloperTools();
    		    builder.Logging.AddDebug();
            #endif

            // Initialize SQLite provider
            Batteries_V2.Init();

            builder.Services.AddMudServices(); // Add MudBlazor services

            // Register SQLite database services
            builder.Services.AddSingleton<UnitService>();
            builder.Services.AddSingleton<WeaponService>();

            return builder.Build();
        }
    }
}
