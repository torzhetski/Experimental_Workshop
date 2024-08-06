using Experimental_Workshop.ApplicationResourses;
using Experimental_Workshop.ApplicationResourses.ValidationRules;
using Experimental_Workshop.MVVM.Model;
using Experimental_Workshop.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Experimental_Workshop.MVVM.ViewModel.DataTableViewModel
{
    public class WorkersDataTableVM : ObservableObject
    {
        private MainRepositoryService _mainRepositoryService;
        private bool _isVisible = false;
        private Worker _currentWorker;

        private ObservableCollection<Worker> _workers;
        private Worker _newWorker;

        public ObservableCollection<Worker> Workers
        {
            get => _workers;
            set => Set(ref _workers, value);
        }
        public Worker NewWorker
        {
            get => _newWorker;
            set 
            { 
                Set(ref _newWorker, value);
                _newWorker.onJobTitleIdChanged += JobTitleChanhed;
            }
        }
        public bool IsVisible
        {
            get => _isVisible;
            set => Set(ref _isVisible, value);
        }
        public Worker CurrentWorker
        {
            get => _currentWorker;
            set 
            {
                Set(ref _currentWorker, value);
                _currentWorker.onJobTitleIdChanged += JobTitleChanhed;
            }
            //{
            //    if (!ModelsValidation.ValidateWorkerTitleAmount(CurrentWorker, _mainRepositoryService))
            //        Set(ref _currentWorker, value);
            //    else
            //        Set(ref _currentWorker, _currentWorker);
            //}
        }
        public RelayCommand DeleteWorkerCommand { get; set; }
        public RelayCommand SaveWorkerCommand { get; set; }
        public RelayCommand AddWorkerCommand { get; set; }
        public RelayCommand EditAccsessCommand { get; set; }
        public RelayCommand ReplaceTechnoCardfRestToCurrentCommand { get; set; }
        public RelayCommand ReplaceTechnoCardfCurrentToRestCommand { get; set; }
        public RelayCommand ReplaceTechnoCardfRestToCurrentNewCommand { get; set; }
        public RelayCommand ReplaceTechnoCardfCurrentToRestNewCommand { get; set; }


        public WorkersDataTableVM(MainRepositoryService mainRepositoryService) 
        {
            _mainRepositoryService = mainRepositoryService;
            Workers = new ObservableCollection<Worker>(_mainRepositoryService.WorkerRepository.GetAllWorkers());
            NewWorker = new Worker();
            LoadData();
            SaveWorkerCommand = new(SaveWorker);
            DeleteWorkerCommand = new RelayCommand(RemoveWorker);
            AddWorkerCommand = new RelayCommand(AddWorker, CanAddWorker);
            ReplaceTechnoCardfRestToCurrentCommand = new RelayCommand(ReplaceTechnoCardfRestToCurrent);
            ReplaceTechnoCardfCurrentToRestCommand = new RelayCommand(ReplaceTechnologyCardfFromCurrentToRest);
            ReplaceTechnoCardfRestToCurrentNewCommand = new RelayCommand(ReplaceTechnoCardfRestToCurrentNew);
            ReplaceTechnoCardfCurrentToRestNewCommand = new RelayCommand(ReplaceTechnologyCardfFromCurrentToRestNew);
            EditAccsessCommand = new RelayCommand(o => IsVisible = !IsVisible);
        }

        private void LoadData()
        {
            var workers = _mainRepositoryService.WorkerRepository.GetAllWorkers();
            var jobTitles = _mainRepositoryService.JobTitleRepository.GetAllJopTitles();
            var techCards = _mainRepositoryService.TechnologyCardRepository.GetAllTechnologyCards();
            Workers = new ObservableCollection<Worker>(workers);
            foreach (Worker worker in Workers)
            {
                worker.AllJobTitles = new(jobTitles);
                worker.RestTechnologyCards = new(techCards);
                foreach (var tech in worker.TechnologyCards)
                    worker.RestTechnologyCards.Remove(tech);
                
            }
            NewWorker.AllJobTitles = new(jobTitles);
            NewWorker.RestTechnologyCards = new(techCards);
        }



        public void RemoveWorker(object o)
        {
            Worker worker = (Worker)o;
            Workers.Remove(worker); 
            _mainRepositoryService.WorkerRepository.RemoveWorker(worker); 
        }
        

        private bool CanAddWorker(object arg)
        {
            bool flag = false;
            if (NewWorker.Name != default(string) && NewWorker.SecondName != default(string) && NewWorker.DateOfbirth != default(DateTime)&& NewWorker.Email !=default(string) && NewWorker.AllJobTitles.Contains(NewWorker.JobTitle) && NewWorker.TechnologyCards.Count>0)
            {
                flag = true;
            }
            return flag;
        }

        private void AddWorker(object obj)
        {
            //if (!ModelsValidation.ValidateWorkerTitleAmount(NewWorker, _mainRepositoryService, 1)) { 
            Workers.Add(NewWorker);
            NewWorker.JobTitle.AmountOfWorkers++;
            _mainRepositoryService.WorkerRepository.AddWorker(NewWorker);
            NewWorker = new();
            #region Костыль
            var jobTitles = _mainRepositoryService.JobTitleRepository.GetAllJopTitles();
            var techCards = _mainRepositoryService.TechnologyCardRepository.GetAllTechnologyCards();
            NewWorker.AllJobTitles = new(jobTitles);
            NewWorker.RestTechnologyCards = new(techCards);
            #endregion
            
        }

        /// <summary>
        /// ReplacesTechCard from Current to Rest 
        /// </summary>
        /// <param name="obj"></param>
        private void ReplaceTechnologyCardfFromCurrentToRest(object obj)
        {
            if (obj != null)
            {
                Workers[Workers.IndexOf(CurrentWorker)].RestTechnologyCards.Add((TechnologyCard)obj);
                Workers[Workers.IndexOf(CurrentWorker)].TechnologyCards.Remove((TechnologyCard)obj);
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
                    Workers[Workers.IndexOf(CurrentWorker)].RestTechnologyCards.Remove((TechnologyCard)obj);
                    Workers[Workers.IndexOf(CurrentWorker)].TechnologyCards.Add((TechnologyCard)obj);
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
                NewWorker.TechnologyCards.Remove(techCard);
                NewWorker.RestTechnologyCards.Add(techCard);
            }
        }/// <summary>
         /// eplacesTechCard from Rest to Current for a new item
         /// </summary>
         /// <param name="obj"></param>
        private void ReplaceTechnoCardfRestToCurrentNew(object obj)
        {
            if (obj != null)
            {
                var techCard = obj as TechnologyCard;
                NewWorker.TechnologyCards.Add(techCard);
                NewWorker.RestTechnologyCards.Remove(techCard);
            }
        }


        public void SaveWorker(object o)
        {
            //if (!ModelsValidation.ValidateWorkerTitleAmount(CurrentWorker, _mainRepositoryService))
                _mainRepositoryService.WorkerRepository.SaveAllWorkers();
        }
        private void JobTitleChanhed(int previousJobTitleId, int currentJobTitleId)
        {
            var titles =_mainRepositoryService.JobTitleRepository.GetAllJopTitles();
            foreach (var title in titles)
            {
                if (title.Id == previousJobTitleId) title.AmountOfWorkers--;
                if (title.Id == currentJobTitleId) title.AmountOfWorkers++;
            }
        }
    }
}
