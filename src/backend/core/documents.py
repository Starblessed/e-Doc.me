from reportlab.pdfgen import canvas
from reportlab.lib.utils import ImageReader

import numpy as np
import cv2

class Document:
    def __init__(self, image: np.ndarray, dirname: str):
        self.image = image
        self.dirname = dirname

    def to_pdf(self):
        height, width = self.image.shape[:2]
        cv2.imwrite(self.dirname / "result.jpg", self.image)

        c = canvas.Canvas(str(self.dirname / "result.pdf"), pagesize=(width, height))

        c.drawImage(str(self.dirname / "result.jpg"), 0, 0, width=width, height=height)
        c.save()
        return str(self.dirname / "result.pdf")
    
    def to_jpg(self):
        cv2.imwrite(self.dirname / "result.jpg", self.image)
        return str(self.dirname / "result.jpg")

    def to_png(self):
        cv2.imwrite(self.dirname / "result.png", self.image)
        return str(self.dirname / "result.png")