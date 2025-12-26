using Domain.Results;
using FluentAssertions;
using Modules.Tenants.Domain.Tenants;

namespace Modules.Tenants.Domain.Tests.Tenants;

public class TenantNameTests
{
    [Fact]
    public void Create_Empty_ReturnsIsRequired()
    {
        // Arrange
        string input = "";

        // Act
        Result<TenantName> result = TenantName.Create(input);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TenantErrors.Name.IsRequired);
    }
    
    [Fact]
    public void Create_TooLong_ReturnsTooLong()
    {
        // Arrange
        string input = new('a', TenantName.MaxLength + 1);

        // Act
        Result<TenantName> result = TenantName.Create(input);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TenantErrors.Name.TooLong(TenantName.MaxLength));
    }
    
    [Fact]
    public void Create_TooShort_ReturnsToShort()
    {
        // Arrange
        string input = new('a', TenantName.MinLength - 1);

        // Act
        Result<TenantName> result = TenantName.Create(input);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TenantErrors.Name.TooShort(TenantName.MinLength));
    }
    
    [Fact]
    public void Create_Valid_TrimAndSuccess()
    {
        // Arrange
        string trimmed = "Ana Lopez";
        string input = $"  {trimmed}  ";

        // Act
        Result<TenantName> result = TenantName.Create(input);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(trimmed);
    }
}
