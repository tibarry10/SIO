using System;
using System.Windows.Input;
using WPF_Stage.Common;
using WPF_Stage.Properties;

namespace WPF_Stage.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        #region Propriétés
        private int NombreSecret;
        private Random Alea;
        private MainWindow MainWindow1;
        private bool FinPartie = false;
        #endregion

        #region Command Binding
        public ICommand OkCommand { get; set; }
        private void OkExecute(object obj)
        {
            FinPartie = false;

            if (int.TryParse(Proposition, out int nombreUtilisateur))
            {
                TentativesRestantes--;
                if (TentativesRestantes <= 0)
                {
                    Reponse = $"Vous avez perdu ! le nombre était {NombreSecret}. \nVoulez-vous rejouer ?";
                    Couleur = "Red";
                    FinPartie = true;
                }
                else if (nombreUtilisateur < NombreSecret)
                {
                    Reponse = "Trop petit";
                    Couleur = "Blue";
                    ImageSource = "../Images/trop_petit.png";
                }
                else if (nombreUtilisateur > NombreSecret)
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
                    FinPartie = true;
                }
                if (FinPartie)
                {
                    DefaultOk = false;
                    DefaultRejouer = true;
                }
                else
                {
                    DefaultOk = true;
                    DefaultRejouer = false;
                    // Donne le focus et sélectionne tout dans la TextBox via la vue
                    MainWindow1.myTextBox.Focus();
                    MainWindow1.myTextBox.SelectAll();
                }
            }
            else
            {
                Reponse = "Veuiller entrer un nombre valide";
                Couleur = "Gray";
                ImageSource = null;
            }
        }
        private bool OkCanExecute(object obj)
        {
            return !string.IsNullOrEmpty(proposition) &&
                !Reponse.Contains("Bravo") &&
                !Reponse.Contains("perdu");
        }

        public ICommand RejouerCommand { get; set; }
        public void RejouerExecute(object obj)
        {
            Init();
        }
        private bool OnRejouerCommandCanExecute(object obj)
        {
            // si le joueur a gagné ou perdu on autorise le reset
            return Reponse.Contains("Bravo") || Reponse.Contains("perdu");
        }

        #endregion

        #region Binding
        public string Reponse
        {
            get => reponse;
            set => SetProperty(ref reponse, value);
        }
        private string reponse;

        public string Libelle
        {
            get => libelle;
            set => SetProperty(ref libelle, value);
        }
        private string libelle = string.Empty;

        public bool DefaultOk
        {
            get => defaultOk;
            set => SetProperty(ref defaultOk, value);
        }
        private bool defaultOk = true;

        public bool DefaultRejouer
        {
            get => defaultRejouer;
            set => SetProperty(ref defaultRejouer, value);
        }
        private bool defaultRejouer = false;

        public string Proposition
        {
            get => proposition;
            set
            {
                //vérifie si la saisie est valide
                if (string.IsNullOrEmpty(value) || value.IsNumeric())
                {
                    SetProperty(ref proposition, value);
                }
            }
        }
        private string proposition;

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


        private int tentativesRestantes;
        public int TentativesRestantes
        {
            get => tentativesRestantes;
            set => SetProperty(ref tentativesRestantes, value);
        }

        #endregion

        #region Constructeur

        public MainWindowViewModel(MainWindow mainWindow1)
        {
            // Lien avec la View
            MainWindow1 = mainWindow1;
            Alea = new Random();
            Libelle = $"Devinez le nombre entre {Settings.Default.Min} et {Settings.Default.Max}";

            // Command
            OkCommand = new RelayCommand(OkExecute, OkCanExecute);
            RejouerCommand = new RelayCommand(RejouerExecute, OnRejouerCommandCanExecute);

            Init();
        }
        private void Init()
        {
            NombreSecret = Alea.Next(Settings.Default.Min, Settings.Default.Max + 1);
            TentativesRestantes = Settings.Default.CoupMax;
            Proposition = string.Empty;
            Reponse = string.Empty;
            Couleur = "Black";
            ImageSource = null;

        }

        #endregion

        #region Méthodes
        #endregion
    }
}