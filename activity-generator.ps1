$commitCount = Get-Random -Minimum 300 -Maximum 500
$startDate = (Get-Date).AddDays(-365)

Write-Host "Generating $commitCount backdated commits in current repo..."

# Determine a file to update
$dummyFile = "docs/development-log.md"
if (!(Test-Path "docs")) {
    New-Item -ItemType Directory -Force -Path "docs" | Out-Null
}

for ($i = 1; $i -le $commitCount; $i++) {
    # Generate a random date in the last year
    $randomDays = Get-Random -Minimum 0 -Maximum 365
    $randomHours = Get-Random -Minimum 1 -Maximum 23
    $randomMins = Get-Random -Minimum 1 -Maximum 59
    $commitDate = $startDate.AddDays($randomDays).AddHours($randomHours).AddMinutes($randomMins)
    $formattedDate = $commitDate.ToString("yyyy-MM-ddTHH:mm:ss")
    
    # Create an arbitrary change
    "- Development update $i on $formattedDate" | Out-File -FilePath $dummyFile -Append
    git add $dummyFile | Out-Null
    
    # Backdate the commit using environment variables
    $env:GIT_AUTHOR_DATE = $formattedDate
    $env:GIT_COMMITTER_DATE = $formattedDate
    
    git commit -m "docs: progress log update $i" | Out-Null
}

Write-Host "`nDone! Run 'git push origin main' to publish the backdated history."
