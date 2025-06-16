using System;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UserAccountService.Controllers;
using UserAccountService.Models;
using UserAccountService.Models.DTOs;
using UserAccountService.Services;
using Xunit;

namespace UserAccountService.Tests.Controllers;

public class UsersControllerTest
{
    /// <summary>
    /// Tests that the GetMe endpoint returns an OkObjectResult with the correct UserDto
    /// when the user is authenticated and exists.
    /// </summary>
    [Fact]
    public async Task GetMe_ShouldReturnOkWithUserDto_WhenUserIsAuthenticated()
    {
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Email = "test@example.com",
            Tag = "TestUser",
            Name = "name",
            PasswordHash = "hashedpassword"
        };

        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity([
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        ]));
        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(s => s.GetUserByIdAsync(userId)).ReturnsAsync(user);

        var controller = new UsersController(userServiceMock.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = claimsPrincipal
                }
            }
        };

        var result = await controller.GetMe();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var userDto = Assert.IsType<UserDto>(okResult.Value);
        Assert.Equal(user.Id, userDto.Id);
        Assert.Equal(user.Email, userDto.Email);
        Assert.Equal(user.Tag, userDto.Tag);
    }

    /// <summary>
    /// Tests that the GetMe endpoint returns an UnauthorizedResult
    /// when the NameIdentifier claim is missing from the user's claims.
    /// </summary>
    [Fact]
    public async Task GetMe_ShouldReturnUnauthorized_WhenUserIdClaimIsMissing()
    {
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
        var userServiceMock = new Mock<IUserService>();
        var controller = new UsersController(userServiceMock.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = claimsPrincipal
                }
            }
        };

        var result = await controller.GetMe();
        Assert.IsType<UnauthorizedResult>(result);
    }

    /// <summary>
    /// Tests that the GetMe endpoint returns a NotFoundResult
    /// when the user ID claim is present but no user with that ID exists in the service.
    /// </summary>
    [Fact]
    public async Task GetMe_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        // Simulate an authenticated user with a NameIdentifier claim.
        var claimsPrincipal =
            new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, userId.ToString())]));
        var userServiceMock = new Mock<IUserService>();
        // Setup the mock service to return null when GetUserByIdAsync is called, simulating a non-existent user.
        userServiceMock.Setup(s => s.GetUserByIdAsync(userId)).ReturnsAsync((User)null);
        var controller = new UsersController(userServiceMock.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = claimsPrincipal
                }
            }
        };

        var result = await controller.GetMe();
        Assert.IsType<NotFoundResult>(result);
    }

    /// <summary>
    /// Tests that the GetUserById endpoint returns an OkObjectResult with the correct UserDto
    /// when a user with the specified ID exists.
    /// </summary>
    [Fact]
    public async Task GetUserById_ShouldReturnOkWithUserDto_WhenUserExists()
    {
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Email = "test@example.com",
            Tag = "TestUser",
            Name = "name",
            PasswordHash = "hashedpassword"
        };
        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(s => s.GetUserByIdAsync(userId)).ReturnsAsync(user);
        var controller = new UsersController(userServiceMock.Object);

        var result = await controller.GetUserById(userId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var userDto = Assert.IsType<UserDto>(okResult.Value);
        Assert.Equal(user.Id, userDto.Id);
        Assert.Equal(user.Email, userDto.Email);
        Assert.Equal(user.Tag, userDto.Tag);
    }

    /// <summary>
    /// Tests that the GetUserById endpoint returns a NotFoundResult
    /// when no user with the specified ID exists in the service.
    /// </summary>
    [Fact]
    public async Task GetUserById_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        var userId = Guid.NewGuid();
        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(s => s.GetUserByIdAsync(userId)).ReturnsAsync((User)null);
        var controller = new UsersController(userServiceMock.Object);

        var result = await controller.GetUserById(userId);
        Assert.IsType<NotFoundResult>(result);
    }

    /// <summary>
    /// Tests that the SearchUsers endpoint returns an OkObjectResult with a list of UserDtos
    /// when a valid search query is provided and matching users exist.
    /// </summary>
    [Fact]
    public async Task SearchUsers_ShouldReturnOkWithUserDtos_WhenQueryIsValid()
    {
        const string query = "Test";
        var users = new List<User>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Email = "test1@example.com",
                Tag = "TestUser1",
                Name = "name1",
                PasswordHash = "hashedpassword1"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Email = "test2@example.com",
                Tag = "TestUser2",
                Name = "name2",
                PasswordHash = "hashedpassword2"
            }
        };
        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(s => s.SearchUsersAsync(query)).ReturnsAsync(users);
        var controller = new UsersController(userServiceMock.Object);

        var result = await controller.SearchUsers(query);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var userDtos = Assert.IsType<List<UserDto>>(okResult.Value);
        Assert.Equal(users.Count, userDtos.Count);
        Assert.Equal(users[0].Id, userDtos[0].Id);
        Assert.Equal(users[1].Id, userDtos[1].Id);
    }

    /// <summary>
    /// Tests that the SearchUsers endpoint returns a BadRequestObjectResult
    /// when an empty search query is provided.
    /// </summary>
    [Fact]
    public async Task SearchUsers_ShouldReturnBadRequest_WhenQueryIsEmpty()
    {
        const string query = "";
        var userServiceMock = new Mock<IUserService>();
        var controller = new UsersController(userServiceMock.Object);

        var result = await controller.SearchUsers(query);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Search query cannot be empty.", badRequestResult.Value);
    }

    /// <summary>
    /// Tests that the SearchUsers endpoint returns an OkObjectResult with an empty list of UserDtos
    /// when a valid search query is provided but no users match the query.
    /// </summary>
    [Fact]
    public async Task SearchUsers_ShouldReturnOkWithEmptyList_WhenNoUsersMatchQuery()
    {
        const string query = "NonExistentUser";
        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(s => s.SearchUsersAsync(query)).ReturnsAsync(new List<User>());
        var controller = new UsersController(userServiceMock.Object);

        var result = await controller.SearchUsers(query);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var userDtos = Assert.IsType<List<UserDto>>(okResult.Value);
        Assert.Empty(userDtos);
    }
}