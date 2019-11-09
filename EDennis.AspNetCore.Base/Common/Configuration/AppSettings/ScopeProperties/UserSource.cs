namespace EDennis.AspNetCore.Base {
    public enum UserSource {
        OasisNameClaim,
        OasisEmailClaim,
        JwtNameClaim,
        JwtPreferredUserNameClaim,
        JwtSubjectClaim,
        JwtEmailClaim,
        JwtPhoneClaim,
        JwtClientIdClaim,
        SessionId,
        XUserHeader
    }
}