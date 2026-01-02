$email = 'superadmin@store.com'
$password = 'Pa$$w0rd'
$body = @{ email = $email; password = $password } | ConvertTo-Json
try {
    $response = Invoke-RestMethod -Uri 'http://localhost:5025/api/Account/login' -Method Post -ContentType 'application/json' -Body $body
    $response | ConvertTo-Json
} catch {
    $_.Exception.Message
    if ($_.Exception.Response) {
        $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
        $reader.ReadToEnd()
    }
}
