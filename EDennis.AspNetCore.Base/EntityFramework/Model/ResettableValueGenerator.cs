using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading;

namespace EDennis.AspNetCore.Base.EntityFramework {

    /// <summary>
    /// This class provides a value generator which can be
    /// reset to a specific value (e.g., the maximum value
    /// for a Id).
    /// 
    /// This class is based upon the like-named value generator
    /// by Author Vickers (https://github.com/aspnet/EntityFrameworkCore/issues/6872#issuecomment-258025241)
    /// </summary>
    public class ResettableValueGenerator : ValueGenerator<int> {

        //current value of key
        private int _current;

        //values are stored
        public override bool GeneratesTemporaryValues => false;

        //gets the next value
        public override int Next(EntityEntry entry)
            => Interlocked.Increment(ref _current);

        //resets the key to the provided value (default = 0)
        public void Reset(int startingValue = 0) {
            _current = startingValue;
        }
    }
}