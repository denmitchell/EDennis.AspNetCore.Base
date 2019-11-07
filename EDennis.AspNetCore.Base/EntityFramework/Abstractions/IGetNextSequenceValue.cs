using Microsoft.EntityFrameworkCore;

namespace EDennis.AspNetCore.Base.EntityFramework.Abstractions {
    public interface ICompoundId {
        int LocalId { get; set; }
        long GetNextSequenceValue(DbContext context);
    }
}