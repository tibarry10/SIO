using Projet_refaire.Common;
using Projet_refaire.Properties;
using System;
using System.Windows.Input;

namespace Projet_refaire.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        #region Champs privés
        private int NombreSecret; // Nombre secret généré aléatoirement
        private Random _Random; // Générateur de nombres aléatoires
        private MainWindow MainWindow1;
        private bool FinPartie = false; // Indique si la partie est terminée
        #endregion

        #region Commandes
        public ICommand OkCommand { get; set; } // Commande pour valider une proposition
        private void OkExecute(object obj)
        {
            FinPartie = false;

            if (int.TryParse(Proposition, out int nombreUtilisateur)) // Vérifie si la proposition est un nombre
            {
                TentativeRestantes--;
                if (TentativeRestantes <= 0)
                {
                    Reponse = $"Vous avez perdu ! Le nombre était {NombreSecret}. \nVoulez-vous rejouer ?";
                    Couleur = "Red";
                    FinPartie = true; // La partie est terminée
                }

                else if (nombreUtilisateur < NombreSecret) // Proposition trop petite
                {
                    Reponse = "Le nombre que vous avez tapé est petit";
                    Couleur = "Blue";
                    ImageSource = "../Images/trop_petit.png";
                }
                else if (nombreUtilisateur > NombreSecret) // Proposition trop grande
                {
                    Reponse = "Le nombre que vous avez tapé est grand";
                    Couleur = "Blue";
                    ImageSource = "../Images/trop_grand.png";
                }
                else // Proposition correcte
                {
                    Reponse = "Bravo ! Vous avez gagné ! \nVoulez-vous rejouer ?";
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
                    MainWindow1.myTextBox.Focus();
                    MainWindow1.myTextBox.SelectAll();

                }

            }
            else
            {
                Reponse = "Veuillez entrer un nombre valide";
                Couleur = "Gray";
            }

            // Redonne le focus à la TextBox et sélectionne tout
            MainWindow1.myTextBox.Focus();
            MainWindow1.myTextBox.SelectAll();
        }
        private bool OkCanExecute(object obj)
        {
            return !string.IsNullOrEmpty(Proposition);
        }

        public ICommand RejouerCommand { get; set; } // Commande pour réinitialiser la partie // Exécutée lorsqu'on clique sur Rejouer
        public void RejouerExecute(object obj)
        {
            Init();
        }
        private bool RejouerCanExecute(object obj)
        {
            // si le joueur a gagné ou perdu on autorise le reset
            return Reponse.Contains("Bravo") || Reponse.Contains("perdu");
        }
        #endregion

        #region Bindings
        public string Reponse
        {
            get => _Reponse;
            set => SetProperty(ref _Reponse, value);
        }
        private string _Reponse;

        public string Libelle
        {
            get => _Libelle;
            set => SetProperty(ref _Libelle, value);
        }
        private string _Libelle = string.Empty;

        public string Proposition
        {
            get => _Proposition;
            set
            {
                if (string.IsNullOrEmpty(value) || value.IsNumeric())
                        {
                    SetProperty(ref _Proposition, value);
                }
            }
        }
        private string _Proposition;

        public int TentativeRestantes
        {
            get => _TentativeRestantes;
            set => SetProperty(ref _TentativeRestantes, value);
        }
        private int _TentativeRestantes; // Nombre de tentatives restantes


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

        private bool _DefaultOk = true;
        public bool DefaultOk
        {
            get => _DefaultOk;
            set => SetProperty(ref _DefaultOk, value);
        }

        private bool _DefaultRejouer = false;
        public bool DefaultRejouer
        {
            get => _DefaultRejouer;
            set => SetProperty(ref _DefaultRejouer, value);
        }
        #endregion

        #region Constructeur
        public MainWindowViewModel(MainWindow _MainWindow1)
        {
            MainWindow1 = _MainWindow1;
            OkCommand = new RelayCommand(OkExecute, OkCanExecute); // Commande pour valider une proposition
            RejouerCommand = new RelayCommand(RejouerExecute, RejouerCanExecute); // Commande pour rejouer
            Libelle = $"Devinez le nombre entre {Settings.Default.Min} et {Settings.Default.Max}";
            
            _Random = new Random();
            Init(); // Initialise le jeu
        }

        // Initialise ou réinitialise le jeu
        private void Init()
        {
            NombreSecret = _Random.Next(Settings.Default.Min, Settings.Default.Max + 1);
            TentativeRestantes = 7;
            Proposition = string.Empty;
            Reponse = string.Empty;
            Couleur = "Black";
        }
        #endregion

    }
}
