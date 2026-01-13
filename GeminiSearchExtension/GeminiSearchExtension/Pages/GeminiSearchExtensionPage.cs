// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System;

namespace GeminiSearchExtension;

internal sealed partial class GeminiSearchExtensionPage : DynamicListPage
{
    private string _searchQuery = string.Empty;

    public GeminiSearchExtensionPage()
    {
        Icon = IconHelpers.FromRelativePath("Assets\\StoreLogo.png");
        Title = "Google AI";
        Name = "Search";
    }

    public override void UpdateSearchText(string oldText, string newText)
    {
        _searchQuery = newText;
        RaiseItemsChanged(0);
    }

    public override IListItem[] GetItems()
    {
        if (string.IsNullOrWhiteSpace(_searchQuery))
        {
            return [
                new ListItem(new NoOpCommand()) 
                { 
                    Title = "Type your search query...",
                    Icon = IconHelpers.FromRelativePath("Assets\\StoreLogo.png")
                }
            ];
        }

        var encodedQuery = Uri.EscapeDataString(_searchQuery);
        var url = $"https://www.google.com/search?q={encodedQuery}&udm=50";

        return [
            new ListItem(new PageOpenUrlCommand(url)) 
            { 
                Title = $"Search \"{_searchQuery}\" with Google AI",
                Icon = IconHelpers.FromRelativePath("Assets\\StoreLogo.png")
            }
        ];
    }
}

internal sealed partial class PageOpenUrlCommand : InvokableCommand
{
    private readonly string _url;

    public PageOpenUrlCommand(string url)
    {
        _url = url;
    }

    public override string Name => "Open URL";

    public override CommandResult Invoke()
    {
        var openUrlCommand = new OpenUrlCommand(_url);
        openUrlCommand.Invoke();
        return CommandResult.Dismiss();
    }
}
