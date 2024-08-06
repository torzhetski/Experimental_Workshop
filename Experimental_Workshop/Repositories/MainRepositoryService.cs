using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Experimental_Workshop.MVVM.ViewModel.ReportsViewModel;
using System.Windows;

namespace Experimental_Workshop.Repositories
{
    public class MainRepositoryService
    {
        private ApplicationContext dbContext;
        public WorkerRepository WorkerRepository { get; set; }
        public EquipmentRepository EquipmentRepository { get; set; }
        public JobTitleRepository JobTitleRepository { get; set; }
        public MaterialsRepository MaterialsRepository { get; set; }
        public MicroprocessorTypesRepository MicroprocessorTypesRepository { get; set; }
        public TechnologyCardRepository TechnologyCardRepository { get; set; }

        public MainRepositoryService(ApplicationContext context) 
        {
            dbContext = context;
            WorkerRepository = new WorkerRepository(context);
            EquipmentRepository = new EquipmentRepository(context);
            JobTitleRepository = new JobTitleRepository(context);
            MaterialsRepository = new MaterialsRepository(context);
            MicroprocessorTypesRepository = new MicroprocessorTypesRepository(context);
            TechnologyCardRepository = new TechnologyCardRepository(context);
        }
        public List<TechCardEquipUnionVM> techCardEquipunion()
        {
            //to see sql query string
           string e = dbContext.TechnologyCards.Select(e => new TechCardEquipUnionVM
            {
                Name = e.Name,
                StartDate = DateOnly.FromDateTime(e.DateOfStart),
            }).Union(dbContext.Equipment.Select(e => new TechCardEquipUnionVM
            {
                Name = e.Name,
                StartDate = DateOnly.FromDateTime(e.ExpluatationStartDate),
            })).ToQueryString();
            var unionTable = dbContext.TechnologyCards.Select(e => new TechCardEquipUnionVM
            {
                Name = e.Name,
                StartDate = DateOnly.FromDateTime(e.DateOfStart),
            }).Union(dbContext.Equipment.Select(e => new TechCardEquipUnionVM
            {
                Name = e.Name,
                StartDate = DateOnly.FromDateTime(e.ExpluatationStartDate),
            })).ToList();

            return unionTable;
        }
    }
}
