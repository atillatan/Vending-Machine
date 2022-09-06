using System.Net.Mime;
using Machine.Application.Languages;
using Machine.Application.Languages.Commands.LanguageCreate;
using Machine.Application.Languages.Commands.LanguageDelete;
using Machine.Application.Languages.Commands.LanguageUpdate;
using Machine.Application.Languages.Queries.LanguageFindByKey;
using Machine.Application.Languages.Queries.LanguageGetAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Machine.API.Controllers;

[Produces(MediaTypeNames.Application.Json)]
[Route("api/v1/[controller]/[action]")]
public class LanguageController : ControllerBase
{
    protected readonly IMediator _mediator;
    public LanguageController(IMediator mediator)
    {
        this._mediator = mediator;
    }

    /// <summary>        
    /// Add the given Language key and value
    /// </summary>
    /// <remarks>
    /// 
    ///     {
    ///       "LanguageId": ""
    ///       "MessageKey": "",
    ///       "Message":""
    ///     }
    /// 
    /// </remarks>
    /// <param name="request">Required LanguageCreateCommand parameter</param>        
    /// <returns>Returns the newly created Object</returns>
    /// <param name="cancellationToken" example="">CancellationToken provides to Cancel the current operation, all data will be rollback</param>
    /// <response code="200">Successful operation</response>
    /// <response code="400">Invalid parameters supplied</response>         
    /// <response code="404">Object not found</response>        
    [ProducesResponseType(typeof(LanguageDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost]
    public async Task<ActionResult<LanguageDto>> PostLanguage([FromBody] LanguageCreateCommand request, CancellationToken cancellationToken)
    {
        if (request == null) return BadRequest();
        var result = await _mediator.Send(request, cancellationToken);
        return result != null ? Created(Request.Path, result) : NotFound();
    }

    /// <summary>        
    /// Get the LanguageDto for the instance with the given key.       
    /// </summary>
    /// <param name="id" example="uuid">It must be unique with key parameter</param>        
    /// <param name="cancellationToken" example="">CancellationToken provides to Cancel the current operation, all data will be rollback</param>
    /// <returns>Returns LanguageDto</returns>
    /// <response code="200">Successful operation</response>
    /// <response code="400">Invalid parameters supplied</response>         
    /// <response code="404">Object not found</response>
    [ProducesResponseType(typeof(LanguageDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}")]
    public async Task<ActionResult<LanguageDto>> GetLanguage([FromRoute] string languageId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(languageId)) return BadRequest();
        var result = await _mediator.Send(new LanguageFindByKeyQuery { LanguageId = languageId }, cancellationToken);
        return result != null ? Ok(result) : NotFound();
    }

    /// <summary>
    /// update the given Language with the related key.    
    /// </summary>    
    /// <param name="request">Required LanguageUpdateCommand parameter</param>
    /// <param name="cancellationToken" example="">CancellationToken provides to Cancel the current operation, all data will be rollback</param>
    /// <returns>Returns the updated Language</returns>
    /// <response code="200">Successful operation</response>
    /// <response code="400">Invalid parameters supplied</response>         
    /// <response code="404">Object not found</response>        
    [ProducesResponseType(typeof(LanguageDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut]
    public async Task<ActionResult<LanguageDto>> PutLanguage([FromBody] LanguageUpdateCommand request, CancellationToken cancellationToken)
    {
        if (request == null) return BadRequest();
        var result = await _mediator.Send(request, cancellationToken);
        return result != null ? Ok(result) : NotFound();
    }

    /// <summary>
    /// Delete the identified Language from the Machine
    /// </summary>
    /// <param name="cancellationToken" example="">CancellationToken provides to Cancel the current operation, all data will be rollback</param>
    /// <param name="id" example="uuid">It must be unique with key parameter</param>            
    /// <returns>true if the language was found and deleted, false was not deleted.</returns>
    /// <response code="200">Successful operation</response>
    /// <response code="400">Invalid parameters supplied</response>         
    /// <response code="404">Object not found</response>
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteLanguage([FromRoute] string languageId, CancellationToken cancellationToken)
    {
         if (string.IsNullOrEmpty(languageId)) return BadRequest();
        var result = await _mediator.Send(new LanguageDeleteCommand { LanguageId = languageId }, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get all or  Language list.
    /// </summary>        
    /// <param name="cancellationToken" example="">CancellationToken provides to Cancel the current operation</param>
    /// <returns>List of Settins objects</returns>
    /// <response code="200">Successful operation</response>
    /// <response code="400">Invalid parameters supplied</response>         
    /// <response code="404">Object not found</response>
    [ProducesResponseType(typeof(IEnumerable<LanguageDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<LanguageDto>>> GetAllLanguage(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new LanguageGetAllQuery(), cancellationToken);
        return result != null ? Ok(result) : NotFound();
    }

    /// <summary>
    /// Get all or  Language list.
    /// </summary>   
    /// <param name="code" example="de-DE">language code parameter</param>                 
    /// <param name="cancellationToken" example="">CancellationToken provides to Cancel the current oparation</param>
    /// <returns>List of Settins objects</returns>
    /// <response code="200">Successful operation</response>
    /// <response code="400">Invalid parameters supplied</response>         
    /// <response code="404">Object not found</response>
    [ProducesResponseType(typeof(IEnumerable<LanguageDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{languageid}")]
    public async Task<ActionResult<IEnumerable<LanguageDto>>> GetLanguageByCode([FromRoute] string languageid, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new LanguageGetAllQuery(), cancellationToken);

        var newResult = result.Where(x => x.LanguageId.ToLower() == languageid.ToLower());
        Dictionary<string, string> kvpairs = new Dictionary<string, string>();

        foreach (var item in newResult)
            if (!kvpairs.ContainsKey(item.MessageKey)) kvpairs.Add(item.MessageKey, item.Message);

        return kvpairs != null ? Ok(kvpairs) : NotFound();
    }
}
