@echo off
.paket\paket.bootstrapper.exe
.paket\paket.exe restore
"packages\build\FAKE\tools\Fake.exe" "%1"
exit /b %errorlevel%