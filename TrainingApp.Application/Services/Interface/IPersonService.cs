using TrainingApp.Shared.DTOs.RequestDTOs;
using TrainingApp.Shared.DTOs.ResponseDTOs;

namespace TrainingApp.Application.Services.Interface
{
    public interface IPersonService
    {
        StandardResponse<List<PersonResponseDTO>> CreatePersons(List<PersonRequestDTO> persons);
        StandardResponse<PersonProgressResponseDTO> GetPersonProgressById(string personId);
        StandardResponse<PersonResponseDTO> GetPersonById(string personId);
        StandardResponse<List<PersonResponseDTO>> GetPersons();
        
        
    }
}
