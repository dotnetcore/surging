﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Surging.Core.Zookeeper.Configurations
{
    /// <summary>
    /// Defines the <see cref="ZookeeperOption" />
    /// </summary>
    public class ZookeeperOption
    {
        #region 属性

        /// <summary>
        /// Gets or sets the CachePath
        /// </summary>
        public string CachePath { get; set; }

        /// <summary>
        /// Gets or sets the ChRoot
        /// </summary>
        public string ChRoot { get; set; }

        /// <summary>
        /// Gets or sets the CommandPath
        /// </summary>
        public string CommandPath { get; set; }

        /// <summary>
        /// Gets or sets the ConnectionString
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the EnableChildrenMonitor
        /// </summary>
        public string EnableChildrenMonitor { get; set; }

        /// <summary>
        /// Gets or sets the MqttRoutePath
        /// </summary>
        public string MqttRoutePath { get; set; }

        /// <summary>
        /// Gets or sets the ReloadOnChange
        /// </summary>
        public string ReloadOnChange { get; set; }

        /// <summary>
        /// Gets or sets the RoutePath
        /// </summary>
        public string RoutePath { get; set; }

        /// <summary>
        /// Gets or sets the SessionTimeout
        /// </summary>
        public string SessionTimeout { get; set; }

        /// <summary>
        /// Gets or sets the SubscriberPath
        /// </summary>
        public string SubscriberPath { get; set; }

        #endregion 属性
    }
}