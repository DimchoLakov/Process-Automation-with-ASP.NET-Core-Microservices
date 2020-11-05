$count = 0
do {
    $count++
    Write-Output "[$env:STAGE_NAME] Starting container [Attempt: $count]"
    
    $watchdogStart = Invoke-WebRequest -Uri http://localhost:5025/healthchecks-ui#/healthchecks -UseBasicParsing
	
	$webMvcStart = Invoke-WebRequest -Uri http://localhost:5019/ -UseBasicParsing
    
	$webMvcAdminStart = Invoke-WebRequest -Uri http://localhost:5021/ -UseBasicParsing
	
    if ($watchdogStart.statuscode -eq '200' -and
		$webMvcStart.statusCode -eq '200' -and
		$webMvcAdminStart.statusCode -eq '200') {
        $started = $true
    } else {
        Start-Sleep -Seconds 1
    }
	
	Write-Output "Watchdog status code" $watchdogStart.statusCode
	Write-Output "Web MVC status code" $webMvcStart.statusCode
	Write-Output "Web MVC admin status code" $webMvcAdminStart.statusCode
    
} until ($started -or ($count -eq 3))

if (!$started) {
    exit 1
}
