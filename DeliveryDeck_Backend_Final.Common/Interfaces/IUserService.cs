using DeliveryDeck_Backend_Final.Common.DTO.Auth;

namespace DeliveryDeck_Backend_Final.Common.Interfaces
{
    public interface IUserService
    {
        Task<UserProfileDto> GetProfile(Guid userId);
        Task UpdateProfile(Guid userId, UserUpdateProfileDto data);
        Task ResetPassword(ResetPasswordDto data);
        Task<ResetPasswordToken> GetResetPasswordToken(string email);
    }
}
