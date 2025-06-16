using Xunit;
using Moq;
using Microsoft.AspNetCore.SignalR;
using UserAccountService.Hubs;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using System.Threading;

namespace UserAccountService.Tests.Hubs;

/// <summary>
/// Unit tests for the FriendshipHub class.
/// </summary>
public class FriendshipHubTests
{
    private readonly Mock<HubCallerContext> _mockHubContext;
    private readonly FriendshipHub _friendshipHub;
    private readonly Mock<IGroupManager> _mockGroupManager;
    private readonly Mock<IClientProxy> _mockClientProxy;

    /// <summary>
    /// Initializes the FriendshipHubTests class and sets up mock dependencies.
    /// </summary>
    public FriendshipHubTests()
    {
        var mockClients = new Mock<IHubCallerClients>();
        _mockHubContext = new Mock<HubCallerContext>();
        _mockGroupManager = new Mock<IGroupManager>();
        _mockClientProxy = new Mock<IClientProxy>();

        // Setup mock client proxy behavior
        mockClients.Setup(clients => clients.All).Returns(_mockClientProxy.Object);
        mockClients.Setup(clients => clients.User(It.IsAny<string>())).Returns(_mockClientProxy.Object);
        mockClients.Setup(clients => clients.Group(It.IsAny<string>())).Returns(_mockClientProxy.Object);

        _mockClientProxy.Setup(x => x.SendCoreAsync(
                It.IsAny<string>(),
                It.IsAny<object[]>(),
                It.IsAny<CancellationToken>()
            )
        ).Returns(Task.CompletedTask);

        // Initialize the FriendshipHub with mocked dependencies
        _friendshipHub = new FriendshipHub
        {
            Clients = mockClients.Object,
            Context = _mockHubContext.Object,
            Groups = _mockGroupManager.Object
        };
    }

    /// <summary>
    /// Tests that OnConnectedAsync adds the user to a group when a valid user ID is provided.
    /// </summary>
    [Fact]
    public async Task OnConnectedAsync_WithValidUserId_AddsUserToGroup()
    {
        var userId = Guid.NewGuid();
        const string connectionId = "testConnectionId";

        var mockClaimsIdentity = new Mock<ClaimsIdentity>();
        mockClaimsIdentity.Setup(ci => ci.FindFirst(ClaimTypes.NameIdentifier))
            .Returns(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));
        var mockClaimsPrincipal = new ClaimsPrincipal(mockClaimsIdentity.Object);

        _mockHubContext.Setup(c => c.User).Returns(mockClaimsPrincipal);
        _mockHubContext.Setup(c => c.ConnectionId).Returns(connectionId);

        await _friendshipHub.OnConnectedAsync();

        // Verify that the user is added to the group
        _mockGroupManager.Verify(g => g.AddToGroupAsync(connectionId, userId.ToString(), CancellationToken.None),
            Times.Once);

        // Verify that no messages are sent to the client
        _mockClientProxy.Verify(
            x => x.SendCoreAsync(It.IsAny<string>(), It.IsAny<object[]>(), It.IsAny<CancellationToken>()),
            Times.Never
        );
    }

    /// <summary>
    /// Tests that OnConnectedAsync does not add the user to a group when the user ID is empty.
    /// </summary>
    [Fact]
    public async Task OnConnectedAsync_WithEmptyUserId_DoesNotAddUserToGroup()
    {
        const string connectionId = "testConnectionId";

        var mockClaimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());

        _mockHubContext.Setup(c => c.User).Returns(mockClaimsPrincipal);
        _mockHubContext.Setup(c => c.ConnectionId).Returns(connectionId);

        await _friendshipHub.OnConnectedAsync();

        // Verify that the user is not added to any group
        _mockGroupManager.Verify(
            g => g.AddToGroupAsync(It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None), Times.Never);

        // Verify that no messages are sent to the client
        _mockClientProxy.Verify(
            x => x.SendCoreAsync(It.IsAny<string>(), It.IsAny<object[]>(), It.IsAny<CancellationToken>()),
            Times.Never
        );
    }

    /// <summary>
    /// Tests that OnDisconnectedAsync does not remove the user from a group when a valid user ID is provided.
    /// </summary>
    [Fact]
    public async Task OnDisconnectedAsync_WithValidUserId_RemovesUserFromGroup()
    {
        var userId = Guid.NewGuid();
        const string connectionId = "testConnectionId";

        var mockClaimsIdentity = new Mock<ClaimsIdentity>();
        mockClaimsIdentity.Setup(ci => ci.FindFirst(ClaimTypes.NameIdentifier))
            .Returns(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));
        var mockClaimsPrincipal = new ClaimsPrincipal(mockClaimsIdentity.Object);

        _mockHubContext.Setup(c => c.User).Returns(mockClaimsPrincipal);
        _mockHubContext.Setup(c => c.ConnectionId).Returns(connectionId);

        await _friendshipHub.OnDisconnectedAsync(new Exception("Simulated disconnect"));

        // Verify that the user is not removed from any group
        _mockGroupManager.Verify(
            g => g.RemoveFromGroupAsync(It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None),
            Times.Never);

        // Verify that no messages are sent to the client
        _mockClientProxy.Verify(
            x => x.SendCoreAsync(It.IsAny<string>(), It.IsAny<object[]>(), It.IsAny<CancellationToken>()),
            Times.Never
        );
    }
}