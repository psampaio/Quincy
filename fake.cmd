@echo off
.paket\paket.bootstrapper.exe
.paket\paket.exe restore group Build
"packages\build\FAKE\tools\Fake.exe" "%1"
exit /b %errorlevel%