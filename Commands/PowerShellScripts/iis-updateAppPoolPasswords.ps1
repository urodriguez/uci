Import-Module WebAdministration

$userName = "ENDAVA\URodriguez"
$secureIdentityPassword = Read-Host "Enter a Password for user identity '$userName'" -AsSecureString
$credential = New-Object System.Management.Automation.PSCredential($userName, $secureIdentityPassword)

$appPools = Get-ChildItem IIS:\AppPools | Where-Object { ($_.processModel.userName -eq $userName) -and (-not $_.Name.Contains("Test"))}

foreach($appPool in $appPools)
{
    #Stop AppPool
    if ($appPool.state -ne "Stopped") {
        C:\Windows\System32\inetsrv\appcmd stop apppool /apppool.name:$($appPool.Name)
    }

    Write-Host "Updataing pool: $($appPool.Name)" -ForegroundColor Yellow 
    $appPool.processModel.userName = $userName
    $appPool.processModel.password = $credential.GetNetworkCredential().Password
    $appPool.processModel.identityType = 3
    $appPool | Set-Item

    #Stop AppPool
    if ($appPool.state -ne "Started") {
        C:\Windows\System32\inetsrv\appcmd start apppool /apppool.name:$($appPool.Name)
    }
}
 
Write-Host "All application pool passwords have been updated..." -ForegroundColor Green 