using Microsoft.AspNetCore.Mvc;
using Notino.Documents.Model;
using Notino.Documents.Providers;

namespace Notino.Documents;

[Route("documents")]
[ApiController]
public class DocumentsController : ControllerBase
{
    private readonly IDocumentProvider _documentProvider;

    public DocumentsController(IDocumentProvider documentProvider)
    {
        _documentProvider = documentProvider;
    }

    [HttpGet]
    public async Task<ActionResult<List<Document>>> GetDocuments()
        => await _documentProvider.GetAll();
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Document>> GetDocument(Guid id)
        => await _documentProvider.Get(id);
    
    [HttpPost]
    public async Task<ActionResult<Document>> CreateDocument([FromBody] CreateDocumentRequest document)
        => await _documentProvider.Create(document);
    
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Document>> UpdateDocument([FromBody] UpdateDocumentRequest document, Guid id)
        => await _documentProvider.Update(id, document);
}