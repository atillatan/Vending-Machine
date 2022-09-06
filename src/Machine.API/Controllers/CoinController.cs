using System.Net.Mime;
using Machine.Application.Coins;
using Machine.Application.Coins.Commands.CoinCreate;
using Machine.Application.Coins.Commands.CoinDelete;
using Machine.Application.Coins.Commands.CoinUpdate;
using Machine.Application.Coins.Queries.CoinFindByKey;
using Machine.Application.Coins.Queries.CoinGetAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Machine.API.Controllers;

[Produces(MediaTypeNames.Application.Json)]
[Route("api/v1/[controller]/[action]")]
public class CoinController : ControllerBase
{
    protected readonly IMediator _mediator;
    public CoinController(IMediator mediator)
    {
        this._mediator = mediator;
    }

    /// <summary>        
    /// Add the given Coin key and value
    /// </summary>
    /// <remarks>
    /// 
    /// 
    /// </remarks>
    /// <param name="request">Required CoinCreateCommand parameter</param>        
    /// <returns>Returns the newly created Object</returns>
    /// <param name="cancellationToken" example="">CancellationToken provides to Cancel the current operation, all data will be rollback</param>
    /// <response code="200">Successful operation</response>
    /// <response code="400">Invalid parameters supplied</response>         
    /// <response code="404">Object not found</response>        
    [ProducesResponseType(typeof(CoinDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost]
    public async Task<ActionResult<CoinDto>> PostCoin([FromBody] CoinCreateCommand request, CancellationToken cancellationToken)
    {
        if (request == null) return BadRequest();
        var result = await _mediator.Send(request, cancellationToken);
        return result != null ? Created(Request.Path, result) : NotFound();
    }

    /// <summary>        
    /// Get the CoinDto for the instance with the given key.       
    /// </summary>
    /// <param name="coinId" example="string">It must be unique with key parameter</param>       
    
    /// <param name="cancellationToken" example="">CancellationToken provides to Cancel the current operation, all data will be rollback</param>
    /// <returns>Returns CoinDto</returns>
    /// <response code="200">Successful operation</response>
    /// <response code="400">Invalid parameters supplied</response>         
    /// <response code="404">Object not found</response>
    [ProducesResponseType(typeof(CoinDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{coinId}")]
    public async Task<ActionResult<CoinDto>> GetCoin([FromRoute] decimal coinId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CoinFindByKeyQuery { CoinId = coinId }, cancellationToken);
        return result != null ? Ok(result) : NotFound();
    }

    /// <summary>
    /// update the given Coin with the related key.    
    /// </summary>    
    /// <param name="request">Required CoinUpdateCommand parameter</param>
    /// <param name="cancellationToken" example="">CancellationToken provides to Cancel the current operation, all data will be rollback</param>
    /// <returns>Returns the updated Coin</returns>
    /// <response code="200">Successful operation</response>
    /// <response code="400">Invalid parameters supplied</response>         
    /// <response code="404">Object not found</response>        
    [ProducesResponseType(typeof(CoinDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut]
    public async Task<ActionResult<CoinDto>> PutCoin([FromBody] CoinUpdateCommand request, CancellationToken cancellationToken)
    {
        if (request == null) return BadRequest();
        var result = await _mediator.Send(request, cancellationToken);
        return result != null ? Ok(result) : NotFound();
    }

    /// <summary>
    /// Delete the identified Coin from the Machine
    /// </summary>
    /// <param name="cancellationToken" example="">CancellationToken provides to Cancel the current operation, all data will be rollback</param>
    /// <param name="coinId" example="uuid">It must be unique with key parameter</param>                     
    /// <returns>true if the language was found and deleted, false was not deleted.</returns>
    /// <response code="200">Successful operation</response>
    /// <response code="400">Invalid parameters supplied</response>         
    /// <response code="404">Object not found</response>
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{coinId}")]
    public async Task<ActionResult<bool>> DeleteCoin([FromRoute] decimal coinId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CoinDeleteCommand { CoinId = coinId }, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get all or  Coin list.
    /// </summary>        
    /// <param name="cancellationToken" example="">CancellationToken provides to Cancel the current operation</param>
    /// <returns>List of Settins objects</returns>
    /// <response code="200">Successful operation</response>
    /// <response code="400">Invalid parameters supplied</response>         
    /// <response code="404">Object not found</response>
    [ProducesResponseType(typeof(IEnumerable<CoinDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CoinDto>>> GetAllCoin(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CoinGetAllQuery(), cancellationToken);
        return result != null ? Ok(result) : NotFound();
    }

}
