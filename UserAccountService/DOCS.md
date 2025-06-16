<a name='assembly'></a>
# UserAccountService

## Contents

- [AddUserFriendshipAndTokenModels](#T-UserAccountService-Migrations-AddUserFriendshipAndTokenModels 'UserAccountService.Migrations.AddUserFriendshipAndTokenModels')
  - [BuildTargetModel()](#M-UserAccountService-Migrations-AddUserFriendshipAndTokenModels-BuildTargetModel-Microsoft-EntityFrameworkCore-ModelBuilder- 'UserAccountService.Migrations.AddUserFriendshipAndTokenModels.BuildTargetModel(Microsoft.EntityFrameworkCore.ModelBuilder)')
  - [Down()](#M-UserAccountService-Migrations-AddUserFriendshipAndTokenModels-Down-Microsoft-EntityFrameworkCore-Migrations-MigrationBuilder- 'UserAccountService.Migrations.AddUserFriendshipAndTokenModels.Down(Microsoft.EntityFrameworkCore.Migrations.MigrationBuilder)')
  - [Up()](#M-UserAccountService-Migrations-AddUserFriendshipAndTokenModels-Up-Microsoft-EntityFrameworkCore-Migrations-MigrationBuilder- 'UserAccountService.Migrations.AddUserFriendshipAndTokenModels.Up(Microsoft.EntityFrameworkCore.Migrations.MigrationBuilder)')
- [AuthController](#T-UserAccountService-Controllers-AuthController 'UserAccountService.Controllers.AuthController')
  - [#ctor()](#M-UserAccountService-Controllers-AuthController-#ctor-UserAccountService-Services-IUserService,UserAccountService-Services-ITokenService,UserAccountService-Data-UserAccountDbContext- 'UserAccountService.Controllers.AuthController.#ctor(UserAccountService.Services.IUserService,UserAccountService.Services.ITokenService,UserAccountService.Data.UserAccountDbContext)')
  - [Login(request)](#M-UserAccountService-Controllers-AuthController-Login-UserAccountService-Models-DTOs-LoginRequestDto- 'UserAccountService.Controllers.AuthController.Login(UserAccountService.Models.DTOs.LoginRequestDto)')
  - [Logout(request)](#M-UserAccountService-Controllers-AuthController-Logout-UserAccountService-Models-DTOs-RefreshRequestDto- 'UserAccountService.Controllers.AuthController.Logout(UserAccountService.Models.DTOs.RefreshRequestDto)')
  - [Refresh(request)](#M-UserAccountService-Controllers-AuthController-Refresh-UserAccountService-Models-DTOs-RefreshRequestDto- 'UserAccountService.Controllers.AuthController.Refresh(UserAccountService.Models.DTOs.RefreshRequestDto)')
  - [Register(request)](#M-UserAccountService-Controllers-AuthController-Register-UserAccountService-Models-DTOs-RegisterRequestDto- 'UserAccountService.Controllers.AuthController.Register(UserAccountService.Models.DTOs.RegisterRequestDto)')
- [FriendDto](#T-UserAccountService-Models-DTOs-FriendDto 'UserAccountService.Models.DTOs.FriendDto')
  - [BecameFriendsAt](#P-UserAccountService-Models-DTOs-FriendDto-BecameFriendsAt 'UserAccountService.Models.DTOs.FriendDto.BecameFriendsAt')
  - [FriendshipId](#P-UserAccountService-Models-DTOs-FriendDto-FriendshipId 'UserAccountService.Models.DTOs.FriendDto.FriendshipId')
  - [Id](#P-UserAccountService-Models-DTOs-FriendDto-Id 'UserAccountService.Models.DTOs.FriendDto.Id')
  - [Name](#P-UserAccountService-Models-DTOs-FriendDto-Name 'UserAccountService.Models.DTOs.FriendDto.Name')
  - [Tag](#P-UserAccountService-Models-DTOs-FriendDto-Tag 'UserAccountService.Models.DTOs.FriendDto.Tag')
- [FriendRequestDto](#T-UserAccountService-Models-DTOs-FriendRequestDto 'UserAccountService.Models.DTOs.FriendRequestDto')
  - [AddresseeId](#P-UserAccountService-Models-DTOs-FriendRequestDto-AddresseeId 'UserAccountService.Models.DTOs.FriendRequestDto.AddresseeId')
  - [AddresseeName](#P-UserAccountService-Models-DTOs-FriendRequestDto-AddresseeName 'UserAccountService.Models.DTOs.FriendRequestDto.AddresseeName')
  - [AddresseeTag](#P-UserAccountService-Models-DTOs-FriendRequestDto-AddresseeTag 'UserAccountService.Models.DTOs.FriendRequestDto.AddresseeTag')
  - [Id](#P-UserAccountService-Models-DTOs-FriendRequestDto-Id 'UserAccountService.Models.DTOs.FriendRequestDto.Id')
  - [RequestedAt](#P-UserAccountService-Models-DTOs-FriendRequestDto-RequestedAt 'UserAccountService.Models.DTOs.FriendRequestDto.RequestedAt')
  - [RequesterId](#P-UserAccountService-Models-DTOs-FriendRequestDto-RequesterId 'UserAccountService.Models.DTOs.FriendRequestDto.RequesterId')
  - [RequesterName](#P-UserAccountService-Models-DTOs-FriendRequestDto-RequesterName 'UserAccountService.Models.DTOs.FriendRequestDto.RequesterName')
  - [RequesterTag](#P-UserAccountService-Models-DTOs-FriendRequestDto-RequesterTag 'UserAccountService.Models.DTOs.FriendRequestDto.RequesterTag')
- [FriendsController](#T-UserAccountService-Controllers-FriendsController 'UserAccountService.Controllers.FriendsController')
  - [#ctor()](#M-UserAccountService-Controllers-FriendsController-#ctor-UserAccountService-Services-IFriendshipService,Microsoft-Extensions-Logging-ILogger{UserAccountService-Controllers-FriendsController}- 'UserAccountService.Controllers.FriendsController.#ctor(UserAccountService.Services.IFriendshipService,Microsoft.Extensions.Logging.ILogger{UserAccountService.Controllers.FriendsController})')
  - [AcceptFriendRequest(friendshipId)](#M-UserAccountService-Controllers-FriendsController-AcceptFriendRequest-System-Guid- 'UserAccountService.Controllers.FriendsController.AcceptFriendRequest(System.Guid)')
  - [DeclineFriendRequest(friendshipId)](#M-UserAccountService-Controllers-FriendsController-DeclineFriendRequest-System-Guid- 'UserAccountService.Controllers.FriendsController.DeclineFriendRequest(System.Guid)')
  - [GetCurrentUserId()](#M-UserAccountService-Controllers-FriendsController-GetCurrentUserId 'UserAccountService.Controllers.FriendsController.GetCurrentUserId')
  - [GetFriends()](#M-UserAccountService-Controllers-FriendsController-GetFriends 'UserAccountService.Controllers.FriendsController.GetFriends')
  - [GetFriendshipStatus(otherUserId)](#M-UserAccountService-Controllers-FriendsController-GetFriendshipStatus-System-Guid- 'UserAccountService.Controllers.FriendsController.GetFriendshipStatus(System.Guid)')
  - [GetPendingIncomingRequests()](#M-UserAccountService-Controllers-FriendsController-GetPendingIncomingRequests 'UserAccountService.Controllers.FriendsController.GetPendingIncomingRequests')
  - [GetPendingOutgoingRequests()](#M-UserAccountService-Controllers-FriendsController-GetPendingOutgoingRequests 'UserAccountService.Controllers.FriendsController.GetPendingOutgoingRequests')
  - [RemoveFriend(friendId)](#M-UserAccountService-Controllers-FriendsController-RemoveFriend-System-Guid- 'UserAccountService.Controllers.FriendsController.RemoveFriend(System.Guid)')
  - [SendFriendRequest(addresseeId)](#M-UserAccountService-Controllers-FriendsController-SendFriendRequest-System-Guid- 'UserAccountService.Controllers.FriendsController.SendFriendRequest(System.Guid)')
- [Friendship](#T-UserAccountService-Models-Friendship 'UserAccountService.Models.Friendship')
  - [Addressee](#P-UserAccountService-Models-Friendship-Addressee 'UserAccountService.Models.Friendship.Addressee')
  - [AddresseeId](#P-UserAccountService-Models-Friendship-AddresseeId 'UserAccountService.Models.Friendship.AddresseeId')
  - [Id](#P-UserAccountService-Models-Friendship-Id 'UserAccountService.Models.Friendship.Id')
  - [RequestedAt](#P-UserAccountService-Models-Friendship-RequestedAt 'UserAccountService.Models.Friendship.RequestedAt')
  - [Requester](#P-UserAccountService-Models-Friendship-Requester 'UserAccountService.Models.Friendship.Requester')
  - [RequesterId](#P-UserAccountService-Models-Friendship-RequesterId 'UserAccountService.Models.Friendship.RequesterId')
  - [RespondedAt](#P-UserAccountService-Models-Friendship-RespondedAt 'UserAccountService.Models.Friendship.RespondedAt')
  - [Status](#P-UserAccountService-Models-Friendship-Status 'UserAccountService.Models.Friendship.Status')
- [FriendshipHub](#T-UserAccountService-Hubs-FriendshipHub 'UserAccountService.Hubs.FriendshipHub')
  - [GetCurrentUserId()](#M-UserAccountService-Hubs-FriendshipHub-GetCurrentUserId 'UserAccountService.Hubs.FriendshipHub.GetCurrentUserId')
  - [OnConnectedAsync()](#M-UserAccountService-Hubs-FriendshipHub-OnConnectedAsync 'UserAccountService.Hubs.FriendshipHub.OnConnectedAsync')
- [FriendshipResultDto](#T-UserAccountService-Models-DTOs-FriendshipResultDto 'UserAccountService.Models.DTOs.FriendshipResultDto')
  - [FriendshipId](#P-UserAccountService-Models-DTOs-FriendshipResultDto-FriendshipId 'UserAccountService.Models.DTOs.FriendshipResultDto.FriendshipId')
  - [Message](#P-UserAccountService-Models-DTOs-FriendshipResultDto-Message 'UserAccountService.Models.DTOs.FriendshipResultDto.Message')
  - [Status](#P-UserAccountService-Models-DTOs-FriendshipResultDto-Status 'UserAccountService.Models.DTOs.FriendshipResultDto.Status')
  - [Success](#P-UserAccountService-Models-DTOs-FriendshipResultDto-Success 'UserAccountService.Models.DTOs.FriendshipResultDto.Success')
- [FriendshipService](#T-UserAccountService-Services-FriendshipService 'UserAccountService.Services.FriendshipService')
  - [#ctor()](#M-UserAccountService-Services-FriendshipService-#ctor-UserAccountService-Data-UserAccountDbContext,Microsoft-Extensions-Logging-ILogger{UserAccountService-Services-FriendshipService},Microsoft-AspNetCore-SignalR-IHubContext{UserAccountService-Hubs-FriendshipHub}- 'UserAccountService.Services.FriendshipService.#ctor(UserAccountService.Data.UserAccountDbContext,Microsoft.Extensions.Logging.ILogger{UserAccountService.Services.FriendshipService},Microsoft.AspNetCore.SignalR.IHubContext{UserAccountService.Hubs.FriendshipHub})')
  - [AcceptFriendRequestAsync(friendshipId,currentUserId)](#M-UserAccountService-Services-FriendshipService-AcceptFriendRequestAsync-System-Guid,System-Guid- 'UserAccountService.Services.FriendshipService.AcceptFriendRequestAsync(System.Guid,System.Guid)')
  - [DeclineFriendRequestAsync(friendshipId,currentUserId)](#M-UserAccountService-Services-FriendshipService-DeclineFriendRequestAsync-System-Guid,System-Guid- 'UserAccountService.Services.FriendshipService.DeclineFriendRequestAsync(System.Guid,System.Guid)')
  - [GetFriendsAsync(userId)](#M-UserAccountService-Services-FriendshipService-GetFriendsAsync-System-Guid- 'UserAccountService.Services.FriendshipService.GetFriendsAsync(System.Guid)')
  - [GetFriendshipStatusAsync(userId1,userId2)](#M-UserAccountService-Services-FriendshipService-GetFriendshipStatusAsync-System-Guid,System-Guid- 'UserAccountService.Services.FriendshipService.GetFriendshipStatusAsync(System.Guid,System.Guid)')
  - [GetPendingIncomingRequestsAsync(userId)](#M-UserAccountService-Services-FriendshipService-GetPendingIncomingRequestsAsync-System-Guid- 'UserAccountService.Services.FriendshipService.GetPendingIncomingRequestsAsync(System.Guid)')
  - [GetPendingOutgoingRequestsAsync(userId)](#M-UserAccountService-Services-FriendshipService-GetPendingOutgoingRequestsAsync-System-Guid- 'UserAccountService.Services.FriendshipService.GetPendingOutgoingRequestsAsync(System.Guid)')
  - [RemoveFriendAsync(userId,friendId)](#M-UserAccountService-Services-FriendshipService-RemoveFriendAsync-System-Guid,System-Guid- 'UserAccountService.Services.FriendshipService.RemoveFriendAsync(System.Guid,System.Guid)')
  - [SendFriendRequestAsync(requesterId,addresseeId)](#M-UserAccountService-Services-FriendshipService-SendFriendRequestAsync-System-Guid,System-Guid- 'UserAccountService.Services.FriendshipService.SendFriendRequestAsync(System.Guid,System.Guid)')
- [FriendshipStatus](#T-UserAccountService-Models-FriendshipStatus 'UserAccountService.Models.FriendshipStatus')
  - [Accepted](#F-UserAccountService-Models-FriendshipStatus-Accepted 'UserAccountService.Models.FriendshipStatus.Accepted')
  - [Blocked](#F-UserAccountService-Models-FriendshipStatus-Blocked 'UserAccountService.Models.FriendshipStatus.Blocked')
  - [Declined](#F-UserAccountService-Models-FriendshipStatus-Declined 'UserAccountService.Models.FriendshipStatus.Declined')
  - [Pending](#F-UserAccountService-Models-FriendshipStatus-Pending 'UserAccountService.Models.FriendshipStatus.Pending')
- [IFriendshipService](#T-UserAccountService-Services-IFriendshipService 'UserAccountService.Services.IFriendshipService')
  - [AcceptFriendRequestAsync(friendshipId,currentUserId)](#M-UserAccountService-Services-IFriendshipService-AcceptFriendRequestAsync-System-Guid,System-Guid- 'UserAccountService.Services.IFriendshipService.AcceptFriendRequestAsync(System.Guid,System.Guid)')
  - [DeclineFriendRequestAsync(friendshipId,currentUserId)](#M-UserAccountService-Services-IFriendshipService-DeclineFriendRequestAsync-System-Guid,System-Guid- 'UserAccountService.Services.IFriendshipService.DeclineFriendRequestAsync(System.Guid,System.Guid)')
  - [GetFriendsAsync(userId)](#M-UserAccountService-Services-IFriendshipService-GetFriendsAsync-System-Guid- 'UserAccountService.Services.IFriendshipService.GetFriendsAsync(System.Guid)')
  - [GetFriendshipStatusAsync(userId1,userId2)](#M-UserAccountService-Services-IFriendshipService-GetFriendshipStatusAsync-System-Guid,System-Guid- 'UserAccountService.Services.IFriendshipService.GetFriendshipStatusAsync(System.Guid,System.Guid)')
  - [GetPendingIncomingRequestsAsync(userId)](#M-UserAccountService-Services-IFriendshipService-GetPendingIncomingRequestsAsync-System-Guid- 'UserAccountService.Services.IFriendshipService.GetPendingIncomingRequestsAsync(System.Guid)')
  - [GetPendingOutgoingRequestsAsync(userId)](#M-UserAccountService-Services-IFriendshipService-GetPendingOutgoingRequestsAsync-System-Guid- 'UserAccountService.Services.IFriendshipService.GetPendingOutgoingRequestsAsync(System.Guid)')
  - [RemoveFriendAsync(userId,friendId)](#M-UserAccountService-Services-IFriendshipService-RemoveFriendAsync-System-Guid,System-Guid- 'UserAccountService.Services.IFriendshipService.RemoveFriendAsync(System.Guid,System.Guid)')
  - [SendFriendRequestAsync(requesterId,addresseeId)](#M-UserAccountService-Services-IFriendshipService-SendFriendRequestAsync-System-Guid,System-Guid- 'UserAccountService.Services.IFriendshipService.SendFriendRequestAsync(System.Guid,System.Guid)')
- [ITokenService](#T-UserAccountService-Services-ITokenService 'UserAccountService.Services.ITokenService')
  - [GenerateTokens(user)](#M-UserAccountService-Services-ITokenService-GenerateTokens-UserAccountService-Models-User- 'UserAccountService.Services.ITokenService.GenerateTokens(UserAccountService.Models.User)')
  - [InvalidateRefreshTokenAsync(refreshTokenString)](#M-UserAccountService-Services-ITokenService-InvalidateRefreshTokenAsync-System-String- 'UserAccountService.Services.ITokenService.InvalidateRefreshTokenAsync(System.String)')
  - [RefreshAccessTokenAsync(oldRefreshTokenString)](#M-UserAccountService-Services-ITokenService-RefreshAccessTokenAsync-System-String- 'UserAccountService.Services.ITokenService.RefreshAccessTokenAsync(System.String)')
- [IUserService](#T-UserAccountService-Services-IUserService 'UserAccountService.Services.IUserService')
  - [AuthenticateUserAsync(tagOrEmail,password)](#M-UserAccountService-Services-IUserService-AuthenticateUserAsync-System-String,System-String- 'UserAccountService.Services.IUserService.AuthenticateUserAsync(System.String,System.String)')
  - [GetUserByIdAsync(userId)](#M-UserAccountService-Services-IUserService-GetUserByIdAsync-System-Guid- 'UserAccountService.Services.IUserService.GetUserByIdAsync(System.Guid)')
  - [RegisterUserAsync(name,tag,email,password)](#M-UserAccountService-Services-IUserService-RegisterUserAsync-System-String,System-String,System-String,System-String- 'UserAccountService.Services.IUserService.RegisterUserAsync(System.String,System.String,System.String,System.String)')
  - [SearchUsersAsync(query)](#M-UserAccountService-Services-IUserService-SearchUsersAsync-System-String- 'UserAccountService.Services.IUserService.SearchUsersAsync(System.String)')
- [InitialCreate](#T-UserAccountService-Migrations-InitialCreate 'UserAccountService.Migrations.InitialCreate')
  - [BuildTargetModel()](#M-UserAccountService-Migrations-InitialCreate-BuildTargetModel-Microsoft-EntityFrameworkCore-ModelBuilder- 'UserAccountService.Migrations.InitialCreate.BuildTargetModel(Microsoft.EntityFrameworkCore.ModelBuilder)')
  - [Down()](#M-UserAccountService-Migrations-InitialCreate-Down-Microsoft-EntityFrameworkCore-Migrations-MigrationBuilder- 'UserAccountService.Migrations.InitialCreate.Down(Microsoft.EntityFrameworkCore.Migrations.MigrationBuilder)')
  - [Up()](#M-UserAccountService-Migrations-InitialCreate-Up-Microsoft-EntityFrameworkCore-Migrations-MigrationBuilder- 'UserAccountService.Migrations.InitialCreate.Up(Microsoft.EntityFrameworkCore.Migrations.MigrationBuilder)')
- [JwtSettings](#T-UserAccountService-Models-Configuration-JwtSettings 'UserAccountService.Models.Configuration.JwtSettings')
  - [Audience](#P-UserAccountService-Models-Configuration-JwtSettings-Audience 'UserAccountService.Models.Configuration.JwtSettings.Audience')
  - [ExpiryMinutes](#P-UserAccountService-Models-Configuration-JwtSettings-ExpiryMinutes 'UserAccountService.Models.Configuration.JwtSettings.ExpiryMinutes')
  - [Issuer](#P-UserAccountService-Models-Configuration-JwtSettings-Issuer 'UserAccountService.Models.Configuration.JwtSettings.Issuer')
  - [RefreshTokenExpiryDays](#P-UserAccountService-Models-Configuration-JwtSettings-RefreshTokenExpiryDays 'UserAccountService.Models.Configuration.JwtSettings.RefreshTokenExpiryDays')
  - [Secret](#P-UserAccountService-Models-Configuration-JwtSettings-Secret 'UserAccountService.Models.Configuration.JwtSettings.Secret')
- [LoginRequestDto](#T-UserAccountService-Models-DTOs-LoginRequestDto 'UserAccountService.Models.DTOs.LoginRequestDto')
  - [Password](#P-UserAccountService-Models-DTOs-LoginRequestDto-Password 'UserAccountService.Models.DTOs.LoginRequestDto.Password')
  - [TagOrEmail](#P-UserAccountService-Models-DTOs-LoginRequestDto-TagOrEmail 'UserAccountService.Models.DTOs.LoginRequestDto.TagOrEmail')
- [LoginResponseDto](#T-UserAccountService-Models-DTOs-LoginResponseDto 'UserAccountService.Models.DTOs.LoginResponseDto')
  - [AccessToken](#P-UserAccountService-Models-DTOs-LoginResponseDto-AccessToken 'UserAccountService.Models.DTOs.LoginResponseDto.AccessToken')
  - [RefreshToken](#P-UserAccountService-Models-DTOs-LoginResponseDto-RefreshToken 'UserAccountService.Models.DTOs.LoginResponseDto.RefreshToken')
- [RefreshRequestDto](#T-UserAccountService-Models-DTOs-RefreshRequestDto 'UserAccountService.Models.DTOs.RefreshRequestDto')
  - [RefreshToken](#P-UserAccountService-Models-DTOs-RefreshRequestDto-RefreshToken 'UserAccountService.Models.DTOs.RefreshRequestDto.RefreshToken')
- [RefreshToken](#T-UserAccountService-Models-RefreshToken 'UserAccountService.Models.RefreshToken')
  - [CreatedAt](#P-UserAccountService-Models-RefreshToken-CreatedAt 'UserAccountService.Models.RefreshToken.CreatedAt')
  - [ExpiryDate](#P-UserAccountService-Models-RefreshToken-ExpiryDate 'UserAccountService.Models.RefreshToken.ExpiryDate')
  - [Invalidated](#P-UserAccountService-Models-RefreshToken-Invalidated 'UserAccountService.Models.RefreshToken.Invalidated')
  - [JwtId](#P-UserAccountService-Models-RefreshToken-JwtId 'UserAccountService.Models.RefreshToken.JwtId')
  - [Token](#P-UserAccountService-Models-RefreshToken-Token 'UserAccountService.Models.RefreshToken.Token')
  - [Used](#P-UserAccountService-Models-RefreshToken-Used 'UserAccountService.Models.RefreshToken.Used')
  - [User](#P-UserAccountService-Models-RefreshToken-User 'UserAccountService.Models.RefreshToken.User')
  - [UserId](#P-UserAccountService-Models-RefreshToken-UserId 'UserAccountService.Models.RefreshToken.UserId')
- [RegisterRequestDto](#T-UserAccountService-Models-DTOs-RegisterRequestDto 'UserAccountService.Models.DTOs.RegisterRequestDto')
  - [Email](#P-UserAccountService-Models-DTOs-RegisterRequestDto-Email 'UserAccountService.Models.DTOs.RegisterRequestDto.Email')
  - [Name](#P-UserAccountService-Models-DTOs-RegisterRequestDto-Name 'UserAccountService.Models.DTOs.RegisterRequestDto.Name')
  - [Password](#P-UserAccountService-Models-DTOs-RegisterRequestDto-Password 'UserAccountService.Models.DTOs.RegisterRequestDto.Password')
  - [Tag](#P-UserAccountService-Models-DTOs-RegisterRequestDto-Tag 'UserAccountService.Models.DTOs.RegisterRequestDto.Tag')
- [TokenService](#T-UserAccountService-Services-TokenService 'UserAccountService.Services.TokenService')
  - [#ctor()](#M-UserAccountService-Services-TokenService-#ctor-UserAccountService-Data-UserAccountDbContext,Microsoft-Extensions-Options-IOptions{UserAccountService-Models-Configuration-JwtSettings},Microsoft-Extensions-Logging-ILogger{UserAccountService-Services-TokenService}- 'UserAccountService.Services.TokenService.#ctor(UserAccountService.Data.UserAccountDbContext,Microsoft.Extensions.Options.IOptions{UserAccountService.Models.Configuration.JwtSettings},Microsoft.Extensions.Logging.ILogger{UserAccountService.Services.TokenService})')
  - [GenerateTokens(user)](#M-UserAccountService-Services-TokenService-GenerateTokens-UserAccountService-Models-User- 'UserAccountService.Services.TokenService.GenerateTokens(UserAccountService.Models.User)')
  - [InvalidateRefreshTokenAsync(refreshTokenString)](#M-UserAccountService-Services-TokenService-InvalidateRefreshTokenAsync-System-String- 'UserAccountService.Services.TokenService.InvalidateRefreshTokenAsync(System.String)')
  - [RefreshAccessTokenAsync(oldRefreshTokenString)](#M-UserAccountService-Services-TokenService-RefreshAccessTokenAsync-System-String- 'UserAccountService.Services.TokenService.RefreshAccessTokenAsync(System.String)')
- [User](#T-UserAccountService-Models-User 'UserAccountService.Models.User')
  - [CreatedAt](#P-UserAccountService-Models-User-CreatedAt 'UserAccountService.Models.User.CreatedAt')
  - [Email](#P-UserAccountService-Models-User-Email 'UserAccountService.Models.User.Email')
  - [Friendships1](#P-UserAccountService-Models-User-Friendships1 'UserAccountService.Models.User.Friendships1')
  - [Friendships2](#P-UserAccountService-Models-User-Friendships2 'UserAccountService.Models.User.Friendships2')
  - [Id](#P-UserAccountService-Models-User-Id 'UserAccountService.Models.User.Id')
  - [Name](#P-UserAccountService-Models-User-Name 'UserAccountService.Models.User.Name')
  - [PasswordHash](#P-UserAccountService-Models-User-PasswordHash 'UserAccountService.Models.User.PasswordHash')
  - [RefreshTokens](#P-UserAccountService-Models-User-RefreshTokens 'UserAccountService.Models.User.RefreshTokens')
  - [Tag](#P-UserAccountService-Models-User-Tag 'UserAccountService.Models.User.Tag')
- [UserAccountDbContext](#T-UserAccountService-Data-UserAccountDbContext 'UserAccountService.Data.UserAccountDbContext')
  - [#ctor()](#M-UserAccountService-Data-UserAccountDbContext-#ctor-Microsoft-EntityFrameworkCore-DbContextOptions{UserAccountService-Data-UserAccountDbContext}- 'UserAccountService.Data.UserAccountDbContext.#ctor(Microsoft.EntityFrameworkCore.DbContextOptions{UserAccountService.Data.UserAccountDbContext})')
  - [Friendships](#P-UserAccountService-Data-UserAccountDbContext-Friendships 'UserAccountService.Data.UserAccountDbContext.Friendships')
  - [RefreshTokens](#P-UserAccountService-Data-UserAccountDbContext-RefreshTokens 'UserAccountService.Data.UserAccountDbContext.RefreshTokens')
  - [Users](#P-UserAccountService-Data-UserAccountDbContext-Users 'UserAccountService.Data.UserAccountDbContext.Users')
  - [OnModelCreating(modelBuilder)](#M-UserAccountService-Data-UserAccountDbContext-OnModelCreating-Microsoft-EntityFrameworkCore-ModelBuilder- 'UserAccountService.Data.UserAccountDbContext.OnModelCreating(Microsoft.EntityFrameworkCore.ModelBuilder)')
- [UserDto](#T-UserAccountService-Models-DTOs-UserDto 'UserAccountService.Models.DTOs.UserDto')
  - [#ctor(user)](#M-UserAccountService-Models-DTOs-UserDto-#ctor-UserAccountService-Models-User- 'UserAccountService.Models.DTOs.UserDto.#ctor(UserAccountService.Models.User)')
  - [#ctor()](#M-UserAccountService-Models-DTOs-UserDto-#ctor 'UserAccountService.Models.DTOs.UserDto.#ctor')
  - [CreatedAt](#P-UserAccountService-Models-DTOs-UserDto-CreatedAt 'UserAccountService.Models.DTOs.UserDto.CreatedAt')
  - [Email](#P-UserAccountService-Models-DTOs-UserDto-Email 'UserAccountService.Models.DTOs.UserDto.Email')
  - [Id](#P-UserAccountService-Models-DTOs-UserDto-Id 'UserAccountService.Models.DTOs.UserDto.Id')
  - [Name](#P-UserAccountService-Models-DTOs-UserDto-Name 'UserAccountService.Models.DTOs.UserDto.Name')
  - [Tag](#P-UserAccountService-Models-DTOs-UserDto-Tag 'UserAccountService.Models.DTOs.UserDto.Tag')
- [UserService](#T-UserAccountService-Services-UserService 'UserAccountService.Services.UserService')
  - [#ctor()](#M-UserAccountService-Services-UserService-#ctor-UserAccountService-Data-UserAccountDbContext,Microsoft-Extensions-Logging-ILogger{UserAccountService-Services-UserService}- 'UserAccountService.Services.UserService.#ctor(UserAccountService.Data.UserAccountDbContext,Microsoft.Extensions.Logging.ILogger{UserAccountService.Services.UserService})')
  - [AuthenticateUserAsync(tagOrEmail,password)](#M-UserAccountService-Services-UserService-AuthenticateUserAsync-System-String,System-String- 'UserAccountService.Services.UserService.AuthenticateUserAsync(System.String,System.String)')
  - [GetUserByIdAsync(userId)](#M-UserAccountService-Services-UserService-GetUserByIdAsync-System-Guid- 'UserAccountService.Services.UserService.GetUserByIdAsync(System.Guid)')
  - [RegisterUserAsync(name,tag,email,password)](#M-UserAccountService-Services-UserService-RegisterUserAsync-System-String,System-String,System-String,System-String- 'UserAccountService.Services.UserService.RegisterUserAsync(System.String,System.String,System.String,System.String)')
  - [SearchUsersAsync(query)](#M-UserAccountService-Services-UserService-SearchUsersAsync-System-String- 'UserAccountService.Services.UserService.SearchUsersAsync(System.String)')
- [UsersController](#T-UserAccountService-Controllers-UsersController 'UserAccountService.Controllers.UsersController')
  - [#ctor()](#M-UserAccountService-Controllers-UsersController-#ctor-UserAccountService-Services-IUserService- 'UserAccountService.Controllers.UsersController.#ctor(UserAccountService.Services.IUserService)')
  - [GetMe()](#M-UserAccountService-Controllers-UsersController-GetMe 'UserAccountService.Controllers.UsersController.GetMe')
  - [GetUserById(id)](#M-UserAccountService-Controllers-UsersController-GetUserById-System-Guid- 'UserAccountService.Controllers.UsersController.GetUserById(System.Guid)')
  - [SearchUsers(query)](#M-UserAccountService-Controllers-UsersController-SearchUsers-System-String- 'UserAccountService.Controllers.UsersController.SearchUsers(System.String)')

<a name='T-UserAccountService-Migrations-AddUserFriendshipAndTokenModels'></a>
## AddUserFriendshipAndTokenModels `type`

##### Namespace

UserAccountService.Migrations

##### Summary

*Inherit from parent.*

<a name='M-UserAccountService-Migrations-AddUserFriendshipAndTokenModels-BuildTargetModel-Microsoft-EntityFrameworkCore-ModelBuilder-'></a>
### BuildTargetModel() `method`

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Migrations-AddUserFriendshipAndTokenModels-Down-Microsoft-EntityFrameworkCore-Migrations-MigrationBuilder-'></a>
### Down() `method`

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Migrations-AddUserFriendshipAndTokenModels-Up-Microsoft-EntityFrameworkCore-Migrations-MigrationBuilder-'></a>
### Up() `method`

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.

<a name='T-UserAccountService-Controllers-AuthController'></a>
## AuthController `type`

##### Namespace

UserAccountService.Controllers

##### Summary

Handles authentication-related operations such as user registration, login, token refresh, and logout.

<a name='M-UserAccountService-Controllers-AuthController-#ctor-UserAccountService-Services-IUserService,UserAccountService-Services-ITokenService,UserAccountService-Data-UserAccountDbContext-'></a>
### #ctor() `constructor`

##### Summary

Handles authentication-related operations such as user registration, login, token refresh, and logout.

##### Parameters

This constructor has no parameters.

<a name='M-UserAccountService-Controllers-AuthController-Login-UserAccountService-Models-DTOs-LoginRequestDto-'></a>
### Login(request) `method`

##### Summary

Authenticates a user and generates access and refresh tokens.

##### Returns

An [IActionResult](#T-Microsoft-AspNetCore-Mvc-IActionResult 'Microsoft.AspNetCore.Mvc.IActionResult') containing the generated tokens or an error message.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| request | [UserAccountService.Models.DTOs.LoginRequestDto](#T-UserAccountService-Models-DTOs-LoginRequestDto 'UserAccountService.Models.DTOs.LoginRequestDto') | The login credentials provided by the user. |

<a name='M-UserAccountService-Controllers-AuthController-Logout-UserAccountService-Models-DTOs-RefreshRequestDto-'></a>
### Logout(request) `method`

##### Summary

Logs out the user by invalidating the provided refresh token.

##### Returns

An [IActionResult](#T-Microsoft-AspNetCore-Mvc-IActionResult 'Microsoft.AspNetCore.Mvc.IActionResult') indicating the result of the logout operation.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| request | [UserAccountService.Models.DTOs.RefreshRequestDto](#T-UserAccountService-Models-DTOs-RefreshRequestDto 'UserAccountService.Models.DTOs.RefreshRequestDto') | The refresh token to invalidate. |

<a name='M-UserAccountService-Controllers-AuthController-Refresh-UserAccountService-Models-DTOs-RefreshRequestDto-'></a>
### Refresh(request) `method`

##### Summary

Refreshes the access token using a valid refresh token.

##### Returns

An [IActionResult](#T-Microsoft-AspNetCore-Mvc-IActionResult 'Microsoft.AspNetCore.Mvc.IActionResult') containing the new tokens or an error message.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| request | [UserAccountService.Models.DTOs.RefreshRequestDto](#T-UserAccountService-Models-DTOs-RefreshRequestDto 'UserAccountService.Models.DTOs.RefreshRequestDto') | The refresh token provided by the user. |

<a name='M-UserAccountService-Controllers-AuthController-Register-UserAccountService-Models-DTOs-RegisterRequestDto-'></a>
### Register(request) `method`

##### Summary

Registers a new user in the system.

##### Returns

An [IActionResult](#T-Microsoft-AspNetCore-Mvc-IActionResult 'Microsoft.AspNetCore.Mvc.IActionResult') indicating the result of the registration operation.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| request | [UserAccountService.Models.DTOs.RegisterRequestDto](#T-UserAccountService-Models-DTOs-RegisterRequestDto 'UserAccountService.Models.DTOs.RegisterRequestDto') | The registration details provided by the user. |

<a name='T-UserAccountService-Models-DTOs-FriendDto'></a>
## FriendDto `type`

##### Namespace

UserAccountService.Models.DTOs

##### Summary

Represents a friend entity in the system.

<a name='P-UserAccountService-Models-DTOs-FriendDto-BecameFriendsAt'></a>
### BecameFriendsAt `property`

##### Summary

The date and time when the friendship was established.

##### Example

2023-10-01T12:34:56Z

<a name='P-UserAccountService-Models-DTOs-FriendDto-FriendshipId'></a>
### FriendshipId `property`

##### Summary

The unique identifier of the friendship associated with the friend.

##### Example

3f2504e0-4f89-11d3-9a0c-0305e82c3301

<a name='P-UserAccountService-Models-DTOs-FriendDto-Id'></a>
### Id `property`

##### Summary

The unique identifier of the friend.

##### Example

3f2504e0-4f89-11d3-9a0c-0305e82c3301

<a name='P-UserAccountService-Models-DTOs-FriendDto-Name'></a>
### Name `property`

##### Summary

The name of the friend.

##### Example

John Doe

##### Remarks

This field contains the display name of the friend.

<a name='P-UserAccountService-Models-DTOs-FriendDto-Tag'></a>
### Tag `property`

##### Summary

The tag of the friend, in the format 'username#1234'.

##### Example

john.doe#1234

##### Remarks

The tag uniquely identifies the friend within the system.

<a name='T-UserAccountService-Models-DTOs-FriendRequestDto'></a>
## FriendRequestDto `type`

##### Namespace

UserAccountService.Models.DTOs

##### Summary

Represents a request to send a friend request.

<a name='P-UserAccountService-Models-DTOs-FriendRequestDto-AddresseeId'></a>
### AddresseeId `property`

##### Summary

The unique identifier of the user receiving the friend request.

##### Example

3f2504e0-4f89-11d3-9a0c-0305e82c3301

##### Remarks

This field is optional and may be null if the addressee is not identified.

<a name='P-UserAccountService-Models-DTOs-FriendRequestDto-AddresseeName'></a>
### AddresseeName `property`

##### Summary

The name of the user receiving the friend request.

##### Example

jane.doe

##### Remarks

This field is optional and may be null if the addressee name is not provided.

<a name='P-UserAccountService-Models-DTOs-FriendRequestDto-AddresseeTag'></a>
### AddresseeTag `property`

##### Summary

The tag of the user receiving the friend request, in the format 'username#1234'.

##### Example

jane.doe#1234

##### Remarks

This field is optional and may be null if the addressee tag is not provided.

<a name='P-UserAccountService-Models-DTOs-FriendRequestDto-Id'></a>
### Id `property`

##### Summary

The unique identifier of the friend request.

##### Example

3f2504e0-4f89-11d3-9a0c-0305e82c3301

<a name='P-UserAccountService-Models-DTOs-FriendRequestDto-RequestedAt'></a>
### RequestedAt `property`

##### Summary

The date and time when the friend request was sent.

##### Example

2023-10-01T12:34:56Z

<a name='P-UserAccountService-Models-DTOs-FriendRequestDto-RequesterId'></a>
### RequesterId `property`

##### Summary

The unique identifier of the user sending the friend request.

##### Example

3f2504e0-4f89-11d3-9a0c-0305e82c3301

##### Remarks

This field is optional and may be null if the requester is not identified.

<a name='P-UserAccountService-Models-DTOs-FriendRequestDto-RequesterName'></a>
### RequesterName `property`

##### Summary

The name of the user sending the friend request.

##### Example

john.doe

##### Remarks

This field is optional and may be null if the requester name is not provided.

<a name='P-UserAccountService-Models-DTOs-FriendRequestDto-RequesterTag'></a>
### RequesterTag `property`

##### Summary

The tag of the user sending the friend request, in the format 'username#1234'.

##### Example

john.doe#1234

##### Remarks

This field is optional and may be null if the requester tag is not provided.

<a name='T-UserAccountService-Controllers-FriendsController'></a>
## FriendsController `type`

##### Namespace

UserAccountService.Controllers

##### Summary

Manages user friend relationships.

<a name='M-UserAccountService-Controllers-FriendsController-#ctor-UserAccountService-Services-IFriendshipService,Microsoft-Extensions-Logging-ILogger{UserAccountService-Controllers-FriendsController}-'></a>
### #ctor() `constructor`

##### Summary

Manages user friend relationships.

##### Parameters

This constructor has no parameters.

<a name='M-UserAccountService-Controllers-FriendsController-AcceptFriendRequest-System-Guid-'></a>
### AcceptFriendRequest(friendshipId) `method`

##### Summary

Accepts a pending friend request.

##### Returns

An [IActionResult](#T-Microsoft-AspNetCore-Mvc-IActionResult 'Microsoft.AspNetCore.Mvc.IActionResult') indicating the result of the operation.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| friendshipId | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | The unique identifier of the friendship. |

<a name='M-UserAccountService-Controllers-FriendsController-DeclineFriendRequest-System-Guid-'></a>
### DeclineFriendRequest(friendshipId) `method`

##### Summary

Declines or cancels a pending friend request.

##### Returns

An [IActionResult](#T-Microsoft-AspNetCore-Mvc-IActionResult 'Microsoft.AspNetCore.Mvc.IActionResult') indicating the result of the operation.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| friendshipId | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | The unique identifier of the friendship. |

<a name='M-UserAccountService-Controllers-FriendsController-GetCurrentUserId'></a>
### GetCurrentUserId() `method`

##### Summary

Retrieves the current user's ID from the token claims.

##### Returns

The unique identifier of the current user.

##### Parameters

This method has no parameters.

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.InvalidOperationException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.InvalidOperationException 'System.InvalidOperationException') | Thrown when the user ID cannot be determined from the token. |

<a name='M-UserAccountService-Controllers-FriendsController-GetFriends'></a>
### GetFriends() `method`

##### Summary

Retrieves the current user's list of friends.

##### Returns

An [IActionResult](#T-Microsoft-AspNetCore-Mvc-IActionResult 'Microsoft.AspNetCore.Mvc.IActionResult') containing the list of friends.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Controllers-FriendsController-GetFriendshipStatus-System-Guid-'></a>
### GetFriendshipStatus(otherUserId) `method`

##### Summary

Retrieves the friendship status between the current user and another user.

##### Returns

An [IActionResult](#T-Microsoft-AspNetCore-Mvc-IActionResult 'Microsoft.AspNetCore.Mvc.IActionResult') containing the friendship status.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| otherUserId | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | The unique identifier of the other user. |

<a name='M-UserAccountService-Controllers-FriendsController-GetPendingIncomingRequests'></a>
### GetPendingIncomingRequests() `method`

##### Summary

Retrieves the current user's incoming pending friend requests.

##### Returns

An [IActionResult](#T-Microsoft-AspNetCore-Mvc-IActionResult 'Microsoft.AspNetCore.Mvc.IActionResult') containing the list of incoming pending friend requests.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Controllers-FriendsController-GetPendingOutgoingRequests'></a>
### GetPendingOutgoingRequests() `method`

##### Summary

Retrieves the current user's outgoing pending friend requests.

##### Returns

An [IActionResult](#T-Microsoft-AspNetCore-Mvc-IActionResult 'Microsoft.AspNetCore.Mvc.IActionResult') containing the list of outgoing pending friend requests.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Controllers-FriendsController-RemoveFriend-System-Guid-'></a>
### RemoveFriend(friendId) `method`

##### Summary

Removes a friend from the current user's friend list.

##### Returns

An [IActionResult](#T-Microsoft-AspNetCore-Mvc-IActionResult 'Microsoft.AspNetCore.Mvc.IActionResult') indicating the result of the operation.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| friendId | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | The unique identifier of the friend to remove. |

<a name='M-UserAccountService-Controllers-FriendsController-SendFriendRequest-System-Guid-'></a>
### SendFriendRequest(addresseeId) `method`

##### Summary

Sends a friend request to another user.

##### Returns

An [IActionResult](#T-Microsoft-AspNetCore-Mvc-IActionResult 'Microsoft.AspNetCore.Mvc.IActionResult') indicating the result of the operation.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| addresseeId | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | The unique identifier of the user receiving the friend request. |

<a name='T-UserAccountService-Models-Friendship'></a>
## Friendship `type`

##### Namespace

UserAccountService.Models

##### Summary

Represents a friendship between two users, including its status and timestamps.

<a name='P-UserAccountService-Models-Friendship-Addressee'></a>
### Addressee `property`

##### Summary

Gets or sets the user who received the friendship request.

<a name='P-UserAccountService-Models-Friendship-AddresseeId'></a>
### AddresseeId `property`

##### Summary

Gets or sets the unique identifier of the user who received the friendship request.

<a name='P-UserAccountService-Models-Friendship-Id'></a>
### Id `property`

##### Summary

Gets or sets the unique identifier for the friendship.

<a name='P-UserAccountService-Models-Friendship-RequestedAt'></a>
### RequestedAt `property`

##### Summary

Gets or sets the date and time when the friendship request was created.
Defaults to the current UTC time.

<a name='P-UserAccountService-Models-Friendship-Requester'></a>
### Requester `property`

##### Summary

Gets or sets the user who initiated the friendship request.

<a name='P-UserAccountService-Models-Friendship-RequesterId'></a>
### RequesterId `property`

##### Summary

Gets or sets the unique identifier of the user who initiated the friendship request.

<a name='P-UserAccountService-Models-Friendship-RespondedAt'></a>
### RespondedAt `property`

##### Summary

Gets or sets the date and time when the friendship request was responded to.
Null if no response has been made.

<a name='P-UserAccountService-Models-Friendship-Status'></a>
### Status `property`

##### Summary

Gets or sets the current status of the friendship.

<a name='T-UserAccountService-Hubs-FriendshipHub'></a>
## FriendshipHub `type`

##### Namespace

UserAccountService.Hubs

##### Summary

Represents a SignalR hub for managing friendships and user connections.

<a name='M-UserAccountService-Hubs-FriendshipHub-GetCurrentUserId'></a>
### GetCurrentUserId() `method`

##### Summary

Retrieves the unique identifier of the currently connected user.

##### Returns

A [Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') representing the user's unique identifier, or [Empty](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid.Empty 'System.Guid.Empty') if the identifier is invalid or not found.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Hubs-FriendshipHub-OnConnectedAsync'></a>
### OnConnectedAsync() `method`

##### Summary

Handles the event when a client connects to the hub.
Adds the user to a SignalR group based on their unique identifier.

##### Returns

A [Task](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Threading.Tasks.Task 'System.Threading.Tasks.Task') representing the asynchronous operation.

##### Parameters

This method has no parameters.

<a name='T-UserAccountService-Models-DTOs-FriendshipResultDto'></a>
## FriendshipResultDto `type`

##### Namespace

UserAccountService.Models.DTOs

##### Summary

Represents the result of a friendship-related operation.

<a name='P-UserAccountService-Models-DTOs-FriendshipResultDto-FriendshipId'></a>
### FriendshipId `property`

##### Summary

The unique identifier of the friendship, if applicable.

##### Example

3f2504e0-4f89-11d3-9a0c-0305e82c3301

##### Remarks

This field is null if the operation does not involve a specific friendship entity.

<a name='P-UserAccountService-Models-DTOs-FriendshipResultDto-Message'></a>
### Message `property`

##### Summary

A message providing additional information about the operation result.

##### Example

Friendship request sent successfully.

##### Remarks

This message can describe the reason for failure or provide context for success.

<a name='P-UserAccountService-Models-DTOs-FriendshipResultDto-Status'></a>
### Status `property`

##### Summary

The current status of the friendship, if applicable.

##### Example

Pending

##### Remarks

This field is null if the operation does not involve a status update.

<a name='P-UserAccountService-Models-DTOs-FriendshipResultDto-Success'></a>
### Success `property`

##### Summary

Indicates whether the operation was successful.

##### Example

true

<a name='T-UserAccountService-Services-FriendshipService'></a>
## FriendshipService `type`

##### Namespace

UserAccountService.Services

##### Summary

Provides methods for managing friendships between users, including sending requests, accepting/declining requests, 
removing friends, and retrieving friendship-related data.

<a name='M-UserAccountService-Services-FriendshipService-#ctor-UserAccountService-Data-UserAccountDbContext,Microsoft-Extensions-Logging-ILogger{UserAccountService-Services-FriendshipService},Microsoft-AspNetCore-SignalR-IHubContext{UserAccountService-Hubs-FriendshipHub}-'></a>
### #ctor() `constructor`

##### Summary

Provides methods for managing friendships between users, including sending requests, accepting/declining requests, 
removing friends, and retrieving friendship-related data.

##### Parameters

This constructor has no parameters.

<a name='M-UserAccountService-Services-FriendshipService-AcceptFriendRequestAsync-System-Guid,System-Guid-'></a>
### AcceptFriendRequestAsync(friendshipId,currentUserId) `method`

##### Summary

Accepts a pending friend request.

##### Returns

A [FriendshipResultDto](#T-UserAccountService-Models-DTOs-FriendshipResultDto 'UserAccountService.Models.DTOs.FriendshipResultDto') containing the result of the operation.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| friendshipId | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | The ID of the friendship to accept. |
| currentUserId | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | The ID of the user accepting the request. |

<a name='M-UserAccountService-Services-FriendshipService-DeclineFriendRequestAsync-System-Guid,System-Guid-'></a>
### DeclineFriendRequestAsync(friendshipId,currentUserId) `method`

##### Summary

Declines or cancels a pending friend request.

##### Returns

A [FriendshipResultDto](#T-UserAccountService-Models-DTOs-FriendshipResultDto 'UserAccountService.Models.DTOs.FriendshipResultDto') containing the result of the operation.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| friendshipId | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | The ID of the friendship to decline or cancel. |
| currentUserId | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | The ID of the user declining or canceling the request. |

<a name='M-UserAccountService-Services-FriendshipService-GetFriendsAsync-System-Guid-'></a>
### GetFriendsAsync(userId) `method`

##### Summary

Retrieves a list of friends for a given user.

##### Returns

A collection of [FriendDto](#T-UserAccountService-Models-DTOs-FriendDto 'UserAccountService.Models.DTOs.FriendDto') representing the user's friends.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| userId | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | The ID of the user whose friends are being retrieved. |

<a name='M-UserAccountService-Services-FriendshipService-GetFriendshipStatusAsync-System-Guid,System-Guid-'></a>
### GetFriendshipStatusAsync(userId1,userId2) `method`

##### Summary

Retrieves the friendship status between two users.

##### Returns

The [FriendshipStatus](#T-UserAccountService-Models-FriendshipStatus 'UserAccountService.Models.FriendshipStatus') representing the status of the friendship, or null if no friendship exists.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| userId1 | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | The ID of the first user. |
| userId2 | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | The ID of the second user. |

<a name='M-UserAccountService-Services-FriendshipService-GetPendingIncomingRequestsAsync-System-Guid-'></a>
### GetPendingIncomingRequestsAsync(userId) `method`

##### Summary

Retrieves a list of incoming friend requests for a given user.

##### Returns

A collection of [FriendRequestDto](#T-UserAccountService-Models-DTOs-FriendRequestDto 'UserAccountService.Models.DTOs.FriendRequestDto') representing the incoming requests.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| userId | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | The ID of the user whose incoming requests are being retrieved. |

<a name='M-UserAccountService-Services-FriendshipService-GetPendingOutgoingRequestsAsync-System-Guid-'></a>
### GetPendingOutgoingRequestsAsync(userId) `method`

##### Summary

Retrieves a list of outgoing friend requests for a given user.

##### Returns

A collection of [FriendRequestDto](#T-UserAccountService-Models-DTOs-FriendRequestDto 'UserAccountService.Models.DTOs.FriendRequestDto') representing the outgoing requests.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| userId | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | The ID of the user whose outgoing requests are being retrieved. |

<a name='M-UserAccountService-Services-FriendshipService-RemoveFriendAsync-System-Guid,System-Guid-'></a>
### RemoveFriendAsync(userId,friendId) `method`

##### Summary

Removes an existing friendship between two users.

##### Returns

A [FriendshipResultDto](#T-UserAccountService-Models-DTOs-FriendshipResultDto 'UserAccountService.Models.DTOs.FriendshipResultDto') containing the result of the operation.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| userId | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | The ID of the user initiating the removal. |
| friendId | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | The ID of the friend to remove. |

<a name='M-UserAccountService-Services-FriendshipService-SendFriendRequestAsync-System-Guid,System-Guid-'></a>
### SendFriendRequestAsync(requesterId,addresseeId) `method`

##### Summary

Sends a friend request from one user to another.

##### Returns

A [FriendshipResultDto](#T-UserAccountService-Models-DTOs-FriendshipResultDto 'UserAccountService.Models.DTOs.FriendshipResultDto') containing the result of the operation.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| requesterId | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | The ID of the user sending the friend request. |
| addresseeId | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | The ID of the user receiving the friend request. |

<a name='T-UserAccountService-Models-FriendshipStatus'></a>
## FriendshipStatus `type`

##### Namespace

UserAccountService.Models

##### Summary

Represents the status of a friendship request.

<a name='F-UserAccountService-Models-FriendshipStatus-Accepted'></a>
### Accepted `constants`

##### Summary

Indicates that the friendship request has been accepted.

<a name='F-UserAccountService-Models-FriendshipStatus-Blocked'></a>
### Blocked `constants`

##### Summary

Indicates that the friendship has been blocked.

<a name='F-UserAccountService-Models-FriendshipStatus-Declined'></a>
### Declined `constants`

##### Summary

Indicates that the friendship request has been declined.

<a name='F-UserAccountService-Models-FriendshipStatus-Pending'></a>
### Pending `constants`

##### Summary

Indicates that the friendship request is pending.

<a name='T-UserAccountService-Services-IFriendshipService'></a>
## IFriendshipService `type`

##### Namespace

UserAccountService.Services

##### Summary

Defines the contract for managing friendships between users.

<a name='M-UserAccountService-Services-IFriendshipService-AcceptFriendRequestAsync-System-Guid,System-Guid-'></a>
### AcceptFriendRequestAsync(friendshipId,currentUserId) `method`

##### Summary

Accepts a pending friend request.

##### Returns

A [FriendshipResultDto](#T-UserAccountService-Models-DTOs-FriendshipResultDto 'UserAccountService.Models.DTOs.FriendshipResultDto') containing the result of the operation.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| friendshipId | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | The ID of the friendship to accept. |
| currentUserId | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | The ID of the user accepting the request. |

<a name='M-UserAccountService-Services-IFriendshipService-DeclineFriendRequestAsync-System-Guid,System-Guid-'></a>
### DeclineFriendRequestAsync(friendshipId,currentUserId) `method`

##### Summary

Declines a pending friend request.

##### Returns

A [FriendshipResultDto](#T-UserAccountService-Models-DTOs-FriendshipResultDto 'UserAccountService.Models.DTOs.FriendshipResultDto') containing the result of the operation.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| friendshipId | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | The ID of the friendship to decline. |
| currentUserId | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | The ID of the user declining the request. |

<a name='M-UserAccountService-Services-IFriendshipService-GetFriendsAsync-System-Guid-'></a>
### GetFriendsAsync(userId) `method`

##### Summary

Retrieves a list of friends for a given user.

##### Returns

A collection of [FriendDto](#T-UserAccountService-Models-DTOs-FriendDto 'UserAccountService.Models.DTOs.FriendDto') representing the user's friends.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| userId | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | The ID of the user whose friends are being retrieved. |

<a name='M-UserAccountService-Services-IFriendshipService-GetFriendshipStatusAsync-System-Guid,System-Guid-'></a>
### GetFriendshipStatusAsync(userId1,userId2) `method`

##### Summary

Retrieves the friendship status between two users.

##### Returns

The [FriendshipStatus](#T-UserAccountService-Models-FriendshipStatus 'UserAccountService.Models.FriendshipStatus') representing the status of the friendship, or null if no friendship exists.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| userId1 | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | The ID of the first user. |
| userId2 | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | The ID of the second user. |

<a name='M-UserAccountService-Services-IFriendshipService-GetPendingIncomingRequestsAsync-System-Guid-'></a>
### GetPendingIncomingRequestsAsync(userId) `method`

##### Summary

Retrieves a list of incoming friend requests for a given user.

##### Returns

A collection of [FriendRequestDto](#T-UserAccountService-Models-DTOs-FriendRequestDto 'UserAccountService.Models.DTOs.FriendRequestDto') representing the incoming requests.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| userId | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | The ID of the user whose incoming requests are being retrieved. |

<a name='M-UserAccountService-Services-IFriendshipService-GetPendingOutgoingRequestsAsync-System-Guid-'></a>
### GetPendingOutgoingRequestsAsync(userId) `method`

##### Summary

Retrieves a list of outgoing friend requests for a given user.

##### Returns

A collection of [FriendRequestDto](#T-UserAccountService-Models-DTOs-FriendRequestDto 'UserAccountService.Models.DTOs.FriendRequestDto') representing the outgoing requests.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| userId | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | The ID of the user whose outgoing requests are being retrieved. |

<a name='M-UserAccountService-Services-IFriendshipService-RemoveFriendAsync-System-Guid,System-Guid-'></a>
### RemoveFriendAsync(userId,friendId) `method`

##### Summary

Removes an existing friendship between two users.

##### Returns

A [FriendshipResultDto](#T-UserAccountService-Models-DTOs-FriendshipResultDto 'UserAccountService.Models.DTOs.FriendshipResultDto') containing the result of the operation.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| userId | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | The ID of the user initiating the removal. |
| friendId | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | The ID of the friend to remove. |

<a name='M-UserAccountService-Services-IFriendshipService-SendFriendRequestAsync-System-Guid,System-Guid-'></a>
### SendFriendRequestAsync(requesterId,addresseeId) `method`

##### Summary

Sends a friend request from one user to another.

##### Returns

A [FriendshipResultDto](#T-UserAccountService-Models-DTOs-FriendshipResultDto 'UserAccountService.Models.DTOs.FriendshipResultDto') containing the result of the operation.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| requesterId | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | The ID of the user sending the friend request. |
| addresseeId | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | The ID of the user receiving the friend request. |

<a name='T-UserAccountService-Services-ITokenService'></a>
## ITokenService `type`

##### Namespace

UserAccountService.Services

##### Summary

Defines the contract for token management, including generation, refreshing, and invalidation of tokens.

<a name='M-UserAccountService-Services-ITokenService-GenerateTokens-UserAccountService-Models-User-'></a>
### GenerateTokens(user) `method`

##### Summary

Generates a new access token and refresh token for the specified user.

##### Returns

A tuple containing the access token as a string and the refresh token as a [RefreshToken](#T-UserAccountService-Models-RefreshToken 'UserAccountService.Models.RefreshToken').

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| user | [UserAccountService.Models.User](#T-UserAccountService-Models-User 'UserAccountService.Models.User') | The user for whom the tokens are being generated. |

<a name='M-UserAccountService-Services-ITokenService-InvalidateRefreshTokenAsync-System-String-'></a>
### InvalidateRefreshTokenAsync(refreshTokenString) `method`

##### Summary

Invalidates the specified refresh token.

##### Returns

A task that represents the asynchronous operation. The task result is a boolean indicating whether the refresh token was successfully invalidated.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| refreshTokenString | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The string representation of the refresh token to invalidate. |

<a name='M-UserAccountService-Services-ITokenService-RefreshAccessTokenAsync-System-String-'></a>
### RefreshAccessTokenAsync(oldRefreshTokenString) `method`

##### Summary

Refreshes the access token using the provided old refresh token string.

##### Returns

A task that represents the asynchronous operation. The task result contains a tuple with the new access token as a string and the new refresh token as a [RefreshToken](#T-UserAccountService-Models-RefreshToken 'UserAccountService.Models.RefreshToken'), or null if the refresh operation fails.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| oldRefreshTokenString | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The string representation of the old refresh token. |

<a name='T-UserAccountService-Services-IUserService'></a>
## IUserService `type`

##### Namespace

UserAccountService.Services

##### Summary

Defines the contract for user management, including registration, authentication, retrieval, and searching.

<a name='M-UserAccountService-Services-IUserService-AuthenticateUserAsync-System-String,System-String-'></a>
### AuthenticateUserAsync(tagOrEmail,password) `method`

##### Summary

Authenticates a user using their tag or email and password.

##### Returns

A [User](#T-UserAccountService-Models-User 'UserAccountService.Models.User') object representing the authenticated user, or null if authentication fails.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| tagOrEmail | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The tag or email of the user attempting to authenticate. |
| password | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The password for the user account. |

<a name='M-UserAccountService-Services-IUserService-GetUserByIdAsync-System-Guid-'></a>
### GetUserByIdAsync(userId) `method`

##### Summary

Retrieves a user by their unique identifier.

##### Returns

A [User](#T-UserAccountService-Models-User 'UserAccountService.Models.User') object representing the user, or null if the user is not found.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| userId | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | The unique identifier of the user. |

<a name='M-UserAccountService-Services-IUserService-RegisterUserAsync-System-String,System-String,System-String,System-String-'></a>
### RegisterUserAsync(name,tag,email,password) `method`

##### Summary

Registers a new user with the specified details.

##### Returns

A [User](#T-UserAccountService-Models-User 'UserAccountService.Models.User') object representing the registered user, or null if registration fails.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| name | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The name of the user. |
| tag | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The unique tag associated with the user. |
| email | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The email address of the user. |
| password | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The password for the user account. |

<a name='M-UserAccountService-Services-IUserService-SearchUsersAsync-System-String-'></a>
### SearchUsersAsync(query) `method`

##### Summary

Searches for users based on a query string.

##### Returns

A collection of [User](#T-UserAccountService-Models-User 'UserAccountService.Models.User') objects matching the search criteria.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| query | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The search query used to find users. |

<a name='T-UserAccountService-Migrations-InitialCreate'></a>
## InitialCreate `type`

##### Namespace

UserAccountService.Migrations

##### Summary

*Inherit from parent.*

<a name='M-UserAccountService-Migrations-InitialCreate-BuildTargetModel-Microsoft-EntityFrameworkCore-ModelBuilder-'></a>
### BuildTargetModel() `method`

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Migrations-InitialCreate-Down-Microsoft-EntityFrameworkCore-Migrations-MigrationBuilder-'></a>
### Down() `method`

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Migrations-InitialCreate-Up-Microsoft-EntityFrameworkCore-Migrations-MigrationBuilder-'></a>
### Up() `method`

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.

<a name='T-UserAccountService-Models-Configuration-JwtSettings'></a>
## JwtSettings `type`

##### Namespace

UserAccountService.Models.Configuration

##### Summary

Represents the configuration settings for JWT (JSON Web Token) authentication.

<a name='P-UserAccountService-Models-Configuration-JwtSettings-Audience'></a>
### Audience `property`

##### Summary

Gets or sets the audience for the JWTs.

<a name='P-UserAccountService-Models-Configuration-JwtSettings-ExpiryMinutes'></a>
### ExpiryMinutes `property`

##### Summary

Gets or sets the expiry time for JWTs, in minutes.

<a name='P-UserAccountService-Models-Configuration-JwtSettings-Issuer'></a>
### Issuer `property`

##### Summary

Gets or sets the issuer of the JWTs.

<a name='P-UserAccountService-Models-Configuration-JwtSettings-RefreshTokenExpiryDays'></a>
### RefreshTokenExpiryDays `property`

##### Summary

Gets or sets the expiry time for refresh tokens, in days.

<a name='P-UserAccountService-Models-Configuration-JwtSettings-Secret'></a>
### Secret `property`

##### Summary

Gets or sets the secret key used for signing JWTs.

<a name='T-UserAccountService-Models-DTOs-LoginRequestDto'></a>
## LoginRequestDto `type`

##### Namespace

UserAccountService.Models.DTOs

##### Summary

Represents a request to log in to the system.

<a name='P-UserAccountService-Models-DTOs-LoginRequestDto-Password'></a>
### Password `property`

##### Summary

The password associated with the user's account.

##### Example

SecurePassword123!

##### Remarks

The password must match the one set during registration.

<a name='P-UserAccountService-Models-DTOs-LoginRequestDto-TagOrEmail'></a>
### TagOrEmail `property`

##### Summary

The user's tag or email used for authentication.

##### Example

john.doe#1234

##### Remarks

This field can contain either:
- A tag in the format 'username#1234'.
- A valid email address.

<a name='T-UserAccountService-Models-DTOs-LoginResponseDto'></a>
## LoginResponseDto `type`

##### Namespace

UserAccountService.Models.DTOs

##### Summary

Represents the response returned after a successful login.

<a name='P-UserAccountService-Models-DTOs-LoginResponseDto-AccessToken'></a>
### AccessToken `property`

##### Summary

The access token issued upon successful authentication.

##### Example

eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...

##### Remarks

The access token is used to authorize requests to protected resources.

<a name='P-UserAccountService-Models-DTOs-LoginResponseDto-RefreshToken'></a>
### RefreshToken `property`

##### Summary

The refresh token issued to obtain a new access token when the current one expires.

##### Example

eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...

##### Remarks

The refresh token is a secure string and must be kept confidential.

<a name='T-UserAccountService-Models-DTOs-RefreshRequestDto'></a>
## RefreshRequestDto `type`

##### Namespace

UserAccountService.Models.DTOs

##### Summary

Represents a request to refresh an authentication token.

<a name='P-UserAccountService-Models-DTOs-RefreshRequestDto-RefreshToken'></a>
### RefreshToken `property`

##### Summary

The refresh token used to obtain a new access token.

##### Example

eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...

##### Remarks

The refresh token is a secure string issued during authentication and must be valid.

<a name='T-UserAccountService-Models-RefreshToken'></a>
## RefreshToken `type`

##### Namespace

UserAccountService.Models

##### Summary

Represents a refresh token used for authentication and session management.

<a name='P-UserAccountService-Models-RefreshToken-CreatedAt'></a>
### CreatedAt `property`

##### Summary

Gets or sets the date and time when the refresh token was created.
Defaults to the current UTC time.

<a name='P-UserAccountService-Models-RefreshToken-ExpiryDate'></a>
### ExpiryDate `property`

##### Summary

Gets or sets the expiry date and time of the refresh token.

<a name='P-UserAccountService-Models-RefreshToken-Invalidated'></a>
### Invalidated `property`

##### Summary

Gets or sets a value indicating whether the refresh token has been invalidated.

<a name='P-UserAccountService-Models-RefreshToken-JwtId'></a>
### JwtId `property`

##### Summary

Gets or sets the unique identifier of the associated JWT.

<a name='P-UserAccountService-Models-RefreshToken-Token'></a>
### Token `property`

##### Summary

Gets or sets the unique token string.

<a name='P-UserAccountService-Models-RefreshToken-Used'></a>
### Used `property`

##### Summary

Gets or sets a value indicating whether the refresh token has been used.

<a name='P-UserAccountService-Models-RefreshToken-User'></a>
### User `property`

##### Summary

Gets or sets the user associated with the refresh token.

<a name='P-UserAccountService-Models-RefreshToken-UserId'></a>
### UserId `property`

##### Summary

Gets or sets the unique identifier of the user associated with the refresh token.

<a name='T-UserAccountService-Models-DTOs-RegisterRequestDto'></a>
## RegisterRequestDto `type`

##### Namespace

UserAccountService.Models.DTOs

##### Summary

Represents a request to register a new user.

<a name='P-UserAccountService-Models-DTOs-RegisterRequestDto-Email'></a>
### Email `property`

##### Summary

The user's email address.

##### Example

john.doe@example.com

##### Remarks

The email address must be valid according to standard email format rules.

<a name='P-UserAccountService-Models-DTOs-RegisterRequestDto-Name'></a>
### Name `property`

##### Summary

The desired username for the new account.

##### Example

john.doe

<a name='P-UserAccountService-Models-DTOs-RegisterRequestDto-Password'></a>
### Password `property`

##### Summary

The password for the new account.

##### Example

SecurePassword123!

##### Remarks

The password should be strong and secure to protect the user's account.

<a name='P-UserAccountService-Models-DTOs-RegisterRequestDto-Tag'></a>
### Tag `property`

##### Summary

The unique tag associated with the user, in the format 'username#1234'.

##### Example

john.doe#1234

##### Remarks

The tag must consist of:
- A username part containing 1–45 alphanumeric characters or underscores.
- Followed by a '#' and a 4-digit number.

<a name='T-UserAccountService-Services-TokenService'></a>
## TokenService `type`

##### Namespace

UserAccountService.Services

##### Summary

Provides methods for generating, refreshing, and invalidating tokens.

<a name='M-UserAccountService-Services-TokenService-#ctor-UserAccountService-Data-UserAccountDbContext,Microsoft-Extensions-Options-IOptions{UserAccountService-Models-Configuration-JwtSettings},Microsoft-Extensions-Logging-ILogger{UserAccountService-Services-TokenService}-'></a>
### #ctor() `constructor`

##### Summary

Provides methods for generating, refreshing, and invalidating tokens.

##### Parameters

This constructor has no parameters.

<a name='M-UserAccountService-Services-TokenService-GenerateTokens-UserAccountService-Models-User-'></a>
### GenerateTokens(user) `method`

##### Summary

Generates a new access token and refresh token for the specified user.

##### Returns

A tuple containing the access token as a string and the refresh token as a [RefreshToken](#T-UserAccountService-Models-RefreshToken 'UserAccountService.Models.RefreshToken').

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| user | [UserAccountService.Models.User](#T-UserAccountService-Models-User 'UserAccountService.Models.User') | The user for whom the tokens are being generated. |

<a name='M-UserAccountService-Services-TokenService-InvalidateRefreshTokenAsync-System-String-'></a>
### InvalidateRefreshTokenAsync(refreshTokenString) `method`

##### Summary

Invalidates the specified refresh token.

##### Returns

A task that represents the asynchronous operation. The task result is a boolean indicating whether the refresh token was successfully invalidated.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| refreshTokenString | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The string representation of the refresh token to invalidate. |

<a name='M-UserAccountService-Services-TokenService-RefreshAccessTokenAsync-System-String-'></a>
### RefreshAccessTokenAsync(oldRefreshTokenString) `method`

##### Summary

Refreshes the access token using the provided old refresh token string.

##### Returns

A task that represents the asynchronous operation. The task result contains a tuple with the new access token as a string and the new refresh token as a [RefreshToken](#T-UserAccountService-Models-RefreshToken 'UserAccountService.Models.RefreshToken'), or null if the refresh operation fails.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| oldRefreshTokenString | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The string representation of the old refresh token. |

<a name='T-UserAccountService-Models-User'></a>
## User `type`

##### Namespace

UserAccountService.Models

##### Summary

Represents a user in the system, including their personal details, authentication information, and relationships.

<a name='P-UserAccountService-Models-User-CreatedAt'></a>
### CreatedAt `property`

##### Summary

Gets or sets the date and time when the user was created.
Defaults to the current UTC time.

<a name='P-UserAccountService-Models-User-Email'></a>
### Email `property`

##### Summary

Gets or sets the email address of the user.

<a name='P-UserAccountService-Models-User-Friendships1'></a>
### Friendships1 `property`

##### Summary

Gets or sets the collection of friendships where the user is the first participant.

<a name='P-UserAccountService-Models-User-Friendships2'></a>
### Friendships2 `property`

##### Summary

Gets or sets the collection of friendships where the user is the second participant.

<a name='P-UserAccountService-Models-User-Id'></a>
### Id `property`

##### Summary

Gets or sets the unique identifier for the user.

<a name='P-UserAccountService-Models-User-Name'></a>
### Name `property`

##### Summary

Gets or sets the name of the user.

<a name='P-UserAccountService-Models-User-PasswordHash'></a>
### PasswordHash `property`

##### Summary

Gets or sets the hashed password for the user account.

<a name='P-UserAccountService-Models-User-RefreshTokens'></a>
### RefreshTokens `property`

##### Summary

Gets or sets the collection of refresh tokens associated with the user.

<a name='P-UserAccountService-Models-User-Tag'></a>
### Tag `property`

##### Summary

Gets or sets the unique tag associated with the user.

<a name='T-UserAccountService-Data-UserAccountDbContext'></a>
## UserAccountDbContext `type`

##### Namespace

UserAccountService.Data

##### Summary

Represents the database context for the User Account Service.
Provides access to the database tables and configuration for entity relationships.

<a name='M-UserAccountService-Data-UserAccountDbContext-#ctor-Microsoft-EntityFrameworkCore-DbContextOptions{UserAccountService-Data-UserAccountDbContext}-'></a>
### #ctor() `constructor`

##### Summary

Represents the database context for the User Account Service.
Provides access to the database tables and configuration for entity relationships.

##### Parameters

This constructor has no parameters.

<a name='P-UserAccountService-Data-UserAccountDbContext-Friendships'></a>
### Friendships `property`

##### Summary

Gets or sets the database table for friendships.

<a name='P-UserAccountService-Data-UserAccountDbContext-RefreshTokens'></a>
### RefreshTokens `property`

##### Summary

Gets or sets the database table for refresh tokens.

<a name='P-UserAccountService-Data-UserAccountDbContext-Users'></a>
### Users `property`

##### Summary

Gets or sets the database table for users.

<a name='M-UserAccountService-Data-UserAccountDbContext-OnModelCreating-Microsoft-EntityFrameworkCore-ModelBuilder-'></a>
### OnModelCreating(modelBuilder) `method`

##### Summary

Configures the entity relationships and indexes for the database tables.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| modelBuilder | [Microsoft.EntityFrameworkCore.ModelBuilder](#T-Microsoft-EntityFrameworkCore-ModelBuilder 'Microsoft.EntityFrameworkCore.ModelBuilder') | The [ModelBuilder](#T-Microsoft-EntityFrameworkCore-ModelBuilder 'Microsoft.EntityFrameworkCore.ModelBuilder') used to configure the entity models. |

<a name='T-UserAccountService-Models-DTOs-UserDto'></a>
## UserDto `type`

##### Namespace

UserAccountService.Models.DTOs

##### Summary

Represents a user entity in the system.

<a name='M-UserAccountService-Models-DTOs-UserDto-#ctor-UserAccountService-Models-User-'></a>
### #ctor(user) `constructor`

##### Summary

Initializes a new instance of the [UserDto](#T-UserAccountService-Models-DTOs-UserDto 'UserAccountService.Models.DTOs.UserDto') class using a [User](#T-UserAccountService-Models-User 'UserAccountService.Models.User') entity.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| user | [UserAccountService.Models.User](#T-UserAccountService-Models-User 'UserAccountService.Models.User') | The user entity to map to the DTO. |

<a name='M-UserAccountService-Models-DTOs-UserDto-#ctor'></a>
### #ctor() `constructor`

##### Summary

Initializes a new instance of the [UserDto](#T-UserAccountService-Models-DTOs-UserDto 'UserAccountService.Models.DTOs.UserDto') class.

##### Parameters

This constructor has no parameters.

<a name='P-UserAccountService-Models-DTOs-UserDto-CreatedAt'></a>
### CreatedAt `property`

##### Summary

The date and time when the user was created.

##### Example

2023-10-01T12:34:56Z

<a name='P-UserAccountService-Models-DTOs-UserDto-Email'></a>
### Email `property`

##### Summary

The email address of the user.

##### Example

john.doe@example.com

##### Remarks

The email must be a valid email address format.

<a name='P-UserAccountService-Models-DTOs-UserDto-Id'></a>
### Id `property`

##### Summary

The unique identifier of the user.

##### Example

3f2504e0-4f89-11d3-9a0c-0305e82c3301

<a name='P-UserAccountService-Models-DTOs-UserDto-Name'></a>
### Name `property`

##### Summary

The name of the user.

##### Example

John Doe

##### Remarks

This field contains the display name of the user.

<a name='P-UserAccountService-Models-DTOs-UserDto-Tag'></a>
### Tag `property`

##### Summary

The tag of the user, in the format 'username#1234'.

##### Example

john.doe#1234

##### Remarks

The tag uniquely identifies the user within the system.

<a name='T-UserAccountService-Services-UserService'></a>
## UserService `type`

##### Namespace

UserAccountService.Services

##### Summary

Provides methods for user management, including registration, authentication, retrieval, and searching.

<a name='M-UserAccountService-Services-UserService-#ctor-UserAccountService-Data-UserAccountDbContext,Microsoft-Extensions-Logging-ILogger{UserAccountService-Services-UserService}-'></a>
### #ctor() `constructor`

##### Summary

Provides methods for user management, including registration, authentication, retrieval, and searching.

##### Parameters

This constructor has no parameters.

<a name='M-UserAccountService-Services-UserService-AuthenticateUserAsync-System-String,System-String-'></a>
### AuthenticateUserAsync(tagOrEmail,password) `method`

##### Summary

Authenticates a user using their tag or email and password.

##### Returns

A [User](#T-UserAccountService-Models-User 'UserAccountService.Models.User') object representing the authenticated user, or null if authentication fails.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| tagOrEmail | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The tag or email of the user attempting to authenticate. |
| password | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The password for the user account. |

<a name='M-UserAccountService-Services-UserService-GetUserByIdAsync-System-Guid-'></a>
### GetUserByIdAsync(userId) `method`

##### Summary

Retrieves a user by their unique identifier.

##### Returns

A [User](#T-UserAccountService-Models-User 'UserAccountService.Models.User') object representing the user, or null if the user is not found.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| userId | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | The unique identifier of the user. |

<a name='M-UserAccountService-Services-UserService-RegisterUserAsync-System-String,System-String,System-String,System-String-'></a>
### RegisterUserAsync(name,tag,email,password) `method`

##### Summary

Registers a new user with the specified details.

##### Returns

A [User](#T-UserAccountService-Models-User 'UserAccountService.Models.User') object representing the registered user, or null if registration fails.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| name | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The name of the user. |
| tag | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The unique tag associated with the user. |
| email | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The email address of the user. |
| password | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The password for the user account. |

<a name='M-UserAccountService-Services-UserService-SearchUsersAsync-System-String-'></a>
### SearchUsersAsync(query) `method`

##### Summary

Searches for users based on a query string.

##### Returns

A collection of [User](#T-UserAccountService-Models-User 'UserAccountService.Models.User') objects matching the search criteria.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| query | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The search query used to find users. |

<a name='T-UserAccountService-Controllers-UsersController'></a>
## UsersController `type`

##### Namespace

UserAccountService.Controllers

##### Summary

Controller for managing user-related operations.
Requires authorization for all endpoints.

<a name='M-UserAccountService-Controllers-UsersController-#ctor-UserAccountService-Services-IUserService-'></a>
### #ctor() `constructor`

##### Summary

Controller for managing user-related operations.
Requires authorization for all endpoints.

##### Parameters

This constructor has no parameters.

<a name='M-UserAccountService-Controllers-UsersController-GetMe'></a>
### GetMe() `method`

##### Summary

Retrieves the currently authenticated user's information.

##### Returns

An [IActionResult](#T-Microsoft-AspNetCore-Mvc-IActionResult 'Microsoft.AspNetCore.Mvc.IActionResult') containing the user's information if found,
or an appropriate error response (Unauthorized or NotFound).

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Controllers-UsersController-GetUserById-System-Guid-'></a>
### GetUserById(id) `method`

##### Summary

Retrieves a user's information by their unique identifier.

##### Returns

An [IActionResult](#T-Microsoft-AspNetCore-Mvc-IActionResult 'Microsoft.AspNetCore.Mvc.IActionResult') containing the user's information if found,
or a NotFound response if the user does not exist.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| id | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | The unique identifier of the user. |

<a name='M-UserAccountService-Controllers-UsersController-SearchUsers-System-String-'></a>
### SearchUsers(query) `method`

##### Summary

Searches for users based on a query string.

##### Returns

An [IActionResult](#T-Microsoft-AspNetCore-Mvc-IActionResult 'Microsoft.AspNetCore.Mvc.IActionResult') containing a list of users matching the query,
or a BadRequest response if the query is empty.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| query | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The search query string. |
