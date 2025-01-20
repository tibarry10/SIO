using Chifoumi.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;

namespace Chifoumi.ViewModels
{
    public enum ChoixEnum { Pierre, Feuille, Ciseau}
    class MainWindowViewModel : ViewModelBase
    {
        #region champs privés
        //private int _PropHasard;
        private Random _Random;
        private bool _FinPartie;
        #endregion

        #region Command
        public ICommand PierreCommand { get; set; }
        private void PierreExecute(object obj)
        {
            Jeu(ChoixEnum.Pierre);
            
        }
        private bool PierreCanExecute(object obj)
        {
            return !_FinPartie;
        }

        public ICommand FeuilleCommand { get; set; }
        private void FeuilleExecute(object obj)
        {
            Jeu(ChoixEnum.Feuille);

        }
        private bool FeuilleCanExecute(object obj)
        {
            return !_FinPartie; // Les boutons sont actifs tant que la partie n'est pas terminée
        }

        public ICommand CiseauCommand { get; set; }
        private void CiseauExecute(object obj)
        {
            Jeu(ChoixEnum.Ciseau);
        }
        private bool CiseauCanExecute(object obj)
        {
            return !_FinPartie;
        }

        public ICommand RejouerCommand { get; set; }
        private void RejouerExecute(object obj)
        {
            ScoreUtilisateur = 0;
            ScoreMachine = 0;
            Reponse = "Nouvelle partie commencée !";
            _FinPartie = false;

            // Réinitialiser les images
            ImagePierre = "../images/Pierre.png";
            ImageFeuille = "../images/Feuille.png";
            ImageCiseau = "../images/ciseau.png";
        }

        private void Jeu(ChoixEnum choixUtilisateur)
        {
            if (_FinPartie) return; // Si la partie est terminée, ne rien faire

            ChoixEnum choixMachine = (ChoixEnum)_Random.Next(0, 3);

            // Réinitialiser toutes les images à leurs versions grisées
            ImagePierre = "../images/Pierre2.png";
            ImageFeuille = "../images/Feuille2.png";
            ImageCiseau = "../images/Ciseau2.png";
            switch (choixMachine)
            {
                case ChoixEnum.Pierre: ImagePierre = "../images/Pierre.png"; break;
                case ChoixEnum.Feuille: ImageFeuille = "../images/Feuille.png"; break;
                case ChoixEnum.Ciseau: ImageCiseau = "../images/Ciseau.png"; break;

            }

            // Affectation des points
            if (choixUtilisateur == choixMachine)
            {
                Reponse = "Egalité";
                ScoreUtilisateur = 0;
                ScoreMachine = 0;
                
            }

            else if ((choixUtilisateur == ChoixEnum.Pierre && choixMachine == ChoixEnum.Ciseau) ||
                (choixUtilisateur == ChoixEnum.Feuille  && choixMachine == ChoixEnum.Pierre ) || 
                (choixUtilisateur == ChoixEnum.Ciseau && choixMachine == ChoixEnum.Feuille))
            {
                Reponse = "Vous gagnez";
                ScoreUtilisateur++;
                ScoreMachine = 0;

            }
            else
            {
                Reponse = "La machine gagne !";
                ScoreMachine++;
                ScoreUtilisateur = 0;
            }

            VerifierFindePartie();
        }
        #endregion

        #region Binding
        public string Reponse
        {
            get => _Reponse;
            set => SetProperty(ref _Reponse, value);
        }
        private string _Reponse;

        public string ImageSource
        {
            get => _ImageSource;
            set => SetProperty(ref _ImageSource, value);
        }
        private string _ImageSource;

        private string _ImagePierre = "../images/Pierre.png";
        public string ImagePierre
        {
            get => _ImagePierre;
            set => SetProperty(ref _ImagePierre, value);
        }

        private string _ImageFeuille = "../images/Feuille.png";
        public string ImageFeuille
        {
            get => _ImageFeuille;
            set => SetProperty(ref _ImageFeuille, value);
        }

        private string _ImageCiseau = "../images/ciseau.png";
        public string ImageCiseau
        {
            get => _ImageCiseau;
            set => SetProperty(ref _ImageCiseau, value);
        }

        public int ScoreUtilisateur
        {
            get => _ScoreUtilisateur;
            set => SetProperty(ref _ScoreUtilisateur, value);
        }
        private int _ScoreUtilisateur;

        public int ScoreMachine
        {
            get => _ScoreMachine;
            set => SetProperty(ref _ScoreMachine, value);
        }
        private int _ScoreMachine;

        #endregion

        #region Constructeur

        public MainWindowViewModel()
        {
            PierreCommand = new RelayCommand(PierreExecute, PierreCanExecute);
            FeuilleCommand = new RelayCommand(FeuilleExecute, FeuilleCanExecute);
            CiseauCommand = new RelayCommand(CiseauExecute, CiseauCanExecute);
            RejouerCommand = new RelayCommand(RejouerExecute);
            _Random = new Random();
            Init();

        }

        private void Init()
        {
            Reponse = string.Empty;
            ImageSource = string.Empty;

        }

        private void VerifierFindePartie()
        {
            if (ScoreUtilisateur == 3)
            {
                Reponse = "Félicitations, vous avez gagné la partie !";
                _FinPartie = true;
            }
            else if (ScoreMachine == 3)
            {
                Reponse = "La Machine a gagné la partie !";
                _FinPartie = true;
            }
        }

        #endregion 

    }
}
