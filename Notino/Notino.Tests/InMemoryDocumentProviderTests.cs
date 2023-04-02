using System.Text.Json;
using FluentAssertions;
using Notino.Documents.Model;
using Notino.Documents.Providers;
using Notino.Exceptions;

namespace Notino.Tests;

public class InMemoryDocumentProviderTests
{
    private IDocumentProvider Provider { get; } = new InMemoryDocumentProvider();
  
    [Fact]
    public async Task Create_HappyPath()
    {
        // act
        var result = await CreateDocument(new CreateDocumentRequest
        {
            Tags = new List<string>{"TAG1"},
            Data = new JsonElement()
        });

        // assert
        result.Tags.Should().BeEquivalentTo("TAG1");
        result.Data.Should().NotBeNull();
        result.Version.Should().Be(0);
    }

    [Fact]
    public async Task Get_HappyPath()
    {
        // arrange
        var document = await CreateDocument();
        
        // act
        var result = await Provider.Get(document.Id);
        
        // assert
        result.Should().BeEquivalentTo(document);
    }
    
    [Fact]
    public async Task Get_NotFound()
    {
        // arrange
        var action = () => Provider.Get(Guid.NewGuid());
        
        // act & assert
        await action.Should().ThrowAsync<ItemNotFoundException>();
    }

    [Fact]
    public async Task Update_HappyPath()
    {
        // arrange
        var document = await CreateDocument();
        
        // act
        var updatedDocument = await Provider.Update(document.Id, new UpdateDocumentRequest
        {
            Data = document.Data,
            Tags = new List<string> {"NEWTAG"},
            Version = document.Version + 1
        });
        
        // assert
        updatedDocument.Tags.Should().BeEquivalentTo("NEWTAG");
        updatedDocument.Version.Should().Be(1);
    }
    
    [Fact]
    public async Task Update_Race()
    {
        // arrange
        var document = await CreateDocument();
        
        // act
        await Provider.Update(document.Id, new UpdateDocumentRequest
        {
            Data = document.Data,
            Tags = new List<string> {"NEWTAG"},
            Version = document.Version + 1
        });
        
        var updateAgain = () => Provider.Update(document.Id, new UpdateDocumentRequest
        {
            Data = document.Data,
            Tags = new List<string> {"OTHERTAG"},
            Version = document.Version + 1
        });

        var updatedDocument = await Provider.Get(document.Id);
        
        // assert
        await updateAgain.Should().ThrowAsync<AlreadyUpdatedException>();
        updatedDocument.Version.Should().Be(1);
        updatedDocument.Tags.Should().BeEquivalentTo("NEWTAG"); // the first update should win
    }
    
    [Fact]
    public async Task Update_NotFound()
    {
        // arrange
        var action = () => Provider.Update(Guid.NewGuid(), new UpdateDocumentRequest
        {
            Data = new JsonElement(),
            Tags = new List<string>(),
            Version = 4
        });
        
        // act & assert
        await action.Should().ThrowAsync<ItemNotFoundException>();
    }

    private Task<Document> CreateDocument(CreateDocumentRequest? createDocumentRequest = null)
    {
        var request = createDocumentRequest ?? new CreateDocumentRequest
        {
            Tags = new List<string> {"DEFAULT_TAG"},
            Data = new JsonElement()
        };

        return Provider.Create(request);
    }
    
}