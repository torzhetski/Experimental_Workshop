using Experimental_Workshop.MVVM.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ApplicationContext : DbContext
{
    public DbSet<Worker> Workers { get; set; } = null!;
    public DbSet<JobTitle> JobTitles { get; set; } = null!;
    public DbSet<Equipment> Equipment { get; set; } = null!;
    public DbSet<Materials> Materials { get; set; } = null!;
    public DbSet<MicroprocessorTypes> MicroprocessorTypes { get; set; } = null!;
    public DbSet<TechnologyCard> TechnologyCards { get; set; } = null!;
    public ApplicationContext() 
    {
        //Database.EnsureDeleted();
        Database.EnsureCreated(); 
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=ExperimentalWorkshop;Trusted_Connection=True;");
    }
        
}
