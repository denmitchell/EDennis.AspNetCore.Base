
namespace EDennis.AspNetCore.Base {
    public class UserSources {

        public readonly static UserSource DEFAULT_UNAUTHENTICATED_USER_SOURCE = UserSource.XUserHeader;
        public readonly static UserSource DEFAULT_AUTHENTICATED_USER_SOURCE = UserSource.JwtNameClaim;

        public UserSource UnauthenticatedUserSource { get; set; } = DEFAULT_UNAUTHENTICATED_USER_SOURCE;
        public UserSource AuthenticatedUserSource { get; set; } = DEFAULT_AUTHENTICATED_USER_SOURCE;

    }
}
