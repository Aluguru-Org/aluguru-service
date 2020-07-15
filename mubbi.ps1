[CmdletBinding()]
param (
    [Parameter(Mandatory=$true)]
    [ValidateNotNull()]
    [ValidateNotNullOrEmpty()]
    [ValidateSet("start", "stop", "bootstrap")]
    [String]
    $Operation
)
begin {
    Write-Host "======================> Mubbi.ps1 started" -ForegroundColor Green
    Write-Host "Operation:" -NoNewline
    Write-Host "$Operation" -ForegroundColor Yellow
}

process {
    Write-Host "======================> Mubbi.ps1 processing" -ForegroundColor Green

    if ($Operation -eq "start") {
        docker-compose up -d
    } elseif ($Operation -eq "stop") {
        docker-compose down
    } elseif ($Operation -eq "bootstrap") {
        Write-Host "           _____                    _____                    _____                    _____                    _____          " -ForegroundColor DarkRed
        Write-Host "          /\    \                  /\    \                  /\    \                  /\    \                  /\    \         " -ForegroundColor DarkRed
        Write-Host "         /::\____\                /::\____\                /::\    \                /::\    \                /::\    \        " -ForegroundColor DarkRed
        Write-Host "        /::::|   |               /:::/    /               /::::\    \              /::::\    \               \:::\    \       " -ForegroundColor DarkRed
        Write-Host "       /:::::|   |              /:::/    /               /::::::\    \            /::::::\    \               \:::\    \      " -ForegroundColor DarkRed
        Write-Host "      /::::::|   |             /:::/    /               /:::/\:::\    \          /:::/\:::\    \               \:::\    \     " -ForegroundColor DarkRed
        Write-Host "     /:::/|::|   |            /:::/    /               /:::/__\:::\    \        /:::/__\:::\    \               \:::\    \    " -ForegroundColor DarkRed
        Write-Host "    /:::/ |::|   |           /:::/    /               /::::\   \:::\    \      /::::\   \:::\    \              /::::\    \   " -ForegroundColor DarkRed
        Write-Host "   /:::/  |::|___|______    /:::/    /      _____    /::::::\   \:::\    \    /::::::\   \:::\    \    ____    /::::::\    \  " -ForegroundColor DarkRed
        Write-Host "  /:::/   |::::::::\    \  /:::/____/      /\    \  /:::/\:::\   \:::\ ___\  /:::/\:::\   \:::\ ___\  /\   \  /:::/\:::\    \ " -ForegroundColor DarkRed
        Write-Host " /:::/    |:::::::::\____\|:::|    /      /::\____\/:::/__\:::\   \:::|    |/:::/__\:::\   \:::|    |/::\   \/:::/  \:::\____\" -ForegroundColor DarkRed
        Write-Host " \::/    / ~~~~~/:::/    /|:::|____\     /:::/    /\:::\   \:::\  /:::|____|\:::\   \:::\  /:::|____|\:::\  /:::/    \::/    /" -ForegroundColor DarkRed
        Write-Host "  \/____/      /:::/    /  \:::\    \   /:::/    /  \:::\   \:::\/:::/    /  \:::\   \:::\/:::/    /  \:::\/:::/    / \/____/ " -ForegroundColor DarkRed
        Write-Host "              /:::/    /    \:::\    \ /:::/    /    \:::\   \::::::/    /    \:::\   \::::::/    /    \::::::/    /          " -ForegroundColor DarkRed
        Write-Host "             /:::/    /      \:::\    /:::/    /      \:::\   \::::/    /      \:::\   \::::/    /      \::::/____/           " -ForegroundColor DarkRed
        Write-Host "            /:::/    /        \:::\__/:::/    /        \:::\  /:::/    /        \:::\  /:::/    /        \:::\    \           " -ForegroundColor DarkRed
        Write-Host "           /:::/    /          \::::::::/    /          \:::\/:::/    /          \:::\/:::/    /          \:::\    \          " -ForegroundColor DarkRed
        Write-Host "          /:::/    /            \::::::/    /            \::::::/    /            \::::::/    /            \:::\    \         " -ForegroundColor DarkRed
        Write-Host "         /:::/    /              \::::/    /              \::::/    /              \::::/    /              \:::\____\        " -ForegroundColor DarkRed
        Write-Host "         \::/    /                \::/____/                \::/____/                \::/____/                \::/    /        " -ForegroundColor DarkRed
        Write-Host "          \/____/                  ~~                       ~~                       ~~                       \/____/         " -ForegroundColor DarkRed
        Write-Host "                                                                                                                              " -ForegroundColor DarkRed
               
        docker-compose build --no-cache
        docker-compose up -d
    }
}

end {
    Write-Host "======================> Mubbi.ps1 finished processing" -ForegroundColor Green
}

