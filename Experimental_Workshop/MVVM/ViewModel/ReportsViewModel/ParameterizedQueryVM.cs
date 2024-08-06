using Experimental_Workshop.ApplicationResourses;
using Experimental_Workshop.MVVM.Model;
using Experimental_Workshop.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Experimental_Workshop.MVVM.ViewModel.ReportsViewModel
{
    public class ParameterizedQueryVM : ObservableObject
    {
        private MainRepositoryService _mainRepositoryService;
        private ObservableCollection<TechnologyCard> _technologyCards;
        private int power;
        private int frequency;

        public ObservableCollection<TechnologyCard> TechnologyCards
        {
            get => _technologyCards;
            set => Set(ref _technologyCards, value);
        }
        public int Power
        {
            get => power;
            set => Set(ref  power, value);
        }
        public int Frequency
        {
            get => frequency;
            set => Set(ref frequency, value);
        }

        public RelayCommand SearchCommand { get; set; }

        public ParameterizedQueryVM(MainRepositoryService mainRepositoryService)
        {
            _mainRepositoryService = mainRepositoryService;
            TechnologyCards = new ObservableCollection<TechnologyCard>();
            SearchCommand = new RelayCommand(Search, CanSearch);
        }

        private bool CanSearch(object arg)
        {
            return (Frequency != 0) || (Power != 0);
        }

        private void Search(object obj)
        {
            TechnologyCards = new ObservableCollection<TechnologyCard>(_mainRepositoryService.TechnologyCardRepository.GetTechnologyCardsWithGivenParametrs(Power, Frequency));
        }
    }
}
