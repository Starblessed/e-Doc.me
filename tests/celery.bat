@ECHO OFF
cd ..
uv run celery -A src.backend.tasks.celery_app.celery_app worker --loglevel=info --pool=solo
pause