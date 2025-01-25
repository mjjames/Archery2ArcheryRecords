using Archer2ArcheryRecords.ViewModels;

namespace Archer2ArcheryRecords;

public partial class MainPage : ContentPage
{
    public MainPage(ArcheryRecordLoaderViewModel archeryRecordLoaderViewModel)
    {
        InitializeComponent();
        BindingContext = archeryRecordLoaderViewModel;
    }
}