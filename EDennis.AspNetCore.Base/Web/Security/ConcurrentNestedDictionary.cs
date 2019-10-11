using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.Security {
    public class ConcurrentNestedDictionary<TOuterKey, TInnerKey, TValue> {
        private ConcurrentDictionary<TOuterKey, ConcurrentDictionary<TInnerKey, TValue>> _dict
            = new ConcurrentDictionary<TOuterKey, ConcurrentDictionary<TInnerKey, TValue>>();

        /// <summary>
        /// Use this method when you have the keys, but not the value
        /// </summary>
        /// <param name="outerKey"></param>
        /// <param name="innerKey"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGet(TOuterKey outerKey, TInnerKey innerKey, out TValue value) {
            var innerDict = _dict.GetOrAdd(outerKey, new ConcurrentDictionary<TInnerKey, TValue>());
            bool found = innerDict.TryGetValue(innerKey, out TValue value2);
            value = value2;
            return found;
        }

        /// <summary>
        /// Use this method when you have the keys and the value
        /// </summary>
        /// <param name="outerKey"></param>
        /// <param name="innerKey"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Add(TOuterKey outerKey, TInnerKey innerKey, TValue value) {
            bool found = TryGet(outerKey, innerKey, out _);
            _dict[outerKey][innerKey] = value;
            return found;
        }

    }
}
