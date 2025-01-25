using Archer2ArcheryRecords.ViewModels;
using Archery2ArcheryRecords.Core.Services;
using Microsoft.Extensions.Logging;

namespace Archer2ArcheryRecords;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .RegisterServices()
            .RegisterViews()
            .RegisterViewModels()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
    
    public static MauiAppBuilder RegisterServices(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddTransient<IScoreCardParserService, ScoreCardParserService>();
        mauiAppBuilder.Services.AddTransient<IScoreCardRetrievalService, ScoreCardRetrievalService>();
        mauiAppBuilder.Services.AddTransient<ScoreCardProcessorService>();

        // More services registered here.

        return mauiAppBuilder;        
    }

    public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<ArcheryRecordLoaderViewModel>();

        return mauiAppBuilder;        
    }

    public static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<MainPage>();
        
        return mauiAppBuilder;        
    }
}