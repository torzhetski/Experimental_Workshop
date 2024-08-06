using Experimental_Workshop.ApplicationResourses;
using Experimental_Workshop.MVVM.Model;
using Experimental_Workshop.Repositories;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Experimental_Workshop.MVVM.ViewModel.DataTableViewModel
{
    public class MaterialsDataTableVM : ObservableObject
    {
        private MainRepositoryService _mainRepositoryService;
        private ObservableCollection<Materials> _materials;
        private Materials _newMaterial;
        private Materials _currentMaterial;
        private bool _isVisible = false;

        public ObservableCollection<Materials> Materials
        {
            get => _materials;
            set => Set(ref _materials, value);
        }
        public Materials NewMaterial
        {
            get => _newMaterial;
            set => Set(ref _newMaterial, value);
        }
        public Materials CurrentMaterial
        {
            get => _currentMaterial;
            set => Set(ref _currentMaterial, value);
        }
        public bool IsVisible
        {
            get => _isVisible;
            set => Set(ref _isVisible, value);
        }
        public RelayCommand DeleteMaterialCommand { get; set; }
        public RelayCommand SaveMaterialsCommand { get; set; }
        public RelayCommand AddMaterialCommand { get; set; }
        public RelayCommand EditAccsessCommand { get; set; }
        public RelayCommand ReplaceTechnoCardfRestToCurrentCommand { get; set; }
        public RelayCommand ReplaceTechnoCardfCurrentToRestCommand { get; set; }
        public RelayCommand ReplaceTechnoCardfRestToCurrentNewCommand { get; set; }
        public RelayCommand ReplaceTechnoCardfCurrentToRestNewCommand { get; set; }

        public MaterialsDataTableVM(MainRepositoryService mainRepository) 
        {
            _mainRepositoryService = mainRepository;
            NewMaterial = new Materials();
            LoadData();
            SaveMaterialsCommand = new(SaveMaterials);
            DeleteMaterialCommand = new RelayCommand(RemoveMaterial);
            AddMaterialCommand = new RelayCommand(AddMaterial, CanAddMaterial);
            ReplaceTechnoCardfRestToCurrentCommand = new RelayCommand(ReplaceTechnoCardfRestToCurrent);
            ReplaceTechnoCardfCurrentToRestCommand = new RelayCommand(ReplaceTechnologyCardfFromCurrentToRest);
            ReplaceTechnoCardfRestToCurrentNewCommand = new RelayCommand(ReplaceTechnoCardfRestToCurrentNew);
            ReplaceTechnoCardfCurrentToRestNewCommand = new RelayCommand(ReplaceTechnologyCardfFromCurrentToRestNew);
            EditAccsessCommand = new RelayCommand(o => IsVisible = !IsVisible);

        }

        private void LoadData()
        {
            var materials = _mainRepositoryService.MaterialsRepository.GetAllMaterials();
            var techCards = _mainRepositoryService.TechnologyCardRepository.GetAllTechnologyCards();
            Materials = new ObservableCollection<Materials>(materials);
            foreach (var material in Materials)
            {
                material.RestTechnologyCards = new(techCards);
                foreach (var tech in material.TechnologyCards)
                    material.RestTechnologyCards.Remove(tech);

            }
            NewMaterial.RestTechnologyCards = new(techCards);
        }


        public void RemoveMaterial(object o)
        {
            Materials material = (Materials)o;
            Materials.Remove(material);
            _mainRepositoryService.MaterialsRepository.RemoveMaterial(material);
        }


        private bool CanAddMaterial(object arg)
        {
            bool flag = false;
            if (!NewMaterial.Name.IsNullOrEmpty() && NewMaterial.Amount != default(int) && NewMaterial.TechnologyCards.Count > 0)
            { flag = true; }
            return flag;
        }

        private void AddMaterial(object obj)
        {
            Materials.Add(NewMaterial);
            _mainRepositoryService.MaterialsRepository.AddMaterial(NewMaterial);
            NewMaterial = new();
            #region Костыль
            var techCards = _mainRepositoryService.TechnologyCardRepository.GetAllTechnologyCards();
            NewMaterial.RestTechnologyCards = new(techCards);
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
                Materials[Materials.IndexOf(CurrentMaterial)].RestTechnologyCards.Add((TechnologyCard)obj);
                Materials[Materials.IndexOf(CurrentMaterial)].TechnologyCards.Remove((TechnologyCard)obj);
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
                Materials[Materials.IndexOf(CurrentMaterial)].RestTechnologyCards.Remove((TechnologyCard)obj);
                Materials[Materials.IndexOf(CurrentMaterial)].TechnologyCards.Add((TechnologyCard)obj);
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
                NewMaterial.TechnologyCards.Remove(techCard);
                NewMaterial.RestTechnologyCards.Add(techCard);
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
                var techCard = obj as TechnologyCard;
                NewMaterial.TechnologyCards.Add(techCard);
                NewMaterial.RestTechnologyCards.Remove(techCard);
            }
        }


        public void SaveMaterials(object o)
        {
            _mainRepositoryService.MaterialsRepository.SaveAllMaterials();
        }

    }
}
