// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System;

namespace GeminiSearchExtension;


internal sealed partial class GoogleAISearchFallbackItem : FallbackCommandItem
{
    private readonly GoogleAISearchCommand _searchCommand;
    private readonly IIconInfo _icon;

    public GoogleAISearchFallbackItem() : base(new GoogleAISearchCommand(), string.Empty)
    {
        _searchCommand = (GoogleAISearchCommand)Command!;
        _icon = IconHelpers.FromRelativePath("Assets\\StoreLogo.png");
        // Immediately trigger hiding logic
        UpdateQuery(string.Empty);
    }

    public override void UpdateQuery(string query)
    {
        _searchCommand.Query = query;

        var shouldShow = !string.IsNullOrWhiteSpace(query);
        var isCurrentlyShown = Command != null;

        // Only update properties when visibility state changes
        if (shouldShow && !isCurrentlyShown)
        {
            Command = _searchCommand;
            Title = "Search with Google AI";
            Icon = _icon;
        }
        else if (!shouldShow && isCurrentlyShown)
        {
            Command = null;
            Title = string.Empty;
            Icon = null;
        }
    }
}

// The actual search command to open the Google AI page
internal sealed partial class GoogleAISearchCommand : InvokableCommand
{
    public string Query { get; set; } = string.Empty;

    public override string Name => "Search with Google AI";

    public override CommandResult Invoke()
    {
        if (string.IsNullOrWhiteSpace(Query))
        {
            return CommandResult.Dismiss();
        }

        var encodedQuery = Uri.EscapeDataString(Query);
        var url = $"https://www.google.com/search?q={encodedQuery}&udm=50";
        
        // Use OpenUrlCommand to open the URL
        var openUrlCommand = new OpenUrlCommand(url);
        openUrlCommand.Invoke();
        return CommandResult.Dismiss();
    }
}