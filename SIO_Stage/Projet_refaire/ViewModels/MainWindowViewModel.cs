using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Projet_refaire.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        private int NombreSecret;
        private Random _Random;
        private int _TentativeRestantes;
        private MainWindow MainWindow1;

        public ICommand OkCommand { get; set; } // Command pour valider une proposition
        public ICommand RejouerCommand { get; set; } // Command pour Réinitialiser une partie

        private string _Reponse;
        public string Reponse
        {
            get => _Reponse;
            set => SetProperty(ref _Reponse, value);
        }

        private string _Proposition;
        public string Proposition
        {
            get => _Proposition;
            set => SetProperty(ref _Proposition, value);
        }

        public int TentativeRestantes
        {
            get => _TentativeRestantes;
            set => SetProperty(ref _TentativeRestantes, value);
        }

        private string _Couleur;
        public string Couleur
        {
            get => _Couleur;
            set => SetProperty(ref _Couleur, value);
        }

        private string _ImageSource;
        public string ImageSource
        {
            get => _ImageSource;
            set => SetProperty(ref _ImageSource, value);
        }

        public MainWindowViewModel(MainWindow _MainWindow1)
        {
            MainWindow1 = _MainWindow1;
            OkCommand = new RelayCommand(OnProposition);
            RejouerCommand = new RelayCommand(OnRejouer);
            _Random = new Random();
            CommencePartie();
        }

        private void CommencePartie()
        {
            NombreSecret = _Random.Next(1, 101);
            TentativeRestantes = 7;
            Proposition = string.Empty;
            Reponse = string.Empty;
            Couleur = "Black";
        }

        private void OnProposition(object obj)
        {
            TentativeRestantes--;
            if (TentativeRestantes <= 0)
            {
                Reponse = $"Vous avez perdu ! Le nombre était {NombreSecret}\nVoulez-vous rejouer ?";
                return;
            }
            if (int.TryParse(Proposition, out int nombreUtilisateur))
            {
                if (nombreUtilisateur < NombreSecret)
                {
                    Reponse = "Le nombre que vous avez tapé est petit";
                    Couleur = "Red";
                    ImageSource = "../Images/trop_petit.png";
                }

                else if (nombreUtilisateur > NombreSecret)
                {
                    Reponse = "Le nombre que vous avez tapé est grand";
                    Couleur = "Red";
                    ImageSource = "../Images/trop_grand.png";
                }
                else
                {
                    Reponse = "Bravo ! Vous avez gagné";
                    Couleur = "Green";
                    ImageSource = "../Images/gagne.jpg";
                }
            }

        }

        private bool OnPropositionCanExecute (object obj)
        {
            return !string.IsNullOrEmpty(Proposition) &&
                !Reponse.Contains("Bravo") &&
                !Reponse.Contains("perdu");
        }

        public void OnRejouer(object obj)
        {
            CommencePartie();
            MainWindow1.myTextBox.Focus();
            MainWindow1.myTextBox.SelectAll();
        } 
      

    }
}
