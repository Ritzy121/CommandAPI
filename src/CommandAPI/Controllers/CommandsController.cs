using System.Collections.Generic;
using AutoMapper;
using CommandAPI.Data;
using CommandAPI.Models;
using Microsoft.AspNetCore.Mvc;
using CommandAPI.Dtos;
using Microsoft.AspNetCore.JsonPatch;

namespace CommandAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandAPIRepo _repository;
        private readonly IMapper _mapper;

        //Random Change

        public CommandsController(ICommandAPIRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetAllCommands()
        {
            var commandItems = _repository.GetAllCommands();

            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandItems));
        }
        
        [HttpGet("{id}", Name="GetCommandById")]
        public ActionResult<CommandReadDto> GetCommandById(int id)
        {
            var commandItem = _repository.GetCommandById(id);
            if (commandItem == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<CommandReadDto>(commandItem));
        }
    
        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommand(CommandCreateDto command)
        {
            var model = _mapper.Map<Command>(command);

            _repository.CreateCommand(model);

            _repository.SaveChanges();

            var dto = _mapper.Map<CommandReadDto>(model);

            return CreatedAtRoute(nameof(GetCommandById), new {Id = dto.Id}, dto);
        }
    
        [HttpPut("{id}")]
        public ActionResult UpdateCommand([FromRoute]int id, [FromBody]CommandUpdateDto command)
        {
            return Update(id, command);
        }  

        [HttpPut]
        public ActionResult UpdateCommand2([FromQuery] int id, [FromBody]CommandUpdateDto command)
        {
            return Update(id, command);
        }

        [NonAction]
        private ActionResult Update(int id, CommandUpdateDto command)
        {
            var commandFromDb = _repository.GetCommandById(id);
            if(commandFromDb == null)
                return NotFound();

            _mapper.Map(command, commandFromDb);

            _repository.UpdateCommand(commandFromDb);

            _repository.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}")]
        public ActionResult PartialCommandUpdate(int id, JsonPatchDocument<CommandUpdateDto> patchDoc)
        {
            //get command from database
            var commandFromDb = _repository.GetCommandById(id);
            if(commandFromDb == null)
                return NotFound();

            //convert to DTO
            var commandToPatch = _mapper.Map<CommandUpdateDto>(commandFromDb);

            //apply path to dto object
            patchDoc.ApplyTo(commandToPatch, ModelState);

            //validate pathed dobject to ensure correctness
            if(!TryValidateModel(commandToPatch))
            {
                return ValidationProblem(ModelState);
            }

            //map the pathced object to the command object in database
            _mapper.Map(commandToPatch, commandFromDb);

            _repository.UpdateCommand(commandFromDb);

            _repository.SaveChanges();

            return NoContent();
        }
    
        [HttpDelete("{id}")]
        public ActionResult DeleteCommand(int id)
        {
            var commandModelFromRepo = _repository.GetCommandById(id);
            if(commandModelFromRepo == null)
            {
                return NotFound();
            }

             _repository.DeleteCommand(commandModelFromRepo);

            _repository.SaveChanges();
            
            return NoContent();
        }

    }
}
