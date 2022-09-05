
using Machine.Application.Common.Interface;
using Machine.Infrastructure.Services;
using FluentAssertions;
using Machine.Application.Common.Exceptions;

namespace Machine.Infrastructure.UnitTests;

[Collection("VendingMachine")]
public class VendingMachineTests : IClassFixture<ApplicationFixture>
{
    private readonly IVendingMachine _sut; //System under test

    public VendingMachineTests(ApplicationFixture fixture)
    {
        _sut = fixture.SvcProvider.GetService<IVendingMachine>();       
    }

    [Fact]
    public void Show_ShouldShowProducts()
    {
        // Arrange

        // Act
        var productDtos = _sut.Show();

        // Assert  
        productDtos.Should().NotBeNullOrEmpty();
        productDtos.Count.Should().BeGreaterThan(1);

        Console.WriteLine($"Assert: expected result > 1, received result:{productDtos.Count}");
    }

    [Theory]
    [InlineData(2)]
    [InlineData(1)]
    [InlineData(0.50)]
    [InlineData(0.20)]
    [InlineData(0.10)]
    [InlineData(0.05)]
    public void AddCoin_ShouldAddToBalance_WhenParameterIsValid(decimal amount)
    {
        // Arrange
        decimal startupBalance = _sut.CurrentBalance;

        // Act
        _sut.EnterCoin(amount);

        // Assert  
        _sut.CurrentBalance.Should().BeGreaterThan(startupBalance);
        decimal balance = _sut.CurrentBalance - startupBalance;

        Console.WriteLine($"Assert: expected result : {amount}, received result:{balance}");
    }


    [Theory]
    [InlineData(-2)]
    [InlineData(-1)]
    [InlineData(20000)]
    [InlineData(0.02)]
    [InlineData(0.01)]
    public void AddCoin_ShouldAcceptOnlyValidCoins_WhenParameterIsValid(decimal amount)
    {
        // Arrange
        decimal startupBalance = _sut.CurrentBalance;

        // Act
        Action act = () => _sut.AddCoin(amount);

        // Assert  
        act.Should().Throw<InvalidCoinException>();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void Select_ShouldBeDispenseProduct_WhenParameterIsValid(int productId)
    {
        // Arrange
        _sut.EnterCoin(2);
        decimal startupBalance = _sut.CurrentBalance;

        // Act
        var change = _sut.Select(productId);

        // Assert  
        _sut.CurrentBalance.Should().BeLessThan(startupBalance);
    }

}
