using System.ComponentModel;
using Archery2ArcheryRecords.Core.Models;
using Archery2ArcheryRecords.Core.Services;
using AsyncAwaitBestPractices.MVVM;

namespace Archer2ArcheryRecords.ViewModels;

public class ArcheryRecordLoaderViewModel : INotifyPropertyChanged
{
    private int _totalScoreCards;
    private IReadOnlyCollection<FileResult> _scoreCardPaths = [];
    private ScoreCards _scoreCards;
    private readonly IFilePicker _filePicker;
    private readonly ScoreCardProcessorService _scorecardProcessor;
    
    public ArcheryRecordLoaderViewModel(IFilePicker filePicker, ScoreCardProcessorService scorecardProcessor)
    {
        _filePicker = filePicker;
        _scorecardProcessor = scorecardProcessor;
        PickFiles = new AsyncCommand(OnPickFiles);
    }

    public IAsyncCommand PickFiles { get; init; }

    public int TotalScoreCards
    {
        get => _totalScoreCards;
        private set
        {
            _totalScoreCards = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalScoreCards)));
        }
    }

    public IReadOnlyCollection<FileResult> ScoreCardPaths
    {
        get => _scoreCardPaths;
        private set
        {
            _scoreCardPaths = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ScoreCardPaths)));
        }
    }

    public ScoreCards ScoreCards
    {
        get => _scoreCards;
        private set
        {
            _scoreCards = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ScoreCards)));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private async Task OnPickFiles()
    {
        try
        {
            var files = await _filePicker.PickMultipleAsync(new PickOptions
            {
                FileTypes = FilePickerFileType.Pdf,
                PickerTitle = "Pick your Score Cards"
            });
            ScoreCardPaths = files.ToList().AsReadOnly();
            TotalScoreCards = ScoreCardPaths.Count;
        }
        catch (Exception ex)
        {
            //todo: use a proper service to display the error
            await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async Task OnProcessScoreCards()
    {
        try
        {
            var scoreCards = await _scorecardProcessor.ProcessScoreCardsAsync(ScoreCardPaths);
            ScoreCards = scoreCards;
        }
        catch (Exception ex)
        {
            
            //todo: use a proper service to display the error   
            await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
        }
    }

}