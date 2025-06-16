<a name='assembly'></a>
# UserAccountService.Tests

## Contents

- [AuthControllerTest](#T-UserAccountService-Tests-Controllers-AuthControllerTest 'UserAccountService.Tests.Controllers.AuthControllerTest')
  - [#ctor()](#M-UserAccountService-Tests-Controllers-AuthControllerTest-#ctor 'UserAccountService.Tests.Controllers.AuthControllerTest.#ctor')
  - [Dispose()](#M-UserAccountService-Tests-Controllers-AuthControllerTest-Dispose 'UserAccountService.Tests.Controllers.AuthControllerTest.Dispose')
  - [Login_WithInvalidCredentials_ReturnsUnauthorized()](#M-UserAccountService-Tests-Controllers-AuthControllerTest-Login_WithInvalidCredentials_ReturnsUnauthorized 'UserAccountService.Tests.Controllers.AuthControllerTest.Login_WithInvalidCredentials_ReturnsUnauthorized')
  - [Login_WithInvalidModelState_ReturnsBadRequest()](#M-UserAccountService-Tests-Controllers-AuthControllerTest-Login_WithInvalidModelState_ReturnsBadRequest 'UserAccountService.Tests.Controllers.AuthControllerTest.Login_WithInvalidModelState_ReturnsBadRequest')
  - [Login_WithValidCredentials_ReturnsOkWithTokens()](#M-UserAccountService-Tests-Controllers-AuthControllerTest-Login_WithValidCredentials_ReturnsOkWithTokens 'UserAccountService.Tests.Controllers.AuthControllerTest.Login_WithValidCredentials_ReturnsOkWithTokens')
  - [Logout_WithInvalidModelState_ReturnsBadRequest()](#M-UserAccountService-Tests-Controllers-AuthControllerTest-Logout_WithInvalidModelState_ReturnsBadRequest 'UserAccountService.Tests.Controllers.AuthControllerTest.Logout_WithInvalidModelState_ReturnsBadRequest')
  - [Logout_WithInvalidRefreshToken_ReturnsBadRequest()](#M-UserAccountService-Tests-Controllers-AuthControllerTest-Logout_WithInvalidRefreshToken_ReturnsBadRequest 'UserAccountService.Tests.Controllers.AuthControllerTest.Logout_WithInvalidRefreshToken_ReturnsBadRequest')
  - [Logout_WithValidRefreshToken_ReturnsOk()](#M-UserAccountService-Tests-Controllers-AuthControllerTest-Logout_WithValidRefreshToken_ReturnsOk 'UserAccountService.Tests.Controllers.AuthControllerTest.Logout_WithValidRefreshToken_ReturnsOk')
  - [Refresh_WithInvalidModelState_ReturnsBadRequest()](#M-UserAccountService-Tests-Controllers-AuthControllerTest-Refresh_WithInvalidModelState_ReturnsBadRequest 'UserAccountService.Tests.Controllers.AuthControllerTest.Refresh_WithInvalidModelState_ReturnsBadRequest')
  - [Refresh_WithInvalidRefreshToken_ReturnsUnauthorized()](#M-UserAccountService-Tests-Controllers-AuthControllerTest-Refresh_WithInvalidRefreshToken_ReturnsUnauthorized 'UserAccountService.Tests.Controllers.AuthControllerTest.Refresh_WithInvalidRefreshToken_ReturnsUnauthorized')
  - [Refresh_WithValidRefreshToken_ReturnsOkWithNewTokens()](#M-UserAccountService-Tests-Controllers-AuthControllerTest-Refresh_WithValidRefreshToken_ReturnsOkWithNewTokens 'UserAccountService.Tests.Controllers.AuthControllerTest.Refresh_WithValidRefreshToken_ReturnsOkWithNewTokens')
  - [Register_WithExistingUser_ReturnsBadRequest()](#M-UserAccountService-Tests-Controllers-AuthControllerTest-Register_WithExistingUser_ReturnsBadRequest 'UserAccountService.Tests.Controllers.AuthControllerTest.Register_WithExistingUser_ReturnsBadRequest')
  - [Register_WithInvalidModelState_ReturnsBadRequest()](#M-UserAccountService-Tests-Controllers-AuthControllerTest-Register_WithInvalidModelState_ReturnsBadRequest 'UserAccountService.Tests.Controllers.AuthControllerTest.Register_WithInvalidModelState_ReturnsBadRequest')
  - [Register_WithValidData_ReturnsCreatedAtAction()](#M-UserAccountService-Tests-Controllers-AuthControllerTest-Register_WithValidData_ReturnsCreatedAtAction 'UserAccountService.Tests.Controllers.AuthControllerTest.Register_WithValidData_ReturnsCreatedAtAction')
- [FriendsControllerTest](#T-UserAccountService-Tests-Controllers-FriendsControllerTest 'UserAccountService.Tests.Controllers.FriendsControllerTest')
  - [#ctor()](#M-UserAccountService-Tests-Controllers-FriendsControllerTest-#ctor 'UserAccountService.Tests.Controllers.FriendsControllerTest.#ctor')
  - [AcceptFriendRequest_ShouldReturnBadRequest_ForOtherFailures()](#M-UserAccountService-Tests-Controllers-FriendsControllerTest-AcceptFriendRequest_ShouldReturnBadRequest_ForOtherFailures 'UserAccountService.Tests.Controllers.FriendsControllerTest.AcceptFriendRequest_ShouldReturnBadRequest_ForOtherFailures')
  - [AcceptFriendRequest_ShouldReturnForbid_WhenNotAuthorized()](#M-UserAccountService-Tests-Controllers-FriendsControllerTest-AcceptFriendRequest_ShouldReturnForbid_WhenNotAuthorized 'UserAccountService.Tests.Controllers.FriendsControllerTest.AcceptFriendRequest_ShouldReturnForbid_WhenNotAuthorized')
  - [AcceptFriendRequest_ShouldReturnNotFound_WhenFriendshipNotFound()](#M-UserAccountService-Tests-Controllers-FriendsControllerTest-AcceptFriendRequest_ShouldReturnNotFound_WhenFriendshipNotFound 'UserAccountService.Tests.Controllers.FriendsControllerTest.AcceptFriendRequest_ShouldReturnNotFound_WhenFriendshipNotFound')
  - [AcceptFriendRequest_ShouldReturnOk_WhenAcceptanceIsSuccessful()](#M-UserAccountService-Tests-Controllers-FriendsControllerTest-AcceptFriendRequest_ShouldReturnOk_WhenAcceptanceIsSuccessful 'UserAccountService.Tests.Controllers.FriendsControllerTest.AcceptFriendRequest_ShouldReturnOk_WhenAcceptanceIsSuccessful')
  - [ControllerMethods_ShouldLogError_WhenUserIdClaimIsInvalid()](#M-UserAccountService-Tests-Controllers-FriendsControllerTest-ControllerMethods_ShouldLogError_WhenUserIdClaimIsInvalid 'UserAccountService.Tests.Controllers.FriendsControllerTest.ControllerMethods_ShouldLogError_WhenUserIdClaimIsInvalid')
  - [ControllerMethods_ShouldThrowInvalidOperationException_WhenUserIdClaimIsInvalid()](#M-UserAccountService-Tests-Controllers-FriendsControllerTest-ControllerMethods_ShouldThrowInvalidOperationException_WhenUserIdClaimIsInvalid 'UserAccountService.Tests.Controllers.FriendsControllerTest.ControllerMethods_ShouldThrowInvalidOperationException_WhenUserIdClaimIsInvalid')
  - [DeclineFriendRequest_ShouldReturnBadRequest_ForOtherDeclineFailures()](#M-UserAccountService-Tests-Controllers-FriendsControllerTest-DeclineFriendRequest_ShouldReturnBadRequest_ForOtherDeclineFailures 'UserAccountService.Tests.Controllers.FriendsControllerTest.DeclineFriendRequest_ShouldReturnBadRequest_ForOtherDeclineFailures')
  - [DeclineFriendRequest_ShouldReturnForbid_WhenDeclineNotAuthorized()](#M-UserAccountService-Tests-Controllers-FriendsControllerTest-DeclineFriendRequest_ShouldReturnForbid_WhenDeclineNotAuthorized 'UserAccountService.Tests.Controllers.FriendsControllerTest.DeclineFriendRequest_ShouldReturnForbid_WhenDeclineNotAuthorized')
  - [DeclineFriendRequest_ShouldReturnNotFound_WhenDeclineFriendshipNotFound()](#M-UserAccountService-Tests-Controllers-FriendsControllerTest-DeclineFriendRequest_ShouldReturnNotFound_WhenDeclineFriendshipNotFound 'UserAccountService.Tests.Controllers.FriendsControllerTest.DeclineFriendRequest_ShouldReturnNotFound_WhenDeclineFriendshipNotFound')
  - [DeclineFriendRequest_ShouldReturnOk_WhenDeclinedSuccessfully()](#M-UserAccountService-Tests-Controllers-FriendsControllerTest-DeclineFriendRequest_ShouldReturnOk_WhenDeclinedSuccessfully 'UserAccountService.Tests.Controllers.FriendsControllerTest.DeclineFriendRequest_ShouldReturnOk_WhenDeclinedSuccessfully')
  - [GetFriends_ShouldReturnOkWithEmptyList_WhenNoFriends()](#M-UserAccountService-Tests-Controllers-FriendsControllerTest-GetFriends_ShouldReturnOkWithEmptyList_WhenNoFriends 'UserAccountService.Tests.Controllers.FriendsControllerTest.GetFriends_ShouldReturnOkWithEmptyList_WhenNoFriends')
  - [GetFriends_ShouldReturnOkWithFriendsList()](#M-UserAccountService-Tests-Controllers-FriendsControllerTest-GetFriends_ShouldReturnOkWithFriendsList 'UserAccountService.Tests.Controllers.FriendsControllerTest.GetFriends_ShouldReturnOkWithFriendsList')
  - [GetFriendshipStatus_ShouldReturnCorrectStatus_WhenFriendshipExists()](#M-UserAccountService-Tests-Controllers-FriendsControllerTest-GetFriendshipStatus_ShouldReturnCorrectStatus_WhenFriendshipExists 'UserAccountService.Tests.Controllers.FriendsControllerTest.GetFriendshipStatus_ShouldReturnCorrectStatus_WhenFriendshipExists')
  - [GetFriendshipStatus_ShouldReturnNoneStatus_WhenNoFriendshipExists()](#M-UserAccountService-Tests-Controllers-FriendsControllerTest-GetFriendshipStatus_ShouldReturnNoneStatus_WhenNoFriendshipExists 'UserAccountService.Tests.Controllers.FriendsControllerTest.GetFriendshipStatus_ShouldReturnNoneStatus_WhenNoFriendshipExists')
  - [GetFriendshipStatus_ShouldReturnSelfStatus_WhenCheckingOwnId()](#M-UserAccountService-Tests-Controllers-FriendsControllerTest-GetFriendshipStatus_ShouldReturnSelfStatus_WhenCheckingOwnId 'UserAccountService.Tests.Controllers.FriendsControllerTest.GetFriendshipStatus_ShouldReturnSelfStatus_WhenCheckingOwnId')
  - [GetPendingIncomingRequests_ShouldReturnOkWithEmptyList_WhenNoRequests()](#M-UserAccountService-Tests-Controllers-FriendsControllerTest-GetPendingIncomingRequests_ShouldReturnOkWithEmptyList_WhenNoRequests 'UserAccountService.Tests.Controllers.FriendsControllerTest.GetPendingIncomingRequests_ShouldReturnOkWithEmptyList_WhenNoRequests')
  - [GetPendingIncomingRequests_ShouldReturnOkWithRequestsList()](#M-UserAccountService-Tests-Controllers-FriendsControllerTest-GetPendingIncomingRequests_ShouldReturnOkWithRequestsList 'UserAccountService.Tests.Controllers.FriendsControllerTest.GetPendingIncomingRequests_ShouldReturnOkWithRequestsList')
  - [GetPendingOutgoingRequests_ShouldReturnOkWithEmptyList_WhenNoRequests()](#M-UserAccountService-Tests-Controllers-FriendsControllerTest-GetPendingOutgoingRequests_ShouldReturnOkWithEmptyList_WhenNoRequests 'UserAccountService.Tests.Controllers.FriendsControllerTest.GetPendingOutgoingRequests_ShouldReturnOkWithEmptyList_WhenNoRequests')
  - [GetPendingOutgoingRequests_ShouldReturnOkWithRequestsList()](#M-UserAccountService-Tests-Controllers-FriendsControllerTest-GetPendingOutgoingRequests_ShouldReturnOkWithRequestsList 'UserAccountService.Tests.Controllers.FriendsControllerTest.GetPendingOutgoingRequests_ShouldReturnOkWithRequestsList')
  - [GetPropertyValue(src,propName)](#M-UserAccountService-Tests-Controllers-FriendsControllerTest-GetPropertyValue-System-Object,System-String- 'UserAccountService.Tests.Controllers.FriendsControllerTest.GetPropertyValue(System.Object,System.String)')
  - [RemoveFriend_ShouldReturnBadRequest_ForOtherFailures()](#M-UserAccountService-Tests-Controllers-FriendsControllerTest-RemoveFriend_ShouldReturnBadRequest_ForOtherFailures 'UserAccountService.Tests.Controllers.FriendsControllerTest.RemoveFriend_ShouldReturnBadRequest_ForOtherFailures')
  - [RemoveFriend_ShouldReturnNotFound_WhenFriendshipNotFound()](#M-UserAccountService-Tests-Controllers-FriendsControllerTest-RemoveFriend_ShouldReturnNotFound_WhenFriendshipNotFound 'UserAccountService.Tests.Controllers.FriendsControllerTest.RemoveFriend_ShouldReturnNotFound_WhenFriendshipNotFound')
  - [RemoveFriend_ShouldReturnOk_WhenRemovedSuccessfully()](#M-UserAccountService-Tests-Controllers-FriendsControllerTest-RemoveFriend_ShouldReturnOk_WhenRemovedSuccessfully 'UserAccountService.Tests.Controllers.FriendsControllerTest.RemoveFriend_ShouldReturnOk_WhenRemovedSuccessfully')
  - [SendFriendRequest_ShouldReturnBadRequest_WhenRequestFails()](#M-UserAccountService-Tests-Controllers-FriendsControllerTest-SendFriendRequest_ShouldReturnBadRequest_WhenRequestFails 'UserAccountService.Tests.Controllers.FriendsControllerTest.SendFriendRequest_ShouldReturnBadRequest_WhenRequestFails')
  - [SendFriendRequest_ShouldReturnOk_WhenRequestIsSuccessful()](#M-UserAccountService-Tests-Controllers-FriendsControllerTest-SendFriendRequest_ShouldReturnOk_WhenRequestIsSuccessful 'UserAccountService.Tests.Controllers.FriendsControllerTest.SendFriendRequest_ShouldReturnOk_WhenRequestIsSuccessful')
- [FriendshipHubTests](#T-UserAccountService-Tests-Hubs-FriendshipHubTests 'UserAccountService.Tests.Hubs.FriendshipHubTests')
  - [#ctor()](#M-UserAccountService-Tests-Hubs-FriendshipHubTests-#ctor 'UserAccountService.Tests.Hubs.FriendshipHubTests.#ctor')
  - [OnConnectedAsync_WithEmptyUserId_DoesNotAddUserToGroup()](#M-UserAccountService-Tests-Hubs-FriendshipHubTests-OnConnectedAsync_WithEmptyUserId_DoesNotAddUserToGroup 'UserAccountService.Tests.Hubs.FriendshipHubTests.OnConnectedAsync_WithEmptyUserId_DoesNotAddUserToGroup')
  - [OnConnectedAsync_WithValidUserId_AddsUserToGroup()](#M-UserAccountService-Tests-Hubs-FriendshipHubTests-OnConnectedAsync_WithValidUserId_AddsUserToGroup 'UserAccountService.Tests.Hubs.FriendshipHubTests.OnConnectedAsync_WithValidUserId_AddsUserToGroup')
  - [OnDisconnectedAsync_WithValidUserId_RemovesUserFromGroup()](#M-UserAccountService-Tests-Hubs-FriendshipHubTests-OnDisconnectedAsync_WithValidUserId_RemovesUserFromGroup 'UserAccountService.Tests.Hubs.FriendshipHubTests.OnDisconnectedAsync_WithValidUserId_RemovesUserFromGroup')
- [FriendshipServiceTest](#T-UserAccountService-Tests-Services-FriendshipServiceTest 'UserAccountService.Tests.Services.FriendshipServiceTest')
  - [#ctor()](#M-UserAccountService-Tests-Services-FriendshipServiceTest-#ctor 'UserAccountService.Tests.Services.FriendshipServiceTest.#ctor')
  - [AcceptFriendRequestAsync_ByUserNotAddressee_ReturnsError()](#M-UserAccountService-Tests-Services-FriendshipServiceTest-AcceptFriendRequestAsync_ByUserNotAddressee_ReturnsError 'UserAccountService.Tests.Services.FriendshipServiceTest.AcceptFriendRequestAsync_ByUserNotAddressee_ReturnsError')
  - [AcceptFriendRequestAsync_WithNonExistentRequest_ReturnsError()](#M-UserAccountService-Tests-Services-FriendshipServiceTest-AcceptFriendRequestAsync_WithNonExistentRequest_ReturnsError 'UserAccountService.Tests.Services.FriendshipServiceTest.AcceptFriendRequestAsync_WithNonExistentRequest_ReturnsError')
  - [AcceptFriendRequestAsync_WithValidRequest_AcceptsAndNotifiesBothUsers()](#M-UserAccountService-Tests-Services-FriendshipServiceTest-AcceptFriendRequestAsync_WithValidRequest_AcceptsAndNotifiesBothUsers 'UserAccountService.Tests.Services.FriendshipServiceTest.AcceptFriendRequestAsync_WithValidRequest_AcceptsAndNotifiesBothUsers')
  - [DeclineFriendRequestAsync_ByAddressee_RemovesRequestAndNotifies()](#M-UserAccountService-Tests-Services-FriendshipServiceTest-DeclineFriendRequestAsync_ByAddressee_RemovesRequestAndNotifies 'UserAccountService.Tests.Services.FriendshipServiceTest.DeclineFriendRequestAsync_ByAddressee_RemovesRequestAndNotifies')
  - [DeclineFriendRequestAsync_ByUnauthorizedUser_ReturnsError()](#M-UserAccountService-Tests-Services-FriendshipServiceTest-DeclineFriendRequestAsync_ByUnauthorizedUser_ReturnsError 'UserAccountService.Tests.Services.FriendshipServiceTest.DeclineFriendRequestAsync_ByUnauthorizedUser_ReturnsError')
  - [DeclineFriendRequestAsync_WithNonExistentRequest_ReturnsError()](#M-UserAccountService-Tests-Services-FriendshipServiceTest-DeclineFriendRequestAsync_WithNonExistentRequest_ReturnsError 'UserAccountService.Tests.Services.FriendshipServiceTest.DeclineFriendRequestAsync_WithNonExistentRequest_ReturnsError')
  - [Dispose()](#M-UserAccountService-Tests-Services-FriendshipServiceTest-Dispose 'UserAccountService.Tests.Services.FriendshipServiceTest.Dispose')
  - [GetFriendsAsync_ShouldReturnEmptyList_WhenNoFriends()](#M-UserAccountService-Tests-Services-FriendshipServiceTest-GetFriendsAsync_ShouldReturnEmptyList_WhenNoFriends 'UserAccountService.Tests.Services.FriendshipServiceTest.GetFriendsAsync_ShouldReturnEmptyList_WhenNoFriends')
  - [GetFriendsAsync_ShouldReturnListOfFriends()](#M-UserAccountService-Tests-Services-FriendshipServiceTest-GetFriendsAsync_ShouldReturnListOfFriends 'UserAccountService.Tests.Services.FriendshipServiceTest.GetFriendsAsync_ShouldReturnListOfFriends')
  - [GetFriendshipStatusAsync_ShouldReturnCorrectStatus()](#M-UserAccountService-Tests-Services-FriendshipServiceTest-GetFriendshipStatusAsync_ShouldReturnCorrectStatus-UserAccountService-Models-FriendshipStatus- 'UserAccountService.Tests.Services.FriendshipServiceTest.GetFriendshipStatusAsync_ShouldReturnCorrectStatus(UserAccountService.Models.FriendshipStatus)')
  - [GetFriendshipStatusAsync_ShouldReturnNull_WhenNoFriendship()](#M-UserAccountService-Tests-Services-FriendshipServiceTest-GetFriendshipStatusAsync_ShouldReturnNull_WhenNoFriendship 'UserAccountService.Tests.Services.FriendshipServiceTest.GetFriendshipStatusAsync_ShouldReturnNull_WhenNoFriendship')
  - [GetPendingIncomingRequestsAsync_ShouldReturnIncomingRequests()](#M-UserAccountService-Tests-Services-FriendshipServiceTest-GetPendingIncomingRequestsAsync_ShouldReturnIncomingRequests 'UserAccountService.Tests.Services.FriendshipServiceTest.GetPendingIncomingRequestsAsync_ShouldReturnIncomingRequests')
  - [GetPendingOutgoingRequestsAsync_ShouldReturnOutgoingRequests()](#M-UserAccountService-Tests-Services-FriendshipServiceTest-GetPendingOutgoingRequestsAsync_ShouldReturnOutgoingRequests 'UserAccountService.Tests.Services.FriendshipServiceTest.GetPendingOutgoingRequestsAsync_ShouldReturnOutgoingRequests')
  - [RemoveFriendAsync_WithExistingFriendship_RemovesFriendAndNotifies()](#M-UserAccountService-Tests-Services-FriendshipServiceTest-RemoveFriendAsync_WithExistingFriendship_RemovesFriendAndNotifies 'UserAccountService.Tests.Services.FriendshipServiceTest.RemoveFriendAsync_WithExistingFriendship_RemovesFriendAndNotifies')
  - [RemoveFriendAsync_WithNonExistentFriendship_ReturnsError()](#M-UserAccountService-Tests-Services-FriendshipServiceTest-RemoveFriendAsync_WithNonExistentFriendship_ReturnsError 'UserAccountService.Tests.Services.FriendshipServiceTest.RemoveFriendAsync_WithNonExistentFriendship_ReturnsError')
  - [SendFriendRequestAsync_FromNonExistentUser_ThrowsExceptionDueToBug()](#M-UserAccountService-Tests-Services-FriendshipServiceTest-SendFriendRequestAsync_FromNonExistentUser_ThrowsExceptionDueToBug 'UserAccountService.Tests.Services.FriendshipServiceTest.SendFriendRequestAsync_FromNonExistentUser_ThrowsExceptionDueToBug')
  - [SendFriendRequestAsync_ToNonExistentUser_ReturnsError()](#M-UserAccountService-Tests-Services-FriendshipServiceTest-SendFriendRequestAsync_ToNonExistentUser_ReturnsError 'UserAccountService.Tests.Services.FriendshipServiceTest.SendFriendRequestAsync_ToNonExistentUser_ReturnsError')
  - [SendFriendRequestAsync_ToSelf_ReturnsError()](#M-UserAccountService-Tests-Services-FriendshipServiceTest-SendFriendRequestAsync_ToSelf_ReturnsError 'UserAccountService.Tests.Services.FriendshipServiceTest.SendFriendRequestAsync_ToSelf_ReturnsError')
  - [SendFriendRequestAsync_WhenFriendshipExists_ReturnsError()](#M-UserAccountService-Tests-Services-FriendshipServiceTest-SendFriendRequestAsync_WhenFriendshipExists_ReturnsError 'UserAccountService.Tests.Services.FriendshipServiceTest.SendFriendRequestAsync_WhenFriendshipExists_ReturnsError')
  - [SendFriendRequestAsync_WithValidUsers_CreatesPendingFriendshipAndNotifies()](#M-UserAccountService-Tests-Services-FriendshipServiceTest-SendFriendRequestAsync_WithValidUsers_CreatesPendingFriendshipAndNotifies 'UserAccountService.Tests.Services.FriendshipServiceTest.SendFriendRequestAsync_WithValidUsers_CreatesPendingFriendshipAndNotifies')
- [TokenServiceTest](#T-UserAccountService-Tests-Services-TokenServiceTest 'UserAccountService.Tests.Services.TokenServiceTest')
  - [#ctor()](#M-UserAccountService-Tests-Services-TokenServiceTest-#ctor 'UserAccountService.Tests.Services.TokenServiceTest.#ctor')
  - [Dispose()](#M-UserAccountService-Tests-Services-TokenServiceTest-Dispose 'UserAccountService.Tests.Services.TokenServiceTest.Dispose')
  - [GenerateTokens_ReturnsValidTokensAndSavesRefreshToken()](#M-UserAccountService-Tests-Services-TokenServiceTest-GenerateTokens_ReturnsValidTokensAndSavesRefreshToken 'UserAccountService.Tests.Services.TokenServiceTest.GenerateTokens_ReturnsValidTokensAndSavesRefreshToken')
  - [InvalidateRefreshTokenAsync_WithAlreadyInvalidatedRefreshToken_ReturnsFalse()](#M-UserAccountService-Tests-Services-TokenServiceTest-InvalidateRefreshTokenAsync_WithAlreadyInvalidatedRefreshToken_ReturnsFalse 'UserAccountService.Tests.Services.TokenServiceTest.InvalidateRefreshTokenAsync_WithAlreadyInvalidatedRefreshToken_ReturnsFalse')
  - [InvalidateRefreshTokenAsync_WithInvalidRefreshToken_ReturnsFalse()](#M-UserAccountService-Tests-Services-TokenServiceTest-InvalidateRefreshTokenAsync_WithInvalidRefreshToken_ReturnsFalse 'UserAccountService.Tests.Services.TokenServiceTest.InvalidateRefreshTokenAsync_WithInvalidRefreshToken_ReturnsFalse')
  - [InvalidateRefreshTokenAsync_WithUsedRefreshToken_ReturnsFalse()](#M-UserAccountService-Tests-Services-TokenServiceTest-InvalidateRefreshTokenAsync_WithUsedRefreshToken_ReturnsFalse 'UserAccountService.Tests.Services.TokenServiceTest.InvalidateRefreshTokenAsync_WithUsedRefreshToken_ReturnsFalse')
  - [InvalidateRefreshTokenAsync_WithValidRefreshToken_InvalidatesToken()](#M-UserAccountService-Tests-Services-TokenServiceTest-InvalidateRefreshTokenAsync_WithValidRefreshToken_InvalidatesToken 'UserAccountService.Tests.Services.TokenServiceTest.InvalidateRefreshTokenAsync_WithValidRefreshToken_InvalidatesToken')
  - [RefreshAccessTokenAsync_WithExpiredRefreshToken_ReturnsNull()](#M-UserAccountService-Tests-Services-TokenServiceTest-RefreshAccessTokenAsync_WithExpiredRefreshToken_ReturnsNull 'UserAccountService.Tests.Services.TokenServiceTest.RefreshAccessTokenAsync_WithExpiredRefreshToken_ReturnsNull')
  - [RefreshAccessTokenAsync_WithInvalidRefreshToken_ReturnsNull()](#M-UserAccountService-Tests-Services-TokenServiceTest-RefreshAccessTokenAsync_WithInvalidRefreshToken_ReturnsNull 'UserAccountService.Tests.Services.TokenServiceTest.RefreshAccessTokenAsync_WithInvalidRefreshToken_ReturnsNull')
  - [RefreshAccessTokenAsync_WithInvalidatedRefreshToken_ReturnsNull()](#M-UserAccountService-Tests-Services-TokenServiceTest-RefreshAccessTokenAsync_WithInvalidatedRefreshToken_ReturnsNull 'UserAccountService.Tests.Services.TokenServiceTest.RefreshAccessTokenAsync_WithInvalidatedRefreshToken_ReturnsNull')
  - [RefreshAccessTokenAsync_WithUsedRefreshToken_ReturnsNull()](#M-UserAccountService-Tests-Services-TokenServiceTest-RefreshAccessTokenAsync_WithUsedRefreshToken_ReturnsNull 'UserAccountService.Tests.Services.TokenServiceTest.RefreshAccessTokenAsync_WithUsedRefreshToken_ReturnsNull')
  - [RefreshAccessTokenAsync_WithValidRefreshToken_ReturnsNewTokens()](#M-UserAccountService-Tests-Services-TokenServiceTest-RefreshAccessTokenAsync_WithValidRefreshToken_ReturnsNewTokens 'UserAccountService.Tests.Services.TokenServiceTest.RefreshAccessTokenAsync_WithValidRefreshToken_ReturnsNewTokens')
- [UserServiceTest](#T-UserAccountService-Tests-Services-UserServiceTest 'UserAccountService.Tests.Services.UserServiceTest')
  - [#ctor()](#M-UserAccountService-Tests-Services-UserServiceTest-#ctor 'UserAccountService.Tests.Services.UserServiceTest.#ctor')
  - [AddTestUser(name,tag,email,password)](#M-UserAccountService-Tests-Services-UserServiceTest-AddTestUser-System-String,System-String,System-String,System-String- 'UserAccountService.Tests.Services.UserServiceTest.AddTestUser(System.String,System.String,System.String,System.String)')
  - [AuthenticateUserAsync_ShouldReturnNull_ForIncorrectPassword()](#M-UserAccountService-Tests-Services-UserServiceTest-AuthenticateUserAsync_ShouldReturnNull_ForIncorrectPassword 'UserAccountService.Tests.Services.UserServiceTest.AuthenticateUserAsync_ShouldReturnNull_ForIncorrectPassword')
  - [AuthenticateUserAsync_ShouldReturnNull_ForNonExistentUser()](#M-UserAccountService-Tests-Services-UserServiceTest-AuthenticateUserAsync_ShouldReturnNull_ForNonExistentUser 'UserAccountService.Tests.Services.UserServiceTest.AuthenticateUserAsync_ShouldReturnNull_ForNonExistentUser')
  - [AuthenticateUserAsync_ShouldReturnUser_WhenCredentialsAreCorrect()](#M-UserAccountService-Tests-Services-UserServiceTest-AuthenticateUserAsync_ShouldReturnUser_WhenCredentialsAreCorrect 'UserAccountService.Tests.Services.UserServiceTest.AuthenticateUserAsync_ShouldReturnUser_WhenCredentialsAreCorrect')
  - [Dispose()](#M-UserAccountService-Tests-Services-UserServiceTest-Dispose 'UserAccountService.Tests.Services.UserServiceTest.Dispose')
  - [GetUserByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()](#M-UserAccountService-Tests-Services-UserServiceTest-GetUserByIdAsync_ShouldReturnNull_WhenUserDoesNotExist 'UserAccountService.Tests.Services.UserServiceTest.GetUserByIdAsync_ShouldReturnNull_WhenUserDoesNotExist')
  - [GetUserByIdAsync_ShouldReturnUser_WhenUserExists()](#M-UserAccountService-Tests-Services-UserServiceTest-GetUserByIdAsync_ShouldReturnUser_WhenUserExists 'UserAccountService.Tests.Services.UserServiceTest.GetUserByIdAsync_ShouldReturnUser_WhenUserExists')
  - [RegisterUserAsync_ShouldRegisterUser_WhenUserDoesNotExist()](#M-UserAccountService-Tests-Services-UserServiceTest-RegisterUserAsync_ShouldRegisterUser_WhenUserDoesNotExist 'UserAccountService.Tests.Services.UserServiceTest.RegisterUserAsync_ShouldRegisterUser_WhenUserDoesNotExist')
  - [RegisterUserAsync_ShouldReturnNull_WhenUserWithSameEmailExists()](#M-UserAccountService-Tests-Services-UserServiceTest-RegisterUserAsync_ShouldReturnNull_WhenUserWithSameEmailExists 'UserAccountService.Tests.Services.UserServiceTest.RegisterUserAsync_ShouldReturnNull_WhenUserWithSameEmailExists')
  - [RegisterUserAsync_ShouldReturnNull_WhenUserWithSameTagExists()](#M-UserAccountService-Tests-Services-UserServiceTest-RegisterUserAsync_ShouldReturnNull_WhenUserWithSameTagExists 'UserAccountService.Tests.Services.UserServiceTest.RegisterUserAsync_ShouldReturnNull_WhenUserWithSameTagExists')
  - [SearchUsersAsync_ShouldBeCaseInsensitive()](#M-UserAccountService-Tests-Services-UserServiceTest-SearchUsersAsync_ShouldBeCaseInsensitive 'UserAccountService.Tests.Services.UserServiceTest.SearchUsersAsync_ShouldBeCaseInsensitive')
  - [SearchUsersAsync_ShouldReturnEmpty_WhenNoMatches()](#M-UserAccountService-Tests-Services-UserServiceTest-SearchUsersAsync_ShouldReturnEmpty_WhenNoMatches 'UserAccountService.Tests.Services.UserServiceTest.SearchUsersAsync_ShouldReturnEmpty_WhenNoMatches')
  - [SearchUsersAsync_ShouldReturnEmpty_WhenQueryIsWhitespace()](#M-UserAccountService-Tests-Services-UserServiceTest-SearchUsersAsync_ShouldReturnEmpty_WhenQueryIsWhitespace 'UserAccountService.Tests.Services.UserServiceTest.SearchUsersAsync_ShouldReturnEmpty_WhenQueryIsWhitespace')
  - [SearchUsersAsync_ShouldReturnMatchingUsers()](#M-UserAccountService-Tests-Services-UserServiceTest-SearchUsersAsync_ShouldReturnMatchingUsers 'UserAccountService.Tests.Services.UserServiceTest.SearchUsersAsync_ShouldReturnMatchingUsers')
- [UsersControllerTest](#T-UserAccountService-Tests-Controllers-UsersControllerTest 'UserAccountService.Tests.Controllers.UsersControllerTest')
  - [GetMe_ShouldReturnNotFound_WhenUserDoesNotExist()](#M-UserAccountService-Tests-Controllers-UsersControllerTest-GetMe_ShouldReturnNotFound_WhenUserDoesNotExist 'UserAccountService.Tests.Controllers.UsersControllerTest.GetMe_ShouldReturnNotFound_WhenUserDoesNotExist')
  - [GetMe_ShouldReturnOkWithUserDto_WhenUserIsAuthenticated()](#M-UserAccountService-Tests-Controllers-UsersControllerTest-GetMe_ShouldReturnOkWithUserDto_WhenUserIsAuthenticated 'UserAccountService.Tests.Controllers.UsersControllerTest.GetMe_ShouldReturnOkWithUserDto_WhenUserIsAuthenticated')
  - [GetMe_ShouldReturnUnauthorized_WhenUserIdClaimIsMissing()](#M-UserAccountService-Tests-Controllers-UsersControllerTest-GetMe_ShouldReturnUnauthorized_WhenUserIdClaimIsMissing 'UserAccountService.Tests.Controllers.UsersControllerTest.GetMe_ShouldReturnUnauthorized_WhenUserIdClaimIsMissing')
  - [GetUserById_ShouldReturnNotFound_WhenUserDoesNotExist()](#M-UserAccountService-Tests-Controllers-UsersControllerTest-GetUserById_ShouldReturnNotFound_WhenUserDoesNotExist 'UserAccountService.Tests.Controllers.UsersControllerTest.GetUserById_ShouldReturnNotFound_WhenUserDoesNotExist')
  - [GetUserById_ShouldReturnOkWithUserDto_WhenUserExists()](#M-UserAccountService-Tests-Controllers-UsersControllerTest-GetUserById_ShouldReturnOkWithUserDto_WhenUserExists 'UserAccountService.Tests.Controllers.UsersControllerTest.GetUserById_ShouldReturnOkWithUserDto_WhenUserExists')
  - [SearchUsers_ShouldReturnBadRequest_WhenQueryIsEmpty()](#M-UserAccountService-Tests-Controllers-UsersControllerTest-SearchUsers_ShouldReturnBadRequest_WhenQueryIsEmpty 'UserAccountService.Tests.Controllers.UsersControllerTest.SearchUsers_ShouldReturnBadRequest_WhenQueryIsEmpty')
  - [SearchUsers_ShouldReturnOkWithEmptyList_WhenNoUsersMatchQuery()](#M-UserAccountService-Tests-Controllers-UsersControllerTest-SearchUsers_ShouldReturnOkWithEmptyList_WhenNoUsersMatchQuery 'UserAccountService.Tests.Controllers.UsersControllerTest.SearchUsers_ShouldReturnOkWithEmptyList_WhenNoUsersMatchQuery')
  - [SearchUsers_ShouldReturnOkWithUserDtos_WhenQueryIsValid()](#M-UserAccountService-Tests-Controllers-UsersControllerTest-SearchUsers_ShouldReturnOkWithUserDtos_WhenQueryIsValid 'UserAccountService.Tests.Controllers.UsersControllerTest.SearchUsers_ShouldReturnOkWithUserDtos_WhenQueryIsValid')

<a name='T-UserAccountService-Tests-Controllers-AuthControllerTest'></a>
## AuthControllerTest `type`

##### Namespace

UserAccountService.Tests.Controllers

##### Summary

Unit tests for the AuthController class.

<a name='M-UserAccountService-Tests-Controllers-AuthControllerTest-#ctor'></a>
### #ctor() `constructor`

##### Summary

Initializes a new instance of the AuthControllerTest class.
Sets up mocks and an in-memory database for testing.

##### Parameters

This constructor has no parameters.

<a name='M-UserAccountService-Tests-Controllers-AuthControllerTest-Dispose'></a>
### Dispose() `method`

##### Summary

Cleans up resources after each test.
Ensures the in-memory database is deleted and disposed.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Controllers-AuthControllerTest-Login_WithInvalidCredentials_ReturnsUnauthorized'></a>
### Login_WithInvalidCredentials_ReturnsUnauthorized() `method`

##### Summary

Tests the Login method with invalid credentials.
Verifies that an UnauthorizedObjectResult is returned with an appropriate error message.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Controllers-AuthControllerTest-Login_WithInvalidModelState_ReturnsBadRequest'></a>
### Login_WithInvalidModelState_ReturnsBadRequest() `method`

##### Summary

Tests the Login method with invalid model state.
Verifies that a BadRequestObjectResult is returned and no authentication is attempted.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Controllers-AuthControllerTest-Login_WithValidCredentials_ReturnsOkWithTokens'></a>
### Login_WithValidCredentials_ReturnsOkWithTokens() `method`

##### Summary

Tests the Login method with valid credentials.
Verifies that an OkObjectResult is returned with access and refresh tokens.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Controllers-AuthControllerTest-Logout_WithInvalidModelState_ReturnsBadRequest'></a>
### Logout_WithInvalidModelState_ReturnsBadRequest() `method`

##### Summary

Tests the Logout method with invalid model state.
Verifies that a BadRequestObjectResult is returned and no token invalidation is attempted.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Controllers-AuthControllerTest-Logout_WithInvalidRefreshToken_ReturnsBadRequest'></a>
### Logout_WithInvalidRefreshToken_ReturnsBadRequest() `method`

##### Summary

Tests the Logout method with an invalid refresh token.
Verifies that a BadRequestObjectResult is returned with an appropriate error message.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Controllers-AuthControllerTest-Logout_WithValidRefreshToken_ReturnsOk'></a>
### Logout_WithValidRefreshToken_ReturnsOk() `method`

##### Summary

Tests the Logout method with a valid refresh token.
Verifies that an OkObjectResult is returned with a success message.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Controllers-AuthControllerTest-Refresh_WithInvalidModelState_ReturnsBadRequest'></a>
### Refresh_WithInvalidModelState_ReturnsBadRequest() `method`

##### Summary

Tests the Refresh method with invalid model state.
Verifies that a BadRequestObjectResult is returned and no token refresh is attempted.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Controllers-AuthControllerTest-Refresh_WithInvalidRefreshToken_ReturnsUnauthorized'></a>
### Refresh_WithInvalidRefreshToken_ReturnsUnauthorized() `method`

##### Summary

Tests the Refresh method with an invalid refresh token.
Verifies that an UnauthorizedObjectResult is returned with an appropriate error message.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Controllers-AuthControllerTest-Refresh_WithValidRefreshToken_ReturnsOkWithNewTokens'></a>
### Refresh_WithValidRefreshToken_ReturnsOkWithNewTokens() `method`

##### Summary

Tests the Refresh method with a valid refresh token.
Verifies that an OkObjectResult is returned with new access and refresh tokens.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Controllers-AuthControllerTest-Register_WithExistingUser_ReturnsBadRequest'></a>
### Register_WithExistingUser_ReturnsBadRequest() `method`

##### Summary

Tests the Register method with an existing user.
Verifies that a BadRequestObjectResult is returned with an appropriate error message.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Controllers-AuthControllerTest-Register_WithInvalidModelState_ReturnsBadRequest'></a>
### Register_WithInvalidModelState_ReturnsBadRequest() `method`

##### Summary

Tests the Register method with invalid model state.
Verifies that a BadRequestObjectResult is returned and no user registration is attempted.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Controllers-AuthControllerTest-Register_WithValidData_ReturnsCreatedAtAction'></a>
### Register_WithValidData_ReturnsCreatedAtAction() `method`

##### Summary

Tests the Register method with valid data.
Verifies that a CreatedAtActionResult is returned and the user is registered successfully.

##### Parameters

This method has no parameters.

<a name='T-UserAccountService-Tests-Controllers-FriendsControllerTest'></a>
## FriendsControllerTest `type`

##### Namespace

UserAccountService.Tests.Controllers

##### Summary

Unit tests for the FriendsController class.

<a name='M-UserAccountService-Tests-Controllers-FriendsControllerTest-#ctor'></a>
### #ctor() `constructor`

##### Summary

Initializes a new instance of the FriendsControllerTest class.
Sets up mocks and the controller context with a valid user ID claim.

##### Parameters

This constructor has no parameters.

<a name='M-UserAccountService-Tests-Controllers-FriendsControllerTest-AcceptFriendRequest_ShouldReturnBadRequest_ForOtherFailures'></a>
### AcceptFriendRequest_ShouldReturnBadRequest_ForOtherFailures() `method`

##### Summary

Tests that AcceptFriendRequest returns BadRequestObjectResult for other failures.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Controllers-FriendsControllerTest-AcceptFriendRequest_ShouldReturnForbid_WhenNotAuthorized'></a>
### AcceptFriendRequest_ShouldReturnForbid_WhenNotAuthorized() `method`

##### Summary

Tests that AcceptFriendRequest returns ForbidResult when the user is not authorized.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Controllers-FriendsControllerTest-AcceptFriendRequest_ShouldReturnNotFound_WhenFriendshipNotFound'></a>
### AcceptFriendRequest_ShouldReturnNotFound_WhenFriendshipNotFound() `method`

##### Summary

Tests that AcceptFriendRequest returns NotFoundObjectResult when the friendship is not found.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Controllers-FriendsControllerTest-AcceptFriendRequest_ShouldReturnOk_WhenAcceptanceIsSuccessful'></a>
### AcceptFriendRequest_ShouldReturnOk_WhenAcceptanceIsSuccessful() `method`

##### Summary

Tests that AcceptFriendRequest returns OkObjectResult when the acceptance is successful.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Controllers-FriendsControllerTest-ControllerMethods_ShouldLogError_WhenUserIdClaimIsInvalid'></a>
### ControllerMethods_ShouldLogError_WhenUserIdClaimIsInvalid() `method`

##### Summary

Tests that an error is logged when the User ID claim is invalid.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Controllers-FriendsControllerTest-ControllerMethods_ShouldThrowInvalidOperationException_WhenUserIdClaimIsInvalid'></a>
### ControllerMethods_ShouldThrowInvalidOperationException_WhenUserIdClaimIsInvalid() `method`

##### Summary

Tests that all controller methods throw an InvalidOperationException when the User ID claim is invalid.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Controllers-FriendsControllerTest-DeclineFriendRequest_ShouldReturnBadRequest_ForOtherDeclineFailures'></a>
### DeclineFriendRequest_ShouldReturnBadRequest_ForOtherDeclineFailures() `method`

##### Summary

Tests that DeclineFriendRequest returns BadRequestObjectResult for other failures.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Controllers-FriendsControllerTest-DeclineFriendRequest_ShouldReturnForbid_WhenDeclineNotAuthorized'></a>
### DeclineFriendRequest_ShouldReturnForbid_WhenDeclineNotAuthorized() `method`

##### Summary

Tests that DeclineFriendRequest returns ForbidResult when the user is not authorized.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Controllers-FriendsControllerTest-DeclineFriendRequest_ShouldReturnNotFound_WhenDeclineFriendshipNotFound'></a>
### DeclineFriendRequest_ShouldReturnNotFound_WhenDeclineFriendshipNotFound() `method`

##### Summary

Tests that DeclineFriendRequest returns NotFoundObjectResult when the friendship is not found.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Controllers-FriendsControllerTest-DeclineFriendRequest_ShouldReturnOk_WhenDeclinedSuccessfully'></a>
### DeclineFriendRequest_ShouldReturnOk_WhenDeclinedSuccessfully() `method`

##### Summary

Tests that DeclineFriendRequest returns OkObjectResult when the request is declined successfully.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Controllers-FriendsControllerTest-GetFriends_ShouldReturnOkWithEmptyList_WhenNoFriends'></a>
### GetFriends_ShouldReturnOkWithEmptyList_WhenNoFriends() `method`

##### Summary

Tests that GetFriends returns OkObjectResult with an empty list when there are no friends.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Controllers-FriendsControllerTest-GetFriends_ShouldReturnOkWithFriendsList'></a>
### GetFriends_ShouldReturnOkWithFriendsList() `method`

##### Summary

Tests that GetFriends returns OkObjectResult with a list of friends.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Controllers-FriendsControllerTest-GetFriendshipStatus_ShouldReturnCorrectStatus_WhenFriendshipExists'></a>
### GetFriendshipStatus_ShouldReturnCorrectStatus_WhenFriendshipExists() `method`

##### Summary

Tests that GetFriendshipStatus returns the correct status when a friendship exists.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Controllers-FriendsControllerTest-GetFriendshipStatus_ShouldReturnNoneStatus_WhenNoFriendshipExists'></a>
### GetFriendshipStatus_ShouldReturnNoneStatus_WhenNoFriendshipExists() `method`

##### Summary

Tests that GetFriendshipStatus returns "None" status when no friendship exists.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Controllers-FriendsControllerTest-GetFriendshipStatus_ShouldReturnSelfStatus_WhenCheckingOwnId'></a>
### GetFriendshipStatus_ShouldReturnSelfStatus_WhenCheckingOwnId() `method`

##### Summary

Tests that GetFriendshipStatus returns "Self" status when checking the current user's own ID.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Controllers-FriendsControllerTest-GetPendingIncomingRequests_ShouldReturnOkWithEmptyList_WhenNoRequests'></a>
### GetPendingIncomingRequests_ShouldReturnOkWithEmptyList_WhenNoRequests() `method`

##### Summary

Tests that GetPendingIncomingRequests returns OkObjectResult with an empty list when there are no incoming requests.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Controllers-FriendsControllerTest-GetPendingIncomingRequests_ShouldReturnOkWithRequestsList'></a>
### GetPendingIncomingRequests_ShouldReturnOkWithRequestsList() `method`

##### Summary

Tests that GetPendingIncomingRequests returns OkObjectResult with a list of incoming friend requests.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Controllers-FriendsControllerTest-GetPendingOutgoingRequests_ShouldReturnOkWithEmptyList_WhenNoRequests'></a>
### GetPendingOutgoingRequests_ShouldReturnOkWithEmptyList_WhenNoRequests() `method`

##### Summary

Tests that GetPendingOutgoingRequests returns OkObjectResult with an empty list when there are no outgoing requests.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Controllers-FriendsControllerTest-GetPendingOutgoingRequests_ShouldReturnOkWithRequestsList'></a>
### GetPendingOutgoingRequests_ShouldReturnOkWithRequestsList() `method`

##### Summary

Tests that GetPendingOutgoingRequests returns OkObjectResult with a list of outgoing friend requests.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Controllers-FriendsControllerTest-GetPropertyValue-System-Object,System-String-'></a>
### GetPropertyValue(src,propName) `method`

##### Summary

Retrieves the value of a specified property from an object.

##### Returns

The value of the property, or null if the property does not exist.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| src | [System.Object](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Object 'System.Object') | The source object. |
| propName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The name of the property to retrieve. |

<a name='M-UserAccountService-Tests-Controllers-FriendsControllerTest-RemoveFriend_ShouldReturnBadRequest_ForOtherFailures'></a>
### RemoveFriend_ShouldReturnBadRequest_ForOtherFailures() `method`

##### Summary

Tests that RemoveFriend returns BadRequestObjectResult for other failures.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Controllers-FriendsControllerTest-RemoveFriend_ShouldReturnNotFound_WhenFriendshipNotFound'></a>
### RemoveFriend_ShouldReturnNotFound_WhenFriendshipNotFound() `method`

##### Summary

Tests that RemoveFriend returns NotFoundObjectResult when the friendship is not found.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Controllers-FriendsControllerTest-RemoveFriend_ShouldReturnOk_WhenRemovedSuccessfully'></a>
### RemoveFriend_ShouldReturnOk_WhenRemovedSuccessfully() `method`

##### Summary

Tests that RemoveFriend returns OkObjectResult when the friend is removed successfully.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Controllers-FriendsControllerTest-SendFriendRequest_ShouldReturnBadRequest_WhenRequestFails'></a>
### SendFriendRequest_ShouldReturnBadRequest_WhenRequestFails() `method`

##### Summary

Tests that SendFriendRequest returns BadRequestObjectResult when the request fails.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Controllers-FriendsControllerTest-SendFriendRequest_ShouldReturnOk_WhenRequestIsSuccessful'></a>
### SendFriendRequest_ShouldReturnOk_WhenRequestIsSuccessful() `method`

##### Summary

Tests that SendFriendRequest returns OkObjectResult when the request is successful.

##### Parameters

This method has no parameters.

<a name='T-UserAccountService-Tests-Hubs-FriendshipHubTests'></a>
## FriendshipHubTests `type`

##### Namespace

UserAccountService.Tests.Hubs

##### Summary

Unit tests for the FriendshipHub class.

<a name='M-UserAccountService-Tests-Hubs-FriendshipHubTests-#ctor'></a>
### #ctor() `constructor`

##### Summary

Initializes the FriendshipHubTests class and sets up mock dependencies.

##### Parameters

This constructor has no parameters.

<a name='M-UserAccountService-Tests-Hubs-FriendshipHubTests-OnConnectedAsync_WithEmptyUserId_DoesNotAddUserToGroup'></a>
### OnConnectedAsync_WithEmptyUserId_DoesNotAddUserToGroup() `method`

##### Summary

Tests that OnConnectedAsync does not add the user to a group when the user ID is empty.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Hubs-FriendshipHubTests-OnConnectedAsync_WithValidUserId_AddsUserToGroup'></a>
### OnConnectedAsync_WithValidUserId_AddsUserToGroup() `method`

##### Summary

Tests that OnConnectedAsync adds the user to a group when a valid user ID is provided.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Hubs-FriendshipHubTests-OnDisconnectedAsync_WithValidUserId_RemovesUserFromGroup'></a>
### OnDisconnectedAsync_WithValidUserId_RemovesUserFromGroup() `method`

##### Summary

Tests that OnDisconnectedAsync does not remove the user from a group when a valid user ID is provided.

##### Parameters

This method has no parameters.

<a name='T-UserAccountService-Tests-Services-FriendshipServiceTest'></a>
## FriendshipServiceTest `type`

##### Namespace

UserAccountService.Tests.Services

##### Summary

Unit tests for the FriendshipService class.

<a name='M-UserAccountService-Tests-Services-FriendshipServiceTest-#ctor'></a>
### #ctor() `constructor`

##### Summary

Initializes the FriendshipServiceTest class and sets up mock dependencies and test data.

##### Parameters

This constructor has no parameters.

<a name='M-UserAccountService-Tests-Services-FriendshipServiceTest-AcceptFriendRequestAsync_ByUserNotAddressee_ReturnsError'></a>
### AcceptFriendRequestAsync_ByUserNotAddressee_ReturnsError() `method`

##### Summary

Tests that AcceptFriendRequestAsync returns an error if the user is not the addressee.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Services-FriendshipServiceTest-AcceptFriendRequestAsync_WithNonExistentRequest_ReturnsError'></a>
### AcceptFriendRequestAsync_WithNonExistentRequest_ReturnsError() `method`

##### Summary

Tests that AcceptFriendRequestAsync returns an error when the friend request does not exist.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Services-FriendshipServiceTest-AcceptFriendRequestAsync_WithValidRequest_AcceptsAndNotifiesBothUsers'></a>
### AcceptFriendRequestAsync_WithValidRequest_AcceptsAndNotifiesBothUsers() `method`

##### Summary

Tests that AcceptFriendRequestAsync accepts a valid friend request and notifies both users.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Services-FriendshipServiceTest-DeclineFriendRequestAsync_ByAddressee_RemovesRequestAndNotifies'></a>
### DeclineFriendRequestAsync_ByAddressee_RemovesRequestAndNotifies() `method`

##### Summary

Tests that DeclineFriendRequestAsync removes a pending friend request and notifies both users.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Services-FriendshipServiceTest-DeclineFriendRequestAsync_ByUnauthorizedUser_ReturnsError'></a>
### DeclineFriendRequestAsync_ByUnauthorizedUser_ReturnsError() `method`

##### Summary

Tests that DeclineFriendRequestAsync returns an error if the user is not authorized.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Services-FriendshipServiceTest-DeclineFriendRequestAsync_WithNonExistentRequest_ReturnsError'></a>
### DeclineFriendRequestAsync_WithNonExistentRequest_ReturnsError() `method`

##### Summary

Tests that DeclineFriendRequestAsync returns an error if the request does not exist.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Services-FriendshipServiceTest-Dispose'></a>
### Dispose() `method`

##### Summary

Cleans up resources used by the test class.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Services-FriendshipServiceTest-GetFriendsAsync_ShouldReturnEmptyList_WhenNoFriends'></a>
### GetFriendsAsync_ShouldReturnEmptyList_WhenNoFriends() `method`

##### Summary

Tests that GetFriendsAsync returns an empty list for a user with no friends.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Services-FriendshipServiceTest-GetFriendsAsync_ShouldReturnListOfFriends'></a>
### GetFriendsAsync_ShouldReturnListOfFriends() `method`

##### Summary

Tests that GetFriendsAsync returns a list of friends for a user.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Services-FriendshipServiceTest-GetFriendshipStatusAsync_ShouldReturnCorrectStatus-UserAccountService-Models-FriendshipStatus-'></a>
### GetFriendshipStatusAsync_ShouldReturnCorrectStatus() `method`

##### Summary

Tests GetFriendshipStatusAsync for various friendship statuses.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Services-FriendshipServiceTest-GetFriendshipStatusAsync_ShouldReturnNull_WhenNoFriendship'></a>
### GetFriendshipStatusAsync_ShouldReturnNull_WhenNoFriendship() `method`

##### Summary

Tests that GetFriendshipStatusAsync returns null when no friendship exists.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Services-FriendshipServiceTest-GetPendingIncomingRequestsAsync_ShouldReturnIncomingRequests'></a>
### GetPendingIncomingRequestsAsync_ShouldReturnIncomingRequests() `method`

##### Summary

Tests that GetPendingIncomingRequestsAsync returns a list of incoming friend requests.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Services-FriendshipServiceTest-GetPendingOutgoingRequestsAsync_ShouldReturnOutgoingRequests'></a>
### GetPendingOutgoingRequestsAsync_ShouldReturnOutgoingRequests() `method`

##### Summary

Tests that GetPendingOutgoingRequestsAsync returns a list of outgoing friend requests.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Services-FriendshipServiceTest-RemoveFriendAsync_WithExistingFriendship_RemovesFriendAndNotifies'></a>
### RemoveFriendAsync_WithExistingFriendship_RemovesFriendAndNotifies() `method`

##### Summary

Tests that RemoveFriendAsync removes an existing friendship and notifies the affected users.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Services-FriendshipServiceTest-RemoveFriendAsync_WithNonExistentFriendship_ReturnsError'></a>
### RemoveFriendAsync_WithNonExistentFriendship_ReturnsError() `method`

##### Summary

Tests that RemoveFriendAsync returns an error if the friendship does not exist.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Services-FriendshipServiceTest-SendFriendRequestAsync_FromNonExistentUser_ThrowsExceptionDueToBug'></a>
### SendFriendRequestAsync_FromNonExistentUser_ThrowsExceptionDueToBug() `method`

##### Summary

Tests that SendFriendRequestAsync throws NullReferenceException when the requester user does not exist, due to a bug in the service.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Services-FriendshipServiceTest-SendFriendRequestAsync_ToNonExistentUser_ReturnsError'></a>
### SendFriendRequestAsync_ToNonExistentUser_ReturnsError() `method`

##### Summary

Tests that SendFriendRequestAsync returns an error when the recipient user does not exist.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Services-FriendshipServiceTest-SendFriendRequestAsync_ToSelf_ReturnsError'></a>
### SendFriendRequestAsync_ToSelf_ReturnsError() `method`

##### Summary

Tests that SendFriendRequestAsync returns an error when attempting to send a friend request to oneself.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Services-FriendshipServiceTest-SendFriendRequestAsync_WhenFriendshipExists_ReturnsError'></a>
### SendFriendRequestAsync_WhenFriendshipExists_ReturnsError() `method`

##### Summary

Tests that SendFriendRequestAsync returns an error if a friendship already exists.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Services-FriendshipServiceTest-SendFriendRequestAsync_WithValidUsers_CreatesPendingFriendshipAndNotifies'></a>
### SendFriendRequestAsync_WithValidUsers_CreatesPendingFriendshipAndNotifies() `method`

##### Summary

Tests that SendFriendRequestAsync creates a pending friendship and notifies the recipient when valid users are provided.

##### Parameters

This method has no parameters.

<a name='T-UserAccountService-Tests-Services-TokenServiceTest'></a>
## TokenServiceTest `type`

##### Namespace

UserAccountService.Tests.Services

##### Summary

Unit tests for the TokenService class.

<a name='M-UserAccountService-Tests-Services-TokenServiceTest-#ctor'></a>
### #ctor() `constructor`

##### Summary

Initializes the TokenServiceTest class and sets up mock dependencies and test data.

##### Parameters

This constructor has no parameters.

<a name='M-UserAccountService-Tests-Services-TokenServiceTest-Dispose'></a>
### Dispose() `method`

##### Summary

Cleans up resources used by the test class.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Services-TokenServiceTest-GenerateTokens_ReturnsValidTokensAndSavesRefreshToken'></a>
### GenerateTokens_ReturnsValidTokensAndSavesRefreshToken() `method`

##### Summary

Tests that GenerateTokens returns valid access and refresh tokens and saves the refresh token in the database.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Services-TokenServiceTest-InvalidateRefreshTokenAsync_WithAlreadyInvalidatedRefreshToken_ReturnsFalse'></a>
### InvalidateRefreshTokenAsync_WithAlreadyInvalidatedRefreshToken_ReturnsFalse() `method`

##### Summary

Tests that InvalidateRefreshTokenAsync returns false when an already invalidated refresh token is provided.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Services-TokenServiceTest-InvalidateRefreshTokenAsync_WithInvalidRefreshToken_ReturnsFalse'></a>
### InvalidateRefreshTokenAsync_WithInvalidRefreshToken_ReturnsFalse() `method`

##### Summary

Tests that InvalidateRefreshTokenAsync returns false when an invalid refresh token is provided.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Services-TokenServiceTest-InvalidateRefreshTokenAsync_WithUsedRefreshToken_ReturnsFalse'></a>
### InvalidateRefreshTokenAsync_WithUsedRefreshToken_ReturnsFalse() `method`

##### Summary

Tests that InvalidateRefreshTokenAsync returns false when a used refresh token is provided.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Services-TokenServiceTest-InvalidateRefreshTokenAsync_WithValidRefreshToken_InvalidatesToken'></a>
### InvalidateRefreshTokenAsync_WithValidRefreshToken_InvalidatesToken() `method`

##### Summary

Tests that InvalidateRefreshTokenAsync invalidates a valid refresh token.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Services-TokenServiceTest-RefreshAccessTokenAsync_WithExpiredRefreshToken_ReturnsNull'></a>
### RefreshAccessTokenAsync_WithExpiredRefreshToken_ReturnsNull() `method`

##### Summary

Tests that RefreshAccessTokenAsync returns null when an expired refresh token is provided.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Services-TokenServiceTest-RefreshAccessTokenAsync_WithInvalidRefreshToken_ReturnsNull'></a>
### RefreshAccessTokenAsync_WithInvalidRefreshToken_ReturnsNull() `method`

##### Summary

Tests that RefreshAccessTokenAsync returns null when an invalid refresh token is provided.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Services-TokenServiceTest-RefreshAccessTokenAsync_WithInvalidatedRefreshToken_ReturnsNull'></a>
### RefreshAccessTokenAsync_WithInvalidatedRefreshToken_ReturnsNull() `method`

##### Summary

Tests that RefreshAccessTokenAsync returns null when an invalidated refresh token is provided.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Services-TokenServiceTest-RefreshAccessTokenAsync_WithUsedRefreshToken_ReturnsNull'></a>
### RefreshAccessTokenAsync_WithUsedRefreshToken_ReturnsNull() `method`

##### Summary

Tests that RefreshAccessTokenAsync returns null when a used refresh token is provided.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Services-TokenServiceTest-RefreshAccessTokenAsync_WithValidRefreshToken_ReturnsNewTokens'></a>
### RefreshAccessTokenAsync_WithValidRefreshToken_ReturnsNewTokens() `method`

##### Summary

Tests that RefreshAccessTokenAsync returns new tokens when a valid refresh token is provided.

##### Parameters

This method has no parameters.

<a name='T-UserAccountService-Tests-Services-UserServiceTest'></a>
## UserServiceTest `type`

##### Namespace

UserAccountService.Tests.Services

##### Summary

Unit tests for the UserService class.

<a name='M-UserAccountService-Tests-Services-UserServiceTest-#ctor'></a>
### #ctor() `constructor`

##### Summary

Initializes the UserServiceTest class and sets up mock dependencies and test data.

##### Parameters

This constructor has no parameters.

<a name='M-UserAccountService-Tests-Services-UserServiceTest-AddTestUser-System-String,System-String,System-String,System-String-'></a>
### AddTestUser(name,tag,email,password) `method`

##### Summary

Adds a test user to the database.

##### Returns

The added user.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| name | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The name of the user. |
| tag | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The tag of the user. |
| email | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The email of the user. |
| password | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The password of the user. |

<a name='M-UserAccountService-Tests-Services-UserServiceTest-AuthenticateUserAsync_ShouldReturnNull_ForIncorrectPassword'></a>
### AuthenticateUserAsync_ShouldReturnNull_ForIncorrectPassword() `method`

##### Summary

Tests that AuthenticateUserAsync returns null for an incorrect password.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Services-UserServiceTest-AuthenticateUserAsync_ShouldReturnNull_ForNonExistentUser'></a>
### AuthenticateUserAsync_ShouldReturnNull_ForNonExistentUser() `method`

##### Summary

Tests that AuthenticateUserAsync returns null for a non-existent user.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Services-UserServiceTest-AuthenticateUserAsync_ShouldReturnUser_WhenCredentialsAreCorrect'></a>
### AuthenticateUserAsync_ShouldReturnUser_WhenCredentialsAreCorrect() `method`

##### Summary

Tests that AuthenticateUserAsync returns the user when credentials are correct.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Services-UserServiceTest-Dispose'></a>
### Dispose() `method`

##### Summary

Cleans up resources used by the test class.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Services-UserServiceTest-GetUserByIdAsync_ShouldReturnNull_WhenUserDoesNotExist'></a>
### GetUserByIdAsync_ShouldReturnNull_WhenUserDoesNotExist() `method`

##### Summary

Tests that GetUserByIdAsync returns null when the user does not exist.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Services-UserServiceTest-GetUserByIdAsync_ShouldReturnUser_WhenUserExists'></a>
### GetUserByIdAsync_ShouldReturnUser_WhenUserExists() `method`

##### Summary

Tests that GetUserByIdAsync returns the correct user when the user exists.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Services-UserServiceTest-RegisterUserAsync_ShouldRegisterUser_WhenUserDoesNotExist'></a>
### RegisterUserAsync_ShouldRegisterUser_WhenUserDoesNotExist() `method`

##### Summary

Tests that RegisterUserAsync registers a user when the user does not already exist.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Services-UserServiceTest-RegisterUserAsync_ShouldReturnNull_WhenUserWithSameEmailExists'></a>
### RegisterUserAsync_ShouldReturnNull_WhenUserWithSameEmailExists() `method`

##### Summary

Tests that RegisterUserAsync returns null when a user with the same email already exists.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Services-UserServiceTest-RegisterUserAsync_ShouldReturnNull_WhenUserWithSameTagExists'></a>
### RegisterUserAsync_ShouldReturnNull_WhenUserWithSameTagExists() `method`

##### Summary

Tests that RegisterUserAsync returns null when a user with the same tag already exists.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Services-UserServiceTest-SearchUsersAsync_ShouldBeCaseInsensitive'></a>
### SearchUsersAsync_ShouldBeCaseInsensitive() `method`

##### Summary

Tests that SearchUsersAsync is case-insensitive.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Services-UserServiceTest-SearchUsersAsync_ShouldReturnEmpty_WhenNoMatches'></a>
### SearchUsersAsync_ShouldReturnEmpty_WhenNoMatches() `method`

##### Summary

Tests that SearchUsersAsync returns an empty list when no users match the query.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Services-UserServiceTest-SearchUsersAsync_ShouldReturnEmpty_WhenQueryIsWhitespace'></a>
### SearchUsersAsync_ShouldReturnEmpty_WhenQueryIsWhitespace() `method`

##### Summary

Tests that SearchUsersAsync returns an empty enumerable if the query is whitespace.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Services-UserServiceTest-SearchUsersAsync_ShouldReturnMatchingUsers'></a>
### SearchUsersAsync_ShouldReturnMatchingUsers() `method`

##### Summary

Tests that SearchUsersAsync returns users matching the search query.

##### Parameters

This method has no parameters.

<a name='T-UserAccountService-Tests-Controllers-UsersControllerTest'></a>
## UsersControllerTest `type`

##### Namespace

UserAccountService.Tests.Controllers

<a name='M-UserAccountService-Tests-Controllers-UsersControllerTest-GetMe_ShouldReturnNotFound_WhenUserDoesNotExist'></a>
### GetMe_ShouldReturnNotFound_WhenUserDoesNotExist() `method`

##### Summary

Tests that the GetMe endpoint returns a NotFoundResult
when the user ID claim is present but no user with that ID exists in the service.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Controllers-UsersControllerTest-GetMe_ShouldReturnOkWithUserDto_WhenUserIsAuthenticated'></a>
### GetMe_ShouldReturnOkWithUserDto_WhenUserIsAuthenticated() `method`

##### Summary

Tests that the GetMe endpoint returns an OkObjectResult with the correct UserDto
when the user is authenticated and exists.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Controllers-UsersControllerTest-GetMe_ShouldReturnUnauthorized_WhenUserIdClaimIsMissing'></a>
### GetMe_ShouldReturnUnauthorized_WhenUserIdClaimIsMissing() `method`

##### Summary

Tests that the GetMe endpoint returns an UnauthorizedResult
when the NameIdentifier claim is missing from the user's claims.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Controllers-UsersControllerTest-GetUserById_ShouldReturnNotFound_WhenUserDoesNotExist'></a>
### GetUserById_ShouldReturnNotFound_WhenUserDoesNotExist() `method`

##### Summary

Tests that the GetUserById endpoint returns a NotFoundResult
when no user with the specified ID exists in the service.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Controllers-UsersControllerTest-GetUserById_ShouldReturnOkWithUserDto_WhenUserExists'></a>
### GetUserById_ShouldReturnOkWithUserDto_WhenUserExists() `method`

##### Summary

Tests that the GetUserById endpoint returns an OkObjectResult with the correct UserDto
when a user with the specified ID exists.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Controllers-UsersControllerTest-SearchUsers_ShouldReturnBadRequest_WhenQueryIsEmpty'></a>
### SearchUsers_ShouldReturnBadRequest_WhenQueryIsEmpty() `method`

##### Summary

Tests that the SearchUsers endpoint returns a BadRequestObjectResult
when an empty search query is provided.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Controllers-UsersControllerTest-SearchUsers_ShouldReturnOkWithEmptyList_WhenNoUsersMatchQuery'></a>
### SearchUsers_ShouldReturnOkWithEmptyList_WhenNoUsersMatchQuery() `method`

##### Summary

Tests that the SearchUsers endpoint returns an OkObjectResult with an empty list of UserDtos
when a valid search query is provided but no users match the query.

##### Parameters

This method has no parameters.

<a name='M-UserAccountService-Tests-Controllers-UsersControllerTest-SearchUsers_ShouldReturnOkWithUserDtos_WhenQueryIsValid'></a>
### SearchUsers_ShouldReturnOkWithUserDtos_WhenQueryIsValid() `method`

##### Summary

Tests that the SearchUsers endpoint returns an OkObjectResult with a list of UserDtos
when a valid search query is provided and matching users exist.

##### Parameters

This method has no parameters.
