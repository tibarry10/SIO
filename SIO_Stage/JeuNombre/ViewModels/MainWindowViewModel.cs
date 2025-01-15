using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JeuNombre.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        private int nombreSecret;
        private Random random;
        private int tentativesRestantes;
        public string Nombre { get; set; } = "Entrer un nombre";
        public string Proposition { get; set; }
        public ICommand PropCommand { get; set; }
        public ICommand ResetCommand { get; set; }
        private string reponse;
        public string Reponse
        {
            get => reponse;
            set => SetProperty(ref reponse, value);
        }

        private string couleur;
        public string Couleur
        {
            get => couleur;
            set => SetProperty(ref couleur, value);
        }

        private string imageSource;
        public string ImageSource
        {
            get => imageSource;
            set => SetProperty(ref imageSource, value);
        }


        public int TentativesRestantes
        {
            get => tentativesRestantes;
            set => SetProperty(ref tentativesRestantes, value);
        }
        public MainWindowViewModel()
        {
            PropCommand = new RelayCommand(OnProposition);
            ResetCommand = new RelayCommand(OnReset);
            random = new Random();
            CommenceLeJeu();
        }

        private void CommenceLeJeu()
        {
            nombreSecret = random.Next(1, 101);
            TentativesRestantes = 7;
            Proposition = string.Empty;
            Reponse = string.Empty;
            Couleur = "Black";
        }


        private void OnProposition(object obj)
        {
            if (TentativesRestantes <= 0)
            {
                Reponse = "Vous avez perdu ! Cliquez sur Rejouer pour recommencer !";
                return;
            }

            if (int.TryParse(Proposition, out int nombreUtilisateur))
            {
                if (nombreUtilisateur < nombreSecret)
                {
                    Reponse = "Trop petit";
                    Couleur = "Blue";
                    ImageSource = "../Images/trop_petit.png";
                }
                else if (nombreUtilisateur > nombreSecret)
                {
                    Reponse = "Trop Grand";
                    Couleur = "Orange";
                    ImageSource = "../Images/trop_grand.png";
                }
                else
                {
                    Reponse = "Bravo, c'est le nombre exact !\nVoulez-vous rejouer ?";
                    Couleur = "Green";
                    ImageSource = "../Images/gagne.jpg";
                    return;
                }

                TentativesRestantes--;
                if (TentativesRestantes <= 0)
                {
                    Reponse = $"Vous avez perdu ! le nombre était {nombreSecret}. \nVoulez-vous rejouer ?";
                    Couleur = "Red";
                }
            }
            else
            {
                Reponse = "Veuiller entrer un nombre valide";
                Couleur = "Gray";
                ImageSource = null;
            }
        }

        //Rénitialiser la partie
        private void OnReset(object obj)
        {
            CommenceLeJeu();
        }
    }
}
