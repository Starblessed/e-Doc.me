from celery import Celery
import numpy as np
import cv2

from src.backend.core.models.models import Quadrilateral
from src.backend.core.models.exceptions import ExtractionFailedException, UnreachableError

from src.backend.core.image_operations import auto_extract, crop_and_rectify
from src.backend.core.documents import Document

import os
from pathlib import Path

celery_app = Celery(
    "tasks",
    broker="redis://localhost:6379/0",
    backend="redis://localhost:6379/0",
)

celery_app.conf.update(task_track_started=True)
save_path = Path('output')

def handle_export(image: np.ndarray, export_as: str, task_id: str) -> str:
    os.makedirs(save_path / task_id, exist_ok=True)
    
    doc_path = save_path / task_id
    doc = Document(image, doc_path)

    match export_as:
        case "pdf":
            doc_path = doc.to_pdf()
        case "jpg":
            doc_path = doc.to_jpg()
        case "png":
            doc_path = doc.to_png()
        case _:
            raise UnreachableError("Unreachable code reached. Something went wrong.")
        
    return doc_path

@celery_app.task(bind=True, name="auto_crop_and_rectify")
def auto_crop_and_rectify(self, payload: dict) -> dict:
    """Crops and rectifies a slice of an image.

    Args:
        payload (dict): API request payload.
    Returns:
        dict: Response with result path or extraction failure feedback.
    """
    print(payload)
    image_path: str = payload.get("img_path", "")
    export_as: str = payload.get("export_as", "")

    assert os.path.exists(image_path), f"Provided image path does not exist: {image_path}"
    assert export_as in ["pdf", "jpg", "png"], f"Provided export mode is invalid: {export_as}"

    image = cv2.imread(image_path)

    try:
        region = auto_extract(image)
        if region is None:
            raise ExtractionFailedException("No region was extracted from the provided image.")
    except ExtractionFailedException as e:
        print(f"Error extracting region: {e}")
        return {"result": "unsuccessful", "response": "Auto-extraction didn't find the document."}
    
    rectified_image = crop_and_rectify(image, region, 1080)

    doc_path = handle_export(image=rectified_image, export_as=export_as, task_id=self.request.id)
    
    return os.path.abspath(doc_path)

@celery_app.task(bind=True)
def manual_crop_and_rectify(self, payload: dict) -> dict:
    image_path: str = payload.get("image_path", "")
    export_as: str = payload.get("export_as", "")
    region: list = payload.get("region", "")

    assert os.path.exists(image_path), f"Provided image path does not exist: {image_path}"
    assert export_as in ["pdf", "jpg", "png"], f"Provided export mode is invalid: {export_as}"
    assert region, f"No region provided"

    region: list = [(int(x), int(y)) for x, y in region]
    image = cv2.imread(image_path)

    rectified_image = crop_and_rectify(image, region, 1080)

    doc_path = handle_export(image=rectified_image, export_as=export_as, task_id=self.request.id)

    return {"result": "successful", "output": os.path.abspath(doc_path)}

