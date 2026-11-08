#!/usr/bin/env python3
"""
Combines translated HTML section fragments into a standalone interactive Vietnamese HTML document.
Automatically generates the Sidebar Table of Contents (TOC) from <section>, <h1>, and <h2> tags,
and replaces {{IMG_...}} placeholders with <img src="..." loading="lazy" /> tags.

Usage:
  py .agents/skills/pdf-to-vietnamese-html/scripts/build_html.py \
      --title "Nguyên Lý SOLID trong Lập Trình Game Unity" \
      --subtitle "Bản dịch Tiếng Việt chuyên sâu (Trang 6–47)" \
      --badge "Unity 6 Preview Edition • E-Book Chính Thức" \
      --desc "Bản biên dịch đầy đủ từ tài liệu chính thức của Unity." \
      --meta "📚 Tác giả gốc: Wilmer Lin | 🎯 Phạm vi: Trang 6–47" \
      --parts "scratch/solid_part1.html" "scratch/solid_part2.html" \
      --out "Docs/SOLID_Principles_Unity_VN.html"
"""

import argparse
import os
import re
import sys


def strip_tags(html_str):
    # Remove <small>...</small> for cleaner TOC titles
    html_str = re.sub(r"<small\b[^>]*>.*?</small>", "", html_str, flags=re.DOTALL | re.IGNORECASE)
    html_str = re.sub(r"<br\s*/?>", " ", html_str, flags=re.IGNORECASE)
    return re.sub(r"<[^>]+>", "", html_str).strip()


def generate_toc(body_html):
    section_pattern = re.compile(
        r'<section\s+id="([^"]+)"[^>]*class="[^"]*chapter-section[^"]*"[^>]*>(.*?)</section>',
        re.DOTALL | re.IGNORECASE,
    )
    h1_pattern = re.compile(r"<h1\b[^>]*>(.*?)</h1>", re.DOTALL | re.IGNORECASE)
    badge_pattern = re.compile(r'<span\s+class="chapter-badge"[^>]*>(.*?)</span>', re.DOTALL | re.IGNORECASE)
    h2_pattern = re.compile(r'<h2\s+id="([^"]+)"[^>]*>(.*?)</h2>', re.DOTALL | re.IGNORECASE)

    toc_items = []
    for idx, match in enumerate(section_pattern.finditer(body_html), start=1):
        sec_id = match.group(1)
        sec_content = match.group(2)

        h1_match = h1_pattern.search(sec_content)
        title = strip_tags(h1_match.group(1)) if h1_match else sec_id

        badge_match = badge_pattern.search(sec_content)
        page_label = ""
        if badge_match:
            badge_text = strip_tags(badge_match.group(1))
            p_match = re.search(r"(?:Trang|tr\.)\s*(\d+)", badge_text, re.IGNORECASE)
            if p_match:
                page_label = f"tr.{p_match.group(1)}"

        sub_items = []
        for h2_m in h2_pattern.finditer(sec_content):
            h2_id = h2_m.group(1)
            h2_title = strip_tags(h2_m.group(2))
            sub_items.append(f'<li><a href="#{h2_id}" class="toc-sublink">{h2_title}</a></li>')

        sub_html = ""
        if sub_items:
            sub_html = "\n                    <ul class=\"toc-sublist\">\n                        " + "\n                        ".join(sub_items) + "\n                    </ul>"

        toc_items.append(
            f"""<li class="toc-item">
                    <a href="#{sec_id}" class="toc-link"><span>{idx}. {title}</span><span class="toc-page">{page_label}</span></a>{sub_html}
                </li>"""
        )

    return "\n                ".join(toc_items)


def replace_image_placeholders(body_html, images_rel_dir="images"):
    def repl(match):
        key = match.group(1)
        fname = f"{key.lower()}.png"
        p_match = re.search(r"P(\d+)", key, re.IGNORECASE)
        pno = p_match.group(1) if p_match else ""
        alt = f"Hình minh họa trang {pno}" if pno else "Hình minh họa"
        return f'<img src="{images_rel_dir}/{fname}" alt="{alt}" loading="lazy" />'

    return re.sub(r"\{\{(IMG_[A-Za-z0-9_]+)\}\}", repl, body_html)


def main():
    parser = argparse.ArgumentParser(description="Build standalone Vietnamese HTML document from fragments.")
    parser.add_argument("--title", required=True, help="Main document title")
    parser.add_argument("--subtitle", default="Bản dịch Tiếng Việt chuyên sâu", help="Topbar subtitle")
    parser.add_argument("--badge", default="Tài Liệu Kỹ Thuật • Bản Dịch Tiếng Việt", help="Hero badge text")
    parser.add_argument("--desc", default="", help="Hero description paragraph")
    parser.add_argument("--meta", default="", help="Hero metadata line")
    parser.add_argument("--parts", nargs="+", required=True, help="List of HTML fragment files in order")
    parser.add_argument("--images-dir", default="images", help="Relative images folder path from output HTML")
    parser.add_argument("--template", default="", help="Optional custom template.html path")
    parser.add_argument("--out", required=True, help="Output HTML file path")
    args = parser.parse_args()

    script_dir = os.path.dirname(os.path.abspath(__file__))
    template_path = args.template or os.path.join(os.path.dirname(script_dir), "resources", "template.html")

    with open(template_path, "r", encoding="utf-8") as f:
        template = f.read()

    parts_content = []
    for p in args.parts:
        with open(p, "r", encoding="utf-8") as f:
            parts_content.append(f.read())

    body_html = "\n\n".join(parts_content)
    body_html = replace_image_placeholders(body_html, args.images_dir)
    toc_html = generate_toc(body_html)

    final_html = (
        template.replace("{{DOC_TITLE}}", args.title)
        .replace("{{DOC_SUBTITLE}}", args.subtitle)
        .replace("{{HERO_BADGE}}", args.badge)
        .replace("{{HERO_DESC}}", args.desc)
        .replace("{{HERO_META}}", args.meta)
        .replace("{{TOC_ITEMS}}", toc_html)
        .replace("{{BODY_CONTENT}}", body_html)
    )

    leftover = re.findall(r"\{\{[A-Z0-9_]+\}\}", final_html)
    if leftover:
        print(f"WARNING: Unreplaced placeholders found: {leftover}", file=sys.stderr)

    os.makedirs(os.path.dirname(os.path.abspath(args.out)), exist_ok=True)
    with open(args.out, "w", encoding="utf-8") as f:
        f.write(final_html)

    print(f"Built HTML successfully: {args.out} ({os.path.getsize(args.out)} bytes)")


if __name__ == "__main__":
    main()

