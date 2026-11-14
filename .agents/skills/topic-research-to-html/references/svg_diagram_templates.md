# Thư Viện Mẫu Sơ Đồ SVG Trực Quan & Dễ Hiểu

Các mẫu sơ đồ SVG dưới đây được tối ưu hóa cho giao diện tài liệu (hỗ trợ Dark Mode, responsive qua `viewBox`, bảng màu rõ ràng: Blue `#38bdf8`, Purple `#a855f7`, Green `#10b981`, Red `#f43f5e`, Yellow `#fbbf24`).

> **Nguyên tắc thiết kế sơ đồ cho người Low-tech:**
> - **Dùng tiếng Việt bình dân** trong mọi hộp chữ trên sơ đồ (chỉ giữ từ khóa tiếng Anh trong ngoặc nếu thật sự cần đối chiếu).
> - **Mỗi hộp tối đa 1 ý ngắn gọn** (tránh nhồi nhét các cụm từ hàn lâm như *Type Descriptors, MethodTables, High Frequency Heap*).
> - **Luôn có 1 dòng chú thích ngắn bên dưới sơ đồ** tóm tắt sơ đồ này muốn nói điều gì trong 1-2 câu.

---

## 1. Mẫu Sơ Đồ So Sánh Hai Vùng Lưu Trữ (Dùng Chung vs Tạm Thời)
```xml
<div class="diagram-container">
    <div class="diagram-title"><span>📊 Hình Dung Bộ Nhớ: Kho Dùng Chung vs Khu Vực Tạm</span></div>
    <svg class="diagram-svg" viewBox="0 0 850 300" width="850" height="300" xmlns="http://www.w3.org/2000/svg">
        <rect x="10" y="10" width="830" height="280" rx="10" fill="#0f172a" stroke="#222f49" stroke-width="1.5"/>
        <text x="35" y="40" fill="#94a3b8" font-family="'Plus Jakarta Sans', sans-serif" font-size="13" font-weight="bold">Toàn Bộ Bộ Nhớ Khi Chương Trình Đang Chạy</text>
        
        <!-- Left: Shared / Persistent Area -->
        <rect x="35" y="65" width="360" height="205" rx="8" fill="#1e293b" stroke="#38bdf8" stroke-width="1.5"/>
        <text x="55" y="95" fill="#38bdf8" font-family="'Plus Jakarta Sans', sans-serif" font-size="13" font-weight="700">1. Kho Dùng Chung (Giữ Suốt Lúc Chạy)</text>
        <rect x="55" y="120" width="320" height="45" rx="4" fill="#0b0f19" stroke="#38bdf8"/>
        <text x="70" y="140" fill="#cbd5e1" font-family="'Plus Jakarta Sans', sans-serif" font-size="11.5" font-weight="600">Dữ liệu cố định (Số, chữ đơn giản)</text>
        <text x="70" y="156" fill="#94a3b8" font-family="'Plus Jakarta Sans', sans-serif" font-size="10">Ai trong chương trình cũng nhìn thấy chung 1 bản</text>
        <rect x="55" y="180" width="320" height="45" rx="4" fill="#0b0f19" stroke="#a855f7"/>
        <text x="70" y="200" fill="#c084fc" font-family="'Plus Jakarta Sans', sans-serif" font-size="11.5" font-weight="600">Dây nối giữ đồ vật (Tham chiếu cố định)</text>
        <text x="70" y="216" fill="#94a3b8" font-family="'Plus Jakarta Sans', sans-serif" font-size="10">Còn dây nối này thì đồ vật bên phải không bị xóa</text>

        <!-- Right: Temporary / Managed Area -->
        <rect x="445" y="65" width="370" height="205" rx="8" fill="#1e293b" stroke="#10b981" stroke-width="1.5"/>
        <text x="465" y="95" fill="#34d399" font-family="'Plus Jakarta Sans', sans-serif" font-size="13" font-weight="700">2. Khu Vực Tạo Mới (Có Người Dọn Rác)</text>
        <rect x="465" y="140" width="330" height="65" rx="4" fill="#0b0f19" stroke="#a855f7" stroke-width="1.5"/>
        <text x="480" y="165" fill="#e2e8f0" font-family="'Plus Jakarta Sans', sans-serif" font-size="12" font-weight="600">Các đối tượng tạo ra khi chạy (Nhân vật, UI...)</text>
        <text x="480" y="188" fill="#fb7185" font-family="'Plus Jakarta Sans', sans-serif" font-size="10.5">→ Bị giữ lại mãi nếu còn nối dây từ Kho Dùng Chung</text>

        <!-- Connection Arrow -->
        <path d="M 375 202 L 465 172" fill="none" stroke="#a855f7" stroke-width="2.5" marker-end="url(#arrow-p)"/>
        <defs>
            <marker id="arrow-p" markerWidth="8" markerHeight="8" refX="6" refY="4" orient="auto">
                <path d="M 0 0 L 8 4 L 0 8 z" fill="#a855f7" />
            </marker>
        </defs>
    </svg>
    <p class="callout-text" style="margin-top: 10px;">
        <strong>Cách hiểu nhanh:</strong> Kho Dùng Chung không tự xóa cho đến khi tắt hẳn chương trình. Nếu nó đang giữ dây nối tới một nhân vật ở Khu Vực Tạo Mới, nhân viên dọn rác sẽ tưởng nhân vật đó vẫn đang cần dùng và không dọn đi được.
    </p>
</div>
```

---

## 2. Mẫu Sơ Đồ Cây Quyết Định (Hỏi - Đáp Có / Không)
```xml
<div class="diagram-container">
    <div class="diagram-title"><span>🧭 Hỏi Nhanh Để Chọn Cách Làm Đúng</span></div>
    <svg class="diagram-svg" viewBox="0 0 850 320" width="850" height="320" xmlns="http://www.w3.org/2000/svg">
        <rect x="10" y="10" width="830" height="300" rx="10" fill="#0f172a" stroke="#222f49" stroke-width="1.5"/>
        
        <!-- Start Block -->
        <rect x="275" y="30" width="300" height="42" rx="6" fill="#1e293b" stroke="#38bdf8" stroke-width="2"/>
        <text x="425" y="56" fill="#38bdf8" text-anchor="middle" font-family="'Plus Jakarta Sans', sans-serif" font-size="12.5" font-weight="bold">Bạn đang muốn thêm tính năng mới?</text>
        
        <!-- Decision Rhombus -->
        <polygon points="425,95 585,140 425,185 265,140" fill="#152438" stroke="#a855f7" stroke-width="1.5"/>
        <text x="425" y="144" fill="#e2e8f0" text-anchor="middle" font-family="'Plus Jakarta Sans', sans-serif" font-size="11.5">Có cần nhớ dữ liệu riêng từng vật?</text>

        <!-- Branch 1: No -->
        <path d="M 585 140 L 635 140" fill="none" stroke="#10b981" stroke-width="2"/>
        <text x="610" y="132" fill="#34d399" text-anchor="middle" font-family="'Plus Jakarta Sans', sans-serif" font-size="11" font-weight="bold">Không</text>
        <rect x="635" y="115" width="180" height="50" rx="6" fill="#0d2b24" stroke="#10b981" stroke-width="1.5"/>
        <text x="725" y="137" fill="#34d399" text-anchor="middle" font-family="'Plus Jakarta Sans', sans-serif" font-size="11.5" font-weight="bold">Dùng hàm tiện ích chung</text>
        <text x="725" y="154" fill="#94a3b8" text-anchor="middle" font-family="'Plus Jakarta Sans', sans-serif" font-size="10">(Ví dụ: tính toán, đổi định dạng)</text>

        <!-- Branch 2: Yes -->
        <path d="M 425 185 L 425 225" fill="none" stroke="#f43f5e" stroke-width="2"/>
        <text x="448" y="210" fill="#fb7185" text-anchor="middle" font-family="'Plus Jakarta Sans', sans-serif" font-size="11" font-weight="bold">Có</text>
        <rect x="305" y="225" width="240" height="50" rx="6" fill="#2d121c" stroke="#f43f5e" stroke-width="1.5"/>
        <text x="425" y="247" fill="#fb7185" text-anchor="middle" font-family="'Plus Jakarta Sans', sans-serif" font-size="11.5" font-weight="bold">Gắn dữ liệu lên từng đối tượng</text>
        <text x="425" y="264" fill="#94a3b8" text-anchor="middle" font-family="'Plus Jakarta Sans', sans-serif" font-size="10">(Tránh dùng biến chung dễ lẫn lộn)</text>
    </svg>
</div>
```
