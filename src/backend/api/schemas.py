from pydantic import BaseModel

class DigitalizeRequest(BaseModel):
    img_path: str
    export_as: str
    
    