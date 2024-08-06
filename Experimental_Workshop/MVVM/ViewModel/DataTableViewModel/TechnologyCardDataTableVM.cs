using Experimental_Workshop.ApplicationResourses;
using Experimental_Workshop.ApplicationResourses.ValidationRules;
using Experimental_Workshop.MVVM.Model;
using Experimental_Workshop.Repositories;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.Media3D;

namespace Experimental_Workshop.MVVM.ViewModel.DataTableViewModel
{
    public class TechnologyCardDataTableVM : ObservableObject
    {
        private MainRepositoryService _mainRepositoryService;
        private ObservableCollection<TechnologyCard> _technologyCards;
        private TechnologyCard _newTechnologyCard;
        private TechnologyCard _currentTechnologyCard;
        private bool _isVisible = false;

        public delegate void TechnologyCardPropertyChangedEventHandler();
        public event TechnologyCardPropertyChangedEventHandler? TechnologyCardsChanged;

        public ObservableCollection<TechnologyCard> TechnologyCards
        {
            get => _technologyCards;
            set => Set(ref _technologyCards, value);
        }
        public TechnologyCard NewTechnologyCard
        {
            get => _newTechnologyCard;
            set => Set(ref _newTechnologyCard, value);
        }
        public TechnologyCard CurrentTechnologyCard
        {
            get => _currentTechnologyCard;
            set 
            {
                if (!ModelsValidation.ValidateEquipment(CurrentTechnologyCard,_mainRepositoryService))
                    Set(ref _currentTechnologyCard, value); 
                else
                    Set(ref _currentTechnologyCard,_currentTechnologyCard);
            }
        }
        public bool IsVisible
        {
            get => _isVisible;
            set => Set(ref _isVisible, value);
        }


        public RelayCommand DeleteTechnologyCardCommand { get; set; }
        public RelayCommand SaveTechnologyCardsCommand { get; set; }
        public RelayCommand AddTechnologyCardCommand { get; set; }
        public RelayCommand EditAccsessCommand { get; set; }
        public RelayCommand ReplaceTechnoCardfRestToCurrentCommand { get; set; }
        public RelayCommand ReplaceTechnoCardfCurrentToRestCommand { get; set; }
        public RelayCommand ReplaceTechnoCardfRestToCurrentNewCommand { get; set; }
        public RelayCommand ReplaceTechnoCardfCurrentToRestNewCommand { get; set; }

        

        public TechnologyCardDataTableVM(MainRepositoryService mainRepositoryService) 
        {
            _mainRepositoryService = mainRepositoryService;
            NewTechnologyCard = new TechnologyCard();
            TechnologyCards = new ObservableCollection<TechnologyCard>(_mainRepositoryService.TechnologyCardRepository.GetAllTechnologyCards());

            LoadData();
            SaveTechnologyCardsCommand = new(SaveTechnologyCards);
            DeleteTechnologyCardCommand = new RelayCommand(RemoveTechnologyCard);
            AddTechnologyCardCommand = new RelayCommand(AddTechnologyCard, CanAddTechnologyCard);
            ReplaceTechnoCardfRestToCurrentCommand = new RelayCommand(ReplaceTechnoCardfRestToCurrent);
            ReplaceTechnoCardfCurrentToRestCommand = new RelayCommand(ReplaceTechnologyCardfFromCurrentToRest);
            ReplaceTechnoCardfRestToCurrentNewCommand = new RelayCommand(ReplaceTechnoCardfRestToCurrentNew);
            ReplaceTechnoCardfCurrentToRestNewCommand = new RelayCommand(ReplaceTechnologyCardfFromCurrentToRestNew);
            EditAccsessCommand = new RelayCommand(o => IsVisible = !IsVisible);

        }

        private void LoadData()
        {
            var workers = _mainRepositoryService.WorkerRepository.GetAllWorkers();      
            var materials = _mainRepositoryService.MaterialsRepository.GetAllMaterials();
            var equip = _mainRepositoryService.EquipmentRepository.GetAllEquipment();
            var microProc = _mainRepositoryService.MicroprocessorTypesRepository.GetAllMicroprocessorTypes();

            foreach (var card in TechnologyCards)
            {
                card.RestMaterials = new ObservableCollection<Materials>(materials);
                foreach(var e in card.Materials) card.RestMaterials.Remove(e);
                card.RestWorkers = new ObservableCollection<Worker>(workers);
                foreach (var e in card.Workers) card.RestWorkers.Remove(e);
                card.RestEquipment = new ObservableCollection<Equipment>(equip);
                foreach( var e in card.Equipment) card.RestEquipment.Remove(e);
                card.AllMicroprocessorTypes = new ObservableCollection<MicroprocessorTypes>(microProc);

            }
            NewTechnologyCard.RestEquipment = new ObservableCollection<Equipment>(equip);
            NewTechnologyCard.RestMaterials = new ObservableCollection<Materials> (materials);
            NewTechnologyCard.RestWorkers = new ObservableCollection<Worker> (workers);
            NewTechnologyCard.AllMicroprocessorTypes = new ObservableCollection<MicroprocessorTypes> (microProc);
        }
        public void RemoveTechnologyCard(object obj)
        {
            TechnologyCard card = (TechnologyCard)obj;
            TechnologyCards.Remove(card);
            _mainRepositoryService.TechnologyCardRepository.RemoveTechnologyCard(card);
            TechnologyCardsChanged?.Invoke();
        }


        private bool CanAddTechnologyCard(object arg)
        {
            bool flag = false;
            if (!NewTechnologyCard.Name.IsNullOrEmpty() 
                && NewTechnologyCard.MicroprocessorType != null 
                && NewTechnologyCard.Materials.Count>0 
                && NewTechnologyCard.Workers.Count > 0 
                && NewTechnologyCard.Equipment.Count > 0)


            { flag = true; }
            return flag;
        }

        private void AddTechnologyCard(object obj)
        {
            if (!ModelsValidation.ValidateEquipment(NewTechnologyCard, _mainRepositoryService))
            {
                TechnologyCards.Add(NewTechnologyCard);
                _mainRepositoryService.TechnologyCardRepository.AddTechnologyCard(NewTechnologyCard);
                NewTechnologyCard = new();
                #region костыль
                var workers = _mainRepositoryService.WorkerRepository.GetAllWorkers();
                var materials = _mainRepositoryService.MaterialsRepository.GetAllMaterials();
                var equip = _mainRepositoryService.EquipmentRepository.GetAllEquipment();
                var microProc = _mainRepositoryService.MicroprocessorTypesRepository.GetAllMicroprocessorTypes();
                NewTechnologyCard.RestEquipment = new ObservableCollection<Equipment>(equip);
                NewTechnologyCard.RestMaterials = new ObservableCollection<Materials>(materials);
                NewTechnologyCard.RestWorkers = new ObservableCollection<Worker>(workers);
                #endregion
                NewTechnologyCard.AllMicroprocessorTypes = new ObservableCollection<MicroprocessorTypes>(microProc);
                TechnologyCardsChanged?.Invoke();
            }
        }
        /// <summary>
        /// ReplacesTechCard from Current to Rest for a new item
        /// </summary>
        /// <param name="obj"></param>
        private void ReplaceTechnologyCardfFromCurrentToRest(object obj)
        {
            if (obj is Materials)
            {
                TechnologyCards[TechnologyCards.IndexOf(CurrentTechnologyCard)].Materials.Remove((Materials)obj);
                TechnologyCards[TechnologyCards.IndexOf(CurrentTechnologyCard)].RestMaterials.Add((Materials)obj);
            }
            else if (obj is Worker)
            {
                TechnologyCards[TechnologyCards.IndexOf(CurrentTechnologyCard)].Workers.Remove((Worker)obj);
                TechnologyCards[TechnologyCards.IndexOf(CurrentTechnologyCard)].RestWorkers.Add((Worker)obj);
            }
            else if ( obj is Equipment)
            {
                TechnologyCards[TechnologyCards.IndexOf(CurrentTechnologyCard)].Equipment.Remove((Equipment)obj);
                TechnologyCards[TechnologyCards.IndexOf(CurrentTechnologyCard)].RestEquipment.Add((Equipment)obj);
            }
            else
            {
                throw new Exception("InavalidType");
            }

        }
        /// <summary>
        /// replacesTechCard from Rest to Current
        /// </summary>
        /// <param name="obj"></param>
        private void ReplaceTechnoCardfRestToCurrent(object obj)
        {
            if (obj is Materials)
            {
                TechnologyCards[TechnologyCards.IndexOf(CurrentTechnologyCard)].Materials.Add((Materials)obj);
                TechnologyCards[TechnologyCards.IndexOf(CurrentTechnologyCard)].RestMaterials.Remove((Materials)obj);
            }
            else if (obj is Worker)
            {
                TechnologyCards[TechnologyCards.IndexOf(CurrentTechnologyCard)].Workers.Add((Worker)obj);
                TechnologyCards[TechnologyCards.IndexOf(CurrentTechnologyCard)].RestWorkers.Remove((Worker)obj);
            }
            else if (obj is Equipment)
            {

                
                TechnologyCards[TechnologyCards.IndexOf(CurrentTechnologyCard)].Equipment.Add((Equipment)obj);
                TechnologyCards[TechnologyCards.IndexOf(CurrentTechnologyCard)].RestEquipment.Remove((Equipment)obj);
                
            }
            else
            {
                throw new Exception("InavalidType");
            }
        }
        /// <summary>
        /// ReplacesTechCard from Current to Rest for a new item
        /// </summary>
        /// <param name="obj"></param>
        private void ReplaceTechnologyCardfFromCurrentToRestNew(object obj)
        {
            if (obj is Materials)
            {
                NewTechnologyCard.Materials.Remove((Materials)obj);
                NewTechnologyCard.RestMaterials.Add((Materials)obj);
            }
            else if (obj is Worker)
            {
                NewTechnologyCard.Workers.Remove((Worker)obj);
                NewTechnologyCard.RestWorkers.Add((Worker)obj);
            }
            else if (obj is Equipment)
            {
                NewTechnologyCard.Equipment.Remove((Equipment)obj);
                NewTechnologyCard.RestEquipment.Add((Equipment)obj);
            }
            else
            {
                throw new Exception("InavalidType");
            }
        }
        /// <summary>
        /// eplacesTechCard from Rest to Current for a new item
        /// </summary>
        /// <param name="obj"></param>
        private void ReplaceTechnoCardfRestToCurrentNew(object obj)
        {
            if (obj is Materials)
            {
                NewTechnologyCard.Materials.Add((Materials)obj);
                NewTechnologyCard.RestMaterials.Remove((Materials)obj);
            }
            else if (obj is Worker)
            {
                NewTechnologyCard.Workers.Add((Worker)obj);
                NewTechnologyCard.RestWorkers.Remove((Worker)obj);
            }
            else if (obj is Equipment)
            {
                    
                NewTechnologyCard.Equipment.Add((Equipment)obj);
                NewTechnologyCard.RestEquipment.Remove((Equipment)obj);
                    
            }
            else
            {
                throw new Exception("InavalidType");
            }
        }

        public void SaveTechnologyCards(object o)
        {
            if (!ModelsValidation.ValidateEquipment(CurrentTechnologyCard, _mainRepositoryService))
                _mainRepositoryService.TechnologyCardRepository.SaveAllTechnologyCards();
        }

        
    }
}
