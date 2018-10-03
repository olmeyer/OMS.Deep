@call "%VS120COMNTOOLS%vsvars32.bat"

@msbuild "BuildAll.proj" /t:BuildAll /p:Configuration=Release


pause