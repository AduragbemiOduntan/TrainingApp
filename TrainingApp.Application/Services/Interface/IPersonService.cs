using TrainingApp.Shared.DTOs.RequestDTOs;
using TrainingApp.Shared.DTOs.ResponseDTOs;

namespace TrainingApp.Application.Services.Interface
{
    public interface IPersonService
    {
        StandardResponse<List<PersonResponseDTO>> CreatePersons(List<PersonRequestDTO> persons);
        List<PersonResponseDTO> GetPersons();
        PersonResponseDTO GetPersonById(string id);
    }
}
