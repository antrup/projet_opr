using AutoMapper;
using BugTrackerAPI.Data;
using BugTrackerAPI.Data.DTO;
using BugTrackerAPI.Data.MappingProfiles;
using BugTrackerAPI.Data.Models;
using BugTrackerAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BugTrackerAPI.Tests;

public class TicketFixture : IDisposable
{
    IMapper _mapper;
    public BugTrackerEntities context { get; private set; }
    public ITicketRepository repository { get; private set; }

    public TicketFixture()
    {
        // Add mapper dependency
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<TicketProfile>();
        });
        _mapper = config.CreateMapper();

        // Add db context
        var options = new DbContextOptionsBuilder<BugTrackerEntities>()
    .UseInMemoryDatabase(databaseName: "Tickets")
    .Options;

        context = new BugTrackerEntities(options);

        // Populate associated tables
        context.Applications.Add(new Application()
        {
            Id = 100,
            Name = "TestApp",
        });
        context.Users.Add(new BugTrackerUser()
        {
            Id = "testUser",
            UserName = "test",
            NormalizedUserName = "TEST",
        });
        context.Users.Add(new BugTrackerUser()
        {
            Id = "testUser2",
            UserName = "test2",
            NormalizedUserName = "TEST2",
        });

        // Populate ticket table
        context.Tickets.Add(new Ticket()
        {
            Id = 1,
            Subject = "B",
            Application = 100,
            State = "In progress",
            CreatorId = "testUser",
            OwnerId = "testUser",
            CreationDate = DateTime.Now,
            Description = "test test test",

        });
        context.Tickets.Add(new Ticket()
        {
            Id = 2,
            Subject = "A",
            Application = 100,
            State = "new",
            CreatorId = "testUser2",
            CreationDate = DateTime.Now,
            Description = "test test test",

        });
        context.Tickets.Add(new Ticket()
        {
            Id = 3,
            Subject = "C",
            Application = 100,
            State = "new",
            CreatorId = "testUser",
            CreationDate = DateTime.Now,
            Description = "test test test",

        });
        context.Tickets.Add(new Ticket()
        {
            Id = 4,
            Subject = "E",
            Application = 100,
            State = "In progress",
            CreatorId = "testUser",
            OwnerId = "testUser2",
            CreationDate = DateTime.Now,
            Description = "test test test",

        });
        context.Tickets.Add(new Ticket()
        {
            Id = 5,
            Subject = "D",
            Application = 100,
            State = "In progress",
            CreatorId = "testUser2",
            OwnerId = "testUser",
            CreationDate = DateTime.Now,
            Description = "test test test",

        });
        context.SaveChanges();

        repository = new TicketRepository(context, _mapper);
    }

    public void Dispose()
    {
        context.Dispose();
    }
}

public class TicketsRepository_Tests : IClassFixture<TicketFixture>
{
    TicketFixture _fixture;

    public TicketsRepository_Tests(TicketFixture fixture)
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
        TicketDTO? ticket_existing = null;

        // Act
        ticket_existing = await _fixture.repository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(ticket_existing);
        Assert.Equal("B", ticket_existing.Subject);
        await Assert.ThrowsAnyAsync<Exception>(async () => await _fixture.repository.GetByIdAsync(42));
    }


    /// <summary>
    /// Test the GetAllAsync method
    /// </summary>
    [Fact]
    public async Task GetAllAsync()
    {
        // Arrange
        ApiResult<TicketDTO>? tickets_data = null;

        // Act
        tickets_data = await _fixture.repository.GetAllAsync(0, 10, "Subject", "ASC");

        // Assert
        Assert.NotNull(tickets_data);
        Assert.NotEqual(0, tickets_data.TotalCount);
        Assert.Collection<TicketDTO>(tickets_data.Data,
            item => Assert.Equal(2, item.Id),
            item => Assert.Equal(1, item.Id),
            item => Assert.Equal(3, item.Id),
            item => Assert.Equal(5, item.Id),
            item => Assert.Equal(4, item.Id)
            );


    }

    /// <summary>
    /// Test the InsertAsync methods
    /// </summary>
    [Fact]
    public async Task InsertAsync()
    {
        // Arrange

        ApiResult<TicketDTO>? tickets_data = null;

        // Act
        await _fixture.repository.InsertAsync(new NewTicket()
        {
            Subject = "Z",
            Application = 1,
            Description = "Insert test",
        }, "testUser");
        tickets_data = await _fixture.repository.GetAllAsync(0, 10, "Subject", "ASC");

        //Assert
        Assert.NotNull(tickets_data);
        Assert.Equal(6, tickets_data.TotalCount);
        Assert.Equal("Z", tickets_data.Data[5].Subject);
        Assert.Equal("testUser", tickets_data.Data[5].CreatorId);

        // Clean
        var ticket = await _fixture.context.Tickets.FindAsync(tickets_data.Data[5].Id);
        _fixture.context.Tickets.Remove(ticket);
        _fixture.context.SaveChanges();
    }

    /// <summary>
    /// Test the TakeAsync method
    /// </summary>
    [Fact]
    public async Task TakeAsync()
    {
        // Arrange
        TicketDTO? ticket_taken = null;

        // Act
        await _fixture.repository.TakeAsync(3, "testUser2");
        ticket_taken = await _fixture.repository.GetByIdAsync(3);

        // Assert
        Assert.NotNull(ticket_taken);
        Assert.Equal("testUser2", ticket_taken.OwnerId);
        Assert.Equal("In progress", ticket_taken.State);
        await Assert.ThrowsAnyAsync<Exception>(async () => await _fixture.repository.TakeAsync(42, "testUser2"));
        await Assert.ThrowsAnyAsync<Exception>(async () => await _fixture.repository.TakeAsync(3, "testUser2"));
    }


    /// <summary>
    /// Test the CloseAsync method
    /// </summary>
    [Fact]
    public async Task CloseAsync()
    {
        // Arrange

        TicketDTO? ticket_closed = null;

        // Act
        await _fixture.repository.CloseAsync(4, "testUser2");
        ticket_closed = await _fixture.repository.GetByIdAsync(4);

        // Assert
        Assert.NotNull(ticket_closed);
        Assert.Equal("Closed", ticket_closed.State);
        await Assert.ThrowsAnyAsync<Exception>(async () => await _fixture.repository.CloseAsync(5, "testUser2"));
        await Assert.ThrowsAnyAsync<Exception>(async () => await _fixture.repository.CloseAsync(1, "testUser2"));
    }


    /// <summary>
    /// Test the DeleteAsync method
    /// </summary>
    [Fact]
    public async Task DeleteAsync()
    {
        // Arrange

        _fixture.context.Tickets.Add(new Ticket()
        {
            Id = 13,
            Subject = "BYEBYE",
            Application = 1,
            State = "In progress",
            CreatorId = "testUser",
            OwnerId = "testUser2",
            CreationDate = DateTime.Now,
            Description = "please delete me",

        });

        _fixture.context.SaveChanges();

        // Act
        await _fixture.repository.DeleteAsync(13);

        // Assert
        await Assert.ThrowsAnyAsync<Exception>(async () => await _fixture.repository.GetByIdAsync(13));
        await Assert.ThrowsAnyAsync<Exception>(async () => await _fixture.repository.DeleteAsync(13));
    }

    /// <summary>
    /// Test the GetByCreatorAsync method
    /// </summary>
    [Fact]
    public async Task GetByCreatorAsync()
    {
        // Arrange

        ApiResult<TicketDTO>? tickets_data = null;

        // Act
        tickets_data = await _fixture.repository.GetByCreatorAsync("testUser2", 0, 10, "Subject", "ASC");

        // Assert
        Assert.NotNull(tickets_data);
        Assert.Equal(2, tickets_data.TotalCount);
        Assert.Collection<TicketDTO>(tickets_data.Data,
            item => Assert.Equal(2, item.Id),
            item => Assert.Equal(5, item.Id)
            );
    }


    /// <summary>
    /// Test the GetByOwnerAsync method
    /// </summary>
    [Fact]
    public async Task GetByOwnerAsync()
    {
        // Arrange

        ApiResult<TicketDTO>? tickets_data = null;

        // Act
        tickets_data = await _fixture.repository.GetByOwnerAsync("testUser", 0, 10, "Subject", "ASC");

        // Assert
        Assert.NotNull(tickets_data);
        Assert.Equal(2, tickets_data.TotalCount);
        Assert.Collection<TicketDTO>(tickets_data.Data,
            item => Assert.Equal(1, item.Id),
            item => Assert.Equal(5, item.Id)
            );
    }

    /// <summary>
    /// Test the Getasync method of statsRepositoryu
    /// </summary>
    [Fact]
    public async Task Getasync()
    {
        // Arrange
        StatsDTO? stats = null;
        var repository = new StatsRepository(_fixture.context);

        // Act
        stats = await repository.Getasync("testUser");

        // Assert
        Assert.NotNull(stats);
        Assert.Equal(5, stats.TotalTickets);
        Assert.Equal(3, stats.TotalMyTickets);
        Assert.Equal(2, stats.TotalOwnedTickets);
    }
}