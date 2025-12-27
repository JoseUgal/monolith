using Domain.Results;
using FluentAssertions;
using Modules.Workspaces.Domain.Workspaces;

namespace Modules.Workspaces.Domain.Tests.Workspaces;

public sealed class WorkspaceNameTests
{
    [Fact]
    public void Create_Empty_ReturnsIsRequired()
    {
        // Arrange
        string input = "";

        // Act
        Result<WorkspaceName> result = WorkspaceName.Create(input);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(WorkspaceErrors.Name.IsRequired);
    }
    
    [Fact]
    public void Create_TooLong_ReturnsTooLong()
    {
        // Arrange
        string input = new('a', WorkspaceName.MaxLength + 1);
        
        // Act
        Result<WorkspaceName> result = WorkspaceName.Create(input);
        
        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(WorkspaceErrors.Name.TooLong(WorkspaceName.MaxLength));
    }
    
    [Fact]
    public void Create_Valid_TrimAndSuccess()
    {
        // Arrange
        string trimmed = "Jose's Workspace";
        string input = $"  {trimmed}  ";

        // Act
        Result<WorkspaceName> result = WorkspaceName.Create(input);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(trimmed);
    }
}
