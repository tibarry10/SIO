using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace WPF_Stage.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        private bool propositionSelectionnee;
        private int nombreSecret;
        private Random random;
        private int tentativesRestantes;

        public ICommand PropCommand { get; set; }
        public ICommand ResetCommand { get; set; }

        //Indique si la TextBox doit avoir le focus.
        private bool hasFocus;
        public bool HasFocus
        {
            get => hasFocus;
            set => SetProperty(ref hasFocus, value);
        }

        // Propriété pour gérer la selection automatique
        public bool PropositionSelectionnee
        {
            get => propositionSelectionnee;
            set => SetProperty(ref propositionSelectionnee, value);
        }

        private string reponse;
        public string Reponse
        {
            get => reponse;
            set => SetProperty(ref reponse, value);
        }
        private string proposition;
        public string Proposition
        {
            get => proposition;
            set
            {
                //vérifie si la saisie est valide
                if (string.IsNullOrEmpty(value) || IsTextNumeric(value))
                {
                    SetProperty(ref proposition, value);
                }
            }
        }

        //vérifie si une chaine contient uniquement des chiffres
       

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
            PropCommand = new RelayCommand(OnProposition, OnPropositionCanExecute);
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
            ImageSource = null;

            HasFocus = false;
            HasFocus = true; // donne le focus à la TextBox
            PropositionSelectionnee = true;
        }

        private bool OnPropositionCanExecute(object obj)
        {
            return !string.IsNullOrEmpty(proposition);
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

            // Forcer la sélection du texte
            PropositionSelectionnee = false; //Réinitialiser la sélection
            PropositionSelectionnee = true; //Re-sélectionner le nombre
        }

        private bool IsTextNumeric(string text)
        {
            return Regex.IsMatch(text, "^[0-9]*$"); // Accepte les chaines vides ou contenant uniquement que des chiffres
        }

        //Rénitialiser la partie
        public void OnReset(object obj)
        {
            CommenceLeJeu();
        }
    }
}
