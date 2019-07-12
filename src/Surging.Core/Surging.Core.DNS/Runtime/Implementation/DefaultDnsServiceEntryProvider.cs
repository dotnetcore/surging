﻿using Microsoft.Extensions.Logging;
using Surging.Core.CPlatform;
using Surging.Core.CPlatform.Routing.Template;
using Surging.Core.CPlatform.Runtime.Server;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Surging.Core.DNS.Runtime.Implementation
{
    /// <summary>
    /// Defines the <see cref="DefaultDnsServiceEntryProvider" />
    /// </summary>
    public class DefaultDnsServiceEntryProvider : IDnsServiceEntryProvider
    {
        #region 字段

        /// <summary>
        /// Defines the _logger
        /// </summary>
        private readonly ILogger<DefaultDnsServiceEntryProvider> _logger;

        /// <summary>
        /// Defines the _serviceProvider
        /// </summary>
        private readonly CPlatformContainer _serviceProvider;

        /// <summary>
        /// Defines the _types
        /// </summary>
        private readonly IEnumerable<Type> _types;

        /// <summary>
        /// Defines the _dnsServiceEntry
        /// </summary>
        private DnsServiceEntry _dnsServiceEntry;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultDnsServiceEntryProvider"/> class.
        /// </summary>
        /// <param name="serviceEntryProvider">The serviceEntryProvider<see cref="IServiceEntryProvider"/></param>
        /// <param name="logger">The logger<see cref="ILogger{DefaultDnsServiceEntryProvider}"/></param>
        /// <param name="serviceProvider">The serviceProvider<see cref="CPlatformContainer"/></param>
        public DefaultDnsServiceEntryProvider(IServiceEntryProvider serviceEntryProvider,
            ILogger<DefaultDnsServiceEntryProvider> logger,
            CPlatformContainer serviceProvider)
        {
            _types = serviceEntryProvider.GetTypes();
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        #endregion 构造函数

        #region 方法

        /// <summary>
        /// The CreateServiceEntry
        /// </summary>
        /// <param name="service">The service<see cref="Type"/></param>
        /// <returns>The <see cref="DnsServiceEntry"/></returns>
        public DnsServiceEntry CreateServiceEntry(Type service)
        {
            DnsServiceEntry result = null;
            var routeTemplate = service.GetCustomAttribute<ServiceBundleAttribute>();
            var objInstance = _serviceProvider.GetInstances(service);
            var behavior = objInstance as DnsBehavior;
            var path = RoutePatternParser.Parse(routeTemplate.RouteTemplate, service.Name);
            if (path.Length > 0 && path[0] != '/')
                path = $"/{path}";
            if (behavior != null)
                result = new DnsServiceEntry
                {
                    Behavior = behavior,
                    Type = behavior.GetType(),
                    Path = path,
                };
            return result;
        }

        /// <summary>
        /// 获取服务条目集合。
        /// </summary>
        /// <returns>服务条目集合。</returns>
        public DnsServiceEntry GetEntry()
        {
            var services = _types.ToArray();
            if (_dnsServiceEntry == null)
            {
                _dnsServiceEntry = new DnsServiceEntry();
                foreach (var service in services)
                {
                    var entry = CreateServiceEntry(service);
                    if (entry != null)
                    {
                        _dnsServiceEntry = entry;
                        break;
                    }
                }
                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogDebug($"发现了以下dns服务：{_dnsServiceEntry.Type.FullName}。");
                }
            }
            return _dnsServiceEntry;
        }

        #endregion 方法
    }
}