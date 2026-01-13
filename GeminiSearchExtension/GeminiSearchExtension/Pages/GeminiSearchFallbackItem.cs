// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System;

namespace GeminiSearchExtension;


internal sealed partial class GoogleAISearchFallbackItem : FallbackCommandItem
{
    private string _query = string.Empty;
    private readonly GoogleAISearchCommand _searchCommand;
    private readonly NoOpCommand _noOpCommand = new();
    private bool _hasQuery = false;

    public GoogleAISearchFallbackItem() : base(new NoOpCommand(), " ")
    {
        _searchCommand = new GoogleAISearchCommand();
    }

    // Override Command to return NoOpCommand when empty
    public new ICommand Command => _hasQuery ? _searchCommand : _noOpCommand;

    public override void UpdateQuery(string query)
    {
        _query = query;
        _hasQuery = !string.IsNullOrWhiteSpace(query);
        _searchCommand.Query = query;

        if (_hasQuery)
        {
            Title = "Search with Google AI";
            Icon = IconHelpers.FromRelativePath("Assets\\StoreLogo.png");
        }
        else
        {
            Title = " ";
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