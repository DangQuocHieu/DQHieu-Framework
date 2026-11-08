# Quy chuẩn Dịch thuật & Cấu trúc HTML Fragment (Unity / Kỹ thuật Phần mềm)

## 1. Nguyên tắc Dịch thuật Chuyên ngành (Bilingual Terminology)
- **Không dịch tên riêng của API, Class, Method, Component, Interface trong Unity & C#**: Giữ nguyên trong thẻ `<code>` (ví dụ: `MonoBehaviour`, `GameObject`, `ScriptableObject`, `SerializeField`, `Prefab`, `Rigidbody`, `Collider`, `Update`, `Awake`, `OnEnable`, `OnDestroy`, `UI Toolkit`, `UXML`, `USS`).
- **Tiêu đề và Thuật ngữ cốt lõi**: Dịch nghĩa tiếng Việt chuẩn xác kết hợp mở ngoặc thuật ngữ tiếng Anh gốc ở lần xuất hiện đầu tiên hoặc trên tiêu đề để người đọc dễ tra cứu:
  - *Single-responsibility principle (SRP)* → Nguyên lý Đơn trách nhiệm (Single-responsibility principle)
  - *Open-closed principle (OCP)* → Nguyên lý Đóng–Mở (Open-closed principle)
  - *Liskov substitution principle (LSP)* → Nguyên lý Thay thế Liskov (Liskov substitution principle)
  - *Interface segregation principle (ISP)* → Nguyên lý Phân tách Interface (Interface segregation principle)
  - *Dependency inversion principle (DIP)* → Nguyên lý Đảo ngược Phụ thuộc (Dependency inversion principle)
  - *Loose coupling & High cohesion* → Liên kết lỏng lẻo (loose coupling) và Gắn kết nội tại cao (high cohesion)
  - *Composition over inheritance* → Ưu tiên kết hợp hơn kế thừa (Composition over inheritance)
  - *Garbage Collection (GC) spike* → Đột biến thu gom rác (Garbage collection spike)
  - *Encapsulation* → Đóng gói (Encapsulation)
  - *Abstraction* → Trừu tượng hóa (Abstraction)
  - *Intrinsic / Extrinsic state* → Trạng thái nội tại (Intrinsic / dùng chung) và Trạng thái ngoại tại (Extrinsic / riêng biệt)
- **Mã nguồn (Code blocks)**: Giữ nguyên 100% logic code C# gốc, chỉ dịch phần chú thích (`// comment`) sang tiếng Việt rõ nghĩa.

---

## 2. Mẫu HTML Fragment Chuẩn (Dùng cho `build_html.py`)

Trình biên dịch `build_html.py` tự động quét các thẻ dưới đây để sinh thanh Mục lục (Sidebar TOC) và gắn nút Sao chép code:

```html
<section id="ten-muc-slug" class="chapter-section">
    <div class="chapter-header">
        <span class="chapter-badge">Phần I • Trang 12–17</span>
        <h1>Tiêu Đề Tiếng Việt<br><small>Original English Title</small></h1>
    </div>

    <p class="lead">Đoạn văn mở đầu chương (in đậm/lớn hơn bình thường).</p>
    <p>Nội dung văn bản thông thường có chứa <code>InlineCode</code>.</p>

    <h2 id="muc-con-slug">Tiêu đề mục con cấp 2 (Tự động đưa vào Sidebar TOC)</h2>

    <ul class="feature-list">
        <li><strong>Ý chính:</strong> Giải thích chi tiết.</li>
    </ul>

    <!-- Hình minh họa dùng placeholder từ extract_pdf_section.py -->
    <figure class="doc-figure">
        {{IMG_P13}}
        <figcaption>Chú thích hình ảnh dịch sang tiếng Việt.</figcaption>
    </figure>

    <!-- Khối code C# -->
    <div class="code-block">
        <div class="code-header"><span>C# — TenFile.cs</span><span class="page-ref">Trang 14</span></div>
        <pre><code class="language-csharp">public class Example : MonoBehaviour
{
    // Chú thích tiếng Việt
}</code></pre>
    </div>

    <!-- Hộp thông tin Callout / Factbox -->
    <div class="info-box">
        <div class="info-box-title">
            <span class="info-icon">i</span> Tiêu đề hộp thông tin
        </div>
        <p>Nội dung hộp thông tin phụ trợ.</p>
    </div>
</section>
```

