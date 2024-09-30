using Lab1.Database.Context;
using Lab1.Database.DTOs;
using Lab1.Database.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Database
{
    internal static class DIExtensions
    {
        public static void ConfigurePersistenceLayer()
        {
            Context.DIExtensions.ConfigureDbContext();
            Repository.DIExtensions.ConfigureRepository();
            Service.DIExtensions.ConfigureService();
        }
    }
}
