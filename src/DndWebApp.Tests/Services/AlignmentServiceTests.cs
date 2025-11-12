using static DndWebApp.Tests.Services.TestObjectFactory;
using DndWebApp.Api.Models.World;
using DndWebApp.Api.Services.Implemented;
using Moq;
using Microsoft.Extensions.Logging.Abstractions;
using DndWebApp.Api.Repositories.Interfaces;

namespace DndWebApp.Tests.Services;

public class AlignmentServiceTests
{
    [Fact]
    public async Task AddAndRetrieveAlignments_WorksCorrectly()
    {
        // Arrange
        var repo = new Mock<IRepository<Alignment>>();
        var service = new AlignmentService(repo.Object, NullLogger<AlignmentService>.Instance);

        ICollection<Alignment> alignments = [];

        repo.Setup(r => r.CreateAsync(It.IsAny<Alignment>()))
            .ReturnsAsync((Alignment a) =>
            {
                a.Id = alignments.Count + 1;
                alignments.Add(a);
                return a;
            });

        repo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => alignments
            .FirstOrDefault(a => a.Id == id));

        repo.Setup(r => r.GetMiscellaneousItemsAsync())
            .ReturnsAsync(() => [.. alignments]);

        // Act
        var created1 = await service.CreateAsync(CreateTestAlignmentDto("Lawful Good", "LG", "A lawful good character", 1));
        var created2 = await service.CreateAsync(CreateTestAlignmentDto("Chaotic Evil", "CE", "A chaotic evil character", 2));

        var lawfulGood = await service.GetByIdAsync(created1.Id);
        var chaoticEvil = await service.GetByIdAsync(created2.Id);
        alignments = await service.GetAllAsync();

        // Assert
        Assert.NotNull(lawfulGood);
        Assert.Equal("Lawful Good", lawfulGood.Name);
        Assert.Equal("LG", lawfulGood.Abbreviation);

        Assert.NotNull(chaoticEvil);
        Assert.Equal("Chaotic Evil", chaoticEvil.Name);
        Assert.Equal("A chaotic evil character", chaoticEvil.Description);

        // Asserts for all objects
        Assert.Contains(alignments, a => a.Name == "Lawful Good");
        Assert.Contains(alignments, a => a.Abbreviation == "CE");

        repo.Verify(r => r.CreateAsync(It.IsAny<Alignment>()), Times.Exactly(2));
    }

    [Fact]
    public async Task AddAndRetrieveAlignments_BadInputData_ShouldNotCreate()
    {
        // Arrange
        var repo = new Mock<IRepository<Alignment>>();
        var service = new AlignmentService(repo.Object, NullLogger<AlignmentService>.Instance);

        var noName = CreateTestAlignmentDto("", "LG", "A lawful good character", 1);
        var whitespaceAberration = CreateTestAlignmentDto("Lawful Good", "   ", "A lawful good character", 2);
        var nullDescription = CreateTestAlignmentDto("Lawful Good", "LG", null!, 3);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(noName));
        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(whitespaceAberration));
        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(nullDescription));

        repo.Verify(r => r.CreateAsync(It.IsAny<Alignment>()), Times.Exactly(0));
    }

    [Fact]
    public async Task DeleteAlignment_WorksCorrectly()
    {
        // Arrange
        var repo = new Mock<IRepository<Alignment>>();
        var service = new AlignmentService(repo.Object, NullLogger<AlignmentService>.Instance);

        List<Alignment> alignments = [CreateTestAlignment("Lawful Good", "LG", "A lawful good character", 1)];

        repo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => alignments
            .FirstOrDefault(a => a.Id == id));

        repo.Setup(r => r.DeleteAsync(It.IsAny<Alignment>()))
            .Callback((Alignment a) =>
            {
                alignments.Remove(a);
            });

        // Act
        var id = alignments[0].Id;
        await service.DeleteAsync(id);

        // Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => service.DeleteAsync(id));
        repo.Verify(r => r.DeleteAsync(It.IsAny<Alignment>()), Times.Exactly(1));
    }

    [Fact]
    public async Task GetAndDeleteAlignment_BadId_ShouldNotGetOrDelete()
    {
        // Arrange
        var repo = new Mock<IRepository<Alignment>>();
        var service = new AlignmentService(repo.Object, NullLogger<AlignmentService>.Instance);

        List<Alignment> alignments = [CreateTestAlignment("Lawful Good", "LG", "A lawful good character", 1)];
        alignments[0].Id = 1;

        repo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => alignments
            .FirstOrDefault(a => a.Id == id));

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => service.GetByIdAsync(-1));
        await Assert.ThrowsAsync<NullReferenceException>(() => service.DeleteAsync(-1));

        repo.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Exactly(2));
        repo.Verify(r => r.DeleteAsync(It.IsAny<Alignment>()), Times.Exactly(0));
    }

    [Fact]
    public async Task UpdateAlignment_WorksCorrectly()
    {
        var repo = new Mock<IRepository<Alignment>>();
        var service = new AlignmentService(repo.Object, NullLogger<AlignmentService>.Instance);

        List<Alignment> alignments = [CreateTestAlignment("Lawful Good", "LG", "A lawful good character", 1)];

        var updateDto = CreateTestAlignmentDto("Lawful bad", "LB", "A lawful bad character", 1);

        repo.Setup(r => r.UpdateAsync(It.IsAny<Alignment>()))
            .Callback((Alignment a) =>
            {
                var alignment = alignments.FirstOrDefault(align => align.Id == a.Id);
                alignment!.Name = a.Name;
                alignment!.Abbreviation = a.Abbreviation;
                alignment.Description = a.Description;
            });

        repo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => alignments
            .FirstOrDefault(a => a.Id == id));

        // Act
        await service.UpdateAsync(updateDto);
        var updated = await service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(updated);
        Assert.Equal("Lawful bad", updated.Name);
        Assert.Equal("LB", updated.Abbreviation);
        Assert.Equal("A lawful bad character", updated.Description);

        repo.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Exactly(2));
        repo.Verify(r => r.DeleteAsync(It.IsAny<Alignment>()), Times.Exactly(0));
    }

    [Fact]
    public async Task UpdateAlignments_BadInputData_ShouldNotUpdate()
    {
        var repo = new Mock<IRepository<Alignment>>();
        var service = new AlignmentService(repo.Object, NullLogger<AlignmentService>.Instance);

        List<Alignment> alignments = [CreateTestAlignment("Lawful Good", "LG", "A lawful good character", 1)];

        var noName = CreateTestAlignmentDto("", "LG", "A lawful good character", 1);
        var whitespaceAberration = CreateTestAlignmentDto("Lawful Good", "   ", "A lawful good character", 2);
        var nullDescription = CreateTestAlignmentDto("Lawful Good", "LG", null!, 3);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(noName));
        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(whitespaceAberration));
        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(nullDescription));

        repo.Verify(r => r.UpdateAsync(It.IsAny<Alignment>()), Times.Exactly(0));
    }

    [Fact]
    public void SortBy_WorksCorrectly()
    {
        // Arrange
        var service = new AlignmentService(null!, NullLogger<AlignmentService>.Instance);

        List<Alignment> alignments =
        [
            CreateTestAlignment("Lawful Good", "LG", "A lawful good character", 1),
            CreateTestAlignment("Chaotic Evil", "CE", "A chaotic evil character", 2),
            CreateTestAlignment("True Neutral", "TN", "A true neutral character", 3),
        ];

        // Act
        var sorted = service.SortBy(alignments);

        // Assert
        string[] expectedOrder = ["Lawful Good", "True Neutral", "Chaotic Evil"];
        Assert.Equal(expectedOrder, sorted.Select(s => s.Name));
    }
}
