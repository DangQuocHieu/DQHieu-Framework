---
name: topic-research-to-html
description: >-
  Researches any technical topic (Unity, C#, game architecture, performance, design patterns, graphics)
  using subagents, and synthesizes the findings into an interactive, senior-level HTML documentation
  featuring embedded SVG diagrams, anti-pattern vs best practice code comparisons, and responsive layout.
  Use whenever the user requests in-depth technical research on a topic and wants the output packaged as
  an HTML documentation.
---

# Nghiên Cứu Kỹ Thuật Chuyên Sâu & Đóng Gói Thành Tài Liệu HTML

Skill này tự động hóa quy trình 4 giai đoạn: **Nghiên cứu đa nguồn qua Subagent -> Tổng hợp & cấu trúc kiến thức chuyên sâu -> Trực quan hóa bằng sơ đồ SVG -> Tự động đóng gói thành tài liệu HTML tương tác cao cấp**.

---

## Tổng Quan Luồng Thực Hiện (4 Giai Đoạn)

```
[Chủ đề kỹ thuật từ User]
          │
          ▼
┌──────────────────────────────────────────────┐
│ Giai Đoạn 1: Điều phối Subagent Nghiên Cứu   │
│ (Áp dụng 6 trục nghiên cứu chuyên sâu)       │
└──────────────────────┬───────────────────────┘
                       │
                       ▼
┌──────────────────────────────────────────────┐
│ Giai Đoạn 2: Xây dựng Cấu Trúc Tài Liệu      │
│ (Phong cách Senior Mentorship, 6 phần chuẩn) │
└──────────────────────┬───────────────────────┘
                       │
                       ▼
┌──────────────────────────────────────────────┐
│ Giai Đoạn 3: Soạn Thảo Ruột HTML Fragments   │
│ (Code so sánh Sai vs Đúng + Sơ đồ SVG chuẩn) │
└──────────────────────┬───────────────────────┘
                       │
                       ▼
┌──────────────────────────────────────────────┐
│ Giai Đoạn 4: Tự Động Build Tài Liệu HTML     │
│ (build_doc.py + doc_template.html)           │
└──────────────────────────────────────────────┘
                       │
                       ▼
   [Docs/<Ten_Chu_De_Guide>.html hoàn chỉnh]
```

---

## Hướng Dẫn Thực Hiện Chi Tiết

### Giai Đoạn 1: Điều Phối Subagent Nghiên Cứu

Khi người dùng cung cấp một chủ đề (ví dụ: `static trong Unity`, `Object Pooling`, `Addressables vs Resources`, `Garbage Collection Optimization`):

1. Khởi động subagent nghiên cứu bằng công cụ `invoke_subagent` (với `TypeName: "research"`, `Role: "<Topic> Researcher"`).
2. Áp dụng mẫu prompt chuẩn từ file [research_prompt_template.md](./references/research_prompt_template.md) gồm 6 trục khảo sát:
   - **Bản chất kỹ thuật & Cơ chế nền tảng** (Memory, Stack vs Heap, Vòng đời, GC Root).
   - **Đặc thù trong Engine / Framework** (Game loop, Scene loading, Domain reload, Multi-threading, Coroutine/Async).
   - **Ưu điểm & Nhược điểm kỹ thuật** (Performance vs Coupling, Testing, Global State Mutation).
   - **Ma trận quyết định (Decision Matrix)**: Khi nào NÊN dùng vs KHÔNG NÊN dùng.
   - **Bộ Code Mẫu Đối Chiếu**: Anti-pattern (❌) đối chiếu trực tiếp với Best Practice (✅).
   - **Ý tưởng sơ đồ trực quan hóa** (Memory diagram, Lifecycle, Decision tree).

> **Lưu ý về Subagent:** Sau khi gọi `invoke_subagent`, không chạy vòng lặp thăm dò (polling). Hệ thống sẽ tự động đánh thức khi subagent gửi báo cáo hoàn tất.

---

### Giai Đoạn 2: Chuẩn Hóa Cấu Trúc Tài Liệu Theo Phong Cách Senior Developer

Dựa trên dữ liệu từ subagent, tài liệu HTML đầu ra luôn tuân theo bộ khung 6 phần chuẩn mực:
- **Lời Mở Đầu (Senior Callout):** Góc nhìn thực tế, cảnh báo thói quen sai lầm của người mới bắt đầu.
- **Phần 1: Bản Chất Kỹ Thuật (Fundamentals):** Định nghĩa, phân loại và cơ chế cấp phát bộ nhớ.
- **Phần 2: Đặc Thù Trong Game Engine (Engine Internals):** Vòng đời Scene, Memory Leak, Ghost/Zombie Objects, Domain Reloading.
- **Phần 3: Ưu Điểm & Nhược Điểm Toàn Diện:** Trình bày dạng Grid 2 cột đối chiếu rõ ràng.
- **Phần 4: Ma Trận Quyết Định (Decision Matrix):** Khi nào NÊN dùng vs KHÔNG NÊN dùng, kèm Flowchart tư duy.
- **Phần 5: Thực Chiến Đối Chiếu (Anti-Pattern vs Best Practice):** Tối thiểu 3-4 cặp code có nút copy và giải thích cơ chế ngầm.
- **Phần 6: Bảng Kiểm Tra (Senior Checklist):** 5 tiêu chuẩn vàng trước khi merge Pull Request.

---

### Giai Đoạn 3: Soạn Thảo Ruột HTML Fragments & Sơ Đồ SVG

Soạn các phần nội dung lưu vào file tạm trong thư mục scratch (hoặc ghép trực tiếp). Mỗi phần được bao bọc bởi thẻ `<section>`:

```html
<section class="doc-section" id="ten-section">
    <div class="section-header">
        <span class="section-tag">PHẦN 1</span>
        <h2>Tiêu Đề Phần</h2>
    </div>
    <p class="section-lead">Mô tả ngắn gọn mở đầu...</p>
    
    <h3 id="muc-con-1">1.1 Tiêu Đề Mục Con</h3>
    <!-- Nội dung, Callout, Code, SVG... -->
</section>
```

#### Quy Chuẩn Các Thành Phần Giao Diện Sẵn Có:
1. **Hộp Lưu Ý Chuyên Gia (Senior Callout):**
   ```html
   <div class="callout callout-senior">
       <div class="callout-icon">💡</div>
       <div class="callout-content">
           <div class="callout-title">Góc Nhìn Senior:</div>
           <div class="callout-text">Nội dung chia sẻ kinh nghiệm...</div>
       </div>
   </div>
   ```
   *(Các biến thể khác: `callout-warning`, `callout-tip`, `callout-info`)*

2. **Khối Code Đối Chiếu Chuẩn (Code Block Wrapper):**
   ```html
   <div class="code-block-wrapper">
       <div class="code-block-header">
           <span class="code-label label-danger">❌ ANTI-PATTERN: Lỗi rò rỉ bộ nhớ</span>
           <button class="copy-btn" onclick="copyCode(this)">Copy Code</button>
       </div>
       <pre><code class="language-csharp">// Code minh họa ở đây...</code></pre>
   </div>
   ```
   *(Cho code chuẩn: dùng class `label-success` kèm nhãn `✅ BEST PRACTICE`)*

3. **Sơ Đồ Vector SVG Trực Quan:**
   Tham khảo các mẫu sẵn tại [svg_diagram_templates.md](./references/svg_diagram_templates.md) để nhúng trực tiếp thẻ `<svg viewBox="...">` bên trong `<div class="diagram-container">`.

---

### Giai Đoạn 4: Tự Động Đóng Gói Bằng `build_doc.py`

Khi đã chuẩn bị xong các fragment nội dung, chạy lệnh sau để ghép vào mẫu giao diện chuẩn [doc_template.html](./resources/doc_template.html):

```powershell
py ".agents/skills/topic-research-to-html/scripts/build_doc.py" `
  --title "<Tiêu đề trình duyệt (HTML Title)>" `
  --hero-title "<Tiêu đề lớn hiển thị ở Banner chính>" `
  --sidebar-title "<Tiêu đề ngắn trên Sidebar>" `
  --sidebar-subtitle "<Phụ đề, ví dụ: Cẩm nang chuẩn từ Senior Developer>" `
  --badge "⚡ Senior Mentorship" `
  --desc "<Đoạn tóm tắt nội dung 1-2 câu>" `
  --meta '<span class="meta-pill pill-senior">👨‍💻 Senior Developer</span><span class="meta-pill pill-time">⏱️ 15 phút</span>' `
  --parts "<duong_dan_fragment_1.html>" "<duong_dan_fragment_2.html>" `
  --out "Docs/<Ten_File_Output>.html"
```

Script sẽ tự động:
1. Quét toàn bộ thẻ `<section id="...">`, `<h2>`, `<h3>` để sinh thanh Mục lục (Sidebar TOC) động với liên kết neo cuộn mượt (Scrollspy).
2. Gắn bộ nhận diện syntax highlight Prism/Highlight.js cho code C#/C++/Shader.
3. Kích hoạt bộ chuyển đổi giao diện Sáng / Tối (Dark & Light Mode Switcher).
4. Bật tính năng sao chép code 1-click cho mọi khối code trong tài liệu.

---

## Đầu Ra (Output) Chuẩn
Một tệp HTML duy nhất tại `Docs/<Ten_File>.html` độc lập, gọn nhẹ, có thể mở trực tiếp trên mọi trình duyệt web mà không cần cài đặt thêm web server.

