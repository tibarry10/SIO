using Chifoumi.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Chifoumi.MainWindowViewModel
{
    class MainWindowViewModel : ViewModelBase
    {
        #region champs privés
        private int _PropHasard;
        private Random _Random;
        private bool _FinPartie = false;
        #endregion

        #region Command
        public ICommand PierreCommand { get; set; }
        private void PierreExecute (object obj) 
        {
            _FinPartie = false;
            _PropHasard = _Random.Next(1, 4);



        }
        private bool PierreCanExecute(object obj)
        {
            return _FinPartie;
        }

        public ICommand FeuilleCommand { get; set; }
        private void FeuilleExecute(object obj)
        {
            _PropHasard = _Random.Next(1, 4);

        }
        private bool FeuilleCanExecute(object obj)
        {
            return _FinPartie;
        }

        public ICommand CiseauCommand { get; set; }
        private void CiseauExecute(object obj)
        {
            _PropHasard = _Random.Next(1, 4);

        }
        private bool CiseauCanExecute(object obj)
        {
            return _FinPartie;
        }

        public ICommand RejouerCommand { get; set; }
        private void RejouerExecute(object obj)
        {

        }
        private bool RejouerCanExecute(object obj)
        {
            return _FinPartie;
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

        public int Score
        {
            get => _Score;
            set => SetProperty(ref _Score, value);
        }
        private int _Score;

        #endregion

        #region Constructeur
        public MainWindowViewModel(MainWindowViewModel viewModel)
        {

            PierreCommand = new RelayCommand(PierreExecute, PierreCanExecute);
            FeuilleCommand = new RelayCommand(FeuilleExecute, FeuilleCanExecute);
            CiseauCommand = new RelayCommand(CiseauExecute, CiseauCanExecute);
            RejouerCommand = new RelayCommand(RejouerExecute, RejouerCanExecute);
            _Random = new Random();
            Init();
        }
        private void Init()
        {
            Reponse = string.Empty;
            ImageSource = string.Empty;
            Score = 0;
        }

        #endregion 

    }
}
