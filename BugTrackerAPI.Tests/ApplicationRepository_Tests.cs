using AutoMapper;
using BugTrackerAPI.Data;
using BugTrackerAPI.Data.DTO;
using BugTrackerAPI.Data.MappingProfiles;
using BugTrackerAPI.Data.Models;
using BugTrackerAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BugTrackerAPI.Tests;

public class ApplicationFixture : IDisposable
{
    IMapper _mapper;
    public BugTrackerEntities context { get; private set; }
    public IApplicationRepository repository { get; private set; }

    public ApplicationFixture()
    {
        // Add mapper dependency
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ApplicationProfile>();
        });
        _mapper = config.CreateMapper();

        // Add db context
        var options = new DbContextOptionsBuilder<BugTrackerEntities>()
    .UseInMemoryDatabase(databaseName: "Applications")
    .Options;

        context = new BugTrackerEntities(options);

        // Populate Application table
        context.Applications.Add(new Application()
        {
            Id = 1,
            Name = "D",
        });
        context.Applications.Add(new Application()
        {
            Id = 2,
            Name = "E",
        });
        context.Applications.Add(new Application()
        {
            Id = 3,
            Name = "A",
        });
        context.Applications.Add(new Application()
        {
            Id = 4,
            Name = "B",
        });
        context.Applications.Add(new Application()
        {
            Id = 5,
            Name = "C",
        });

        context.SaveChanges();

        repository = new ApplicationRepository(context, _mapper);
    }

    public void Dispose()
    {
        context.Dispose();
    }
}

public class ApplicationRepository_Tests : IClassFixture<ApplicationFixture>
{
    ApplicationFixture _fixture;

    public ApplicationRepository_Tests(ApplicationFixture fixture)
    {
        _fixture = fixture;
    }

    /// <summary>
    /// Test the GetByIdAsync method
    /// </summary>
    [Fact]
    public async Task GetByIdAsync()
    {
        // Arrange
        ApplicationDTO? application_existing = null;

        // Act
        application_existing = await _fixture.repository.GetByIdAsync(2);

        // Assert
        Assert.NotNull(application_existing);
        Assert.Equal("E", application_existing.Name);
        await Assert.ThrowsAnyAsync<Exception>(async () => await _fixture.repository.GetByIdAsync(42));
    }


    /// <summary>
    /// Test the GetAllAsync method
    /// </summary>
    [Fact]
    public async Task GetAllAsync()
    {
        // Arrange
        ApiResult<ApplicationDTO>? application_data = null;

        // Act
        application_data = await _fixture.repository.GetAllAsync(0, 10, "Name", "ASC");

        // Assert
        Assert.NotNull(application_data);
        Assert.NotEqual(0, application_data.TotalCount);
        Assert.Collection<ApplicationDTO>(application_data.Data,
            item => Assert.Equal(3, item.Id),
            item => Assert.Equal(4, item.Id),
            item => Assert.Equal(5, item.Id),
            item => Assert.Equal(1, item.Id),
            item => Assert.Equal(2, item.Id)
            );


    }

    /// <summary>
    /// Test the InsertAsync methods
    /// </summary>
    [Fact]
    public async Task InsertAsync()
    {
        // Arrange

        ApiResult<ApplicationDTO>? application_data = null;

        // Act
        await _fixture.repository.InsertAsync(new NewApplicationDTO()
        {
            Name = "Z"
        });
        application_data = await _fixture.repository.GetAllAsync(0, 10, "Name", "ASC");

        //Assert
        Assert.NotNull(application_data);
        Assert.Equal(6, application_data.TotalCount);
        Assert.Equal("Z", application_data.Data[5].Name);

        // Clean
        var application = await _fixture.context.Applications.FindAsync(application_data.Data[5].Id);
        _fixture.context.Applications.Remove(application);
        _fixture.context.SaveChanges();
    }

    /// <summary>
    /// Test the DeleteAsync method
    /// </summary>
    [Fact]
    public async Task DeleteAsync()
    {
        // Arrange

        _fixture.context.Applications.Add(new Application()
        {
            Id = 13,
            Name = "BYEBYE",
        });

        _fixture.context.SaveChanges();

        // Act
        await _fixture.repository.DeleteAsync(13);

        // Assert
        await Assert.ThrowsAnyAsync<Exception>(async () => await _fixture.repository.GetByIdAsync(13));
        await Assert.ThrowsAnyAsync<Exception>(async () => await _fixture.repository.DeleteAsync(13));
    }

    /// <summary>
    /// Test the Update method
    /// </summary>
    [Fact]
    public async Task UpdateAsync()
    {
        // Arrange

        ApplicationDTO? application_data = null;

        // Act
        await _fixture.repository.UpdateAsync(new ApplicationDTO()
        {
            Id = 1,
            Name = "DEDE",
        });
        application_data = await _fixture.repository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(application_data);
        Assert.Equal("DEDE", application_data.Name);
    }
}