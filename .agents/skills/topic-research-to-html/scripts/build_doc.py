#!/usr/bin/env python3
"""
Combines research HTML fragments into a high-production interactive HTML documentation.
Automatically scans sections (<section id="...">), headers (<h2>, <h3>), builds a clean
Sidebar Table of Contents (TOC), and replaces template placeholders.

Usage:
    py .agents/skills/topic-research-to-html/scripts/build_doc.py \
        --title "Cẩm Nang Toàn Tập Về Static Trong Unity" \
        --hero-title "Từ Khóa `static` Trong Lập Trình Game Unity" \
        --sidebar-title "Static in Unity" \
        --sidebar-subtitle "Cẩm nang chuẩn từ Senior Developer" \
        --badge "⚡ Senior Mentorship" \
        --desc "Cẩm nang toàn diện giải mã bản chất của static trong C# và Unity..." \
        --meta '<span class="meta-pill pill-senior">👨‍💻 Senior Developer</span><span class="meta-pill pill-time">⏱️ 15 phút</span>' \
        --parts "scratch/part1.html" "scratch/part2.html" \
        --out "Docs/Static_In_Unity_Guide.html"
"""

import argparse
import os
import re
import sys


def slugify(text: str) -> str:
    """Converts a heading string to a clean URL-friendly anchor id."""
    text = re.sub(r"<[^>]+>", "", text)
    text = text.lower().strip()
    text = re.sub(r"[^\w\s-]", "", text)
    text = re.sub(r"[\s_-]+", "-", text)
    return text.strip("-")


def strip_html_tags(html_str: str) -> str:
    """Strips all HTML tags to leave plain text for labels."""
    html_str = re.sub(r"<br\s*/?>", " ", html_str, flags=re.IGNORECASE)
    return re.sub(r"<[^>]+>", "", html_str).strip()


def process_content_and_generate_toc(content_html: str):
    """
    Parses <section id="..."> and inner <h2>, <h3> to build the Sidebar TOC.
    Automatically assigns IDs to <h3> tags that do not have one.
    Returns (processed_content_html, toc_html).
    """
    section_pattern = re.compile(
        r'<section\s+([^>]*?)id="([^"]+)"([^>]*?)>(.*?)</section>',
        re.DOTALL | re.IGNORECASE
    )
    h2_pattern = re.compile(r'<h2\b[^>]*>(.*?)</h2>', re.DOTALL | re.IGNORECASE)
    tag_pattern = re.compile(r'<span\s+class="section-tag"[^>]*>(.*?)</span>', re.DOTALL | re.IGNORECASE)
    h3_pattern = re.compile(r'<h3(\s+[^>]*)?>(.*?)</h3>', re.DOTALL | re.IGNORECASE)

    toc_items = [
        '<li class="nav-item"><a href="#intro" class="nav-link active"><span>☕</span> Lời Mở Đầu</a></li>'
    ]

    modified_html = content_html

    for match in section_pattern.finditer(content_html):
        sec_id = match.group(2)
        sec_body = match.group(4)

        # Extract <h2> title
        h2_match = h2_pattern.search(sec_body)
        h2_title = strip_html_tags(h2_match.group(1)) if h2_match else sec_id

        # Extract section tag (e.g. PHẦN 1)
        tag_match = tag_pattern.search(sec_body)
        tag_label = strip_html_tags(tag_match.group(1)) + " " if tag_match else ""

        # Extract subheadings <h3>
        sub_items = []
        for h3_m in h3_pattern.finditer(sec_body):
            attrs = h3_m.group(1) or ""
            raw_title = h3_m.group(2)
            clean_title = strip_html_tags(raw_title)

            # Check if h3 has existing id
            id_match = re.search(r'id="([^"]+)"', attrs)
            if id_match:
                sub_id = id_match.group(1)
            else:
                sub_id = f"{sec_id}-{slugify(clean_title)}"
                # Replace this specific h3 tag in modified_html with id attribute
                old_h3 = h3_m.group(0)
                new_h3 = f'<h3 id="{sub_id}"{attrs}>{raw_title}</h3>'
                modified_html = modified_html.replace(old_h3, new_h3, 1)

            sub_items.append(
                f'<li><a href="#{sub_id}" class="nav-link">{clean_title}</a></li>'
            )

        sub_html = ""
        if sub_items:
            sub_html = "\n                <ul class=\"nav-sub\">\n                    " + "\n                    ".join(sub_items) + "\n                </ul>"

        toc_items.append(
            f"""<li class="nav-item">
                <a href="#{sec_id}" class="nav-link">
                    <span>{tag_label}</span>{h2_title}
                </a>{sub_html}
            </li>"""
        )

    full_toc = "\n            ".join(toc_items)
    return modified_html, full_toc


def main():
    parser = argparse.ArgumentParser(description="Build interactive HTML documentation from research.")
    parser.add_argument("--template", default="", help="Path to custom template.html")
    parser.add_argument("--title", required=True, help="HTML <title> tag")
    parser.add_argument("--hero-title", required=True, help="Main header title in hero banner")
    parser.add_argument("--sidebar-title", required=True, help="Sidebar title")
    parser.add_argument("--sidebar-subtitle", default="", help="Sidebar subtitle")
    parser.add_argument("--badge", default="⚡ Senior Mentorship", help="Badge text in hero & sidebar")
    parser.add_argument("--desc", default="", help="Hero description text")
    parser.add_argument("--meta", default="", help="Hero meta HTML tags (pills)")
    parser.add_argument("--parts", nargs="+", required=True, help="Fragment HTML files containing <section> tags")
    parser.add_argument("--out", required=True, help="Output destination HTML path")

    args = parser.parse_args()

    # Determine template path
    template_path = args.template
    if not template_path:
        script_dir = os.path.dirname(os.path.abspath(__file__))
        template_path = os.path.join(script_dir, "..", "resources", "doc_template.html")

    if not os.path.exists(template_path):
        print(f"Error: Template not found at {template_path}", file=sys.stderr)
        sys.exit(1)

    with open(template_path, "r", encoding="utf-8") as f:
        template = f.read()

    # Concatenate all parts
    combined_content = []
    for part_file in args.parts:
        if not os.path.exists(part_file):
            print(f"Warning: Part file not found: {part_file}", file=sys.stderr)
            continue
        with open(part_file, "r", encoding="utf-8") as f:
            combined_content.append(f.read().strip())

    full_body = "\n\n".join(combined_content)

    # Process TOC & headings
    processed_content, toc_html = process_content_and_generate_toc(full_body)

    # Fill placeholders
    output_html = template
    replacements = {
        "{{PAGE_TITLE}}": args.title,
        "{{HERO_TITLE}}": args.hero_title,
        "{{HERO_BADGE}}": args.badge,
        "{{SIDEBAR_TITLE}}": args.sidebar_title,
        "{{SIDEBAR_SUBTITLE}}": args.sidebar_subtitle,
        "{{HERO_DESC}}": args.desc,
        "{{HERO_META}}": args.meta,
        "{{SIDEBAR_TOC}}": toc_html,
        "{{MAIN_CONTENT}}": processed_content,
    }

    for key, value in replacements.items():
        output_html = output_html.replace(key, value)

    # Ensure output directory exists
    os.makedirs(os.path.dirname(os.path.abspath(args.out)), exist_ok=True)

    with open(args.out, "w", encoding="utf-8") as f:
        f.write(output_html)

    print(f"Successfully generated documentation: {args.out} ({len(output_html.encode('utf-8')):,} bytes)")


if __name__ == "__main__":
    main()

