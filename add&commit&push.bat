@echo off
git add .
set /p UserInput=Enter your commit message: 
git commit -m "%UserInput%"
git pull
git push origin master