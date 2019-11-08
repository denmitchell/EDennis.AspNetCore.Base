namespace EDennis.AspNetCore.Base {
    public enum UserSource {
        ClaimsPrincipalIdentityName,
        JwtNameClaim,
        JwtPreferredUserNameClaim,
        JwtSubjectClaim,
        JwtEmailClaim,
        JwtPhoneClaim,
        JwtClientIdClaim,
        SessionId,
        XUserHeader,
    }
}