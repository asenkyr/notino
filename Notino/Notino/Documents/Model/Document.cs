using System.Text.Json;

namespace Notino.Documents.Model;

public class CreateDocumentRequest
{
    public required List<string> Tags { get; init; }
    public required JsonElement Data { get; init; }
}

public class UpdateDocumentRequest
{
    public required List<string> Tags { get; init; }
    public required JsonElement Data { get; init; }
    public required int Version { get; init; }
}

public class Document
{
    public required Guid Id { get; init; }
    public required List<string> Tags { get; init; }
    public required JsonElement Data { get; init; }
    public required int Version { get; init; }
}