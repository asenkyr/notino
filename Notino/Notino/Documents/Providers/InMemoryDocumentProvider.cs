using System.Collections.Concurrent;
using Notino.Documents.Model;
using Notino.Exceptions;

namespace Notino.Documents.Providers;

public class InMemoryDocumentProvider : IDocumentProvider
{
    private readonly ConcurrentDictionary<Guid, Document> _data = new();
    
    public Task<Document> Create(CreateDocumentRequest request)
    {
        var doc = new Document
        {
            Id = Guid.NewGuid(),
            Tags = request.Tags,
            Data = request.Data,
            Version = 0
        };

        if (_data.TryAdd(doc.Id, doc))
        {
            return Task.FromResult(doc);
        }

        throw new InvalidOperationException(
            "This should never happen, since we are generating our Guids here");
    }

    public Task<Document> Get(Guid id)
        => Task.FromResult(GetDocument(id));

    public Task<Document> Update(Guid id, UpdateDocumentRequest request)
    {
        var current = GetDocument(id);
        var newDoc = new Document
        {
            Id = id,
            Tags = request.Tags,
            Data = request.Data,
            Version = request.Version
        };

        if (current.Version + 1 != request.Version)
        {
            throw new AlreadyUpdatedException(id);
        }

        if (_data.TryUpdate(id, newDoc, current))
        {
            return Task.FromResult(newDoc);
        }
        
        throw new AlreadyUpdatedException(id);
    }

    public Task<List<Document>> GetAll()
        => Task.FromResult(_data.Values.ToList());

    private Document GetDocument(Guid id)
    {
        if (_data.TryGetValue(id, out var data))
        {
            return data;
        }

        throw new ItemNotFoundException(id);
    }
}