#!/usr/bin/env python3
"""
Extracts text and figures/diagrams from a specific page range of a PDF file.
Designed to save LLM context tokens by avoiding full-page multimodal PDF reads.

Usage:
  1. View TOC / Bookmarks & Contents pages:
     py .agents/skills/pdf-to-vietnamese-html/scripts/extract_pdf_section.py --pdf "path/to/file.pdf" --toc

  2. Extract text + auto-crop figures for a page range:
     py .agents/skills/pdf-to-vietnamese-html/scripts/extract_pdf_section.py \
        --pdf "path/to/file.pdf" \
        --start-page 12 \
        --end-page 47 \
        --out-text "scratch/extracted_text.md" \
        --out-images "Docs/images"
"""

import argparse
import json
import os
import sys

try:
    import pymupdf
except ImportError:
    import fitz as pymupdf


def is_decorative_banner(bbox, page_rect):
    x0, y0, x1, y1 = bbox
    w = x1 - x0
    h = y1 - y0
    # Top chapter cover gradient banner (spans full width at top of page)
    if y0 <= 5 and w >= page_rect.width * 0.95 and h <= page_rect.height * 0.48:
        return True
    # Full page background
    if w >= page_rect.width * 0.98 and h >= page_rect.height * 0.95:
        return True
    return False


def detect_page_figures(page):
    """Returns a list of (x0, y0, x1, y1) clip rectangles for figures on the page."""
    page_rect = page.rect
    img_info = page.get_image_info(xrefs=True)

    raw_rects = []
    for im in img_info:
        bbox = im["bbox"]
        w = bbox[2] - bbox[0]
        h = bbox[3] - bbox[1]
        if w < 60 or h < 60:
            continue
        if is_decorative_banner(bbox, page_rect):
            continue
        raw_rects.append(pymupdf.Rect(bbox))

    # Merge overlapping or fragmented image tiles (like cover art or multi-part diagrams)
    merged = []
    for r in raw_rects:
        added = False
        for i, m in enumerate(merged):
            # If rects overlap or are within 12pt vertically & horizontally
            expanded = pymupdf.Rect(m.x0 - 12, m.y0 - 12, m.x1 + 12, m.y1 + 12)
            if expanded.intersects(r):
                merged[i] = m | r
                added = True
                break
        if not added:
            merged.append(r)

    # Expand figure rect if it sits inside a dark diagram container box in drawings
    draw_rects = []
    for d in page.get_drawings():
        dr = d["rect"]
        if dr.width > 100 and dr.height > 80 and not is_decorative_banner(dr, page_rect):
            draw_rects.append(dr)

    final_rects = []
    for r in merged:
        best = pymupdf.Rect(r)
        for dr in draw_rects:
            # If drawing rect encloses the image rect and isn't a huge full-page callout box
            if (
                dr.x0 <= best.x0 + 5
                and dr.y0 <= best.y0 + 5
                and dr.x1 >= best.x1 - 5
                and dr.y1 >= best.y1 - 5
                and dr.height <= best.height + 60
            ):
                best = best | dr
        # Clamp to page bounds
        best = best & page_rect
        if not is_decorative_banner(best, page_rect):
            final_rects.append(best)

    # Sort top-to-bottom
    final_rects.sort(key=lambda rect: (round(rect.y0), round(rect.x0)))
    return final_rects


def show_toc(doc, max_scan_pages=8):
    toc = doc.get_toc()
    print(f"Total pages: {len(doc)}")
    if toc:
        print("\n=== PDF Bookmarks (TOC) ===")
        for level, title, page in toc:
            indent = "  " * (level - 1)
            print(f"{indent}- {title} (Page {page})")
    print(f"\n=== Text from first {min(max_scan_pages, len(doc))} pages ===")
    for pno in range(1, min(max_scan_pages, len(doc)) + 1):
        txt = doc[pno - 1].get_text("text").strip()
        if txt:
            print(f"\n--- Page {pno} ---\n{txt}")


def extract_range(doc, start_page, end_page, out_text, out_images, dpi=200):
    if out_images:
        os.makedirs(out_images, exist_ok=True)
    if out_text:
        os.makedirs(os.path.dirname(os.path.abspath(out_text)), exist_ok=True)

    lines = []
    manifest = {}

    for pno in range(start_page, min(end_page, len(doc)) + 1):
        page = doc[pno - 1]
        figs = detect_page_figures(page)
        fig_keys = []

        for idx, rect in enumerate(figs, start=1):
            suffix = f"_{idx}" if len(figs) > 1 else ""
            key = f"IMG_P{pno}{suffix}"
            fname = f"{key.lower()}.png"
            if out_images:
                fpath = os.path.join(out_images, fname)
                pix = page.get_pixmap(clip=rect, dpi=dpi)
                pix.save(fpath)
                manifest[key] = {
                    "page": pno,
                    "file": f"images/{fname}",
                    "width": pix.width,
                    "height": pix.height,
                    "bbox": [round(rect.x0), round(rect.y0), round(rect.x1), round(rect.y1)],
                }
            fig_keys.append((key, fname, rect))

        lines.append(f"\n==========================================")
        lines.append(f"PAGE {pno}")
        if fig_keys:
            for key, fname, rect in fig_keys:
                lines.append(f"[FIGURE PLACEHOLDER: {{{{{key}}}}} -> images/{fname} bbox=({round(rect.x0)},{round(rect.y0)},{round(rect.x1)},{round(rect.y1)})]")
        lines.append(f"==========================================")
        lines.append(page.get_text("text").strip())

    full_text = "\n".join(lines)
    if out_text:
        with open(out_text, "w", encoding="utf-8") as f:
            f.write(full_text)
        manifest_path = out_text + ".manifest.json"
        with open(manifest_path, "w", encoding="utf-8") as f:
            json.dump(manifest, f, indent=2, ensure_ascii=False)
        print(f"Extracted pages {start_page}..{end_page} text to: {out_text}")
        print(f"Extracted {len(manifest)} figures to: {out_images}")
        print(f"Saved figure manifest to: {manifest_path}")
    else:
        sys.stdout.buffer.write(full_text.encode("utf-8"))


def main():
    parser = argparse.ArgumentParser(description="Extract PDF TOC, text, and cropped figures.")
    parser.add_argument("--pdf", required=True, help="Path to PDF file")
    parser.add_argument("--toc", action="store_true", help="Print TOC and first pages only")
    parser.add_argument("--start-page", type=int, default=1, help="Start page (1-based)")
    parser.add_argument("--end-page", type=int, default=1, help="End page (1-based, inclusive)")
    parser.add_argument("--out-text", default="", help="Output file path for extracted text")
    parser.add_argument("--out-images", default="", help="Output directory for cropped PNG figures")
    parser.add_argument("--dpi", type=int, default=200, help="DPI for cropped figures (default: 200)")
    args = parser.parse_args()

    doc = pymupdf.open(args.pdf)
    if args.toc:
        show_toc(doc)
    else:
        extract_range(doc, args.start_page, args.end_page, args.out_text, args.out_images, args.dpi)


if __name__ == "__main__":
    main()

