using BulletInBoardServer.Domain;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Test.Infrastructure;

public static class ServiceScopeFactoryConfigurator
{
    public static IServiceScopeFactory Configure(ApplicationDbContext dbContext)
    {
        var serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock
            .Setup(x => x.GetService(typeof(ApplicationDbContext)))
            .Returns(dbContext);

        var serviceScopeMock = new Mock<IServiceScope>();
        serviceScopeMock
            .Setup(x => x.ServiceProvider)
            .Returns(serviceProviderMock.Object);

        var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
        serviceScopeFactoryMock.Setup(x => x.CreateScope())
            .Returns(serviceScopeMock.Object);
        
        serviceProviderMock
            .Setup(x => x.GetService(typeof(IServiceScopeFactory)))
            .Returns(serviceScopeFactoryMock.Object);

        return serviceScopeFactoryMock.Object;
    }
}