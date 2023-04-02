using Microsoft.Extensions.Caching.Memory;
using Notino.Documents.Model;

namespace Notino.Documents.Providers;

public class CachedDocumentProvider : IDocumentProvider
{
    private readonly IDocumentProvider _documentProvider;
    private readonly IMemoryCache _cache;

    private const string CacheKeyPrefix = $"{nameof(CachedDocumentProvider)}";

    public CachedDocumentProvider(IDocumentProvider documentProvider, IMemoryCache cache)
    {
        _documentProvider = documentProvider;
        _cache = cache;
    }

    public async Task<Document> Create(CreateDocumentRequest request)
    {
        var result = await _documentProvider.Create(request);
        _cache.Remove(CreateKey(null));
        return result;
    }

    public async Task<Document> Get(Guid id)
    {
        var result = await _cache.GetOrCreateAsync(
            CreateKey(id), 
            _ => _documentProvider.Get(id));
        
        return result!;
    }

    public async Task<Document> Update(Guid id, UpdateDocumentRequest request)
    {
        var result = await _documentProvider.Update(id, request);
        _cache.Remove(CreateKey(result.Id));
        _cache.Remove(CreateKey(null));
        return result;
    }

    public async Task<List<Document>> GetAll()
    {
        var result = await _cache.GetOrCreateAsync(
            CreateKey(null), 
            _ => _documentProvider.GetAll());
        
        return result!;
    }

    private static string CreateKey(Guid? id)
        => id is null ? CacheKeyPrefix : $"{CacheKeyPrefix}-{id}";
}