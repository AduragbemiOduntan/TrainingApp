using TrainingApp.Shared.DTOs.ResponseDTOs;

namespace TrainingApp.Application.Services.Interface
{
    public interface IPersonService
    {
        List<PersonResponseDTO> CreatePersons();
        List<PersonResponseDTO> GetPersons();
        PersonResponseDTO GetPersonById(string id);
    }
}
