import cv2
import numpy as np

from pathlib import Path

from src.backend.core.models.models import Quadrilateral, ImageDimensions

GAR_MODES = ['min_rect']

def preview_contour(img, contour):
    canvas = img.copy()
    if contour is not None and contour.size != 0:
        cv2.drawContours(canvas, [contour], -1, (0, 255, 0), 3)
    cv2.imshow("Chosen Contour", cv2.resize(canvas, (0, 0), fx=0.25, fy=0.25))
    cv2.waitKey(0)
    cv2.destroyAllWindows()

def get_biggest_contour(contours):
    biggest = np.array([])
    max_area = 0

    for c in contours:
        area = cv2.contourArea(c)
        perimeter = cv2.arcLength(c, True)
        approx = cv2.approxPolyDP(c, 0.02 * perimeter, True)
        if area > max_area and len(approx) == 4:
            biggest = approx
            max_area = area
    
    return biggest

def get_best_document_contour(contours, image_shape):
    img_h, img_w = image_shape[:2]
    img_area = img_h * img_w

    best = None
    best_score = -1

    for c in contours:
        area = cv2.contourArea(c)
        if area < 0.1 * img_area:   # too small
            continue

        peri = cv2.arcLength(c, True)
        approx = cv2.approxPolyDP(c, 0.02 * peri, True)

        if len(approx) != 4:
            continue

        if not cv2.isContourConvex(approx):
            continue

        x, y, w, h = cv2.boundingRect(approx)
        if w == 0 or h == 0:
            continue

        rect_area = w * h
        fill_ratio = area / rect_area if rect_area > 0 else 0

        # score prefers large, box-like shapes
        score = area * fill_ratio

        if score > best_score:
            best = approx
            best_score = score

    return best

def reorder_contour(contour):
    contour = np.asarray(contour, dtype=np.float32)

    if contour.shape == (4,1,2):
        contour = contour.reshape(4,2)

    s = contour.sum(axis=1)
    d = np.diff(contour, axis=1).reshape(-1)

    ordered = np.zeros((4,2), dtype=np.float32)

    ordered[0] = contour[np.argmin(s)]  # top-left
    ordered[2] = contour[np.argmax(s)]  # bottom-right
    ordered[1] = contour[np.argmin(d)]  # top-right
    ordered[3] = contour[np.argmax(d)]  # bottom-left

    return ordered


def get_aspect_ratio(contour, mode: str="minrect") -> float:
    match mode:
        case "minrect":
            (_, _), (w, h), angle = cv2.minAreaRect(contour)

            if h == 0 or w == 0:
                return None
            if abs(angle) <= 45:
                orientation = "landscape" if w > h else "portrait"
            else:
                orientation = "landscape" if w < h else "portrait"

            print(f"W, H: {w}, {h}")

            return max(w, h) / min(w, h), orientation
        case _:
            raise NotImplementedError(f"Mode \"{mode}\" is not a supported mode. Try one of the following: {GAR_MODES}")
        
        
def get_result_dimensions(smallest_dim: int, aspect_ratio: float, orientation: str) -> ImageDimensions:
    if orientation == 'landscape':
        return ImageDimensions(width=int(smallest_dim*aspect_ratio), height=int(smallest_dim))
    else:
        return ImageDimensions(width=int(smallest_dim), height=int(smallest_dim*aspect_ratio))
    
def apply_margin(img: np.ndarray, margin: float=10) -> np.ndarray:
    return img[margin:img.shape[0] - margin, margin:img.shape[1] - margin]


def auto_extract(img: np.ndarray, debug: bool = False) -> Quadrilateral:
    if img is None or img.size == 0:
        return None

    def show(name, frame, factor=0.6):
        if debug:
            prev = cv2.resize(frame, (0, 0), fx=factor, fy=factor)
            cv2.imshow(name, prev)
            cv2.waitKey(0)
            cv2.destroyWindow(name)

    orig_h, orig_w = img.shape[:2]

    target_height = 1000
    scale = target_height / orig_h if orig_h > target_height else 1.0

    work = cv2.resize(
        img,
        (int(orig_w * scale), int(orig_h * scale)),
        interpolation=cv2.INTER_AREA if scale < 1.0 else cv2.INTER_LINEAR
    )

    work_h, work_w = work.shape[:2]
    work_area = work_h * work_w

    gray = cv2.cvtColor(work, cv2.COLOR_BGR2GRAY)
    show("gray", gray)

    blur = cv2.GaussianBlur(gray, (5, 5), 0)
    show("blur", blur)

    edges = cv2.Canny(blur, 50, 150)
    show("edges", edges)

    kernel = np.ones((5, 5), np.uint8)
    closed = cv2.morphologyEx(edges, cv2.MORPH_CLOSE, kernel, iterations=2)
    closed = cv2.dilate(closed, kernel, iterations=1)
    closed = cv2.erode(closed, kernel, iterations=1)
    show("closed", closed)

    contours, _ = cv2.findContours(
        closed,
        cv2.RETR_LIST,
        cv2.CHAIN_APPROX_SIMPLE
    )

    best_quad = None
    best_score = -1.0

    overlay = work.copy()

    for c in contours:
        area = cv2.contourArea(c)
        if area < 0.10 * work_area:
            continue

        peri = cv2.arcLength(c, True)
        approx = cv2.approxPolyDP(c, 0.02 * peri, True)

        if len(approx) != 4:
            continue

        if not cv2.isContourConvex(approx):
            continue

        pts = approx.reshape(4, 2).astype(np.float32)

        x, y, w, h = cv2.boundingRect(approx)
        if w < 40 or h < 40:
            continue

        aspect = max(w, h) / max(1, min(w, h))
        if aspect > 8.0:
            continue

        rect_area = w * h
        fill_ratio = area / rect_area if rect_area > 0 else 0
        if fill_ratio < 0.45:
            continue

        ordered = reorder_contour(pts)
        tl, tr, br, bl = ordered

        def angle(a, b, c):
            ba = a - b
            bc = c - b
            denom = (np.linalg.norm(ba) * np.linalg.norm(bc))
            if denom == 0:
                return 0.0
            cosang = np.clip(np.dot(ba, bc) / denom, -1.0, 1.0)
            return np.degrees(np.arccos(cosang))

        angles = [
            angle(bl, tl, tr),
            angle(tl, tr, br),
            angle(tr, br, bl),
            angle(br, bl, tl),
        ]

        if not all(55 <= a <= 135 for a in angles):
            continue

        score = area * fill_ratio

        cv2.drawContours(overlay, [approx], -1, (0, 255, 255), 2)

        if score > best_score:
            best_score = score
            best_quad = approx

    if best_quad is None:
        if debug:
            show("candidates", overlay)
        return None

    chosen = work.copy()
    cv2.drawContours(chosen, [best_quad], -1, (0, 255, 0), 4)

    if debug:
        show("chosen", chosen)

    if scale != 1.0:
        best_quad = best_quad.astype(np.float32) / scale
        best_quad = np.round(best_quad).astype(np.int32)

    return best_quad


def crop_and_rectify(image: np.ndarray, region: Quadrilateral, min_output_dim: int) -> np.ndarray:
        print(region.shape)
        region = reorder_contour(region)
        aspect_ratio, orientation = get_aspect_ratio(region)

        image_dims = get_result_dimensions(min_output_dim, aspect_ratio, orientation)

        pts_before = np.float32(region)
        pts_after = np.float32([
                                [0, 0],  
                                [image_dims.width - 1, 0],     
                                [image_dims.width - 1, image_dims.height - 1],  
                                [0, image_dims.height - 1]
                            ])
        
        matrix = cv2.getPerspectiveTransform(pts_before, pts_after)

        print(image_dims)

        result_img = cv2.warpPerspective(image, matrix, (image_dims.width, image_dims.height))

        # Apply margin
        result_img = apply_margin(result_img)

        return result_img

if __name__ == "__main__":
    image_path = Path("tests") / "assets" / "test_img9.jpg"
    img = cv2.imread(str(image_path))

    region = auto_extract(img=img)
    preview_contour(img, region)
    
    rectified = crop_and_rectify(img, region=region, min_output_dim=480)

    cv2.imshow("Preview", rectified)
    cv2.waitKey(0)
    cv2.destroyAllWindows()