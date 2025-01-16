using Projet_refaire2.Common;
using Projet_refaire2.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Projet_refaire2.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {

        #region champs privés
        private int _NombreSecret;
        private Random _Random;
        private MainWindow _MainWindow1;
        private bool _FinPartie = false;
        #endregion


        #region Command
        public ICommand OkCommand { get; set; }
        private void OkExecute(object obj)
        {
            _FinPartie = false;
            if (int.TryParse(Proposition, out int nombreUtilisateur))
            {
                TentativesRestantes--;
                if (nombreUtilisateur < 0)
                {
                    Reponse = $"Vous avez perdu ! le nombre était {_NombreSecret}. \nVoulez-vous rejouer ?";
                    Couleur = "Red";
                    _FinPartie = true;
                }
                else if (nombreUtilisateur < _NombreSecret)
                {
                    Reponse = "Le nombre est petit";
                    ImageSource = "../Images/trop_petit.png";
                    Couleur = "Blue";
                }
                else if (nombreUtilisateur > _NombreSecret)
                {
                    Reponse = "Le nombre est grand";
                    ImageSource = "../Images/trop_grand.png";
                    Couleur = "Yellow";
                }
                else
                {
                    Reponse = "Bravo ! \nVous avez gagné !";
                    ImageSource = "../Images/gagne.jpj";
                    Couleur = "Green";
                    _FinPartie = true;
                }
                if (_FinPartie)
                {
                    DefaultOk = false;
                    DefaultRejouer = true;
                }
                else
                {
                    DefaultOk = true;
                    DefaultRejouer = false;
                    _MainWindow1.myTextBox.Focus();
                    _MainWindow1.myTextBox.SelectAll();
                }

            }

            else
            {
                Reponse = "Veuillez entrer un nombre valide";
                Couleur = "Gray";
            }

            _MainWindow1.myTextBox.Focus();
            _MainWindow1.myTextBox.SelectAll();

        }
        private bool OkCanExecute(object obj)
        {
            return !string.IsNullOrEmpty(Proposition) && !_FinPartie;
        }

        public ICommand RejouerCommand { get; set; }
        private void RejouerExecute(object obj)
        {
            Init();
        }



        public bool RejouerCanExecute(object obj)
        {
            return _FinPartie;
        }

        #endregion

        #region Binding
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

        public string Libelle
        {
            get => _Libelle;
            set => SetProperty(ref _Libelle, value);
        }
        private string _Libelle;

        public string Reponse
        {
            get => _Reponse;
            set => SetProperty(ref _Reponse, value);
        }
        private string _Reponse;

        public int TentativesRestantes
        {
            get => _TentativesRestantes;
            set => SetProperty(ref _TentativesRestantes, value);
        }
        private int _TentativesRestantes;

        public string Couleur
        {
            get => _Couleur;
            set => SetProperty(ref _Couleur, value);
        }
        private string _Couleur;

        public string ImageSource
        {
            get => _ImageSource;
            set => SetProperty(ref _ImageSource, value);
        }
        private string _ImageSource;

        public bool DefaultOk
        {
            get => _DefaultOk;
            set => SetProperty(ref _DefaultOk, value);
        }
        private bool _DefaultOk = true;

        public bool DefaultRejouer
        {
            get => _DefaultRejouer;
            set => SetProperty(ref _DefaultRejouer, value);
        }
        private bool _DefaultRejouer = false;

        #endregion

        #region Constructeur

        public MainWindowViewModel(MainWindow mainWindow1)
        {
            _MainWindow1 = mainWindow1;
            OkCommand = new RelayCommand(OkExecute, OkCanExecute);
            RejouerCommand = new RelayCommand(RejouerExecute, RejouerCanExecute);
            Libelle = $"Devinez le nombre entre {Settings.Default.Min} et {Settings.Default.Max}";
        }
        private void Init()
        {
            _NombreSecret = _Random.Next(Settings.Default.Min, Settings.Default.Max + 1);
            TentativesRestantes = Settings.Default.CoupMax;
            Reponse = string.Empty;
            Proposition = string.Empty;
            Couleur = "Black";
        }

        #endregion
    }
}
