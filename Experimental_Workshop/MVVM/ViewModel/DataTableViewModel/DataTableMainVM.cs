using Experimental_Workshop.ApplicationResourses;
using Experimental_Workshop.MVVM.Model;
using Experimental_Workshop.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Experimental_Workshop.MVVM.ViewModel.DataTableViewModel
{
    public class DataTableMainVM : ObservableObject
    {

        private MainRepositoryService _mainRepositoryService;
        private TechnologyCard _lastTechnologyCard;
        private TechnologyCard _preLastTechnologyCard;
        private object _currentDataTable;
        private bool _isVisible = true;
        
        public object CurrentDataTable
        {
            get => _currentDataTable;
            set => Set(ref _currentDataTable, value);
        }
        public TechnologyCard LastTechnologyCard
        {
            get => _lastTechnologyCard;
            set => Set(ref _lastTechnologyCard, value);
        }
        public TechnologyCard PreLastTechnologyCard
        {
            get => _preLastTechnologyCard;
            set => Set (ref  _preLastTechnologyCard, value);
        }
        public bool IsVisible
        {
            get => _isVisible;
            set => Set(ref _isVisible, value);
        }
        public RelayCommand NextDataTableCommand { get; set; }

        public DataTableMainVM(MainRepositoryService mainRepositoryService)
        {
            _mainRepositoryService = mainRepositoryService;
            LoadCards();
            CurrentDataTable = new WorkersDataTableVM(_mainRepositoryService);
            NextDataTableCommand = new RelayCommand(ExecuteNextDataTableCommand);
        }

        private void LoadCards()
        {
            var allCards = _mainRepositoryService.TechnologyCardRepository.GetAllTechnologyCards();
            if (allCards.Count >= 2)
            {
                LastTechnologyCard = allCards[allCards.Count - 1];
                PreLastTechnologyCard = allCards[allCards.Count - 2];
            }
            else
            {
                IsVisible = false;
            }
        }

        private void ExecuteNextDataTableCommand(object commandParameter)
        {
            if (commandParameter is not ID pageId)
            {
                throw new ArgumentException($"Unexpected parameter type. ICommand.CommandParameter must be of type {typeof(ID)}", nameof(commandParameter));
            }

            LoadPage(pageId);
        }
        private void LoadPage(ID pageId)
        {
           
            if (this.CurrentDataTable is TechnologyCardDataTableVM)
                (this.CurrentDataTable as TechnologyCardDataTableVM).TechnologyCardsChanged -= LoadCards;

            switch (pageId)
            {
                case ID.First:
                    this.CurrentDataTable = new WorkersDataTableVM(_mainRepositoryService);
                    break;
                case ID.Second:
                    this.CurrentDataTable = new JobTitleDataTableVM( _mainRepositoryService);
                    break;
                case ID.Third:
                    this.CurrentDataTable = new EquipmentDataTableVM(_mainRepositoryService);
                    break;
                case ID.Fourth:
                    this.CurrentDataTable = new MaterialsDataTableVM(_mainRepositoryService);
                    break;
                case ID.Fifth:
                    this.CurrentDataTable = new MicroprocessorTypesDataTableVM(_mainRepositoryService);
                    break;
                case ID.Sixth:
                    {
                        this.CurrentDataTable = new TechnologyCardDataTableVM(_mainRepositoryService);
                        (this.CurrentDataTable as TechnologyCardDataTableVM).TechnologyCardsChanged += LoadCards;
                    }
                    break;
                default:
                    break;
            }

        }
    }
}
