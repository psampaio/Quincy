@echo off
.paket\paket.exe restore group Build
"packages\build\FAKE\tools\Fake.exe" "%1"
exit /b %errorlevel%