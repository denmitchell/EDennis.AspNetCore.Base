namespace EDennis.AspNetCore.Base.EntityFramework.Abstractions {
    public interface IHasCompoundId {
        long Id { get; set; }
        int LocalId { get; set; }
    }
}