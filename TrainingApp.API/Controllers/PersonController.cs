using Microsoft.AspNetCore.Mvc;
using TrainingApp.Application.Services.Interface;
using TrainingApp.Shared.DTOs.RequestDTOs;

namespace TrainingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController(IPersonService personService) : ControllerBase
    {
        [HttpPost("create-persons")]
        public IActionResult CreatePersons(List<PersonRequestDTO> persons)
        {
                var result = personService.CreatePersons(persons);
                return Ok(result);
        }

        [HttpGet("get-progress")]
        public IActionResult GetPersonProgressById(string id)
        {
            var result = personService.GetPersonById(id);
            return Ok(result);
        }
    }
}
