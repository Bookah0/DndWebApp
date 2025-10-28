using Moq;

namespace DndWebApp.Tests.Services;

/*
Walkthrough:

Replace *Class* with class name (uppercase)
Replace *classvar* with class name (lowercase)
Replace *c* with the first letter of the class (lowercase)
Replace *cs* with a short name for the class, but not only the first letter (lowercase)

Add relevant parameters and assignments to CreateTest*Class*Dto(int id) and CreateTest*Class*(int id)

Decide parameters for two test objects. for example:
- params1 = ("Name1", 1)
- params2 = ("Name2", 2)

Replace CreateTest*Class*1() with CreateTest*Class*(params1)
Replace CreateTest*Class*2() with CreateTest*Class*(params2)
Replace CreateTest*Class*Dto1() with CreateTest*Class*Dto(params1)
Replace CreateTest*Class*Dto2() with CreateTest*Class*Dto(params2)

Replace *testObj* with a name that suits test object 1           
Replace *testObj2* with a name that suits test object 2 
*/

public class *Class*ServiceTests
{
    internal static *Class*Dto CreateTest*Class*Dto(int id)
    {
        return new() { Id = id };
    }

    internal static *Class* CreateTest*Class*(int id)
    {
        return new() { Id = id };
    }

    [Fact]
    public async Task AddAndRetrieve*Class*s_WorksCorrectly()
    {
        // Arrange
        var repo = new Mock<I*Class*Repository>();
        // var additionalRepo = new Mock<I*OtherClass*Repository>();
        var service = new *Class*Service(repo.Object);

        ICollection<*Class*> *classvar*s = [];

        repo.Setup(r => r.CreateAsync(It.IsAny<*Class*>()))
            .ReturnsAsync((*Class* *c*) =>
            {
                *c*.Id = *classvar*s.Count + 1;
                *classvar*s.Add(*c*);
                return *c*;
            });

        repo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => *classvar*s
            .FirstOrDefault(*c* => *c*.Id == id));

        repo.Setup(r => r.GetAllAsync())
            .ReturnsAsync(() => [.. *classvar*s]);

        // Act
        var created1 = await service.CreateAsync(CreateTest*Class*Dto1());
        var created2 = await service.CreateAsync(CreateTest*Class*Dto2());

        var *testObj* = await service.GetByIdAsync(created1.Id);
        var *testObj2* = await service.GetByIdAsync(created2.Id);
        *classvar*s = await service.GetAllAsync();

        // Assert
        Assert.NotNull(*testObj*);
        // Assert.Equal("", *testObj*.Name);
        // Assert.Equal(, *testObj*.);
        // Assert.Equal(, *testObj*.);

        Assert.NotNull(*testObj2*);
        // Assert.Equal("", *testObj2*.Name);
        // Assert.Equal(, *testObj2*.);
        // Assert.Equal(, *testObj2*.);

        // Asserts for all objects
        // Assert.Contains(*classvar*s, *c* => *c*.Name == "");
        // Assert.Contains(*classvar*s, *c* => *c*.Name == "");

        repo.Verify(r => r.CreateAsync(It.IsAny<*Class*>()), Times.Exactly(2));
    }

    [Fact]
    public async Task AddAndRetrieve*Class*s_BadInputData_ShouldNotCreate()
    {
        // Arrange
        var repo = new Mock<I*Class*Repository>();
        // var additionalRepo = new Mock<I*OtherClass*Repository>>();
        var service = new *Class*Service(repo.Object);

        // Add test dtos here, for example:
        // var noName = CreateTest*Class*Dto("");
        // var anotherTestDto = CreateTest*Class*Dto();

        // Act & Assert
        // await Assert.ThrowsAsync<NullReferenceException>(() => service.CreateAsync(noName));
        // await Assert.ThrowsAsync<NullReferenceException>(() => service.CreateAsync(anotherTestDto));
        
        repo.Verify(r => r.CreateAsync(It.IsAny<*Class*>()), Times.Exactly(0));
    }

    [Fact]
    public async Task Delete*Class*_WorksCorrectly()
    {
        // Arrange
        var repo = new Mock<I*Class*Repository>();
        // var additionalRepo = new Mock<I*OtherClass*Repository>>();
        var service = new *Class*Service(repo.Object);

        ICollection<*Class*> *classvar*s = [CreateTest*Class*1()];

        repo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => *classvar*s
            .FirstOrDefault(*c* => *c*.Id == id));

        repo.Setup(r => r.DeleteAsync(It.IsAny<*Class*>()))
            .Callback((*Class* *c*) =>
            {
                *classvar*s.Remove(*c*);
            });

        // Act
        var id = abilities.First().Id;
        await service.DeleteAsync(id);

        // Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => service.DeleteAsync(id));
        repo.Verify(r => r.CreateAsync(It.IsAny<*Class*>()), Times.Exactly(1));
    }

    [Fact]
    public async Task GetAndDelete*Class*_BadId_ShouldNotGetOrDelete()
    {
        // Arrange
        var repo = new Mock<I*Class*Repository>();
        // var additionalRepo = new Mock<I*OtherClass*Repository>>();
        var service = new *Class*Service(repo.Object);

        ICollection<*Class*> *classvar*s = [CreateTest*Class*1()];
        *classvar*s.First().Id = 1;

        repo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => *classvar*s
            .FirstOrDefault(*c* => *c*.Id == id));

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => service.GetByIdAsync(-1));
        await Assert.ThrowsAsync<NullReferenceException>(() => service.DeleteAsync(-1));

        repo.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Exactly(2));
        repo.Verify(r => r.DeleteAsync(It.IsAny<*Class*>()), Times.Exactly(0));
    }
    
    [Fact]
    public async Task Update*Class*_WorksCorrectly()
    {
        var repo = new Mock<I*Class*Repository>();
        // var additionalRepo = new Mock<I*OtherClass*Repository>>();
        var service = new *Class*Service(repo.Object);
        
        ICollection<*Class*> *classvar*s = [CreateTest*Class*1()];
        var updateDto = CreateTest*Class*Dto2();
        updateDto.Id = *classvar*s.First().Id;

        repo.Setup(r => r.UpdateAsync(It.IsAny<*Class*>()))
            .Callback((*Class* *c*) =>
            {
                var *classvar* = *classvar*s.FirstOrDefault(*cs* => *cs*.Id == *c*.Id);
                // Update all data, for example:
                // *classvar*!.Name = *c*.Name;
                // *classvar*. = *c*.;
                // *classvar*. = *c*.;
            });

        repo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => *classvar*s
            .FirstOrDefault(*c* => *c*.Id == id));

        // Act
        await service.UpdateAsync(updateDto);
        var updated = await service.GetByIdAsync(*classvar*s.First().Id);

        // Assert
        Assert.NotNull(updated);
        // Additional asserts
        
        // Assert.Equal(, updated.Name);
        // Assert.Equal(, updated.);
        // Assert.Equal(, updated.);
        
        repo.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Exactly(2));
        repo.Verify(r => r.DeleteAsync(It.IsAny<*Class*>()), Times.Exactly(0));
    }

    [Fact]
    public async Task Update*Class*s_BadInputData_ShouldNotUpdate()
    {
        var repo = new Mock<I*Class*Repository>();
        // var additionalRepo = new Mock<I*OtherClass*Repository>>();
        var service = new *Class*Service(repo.Object);

        ICollection<*Class*> *classvar*s = [CreateTest*Class*1()];

        // Add test dtos here, for example:
        // var noName = CreateTest*Class*Dto();
        // var null = CreateTest*Class*Dto();

        // Act & Assert
        // await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateAsync(noName));
        // await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateAsync(null));

        repo.Verify(r => r.UpdateAsync(It.IsAny<*Class*>()), Times.Exactly(0));
    }

    [Fact]
    public void SortBy_WorksCorrectly()
    {
        // Arrange
        var service = new *Class*Service(null!);

        ICollection <*Class*> *classvar*s =
        [
            CreateTest*Class*1(),
            CreateTest*Class*2(),
            // Additional test classes as needed:
        ];

        // Act & Assert 
        // If there is more than one order:
        var sorted = service.SortBy(*classvar*s, *SortingEnum*);
        string[] expectedOrder = [...];
        Assert.Equal(expectedOrder, sorted.Select(s => s.*sortedWith*));

        // If sorting with a single fixed order:
        var sorted = service.SortBy(*classvar*s);
        string[] expectedOrder = [...];
        Assert.Equal(expectedOrder, sorted.Select(s => s.*sortedWith*));
    }
}
