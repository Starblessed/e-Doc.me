@ECHO OFF
cd ..
uv run uvicorn src.backend.api.app:app
pause