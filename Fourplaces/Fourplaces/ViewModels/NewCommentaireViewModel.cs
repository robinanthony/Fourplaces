using Fourplaces.Models;
using Storm.Mvvm;
using Storm.Mvvm.Navigation;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Fourplaces.ViewModels
{
    public class NewCommentaireViewModel : ViewModelBase
    {
        private long _placeId;
        private string _monCommentaire;
        public ICommand AddCommand { get; set; }

        [NavigationParameter]
        public long PlaceId
        {
            get { return _placeId; }
            set
            {
                SetProperty(ref _placeId, value);
            }
        }

        public string TitleLabel { get; set; }

        public string MonCommentaire
        {
            get => this._monCommentaire;
            set => SetProperty(ref this._monCommentaire, value);
        }

        public NewCommentaireViewModel()
        {
            AddCommand = new Command(AddClicked);
            TitleLabel = "Votre nouveau commentaire";
        }

        public override async Task OnResume()
        {
            await base.OnResume();
        }

        private void AddClicked()
        {
            AddCommentaire();
        }

        private async void AddCommentaire()
        {
            if (!string.IsNullOrWhiteSpace(MonCommentaire))
            {
                (Boolean test, string texte) = await RestService.Rest.AddCommentaire(PlaceId, MonCommentaire);
                if (test)
                {
                    await Application.Current.MainPage.DisplayAlert("Commentaire ajouté", texte, "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Erreur", texte, "OK");
                }
                await NavigationService.PopAsync();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Commentaire éronné", "Votre commentaire ne peut être vide.", "OK");
            }
        }
            
    }
}
