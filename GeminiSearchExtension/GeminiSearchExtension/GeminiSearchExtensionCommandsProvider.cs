// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace GeminiSearchExtension;

public partial class GeminiSearchExtensionCommandsProvider : CommandProvider
{
    private readonly ICommandItem[] _commands;
    private readonly IFallbackCommandItem[] _fallbackCommands;

    public GeminiSearchExtensionCommandsProvider()
    {
        DisplayName = "Google AI";
        Icon = IconHelpers.FromRelativePath("Assets\\StoreLogo.png");
        _commands = [
            new CommandItem(new GeminiSearchExtensionPage()) { Title = DisplayName },
        ];
        _fallbackCommands = [
            new GoogleAISearchFallbackItem(),
        ];
    }

    public override ICommandItem[] TopLevelCommands()
    {
        return _commands;
    }

    public override IFallbackCommandItem[] FallbackCommands()
    {
        return _fallbackCommands;
    }
}
