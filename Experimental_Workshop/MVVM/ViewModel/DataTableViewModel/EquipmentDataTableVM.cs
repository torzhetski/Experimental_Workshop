using Experimental_Workshop.ApplicationResourses;
using Experimental_Workshop.ApplicationResourses.ValidationRules;
using Experimental_Workshop.MVVM.Model;
using Experimental_Workshop.Repositories;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Experimental_Workshop.MVVM.ViewModel.DataTableViewModel
{
    public class EquipmentDataTableVM : ObservableObject
    {
        private MainRepositoryService _mainRepositoryService;
        private ObservableCollection<Equipment> _equipment;
        private Equipment _newEquipment;
        private Equipment _currentEquipment;
        private bool _isVisible = false;

        public ObservableCollection<Equipment> Equipment 
        {
            get => _equipment;
            set => Set(ref _equipment, value);
        }
        public Equipment NewEquipment
        {
            get => _newEquipment; 
            set => Set(ref _newEquipment, value);
        }
        public Equipment CurrentEquipment
        {
            get => _currentEquipment;
            set => Set(ref _currentEquipment, value);
        }
        public bool IsVisible
        {
            get => _isVisible;
            set => Set(ref _isVisible, value);
        }
        public RelayCommand DeleteEquipmentCommand { get; set; }
        public RelayCommand SaveEquipmentCommand { get; set; }
        public RelayCommand AddEquipmentCommand { get; set; }
        public RelayCommand EditAccsessCommand { get; set; }
        public RelayCommand ReplaceTechnoCardfRestToCurrentCommand { get; set; }
        public RelayCommand ReplaceTechnoCardfCurrentToRestCommand { get; set; }
        public RelayCommand ReplaceTechnoCardfRestToCurrentNewCommand { get; set; }
        public RelayCommand ReplaceTechnoCardfCurrentToRestNewCommand { get; set; }

        public EquipmentDataTableVM(MainRepositoryService mainRepository) 
        {
            _mainRepositoryService = mainRepository;
            NewEquipment = new Equipment();    
            LoadData();
            SaveEquipmentCommand = new(SaveEquipment);
            DeleteEquipmentCommand = new RelayCommand(RemoveEquipment);
            AddEquipmentCommand = new RelayCommand(AddEquipment, CanAddEquipment);
            
            ReplaceTechnoCardfRestToCurrentCommand = new RelayCommand(ReplaceTechnoCardfRestToCurrent);
            ReplaceTechnoCardfCurrentToRestCommand = new RelayCommand(ReplaceTechnologyCardfFromCurrentToRest);
            ReplaceTechnoCardfRestToCurrentNewCommand = new RelayCommand(ReplaceTechnoCardfRestToCurrentNew);
            ReplaceTechnoCardfCurrentToRestNewCommand = new RelayCommand(ReplaceTechnologyCardfFromCurrentToRestNew);
            EditAccsessCommand = new RelayCommand(o => IsVisible = !IsVisible);
            
        }
        private void LoadData()
        {
            var equipment = _mainRepositoryService.EquipmentRepository.GetAllEquipment();
            var techCards = _mainRepositoryService.TechnologyCardRepository.GetAllTechnologyCards();
            Equipment = new ObservableCollection<Equipment>(equipment);
            foreach (var equip in Equipment)
            {
                equip.RestTechnologyCards = new(techCards);
                foreach (var tech in equip.TechnologyCards)
                    equip.RestTechnologyCards.Remove(tech);

            }
            NewEquipment.RestTechnologyCards = new(techCards);
        }


        public void RemoveEquipment(object o)
        {
            Equipment equipment = (Equipment)o;
            Equipment.Remove(equipment);
            _mainRepositoryService.EquipmentRepository.RemoveEquipment(equipment);
        }


        private bool CanAddEquipment(object arg)
        {
            bool flag = false;
            if(!NewEquipment.Name.IsNullOrEmpty() && NewEquipment.ExpluatationStartDate != null)
            { flag = true; }
            return flag;
        }

        private void AddEquipment(object obj)
        {
            Equipment.Add(NewEquipment);
            _mainRepositoryService.EquipmentRepository.AddEquipment(NewEquipment);
            NewEquipment = new();
            #region Костыль
            var techCards = _mainRepositoryService.TechnologyCardRepository.GetAllTechnologyCards();         
            NewEquipment.RestTechnologyCards = new(techCards);
            #endregion
        }
        /// <summary>
        /// ReplacesTechCard from Current to Rest for a new item
        /// </summary>
        /// <param name="obj"></param>
        private void ReplaceTechnologyCardfFromCurrentToRest(object obj)
        {
            if (obj != null)
            {
                Equipment[Equipment.IndexOf(CurrentEquipment)].RestTechnologyCards.Add((TechnologyCard)obj);
                Equipment[Equipment.IndexOf(CurrentEquipment)].TechnologyCards.Remove((TechnologyCard)obj);
            }

        }
        /// <summary>
        /// eplacesTechCard from Rest to Current
        /// </summary>
        /// <param name="obj"></param>
        private void ReplaceTechnoCardfRestToCurrent(object obj)
        {
            if (obj != null)
            {
                if (!ModelsValidation.ValidateEquipment(CurrentEquipment, (TechnologyCard)obj))
                {
                    Equipment[Equipment.IndexOf(CurrentEquipment)].RestTechnologyCards.Remove((TechnologyCard)obj);
                    Equipment[Equipment.IndexOf(CurrentEquipment)].TechnologyCards.Add((TechnologyCard)obj);
                }
            }
        }
        /// <summary>
        /// ReplacesTechCard from Current to Rest for a new item
        /// </summary>
        /// <param name="obj"></param>
        private void ReplaceTechnologyCardfFromCurrentToRestNew(object obj)
        {
            if (obj != null)
            {
                var techCard = obj as TechnologyCard;
                NewEquipment.TechnologyCards.Remove(techCard);
                NewEquipment.RestTechnologyCards.Add(techCard);
            }
        }
        /// <summary>
        /// eplacesTechCard from Rest to Current for a new item
        /// </summary>
        /// <param name="obj"></param>
        private void ReplaceTechnoCardfRestToCurrentNew(object obj)
        {
            if (obj != null)
            {
                if (!ModelsValidation.ValidateEquipment(NewEquipment,(TechnologyCard)obj))
                {
                    var techCard = obj as TechnologyCard;
                    NewEquipment.TechnologyCards.Add(techCard);
                    NewEquipment.RestTechnologyCards.Remove(techCard);
                }
            }
        }

        public void SaveEquipment(object o)
        {
            _mainRepositoryService.EquipmentRepository.SaveAllEquipment();
        }
    }
}
