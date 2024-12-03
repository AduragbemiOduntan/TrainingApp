using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingApp.Application.Services.Interface;
using TrainingApp.Infrastructure.DbContext;
using TrainingApp.Shared.DTOs.ResponseDTOs;

namespace TrainingApp.Application.Services.Implementation
{
    public class PersonService(AppDbContext dbContext) : IPersonService
    {
        public List<PersonResponseDTO> CreatePersons()
        {
            throw new NotImplementedException();
        }

        public PersonResponseDTO GetPersonById(string id)
        {
            throw new NotImplementedException();
        }

        public List<PersonResponseDTO> GetPersons()
        {
            throw new NotImplementedException();
        }
    }
}
