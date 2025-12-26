using Domain.Results;
using FluentAssertions;
using Modules.Tenants.Domain.Tenants;

namespace Modules.Tenants.Domain.Tests.Tenants;

public class TenantSlugTests
{
    [Fact]
    public void Create_Empty_ReturnsIsRequired()
    {
        // Arrange
        string input = "";

        // Act
        Result<TenantSlug> result = TenantSlug.Create(input);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TenantErrors.Slug.IsRequired);
    }
    
    [Fact]
    public void Create_TooLong_ReturnsTooLong()
    {
        // Arrange
        string input = new('a', TenantSlug.MaxLength + 1);

        // Act
        Result<TenantSlug> result = TenantSlug.Create(input);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TenantErrors.Slug.TooLong(TenantSlug.MaxLength));
    }
    
    [Fact]
    public void Create_TooShort_ReturnsTooShort()
    {
        // Arrange
        string input = new('a', TenantSlug.MinLength - 1);

        // Act
        Result<TenantSlug> result = TenantSlug.Create(input);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TenantErrors.Slug.TooShort(TenantSlug.MinLength));
    }
    
    [Fact]
    public void Create_Invalid_ReturnsIsInvalid()
    {
        // Arrange
        string input = "ana lopez";

        // Act
        Result<TenantSlug> result = TenantSlug.Create(input);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TenantErrors.Slug.IsInvalid);
    }
    
    [Fact]
    public void Create_Valid_NormalizeAndSuccess()
    {
        // Arrange
        string input = " Ana-Lopez ";

        // Act
        Result<TenantSlug> result = TenantSlug.Create(input);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be("ana-lopez");
    }
}
