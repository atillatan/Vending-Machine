using System.Net.Mime;
using Machine.Application.Products;
using Machine.Application.Products.Commands.ProductCreate;
using Machine.Application.Products.Commands.ProductDelete;
using Machine.Application.Products.Commands.ProductUpdate;
using Machine.Application.Products.Queries.ProductFindByKey;
using Machine.Application.Products.Queries.ProductGetAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Machine.API.Controllers;

[Produces(MediaTypeNames.Application.Json)]
[Route("api/v1/[controller]/[action]")]
public class ProductController : ControllerBase
{
    protected readonly IMediator _mediator;
    public ProductController(IMediator mediator)
    {
        this._mediator = mediator;
    }

    /// <summary>        
    /// Add the given Product key and value
    /// </summary>
    /// <remarks>
    /// 
    ///     {
    ///       "ProductId": ""
    ///       "MessageKey": "",
    ///       "Message":""
    ///     }
    /// 
    /// </remarks>
    /// <param name="request">Required ProductCreateCommand parameter</param>        
    /// <returns>Returns the newly created Object</returns>
    /// <param name="cancellationToken" example="">CancellationToken provides to Cancel the current operation, all data will be rollback</param>
    /// <response code="200">Successful operation</response>
    /// <response code="400">Invalid parameters supplied</response>         
    /// <response code="404">Object not found</response>        
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost]
    public async Task<ActionResult<ProductDto>> PostProduct([FromBody] ProductCreateCommand request, CancellationToken cancellationToken)
    {
        if (request == null) return BadRequest();
        var result = await _mediator.Send(request, cancellationToken);
        return result != null ? Created(Request.Path, result) : NotFound();
    }

    /// <summary>        
    /// Get the ProductDto for the instance with the given key.       
    /// </summary>
    /// <param name="slotId" example="string">It must be unique with key parameter</param>        
    /// <param name="productId" example="uuid">It must be unique with key parameter</param>        
    /// <param name="cancellationToken" example="">CancellationToken provides to Cancel the current operation, all data will be rollback</param>
    /// <returns>Returns ProductDto</returns>
    /// <response code="200">Successful operation</response>
    /// <response code="400">Invalid parameters supplied</response>         
    /// <response code="404">Object not found</response>
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
   [HttpDelete("{slotId}/{productId}")]
    public async Task<ActionResult<ProductDto>> GetProduct([FromRoute] string slotId, [FromRoute] int productId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(slotId) && productId == 0) return BadRequest();
        var result = await _mediator.Send(new ProductFindByKeyQuery { ProductId = productId, SlotId  = slotId }, cancellationToken);
        return result != null ? Ok(result) : NotFound();
    }

    /// <summary>
    /// update the given Product with the related key.    
    /// </summary>    
    /// <param name="request">Required ProductUpdateCommand parameter</param>
    /// <param name="cancellationToken" example="">CancellationToken provides to Cancel the current operation, all data will be rollback</param>
    /// <returns>Returns the updated Product</returns>
    /// <response code="200">Successful operation</response>
    /// <response code="400">Invalid parameters supplied</response>         
    /// <response code="404">Object not found</response>        
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut]
    public async Task<ActionResult<ProductDto>> PutProduct([FromBody] ProductUpdateCommand request, CancellationToken cancellationToken)
    {
        if (request == null) return BadRequest();
        var result = await _mediator.Send(request, cancellationToken);
        return result != null ? Ok(result) : NotFound();
    }

    /// <summary>
    /// Delete the identified Product from the Machine
    /// </summary>
    /// <param name="cancellationToken" example="">CancellationToken provides to Cancel the current operation, all data will be rollback</param>
    /// <param name="productId" example="uuid">It must be unique with key parameter</param>            
    /// <param name="slotId" example="uuid">It must be unique with key parameter</param>            
    /// <returns>true if the language was found and deleted, false was not deleted.</returns>
    /// <response code="200">Successful operation</response>
    /// <response code="400">Invalid parameters supplied</response>         
    /// <response code="404">Object not found</response>
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{slotId}/{productId}")]
    public async Task<ActionResult<bool>> DeleteProduct([FromRoute] string slotId, [FromRoute] string productId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(productId)) return BadRequest();
        var result = await _mediator.Send(new ProductDeleteCommand { ProductId = productId, SlotId = slotId }, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get all or  Product list.
    /// </summary>        
    /// <param name="cancellationToken" example="">CancellationToken provides to Cancel the current operation</param>
    /// <returns>List of Settins objects</returns>
    /// <response code="200">Successful operation</response>
    /// <response code="400">Invalid parameters supplied</response>         
    /// <response code="404">Object not found</response>
    [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProduct(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ProductGetAllQuery(), cancellationToken);
        return result != null ? Ok(result) : NotFound();
    }

}
