using Treinou.Mobile.Models;
using Treinou.Mobile.PageModels;

namespace Treinou.Mobile.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }
    }
}