from src.backend.api.schemas import DigitalizeRequest

from fastapi import FastAPI

from src.backend.tasks.celery_app import auto_crop_and_rectify, manual_crop_and_rectify, celery_app
from celery.result import AsyncResult

app = FastAPI()

@app.post("/digitalize")
def digitalize(req: DigitalizeRequest) -> dict[str, str]:
    # Parse payload
    # Schedule celery tasks
    task = auto_crop_and_rectify.delay(req.model_dump())
    # Return task ID
    return {"task_id": task.id, "status": "PENDING"}

@app.get("/tasks/{task_id}")
def fetch_results(task_id: str) -> dict[str, str | None]:
    # Get AsyncResult state
    task_result = AsyncResult(task_id, app=celery_app)
    # Return task info
    result = {
        "task_id": task_id,
        "status": task_result.status,
        "result": task_result.result if task_result.ready() else '...',
    }
    return result
