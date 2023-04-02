using Notino.Documents.Model;

namespace Notino.Documents.Providers;

public interface IDocumentProvider
{
    Task<Document> Create(CreateDocumentRequest request);
    Task<Document> Get(Guid id);
    Task<Document> Update(Guid id, UpdateDocumentRequest request);
    Task<List<Document>> GetAll();
}