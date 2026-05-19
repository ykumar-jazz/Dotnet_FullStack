using Moq;
using mvc_project.EMS.Application.Interfaces;
using mvc_project.EMS.Application.Services;
using mvc_project.EMS.Domain.Entities;
using Xunit;
namespace mvc_project.EMS.Tests.UnitTests;

public class EmployeeServiceTests
{
    [Fact]
    public async Task GetAllAsync_ShouldReturnEmployees()
    {
        var mockRepo =
            new Mock<IEmployeeRepository>();

        mockRepo.Setup(x =>
            x.GetAllAsync())
        .ReturnsAsync([]);

        EmployeeService service =
            new(mockRepo.Object);

        var result = await service.GetAllAsync();

        Assert.NotEmpty(result);
    }
}
