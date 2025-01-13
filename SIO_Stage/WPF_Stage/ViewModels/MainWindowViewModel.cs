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
        private MainWindow mainWindow1;

        public ICommand PropCommand { get; set; }
        public ICommand ResetCommand { get; set; }

        //Indique si la TextBox doit avoir le focus.
        private bool hasFocus;
        public bool HasFocus
        {
            get => hasFocus;
            set => SetProperty(ref hasFocus, value);
        }

        

        private string reponse;
        public string Reponse
        {
            get => reponse;
            set 
            { 
                if (SetProperty(ref reponse, value))
                {
                    OnPropertyChanged(nameof(CanReset)); // préviens la vue que CanReset a changé
                    CommandManager.InvalidateRequerySuggested(); // Met à jour l'état de la commande PropCommand
                } 
            }
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
        public MainWindowViewModel(MainWindow MainWindow1)
        {
            mainWindow1 = MainWindow1;
            PropCommand = new RelayCommand(OnProposition, OnPropositionCanExecute);
            ResetCommand = new RelayCommand(OnReset, CanAlwaysExecute);
            random = new Random();
            CommenceLeJeu();
        }

        private bool CanAlwaysExecute(object obj)
        {
            return true; // Le bouton Rejouer reste toujours activé
        }


        private void CommenceLeJeu()
        {
            nombreSecret = random.Next(1, 101);
            TentativesRestantes = 7;
            Proposition = string.Empty;
            Reponse = string.Empty;
            Couleur = "Black";
            ImageSource = null;

        }

        private bool OnPropositionCanExecute(object obj)
        {
            return !string.IsNullOrEmpty(proposition) &&
                !Reponse.Contains("Bravo") &&
                !Reponse.Contains("perdu");
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
            // Donne le focus et sélectionne tout dans la TextBox via la vue
            mainWindow1.myTextBox.Focus();
            mainWindow1.myTextBox.SelectAll();


        }

        private bool IsTextNumeric(string text)
        {
            return Regex.IsMatch(text, "^[0-9]*$"); // Accepte les chaines vides ou contenant uniquement que des chiffres
        }

        public bool CanReset
        {
            get
            {
                // si le joueur a gagné ou perdu on autorise le reset
                return Reponse.Contains("Bravo") || Reponse.Contains("perdu");
            }
        }

        //Rénitialiser la partie
        public void OnReset(object obj)
        {
            CommenceLeJeu();
            // Donne le focus et sélectionne tout dans la TextBox via la vue
            mainWindow1.myTextBox.Focus();
            mainWindow1.myTextBox.SelectAll();
        }
    }
}
