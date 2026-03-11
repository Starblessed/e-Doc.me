from dataclasses import dataclass
import numpy as np

@dataclass
class Point:
    x: int
    y: int


@dataclass
class Quadrilateral:
    """Quadrilateral dataclass for irregular perspective document regions.
    """
    tl: Point # Top left
    tr: Point # Top right
    br: Point # Bottom right
    bl: Point # Bottom left

@dataclass
class ImageDimensions:
    width: int
    height: int