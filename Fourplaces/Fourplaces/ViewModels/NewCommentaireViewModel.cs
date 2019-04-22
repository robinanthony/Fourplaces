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
//==============================================================================
//================================= ATTRIBUTS ==================================
//==============================================================================
        public ICommand AddCommand { get; private set; }

        private long _placeId;
        [NavigationParameter]
        public long PlaceId
        {
            get { return this._placeId; }
            set
            {
                SetProperty(ref this._placeId, value);
            }
        }

        private string _titleLabel;
        public string TitleLabel
        {
            get => this._titleLabel;
            set => SetProperty(ref this._titleLabel, value);
        }

        private string _monCommentaire;
        public string MonCommentaire
        {
            get => this._monCommentaire;
            set => SetProperty(ref this._monCommentaire, value);
        }

//==============================================================================
//============================== FCT PRINCIPALES ===============================
//==============================================================================
        public NewCommentaireViewModel()
        {
            this.AddCommand = new Command(this.AddClicked);
            this.TitleLabel = "Votre nouveau commentaire";
        }

        public override async Task OnResume()
        {
            await base.OnResume();
        }

//==============================================================================
//============================== FCT SECONDAIRES ===============================
//==============================================================================
        private void AddClicked()
        {
            AddCommentaire();
        }

        private async void AddCommentaire()
        {
            if (!string.IsNullOrWhiteSpace(this.MonCommentaire))
            {
                (Boolean test, string texte) = await RestService.Rest.AddCommentaire(this.PlaceId, this.MonCommentaire);
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
