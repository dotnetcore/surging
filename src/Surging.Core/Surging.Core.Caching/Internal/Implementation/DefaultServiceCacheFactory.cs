﻿using Surging.Core.CPlatform.Cache;
using Surging.Core.CPlatform.Serialization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surging.Core.Caching.Internal.Implementation
{
    /// <summary>
    /// 默认的服务缓存工程
    /// </summary>
    public class DefaultServiceCacheFactory : IServiceCacheFactory
    {
        #region 字段

        /// <summary>
        /// Defines the _addressModel
        /// </summary>
        private readonly ConcurrentDictionary<string, CacheEndpoint> _addressModel =
               new ConcurrentDictionary<string, CacheEndpoint>();

        /// <summary>
        /// Defines the _serializer
        /// </summary>
        private readonly ISerializer<string> _serializer;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultServiceCacheFactory"/> class.
        /// </summary>
        /// <param name="serializer">The serializer<see cref="ISerializer{string}"/></param>
        public DefaultServiceCacheFactory(ISerializer<string> serializer)
        {
            _serializer = serializer;
        }

        #endregion 构造函数

        #region 方法

        /// <summary>
        /// The CreateServiceCachesAsync
        /// </summary>
        /// <param name="descriptors">The descriptors<see cref="IEnumerable{ServiceCacheDescriptor}"/></param>
        /// <returns>The <see cref="Task{IEnumerable{ServiceCache}}"/></returns>
        public Task<IEnumerable<ServiceCache>> CreateServiceCachesAsync(IEnumerable<ServiceCacheDescriptor> descriptors)
        {
            if (descriptors == null)
                throw new ArgumentNullException(nameof(descriptors));

            descriptors = descriptors.ToArray();
            var routes = new List<ServiceCache>(descriptors.Count());

            routes.AddRange(descriptors.Select(descriptor => new ServiceCache
            {
                CacheEndpoint = CreateAddress(descriptor.AddressDescriptors),
                CacheDescriptor = descriptor.CacheDescriptor
            }));

            return Task.FromResult(routes.AsEnumerable());
        }

        /// <summary>
        /// The CreateAddress
        /// </summary>
        /// <param name="descriptors">The descriptors<see cref="IEnumerable{CacheEndpointDescriptor}"/></param>
        /// <returns>The <see cref="IEnumerable{CacheEndpoint}"/></returns>
        private IEnumerable<CacheEndpoint> CreateAddress(IEnumerable<CacheEndpointDescriptor> descriptors)
        {
            if (descriptors == null)
                yield break;

            foreach (var descriptor in descriptors)
            {
                _addressModel.TryGetValue(descriptor.Value, out CacheEndpoint address);
                if (address == null)
                {
                    var addressType = Type.GetType(descriptor.Type);
                    address = (CacheEndpoint)_serializer.Deserialize(descriptor.Value, addressType);
                    _addressModel.TryAdd(descriptor.Value, address);
                }
                yield return address;
            }
        }

        #endregion 方法
    }
}