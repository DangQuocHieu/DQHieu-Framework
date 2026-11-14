---
name: topic-research-to-html
description: >-
  Researches any technical topic (Unity, C#, game architecture, performance, design patterns, graphics)
  using subagents, and synthesizes the findings into an interactive, beginner-friendly HTML documentation
  with plain-language analogies, glossary, embedded SVG diagrams, step-by-step code comparisons, and responsive layout.
  Use whenever the user requests in-depth technical research on a topic and wants the output packaged as
  an HTML documentation.
---

# Nghiên Cứu Kỹ Thuật & Đóng Gói Thành Tài Liệu HTML Dễ Hiểu

Skill này tự động hóa quy trình 4 giai đoạn: **Nghiên cứu qua Subagent -> Chuyển hóa thành ngôn ngữ bình dân, dễ tiếp cận -> Trực quan hóa bằng sơ đồ SVG tiếng Việt -> Tự động đóng gói thành tài liệu HTML tương tác**.

---

## Nguyên Tắc Vàng Khi Viết Tài Liệu (Cho Người Mới & Low-Tech)

Mục tiêu là giúp **một người chưa rành công nghệ hoặc mới làm quen vẫn hiểu được bản chất**, đồng thời vẫn đảm bảo chính xác để áp dụng vào dự án thật:

1. **Giải thích bình dân trước, thuật ngữ sau:**
   - Hạn chế tối đa từ tiếng Anh chuyên ngành nếu có cách nói tiếng Việt tự nhiên, dễ hiểu.
   - Khi *bắt buộc* dùng thuật ngữ (ví dụ: `Static`, `Heap`, `Garbage Collector`, `Serialization`, `Instance`), **ngay lần đầu xuất hiện** phải giải thích ngắn gọn theo công thức: **Thuật ngữ + ("hiểu đơn giản là...") + Ví dụ đời thực**.
   - Không dùng thuật ngữ này để giải thích cho thuật ngữ khác.
2. **Luôn có ẩn dụ đời thường (`callout-analogy`):**
   - Mỗi khái niệm trừu tượng phải đi kèm ít nhất 1 hình ảnh quen thuộc ngoài đời (ví dụ: *bảng thông báo chung của công ty*, *nhân viên dọn rác*, *bản vẽ thiết kế vs ngôi nhà thật*, *ổ khóa cửa*).
3. **Tách lớp thông tin (Progressive Disclosure):**
   - Kiến thức cốt lõi, dễ hiểu, giải quyết vấn đề thực tế để ở bên ngoài với câu ngắn gọn, súc tích.
   - Chi tiết kỹ thuật sâu (cơ chế trình biên dịch, vùng nhớ cấp thấp, tên gọi hàn lâm) gói vào hộp đóng/mở `<details class="deep-dive">` để người mới không bị ngợp, còn người muốn đào sâu vẫn có thể bấm xem.
4. **Đi từ "Bài toán thực tế" đến "Cách giải quyết":**
   - Trước khi định nghĩa một tính năng là gì, hãy nói rõ **nó sinh ra để giải quyết rắc rối gì** và **nếu không có nó thì sao**.
5. **Code mẫu phải có chú thích tiếng Việt từng bước:**
   - Không chỉ dán code khô khan. Mọi đoạn code minh họa đều phải có comment tiếng Việt giải thích ý nghĩa bằng ngôn ngữ đời thường.

---

## Tổng Quan Luồng Thực Hiện (4 Giai Đoạn)

```
[Chủ đề kỹ thuật từ User]
          │
          ▼
┌──────────────────────────────────────────────┐
│ Giai Đoạn 1: Điều phối Subagent Nghiên Cứu   │
│ (Tìm hiểu bản chất + ví dụ đời thường)       │
└──────────────────────┬───────────────────────┘
                       │
                       ▼
┌──────────────────────────────────────────────┐
│ Giai Đoạn 2: Xây dựng Cấu Trúc 6 Phần        │
│ (Từ điển nhanh -> Dễ hiểu -> Thực hành)      │
└──────────────────────┬───────────────────────┘
                       │
                       ▼
┌──────────────────────────────────────────────┐
│ Giai Đoạn 3: Soạn Thảo HTML Fragments & SVG  │
│ (Ẩn dụ + Sơ đồ tiếng Việt + Code có chú thích)│
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

Khi người dùng cung cấp một chủ đề (ví dụ: `static trong Unity`, `Object Pooling`, `DataManager & JSON`, `Garbage Collection`):

1. Khởi động subagent nghiên cứu bằng công cụ `invoke_subagent` (với `TypeName: "research"`, `Role: "<Topic> Researcher"`).
2. Áp dụng mẫu prompt chuẩn từ file [research_prompt_template.md](./references/research_prompt_template.md) gồm 6 trục khảo sát:
   - **Giải thích bình dân & Từ điển thuật ngữ nhanh**: Bài toán thực tế, ví dụ ẩn dụ đời thường, giải thích 5–8 thuật ngữ cốt lõi bằng ngôn ngữ đời thường.
   - **Bản chất & Cách hoạt động (Từ cơ bản đến nâng cao)**: Tách rõ phần cốt lõi dễ hiểu và phần kỹ thuật sâu.
   - **Đặc thù trong Engine / Framework**: Những tình huống bất ngờ thường gặp khi chạy thật (chuyển Scene, bấm Play/Stop...) và lý do dễ hiểu.
   - **Ưu điểm, Nhược điểm & Khi nào nên dùng**: Diễn đạt bằng hệ quả thực tế thay vì chỉ nêu thuật ngữ.
   - **Bộ Code Mẫu Thực Chiến**: Cách làm dễ gây lỗi (❌) đối chiếu với Cách làm an toàn (✅) kèm comment tiếng Việt từng bước.
   - **Ý tưởng sơ đồ trực quan hóa**: Dùng nhãn tiếng Việt dễ hiểu, gắn với hình ảnh đời thường.

> **Lưu ý về Subagent:** Sau khi gọi `invoke_subagent`, không chạy vòng lặp thăm dò (polling). Hệ thống sẽ tự động đánh thức khi subagent gửi báo cáo hoàn tất.

---

### Giai Đoạn 2: Chuẩn Hóa Cấu Trúc Tài Liệu Dễ Tiếp Cận (6 Phần)

Tài liệu HTML đầu ra luôn giữ bố cục 6 phần rõ ràng, mạch lạc nhưng trình bày theo hướng dẫn dắt từ dễ đến khó:

- **Lời Mở Đầu (Hộp Tóm Tắt Nhanh):** Trả lời trong 2–3 câu: *Chủ đề này là gì? Dùng để làm gì ngoài thực tế?*
- **Phần 1: Hiểu Đơn Giản & Từ Điển Thuật Ngữ Nhanh:**
  - Ví dụ ẩn dụ đời thường xuyên suốt (`.callout-analogy`).
  - Bảng **"Từ Điển Thuật Ngữ Nhanh"** giải thích các từ khóa sẽ gặp trong bài bằng tiếng Việt bình dân.
- **Phần 2: Cách Hoạt Động & Những Điều Hay Gặp Trong Thực Tế (Unity / Engine):**
  - Giải thích từng bước bằng ngôn ngữ tự nhiên, kèm sơ đồ SVG tiếng Việt trực quan.
  - Các chi tiết kỹ thuật chuyên sâu (như bộ nhớ, runtime, trình biên dịch) đặt trong thẻ `<details class="deep-dive">` để người mới có thể bỏ qua nếu chưa cần.
- **Phần 3: Điểm Mạnh & Rủi Ro Thực Tế:**
  - Trình bày dạng Grid 2 cột đối chiếu: *Được lợi gì?* vs *Dễ gặp rắc rối gì?* (mô tả bằng hệ quả cụ thể, dễ hình dung).
- **Phần 4: Khi Nào Nên Dùng? (Bảng Quyết Định Có / Không):**
  - Bảng hoặc sơ đồ cây hỏi–đáp đơn giản dạng *Nếu bạn cần X -> Hãy dùng Y; Nếu gặp tình huống Z -> Tránh dùng*.
- **Phần 5: Ví Dụ Thực Tế (Cách Dễ Lỗi vs Cách An Toàn):**
  - Tối thiểu 3–4 cặp code có nút copy, comment tiếng Việt giải thích từng dòng quan trọng và chỉ rõ hậu quả nếu viết sai.
- **Phần 6: Bảng Tự Kiểm Tra Nhanh (Checklist 5 Điểm):**
  - 5 câu hỏi ngắn gọn bằng ngôn ngữ đời thường để người dùng tự rà soát trước khi đưa code vào dự án.

---

### Giai Đoạn 3: Soạn Thảo Ruột HTML Fragments & Sơ Đồ SVG

Soạn các phần nội dung lưu vào file tạm trong thư mục scratch (hoặc ghép trực tiếp). Mỗi phần được bao bọc bởi thẻ `<section>`:

```html
<section class="doc-section" id="ten-section">
    <div class="section-header">
        <span class="section-tag">PHẦN 1</span>
        <h2>Tiêu Đề Phần</h2>
    </div>
    <p class="section-lead">Mô tả ngắn gọn, dễ hiểu mở đầu phần...</p>
    
    <h3 id="muc-con-1">1.1 Tiêu Đề Mục Con</h3>
    <!-- Nội dung, Callout, Code, SVG... -->
</section>
```

#### Quy Chuẩn Các Thành Phần Giao Diện Sẵn Có:
1. **Hộp Ví Dụ Đời Thường (Analogy Callout — Khuyên dùng nhiều):**
   ```html
   <div class="callout callout-analogy">
       <div class="callout-icon">🌱</div>
       <div class="callout-content">
           <div class="callout-title">Hình dung đơn giản:</div>
           <div class="callout-text">Ví dụ thực tế quen thuộc giúp người mới hiểu ngay...</div>
       </div>
   </div>
   ```
   *(Các biến thể khác: `callout-senior` cho lời khuyên kinh nghiệm, `callout-warning` cho cảnh báo lỗi, `callout-tip` cho mẹo hay, `callout-info` cho ghi chú)*

2. **Hộp Đào Sâu Kỹ Thuật (Collapsible Deep-Dive — Tách lớp cho người muốn tìm hiểu thêm):**
   ```html
   <details class="deep-dive">
       <summary>🔍 Góc đào sâu kỹ thuật (Không bắt buộc cho người mới)</summary>
       <div class="deep-dive-body">
           <p>Giải thích chi tiết cơ chế bên dưới dành cho ai muốn tìm hiểu sâu hơn...</p>
       </div>
   </details>
   ```

3. **Khối Code Đối Chiếu Chuẩn (Code Block Wrapper):**
   ```html
   <div class="code-block-wrapper">
       <div class="code-block-header">
           <span class="code-label label-danger">❌ CÁCH DỄ GÂY LỖI: Lý do ngắn gọn dễ hiểu</span>
           <button class="copy-btn" onclick="copyCode(this)">Copy Code</button>
       </div>
       <pre><code class="language-csharp">// Chú thích tiếng Việt giải thích từng bước bằng ngôn ngữ đời thường...</code></pre>
   </div>
   ```
   *(Cho code chuẩn: dùng class `label-success` kèm nhãn `✅ CÁCH AN TOÀN, DỄ BẢO TRÌ`)*

4. **Sơ Đồ Vector SVG Trực Quan (Tiếng Việt Dễ Hiểu):**
   Tham khảo các mẫu sẵn tại [svg_diagram_templates.md](./references/svg_diagram_templates.md) để nhúng trực tiếp thẻ `<svg viewBox="...">` bên trong `<div class="diagram-container">`, luôn kèm 1–2 câu chú thích giải thích sơ đồ bằng ngôn ngữ bình dân.

---

### Giai Đoạn 4: Tự Động Đóng Gói Bằng `build_doc.py`

Khi đã chuẩn bị xong các fragment nội dung, chạy lệnh sau để ghép vào mẫu giao diện chuẩn [doc_template.html](./resources/doc_template.html):

```powershell
py ".agents/skills/topic-research-to-html/scripts/build_doc.py" `
  --title "<Tiêu đề trình duyệt (HTML Title)>" `
  --hero-title "<Tiêu đề lớn hiển thị ở Banner chính>" `
  --sidebar-title "<Tiêu đề ngắn trên Sidebar>" `
  --sidebar-subtitle "<Phụ đề thân thiện, ví dụ: Hướng dẫn dễ hiểu từ A-Z>" `
  --badge "📘 Cẩm Nang Dễ Hiểu" `
  --desc "<Đoạn tóm tắt nội dung 1-2 câu bằng ngôn ngữ bình dân>" `
  --meta '<span class="meta-pill pill-level">🌱 Thân thiện người mới</span><span class="meta-pill pill-time">⏱️ 10-15 phút</span>' `
  --parts "<duong_dan_fragment_1.html>" "<duong_dan_fragment_2.html>" `
  --out "Docs/<Ten_File_Output>.html"
```

Script sẽ tự động:
1. Quét toàn bộ thẻ `<section id="...">`, `<h2>`, `<h3>` để sinh thanh Mục lục (Sidebar TOC) động với liên kết neo cuộn mượt (Scrollspy).
2. Gắn bộ nhận diện syntax highlight cho code C#/C++/Shader.
3. Kích hoạt bộ chuyển đổi giao diện Sáng / Tối (Dark & Light Mode Switcher).
4. Bật tính năng sao chép code 1-click cho mọi khối code trong tài liệu.

---

## Đầu Ra (Output) Chuẩn
Một tệp HTML duy nhất tại `Docs/<Ten_File>.html` độc lập, gọn nhẹ, bố cục rõ ràng và ngôn ngữ thân thiện với cả người mới hoặc người Low-tech, có thể mở trực tiếp trên mọi trình duyệt web mà không cần cài đặt thêm web server.
