using Experimental_Workshop.MVVM.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Experimental_Workshop.Repositories
{
    public class MaterialsRepository
    {

        private readonly ApplicationContext _dbContext;

        public MaterialsRepository(ApplicationContext context)
        {
            _dbContext = context;
        }
        public List<Materials> GetAllMaterials()
        {
            return _dbContext.Materials.Include(w => w.TechnologyCards).ToList();
        }

        public void SaveAllMaterials()
        {
            _dbContext.SaveChanges();
        }
        public void AddMaterial(Materials material)
        {
            _dbContext.Materials.Add(material);
            _dbContext.SaveChanges();
        }

        public void RemoveMaterial(Materials material)
        {
            _dbContext.Materials.Remove(material);
            _dbContext.SaveChanges();
        }
    }
}

