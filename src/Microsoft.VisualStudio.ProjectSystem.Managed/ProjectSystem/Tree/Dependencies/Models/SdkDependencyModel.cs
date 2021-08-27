﻿// Licensed to the .NET Foundation under one or more agreements. The .NET Foundation licenses this file to you under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Immutable;
using Microsoft.VisualStudio.ProjectSystem.Properties;
using Microsoft.VisualStudio.ProjectSystem.VS.Tree.Dependencies;
using Microsoft.VisualStudio.ProjectSystem.Tree.Dependencies.Subscriptions;
using Microsoft.VisualStudio.ProjectSystem.Tree.Dependencies.Subscriptions.RuleHandlers;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Imaging;

namespace Microsoft.VisualStudio.ProjectSystem.Tree.Dependencies.Models
{
    internal class SdkDependencyModel : DependencyModel
    {
        private static readonly DependencyFlagCache s_flagCache = new DependencyFlagCache(
            resolved: DependencyTreeFlags.SdkDependency + DependencyTreeFlags.SupportsFolderBrowse,
            unresolved: DependencyTreeFlags.SdkDependency);

        private static readonly DependencyIconSet s_iconSet = new DependencyIconSet(
            icon: KnownMonikers.SDK,
            expandedIcon: KnownMonikers.SDK,
            unresolvedIcon: KnownMonikers.SDKWarning,
            unresolvedExpandedIcon: KnownMonikers.SDKWarning);

        private static readonly DependencyIconSet s_implicitIconSet = new DependencyIconSet(
            icon: KnownMonikers.SDKPrivate,
            expandedIcon: KnownMonikers.SDKPrivate,
            unresolvedIcon: KnownMonikers.SDKWarning,
            unresolvedExpandedIcon: KnownMonikers.SDKWarning);

        public override DependencyIconSet IconSet => Implicit ? s_implicitIconSet : s_iconSet;

        public override string ProviderType => SdkRuleHandler.ProviderTypeString;

        public override string? SchemaItemType => SdkReference.PrimaryDataSourceItemType;

        public override string? SchemaName => Resolved ? ResolvedSdkReference.SchemaName : SdkReference.SchemaName;

        public SdkDependencyModel(
            string path,
            string originalItemSpec,
            bool isResolved,
            bool isImplicit,
            IImmutableDictionary<string, string> properties)
            : base(
                caption: GetCaption(path, originalItemSpec, properties),
                path,
                originalItemSpec,
                flags: s_flagCache.Get(isResolved, isImplicit),
                isResolved,
                isImplicit,
                properties)
        {
        }

        private static string GetCaption(string path, string originalItemSpec, IImmutableDictionary<string, string> properties)
        {
            string version = properties.GetStringProperty(ProjectItemMetadata.Version) ?? string.Empty;
            string? baseCaption = new LazyStringSplit(path, ',').FirstOrDefault();
            return (string.IsNullOrEmpty(version) ? baseCaption : $"{baseCaption} ({version})") ?? originalItemSpec;
        }
    }
}
