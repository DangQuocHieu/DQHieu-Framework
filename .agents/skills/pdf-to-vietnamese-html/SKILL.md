---
name: pdf-to-vietnamese-html
description: >-
  Translates technical PDF documents or specific book sections (such as Unity e-books,
  SOLID principles, Design Patterns, or programming guides) into a rich, interactive
  Vietnamese HTML document with extracted high-DPI diagrams, syntax-highlighted code blocks,
  and auto-generated sidebar navigation. Use this skill whenever the user provides a PDF/document
  and asks to translate a section or the whole document into Vietnamese HTML while minimizing token usage.
---

# Biên Dịch Tài Liệu PDF Sang HTML Tiếng Việt (Tiết Kiệm Token)

Skill này tự động hóa khâu trích xuất văn bản, cắt hình ảnh/sơ đồ từ PDF và đóng gói giao diện HTML để **không phải đọc PDF bằng ảnh chụp màn hình (tiết kiệm >85% token ngữ cảnh)** và **không phải viết lại khung giao diện HTML/CSS/JS (tiết kiệm >3.000 output token mỗi lần chạy)**.

## Quy trình thực hiện (4 Bước)

### Bước 1: Kiểm tra Mục lục & Xác định Trang (Không dùng `read_file` trực tiếp lên PDF)
> **QUAN TRỌNG:** Tuyệt đối KHÔNG gọi công cụ `read_file` trực tiếp lên tệp `.pdf` vì công cụ đó sẽ nạp ảnh chụp toàn trang vào ngữ cảnh gây tốn hàng chục ngàn token. Hãy dùng script [extract_pdf_section.py](./scripts/extract_pdf_section.py).

Chạy lệnh sau để xem Mục lục (Bookmarks & các trang đầu) và xác định số trang bắt đầu (`--start-page`) và kết thúc (`--end-page`) của mục cần dịch:

```powershell
py ".agents/skills/pdf-to-vietnamese-html/scripts/extract_pdf_section.py" --pdf "<duong_dan_file.pdf>" --toc
```

### Bước 2: Trích xuất Văn bản Thuần & Tự động Cắt Hình Minh Họa
Khi đã biết phạm vi trang `(X..Y)`, chạy lệnh trích xuất văn bản kèm cắt tự động mọi sơ đồ/hình ảnh ở độ phân giải cao (200 DPI):

```powershell
py ".agents/skills/pdf-to-vietnamese-html/scripts/extract_pdf_section.py" `
  --pdf "<duong_dan_file.pdf>" `
  --start-page <X> `
  --end-page <Y> `
  --out-text "<thu_muc_tam>/extracted_text.md" `
  --out-images "Docs/images"
```

- Script sẽ lưu toàn bộ chữ của các trang `X..Y` vào `<thu_muc_tam>/extracted_text.md` kèm các nhãn vị trí ảnh dạng `{{IMG_P<trang>}}` (hoặc `{{IMG_P<trang>_1}}`, `{{IMG_P<trang>_2}}` nếu 1 trang có nhiều hình).
- Toàn bộ hình ảnh/sơ đồ trong trang (đã tự động loại bỏ banner trang bìa và logo nhỏ ở góc) được lưu thẳng vào `Docs/images/img_p<trang>.png`.

### Bước 3: Dịch Nội Dung Sang Các File HTML Fragment
1. Đọc tệp văn bản nhẹ `<thu_muc_tam>/extracted_text.md` bằng `read_file` và tham khảo quy chuẩn thuật ngữ + mẫu thẻ tại [unity-glossary.md](./references/unity-glossary.md).
2. Dịch đầy đủ, trọn vẹn 100% nội dung (văn bản, danh sách, hộp lưu ý, chú thích hình ảnh và comment trong code) sang tiếng Việt chuyên ngành mượt mà.
3. **Chỉ viết phần ruột `<section class="chapter-section">...</section>`** vào các file fragment tạm (ví dụ: `<thu_muc_tam>/part1.html`, `part2.html`). Không cần viết thẻ `<html>`, `<head>`, `<style>` hay `<script>`.
4. Giữ nguyên các placeholder hình ảnh `{{IMG_P...}}` bên trong thẻ `<figure class="doc-figure">{{IMG_P...}}<figcaption>...</figcaption></figure>`.

### Bước 4: Đóng gói Thành File HTML Hoàn Chỉnh & Kiểm Tra
Chạy script [build_html.py](./scripts/build_html.py) để tự động ghép các fragment vào khung giao diện [template.html](./resources/template.html), tự động sinh thanh Mục lục (Sidebar TOC) từ các thẻ `<section>`, `<h1>`, `<h2>`, và thay thế toàn bộ `{{IMG_...}}` bằng thẻ `<img>`:

```powershell
py ".agents/skills/pdf-to-vietnamese-html/scripts/build_html.py" `
  --title "<Tiêu đề tài liệu Tiếng Việt>" `
  --subtitle "<Phụ đề ngắn gọn, ví dụ: Bản dịch Tiếng Việt • Trang X–Y>" `
  --badge "<Nhãn 배지, ví dụ: Unity 6 E-Book • Bản Dịch Tiếng Việt>" `
  --desc "<Mô tả tóm tắt nội dung phần dịch>" `
  --meta "<Thông tin tác giả / phạm vi trang>" `
  --parts "<thu_muc_tam>/part1.html" "<thu_muc_tam>/part2.html" `
  --images-dir "images" `
  --out "Docs/<Ten_File_Output>.html"
```

**Kiểm tra hoàn tất (Validation):**
- Đảm bảo lệnh `build_html.py` trả về `Built HTML successfully` và không có cảnh báo `WARNING: Unreplaced placeholders found`.
- Cung cấp đường dẫn liên kết tới file HTML trong thư mục `Docs/` cho người dùng.

