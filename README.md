# e-Doc.me

**Local-first document scanning for privacy-conscious users.**

`e-Doc.me` is a simple, open-source Python project that allows users to digitize documents **entirely on their own machines**, without uploading images or files to cloud services.

The core goal of the project is digital privacy and data security:  
**your documents should never leave your computer.**

---

## Motivation

Many document scanning tools rely on cloud-based processing, often without making this explicit. This means sensitive documents — such as IDs, contracts, academic records, or medical paperwork — may be uploaded, processed, stored, or analyzed by third parties.

`e-Doc.me` follows a different approach:

- Local-first processing  
- No cloud uploads  
- No external APIs  
- No telemetry or tracking  

All document handling happens **offline and locally**, under the user’s control.

---

## Features (Version 1)

This initial version provides a minimal and transparent document-scanning pipeline:

- Load an image from the local filesystem  
- Manually crop the document region  
- Apply perspective correction (warp to A4 format)  
- Export the result as a PDF  

The limited scope is intentional, prioritizing simplicity, auditability, and privacy guarantees.

---

## Non-Goals (Current Version)

This version of `e-Doc.me` **does not**:

- Upload files to any server  
- Perform OCR (optical character recognition)  
- Automatically detect document edges  
- Support multi-page documents  
- Encrypt output files  

Future features will only be added if they preserve the local-only, privacy-by-design philosophy of the project.

---

## Privacy Model

- Input files remain on the user’s machine  
- Processing is done locally using open-source libraries  
- No data is transmitted externally  
- No analytics, logging, or tracking mechanisms are implemented  

If the program runs offline, it functions fully.

---

## Installation

Clone the repository:

```bash
git clone https://github.com/Starblessed/e-Doc.me.git
cd e-Doc.me
```

Install dependencies:

```bash
pip install -r requirements.txt
```

Run the main script according to the project structure.

---

## Intended Use

`e-Doc.me` is intended for users who:

- Handle sensitive or personal documents  
- Prefer local processing over cloud-based tools  
- Want transparency and control over their data  
- Value open-source and inspectable software  

The project also serves as an academic and professional exploration of **privacy-preserving data processing**, **security-by-design**, and **digital governance principles**.

---

## Project Status

- Early-stage / experimental  
- Functional but minimal  
- Actively evolving  

Feedback, audits, and contributions are welcome.

---

## License

This project is open-source. See the `LICENSE` file for details.
